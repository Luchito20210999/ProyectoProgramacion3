package pe.edu.pucp.softprog.proyectopro3rs.resources;

import jakarta.ws.rs.Consumes;
import jakarta.ws.rs.GET;
import jakarta.ws.rs.POST;
import jakarta.ws.rs.Path;
import jakarta.ws.rs.PathParam;
import jakarta.ws.rs.Produces;
import jakarta.ws.rs.HeaderParam;
import jakarta.ws.rs.core.Context;
import jakarta.ws.rs.core.UriInfo;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;
import pe.edu.pucp.proyectopro3.bo.webhooks.BokunSignatureValidator;
import pe.edu.pucp.proyectopro3.bo.webhooks.WebhookBokunBO;
import pe.edu.pucp.proyectopro3.bo.webhooks.WebhookBokunBOImpl;
import pe.edu.pucp.proyectopro3.db.DBFactoryProvider;
import pe.edu.pucp.proyectopro3.db.DBManager;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.Statement;
import java.util.HashMap;
import java.util.Map;

@Path("/v1/bokun")
@Produces(MediaType.APPLICATION_JSON)
public class BokunResource {

    private final WebhookBokunBO webhookBO;
    private final BokunSignatureValidator signatureValidator;
    private static final java.util.concurrent.ExecutorService executor = 
            java.util.concurrent.Executors.newFixedThreadPool(4);

    @Context
    private UriInfo uriInfo;

    public BokunResource() {
        webhookBO = new WebhookBokunBOImpl();
        signatureValidator = new BokunSignatureValidator();
    }

    @POST
    @Path("/webhook")
    @Consumes(MediaType.WILDCARD)
    public Response procesarWebhook(
            @HeaderParam("X-Bokun-Signature") String signature,
            @HeaderParam("X-Bokun-Date") String date,
            String payload) {
        try {
            if (payload == null || payload.trim().isEmpty()) {
                return Response.status(Response.Status.BAD_REQUEST)
                        .entity(Map.of(
                                "status", "error",
                                "message", "El payload del webhook no puede estar vacío"
                        ))
                        .build();
            }

            // Validar firma criptográfica HMAC-SHA1
            String path = uriInfo != null ? "/" + uriInfo.getPath() : "/v1/bokun/webhook";
            if (!signatureValidator.esFirmaValida(signature, date, "POST", path, payload)) {
                return Response.status(Response.Status.UNAUTHORIZED)
                        .entity(Map.of(
                                "status", "error",
                                "message", "Firma digital de webhook inválida o ausente"
                        ))
                        .build();
            }

            // Insertar en la cola persistida de base de datos Webhook_Queue (Paso 4 de walkthrough_cristhian.md)
            final int idQueue = insertarEnCola(payload);

            executor.submit(new Runnable() {
                @Override
                public void run() {
                    procesarConReintentos(idQueue, payload, 1);
                }
            });

            return Response.ok()
                    .entity(Map.of(
                            "status", "success",
                            "message", "Webhook recibido y encolado para procesamiento asíncrono",
                            "idQueue", idQueue
                    ))
                    .build();

        } catch (Exception e) {
            return Response.status(Response.Status.INTERNAL_SERVER_ERROR)
                    .entity(Map.of(
                            "status", "error",
                            "message", e.getMessage()
                    ))
                    .build();
        }
    }

    private void procesarConReintentos(final int idQueue, final String payload, final int intento) {
        try {
            webhookBO.procesarWebhook(payload);
            if (idQueue > 0) {
                actualizarCola(idQueue, "PROCESADO", null);
            }
        } catch (Exception e) {
            System.err.println("[BokunResource] Error procesando webhook asíncrono (intento " + intento + "): " + e.getMessage());
            if (idQueue > 0) {
                actualizarCola(idQueue, "ERROR", e.getMessage());
            }
            if (intento < 5) {
                executor.submit(new Runnable() {
                    @Override
                    public void run() {
                        try {
                            Thread.sleep(5000);
                        } catch (InterruptedException ie) {
                            Thread.currentThread().interrupt();
                        }
                        procesarConReintentos(idQueue, payload, intento + 1);
                    }
                });
            } else {
                System.err.println("[BokunResource] Excedido el número máximo de reintentos para el webhook.");
            }
        }
    }

    private int insertarEnCola(String payload) {
        String sql = "INSERT INTO Webhook_Queue (payload, estado, intentos) VALUES (?, 'PENDIENTE', 0)";
        try (Connection conn = DBFactoryProvider.getManager().getConnection();
             PreparedStatement cmd = conn.prepareStatement(sql, Statement.RETURN_GENERATED_KEYS)) {
            cmd.setString(1, payload);
            cmd.executeUpdate();
            try (ResultSet rs = cmd.getGeneratedKeys()) {
                if (rs.next()) {
                    return rs.getInt(1);
                }
            }
        } catch (Exception e) {
            System.err.println("[BokunResource] Error al insertar en cola Webhook_Queue: " + e.getMessage());
        }
        return -1;
    }

    private void actualizarCola(int idQueue, String estado, String error) {
        String sql = "UPDATE Webhook_Queue SET estado = ?, intentos = intentos + 1, fecha_procesamiento = NOW(), mensaje_error = ? WHERE id_queue = ?";
        try (Connection conn = DBFactoryProvider.getManager().getConnection();
             PreparedStatement cmd = conn.prepareStatement(sql)) {
            cmd.setString(1, estado);
            cmd.setString(2, error != null && error.length() > 500 ? error.substring(0, 500) : error);
            cmd.setInt(3, idQueue);
            cmd.executeUpdate();
        } catch (Exception e) {
            System.err.println("[BokunResource] Error al actualizar cola Webhook_Queue: " + e.getMessage());
        }
    }

    @POST
    @Path("/sync/{bookingId}")
    public Response sincronizarBooking(@PathParam("bookingId") String bookingId) {
        try {
            webhookBO.sincronizarBooking(bookingId);

            return Response.ok()
                    .entity(Map.of(
                            "status", "success",
                            "message", "Booking sincronizado correctamente",
                            "bookingId", bookingId
                    ))
                    .build();

        } catch (IllegalArgumentException e) {
            return Response.status(Response.Status.BAD_REQUEST)
                    .entity(Map.of(
                            "status", "error",
                            "message", e.getMessage()
                    ))
                    .build();
        } catch (Exception e) {
            return Response.status(Response.Status.INTERNAL_SERVER_ERROR)
                    .entity(Map.of(
                            "status", "error",
                            "message", e.getMessage()
                    ))
                    .build();
        }
    }

    @GET
    @Path("/diagnostico")
    public Response diagnostico() {
        Map<String, Object> datos = new HashMap<>();

        try {
            DBManager dbManager = DBFactoryProvider.getManager();
            try (Connection conn = dbManager.getConnection()) {
                datos.put("jdbcCatalog", conn.getCatalog());
                datos.put("jdbcSchema", conn.getSchema());

                try (Statement st = conn.createStatement();
                     ResultSet rs = st.executeQuery(
                             "SELECT CURRENT_USER() AS currentUser, "
                                     + "USER() AS sessionUser, DATABASE() AS databaseName")) {
                    if (rs.next()) {
                        datos.put("currentUser", rs.getString("currentUser"));
                        datos.put("sessionUser", rs.getString("sessionUser"));
                        datos.put("databaseName", rs.getString("databaseName"));
                    }
                }

                String sql = "SELECT ROUTINE_SCHEMA, ROUTINE_NAME, DEFINER "
                        + "FROM information_schema.ROUTINES "
                        + "WHERE ROUTINE_TYPE = 'PROCEDURE' "
                        + "AND ROUTINE_SCHEMA = ? "
                        + "AND ROUTINE_NAME = ?";
                try (PreparedStatement ps = conn.prepareStatement(sql)) {
                    ps.setString(1, "SACRSoft");
                    ps.setString(2, "sp_ProcesarYDispersarWebhookBokun");

                    try (ResultSet rs = ps.executeQuery()) {
                        if (rs.next()) {
                            datos.put("procedureFound", true);
                            datos.put("procedureSchema", rs.getString("ROUTINE_SCHEMA"));
                            datos.put("procedureName", rs.getString("ROUTINE_NAME"));
                            datos.put("procedureDefiner", rs.getString("DEFINER"));
                        } else {
                            datos.put("procedureFound", false);
                        }
                    }
                }
            }

            return Response.ok(datos).build();
        } catch (Exception e) {
            datos.put("status", "error");
            datos.put("message", e.getMessage());
            return Response.status(Response.Status.INTERNAL_SERVER_ERROR)
                    .entity(datos)
                    .build();
        }
    }
}

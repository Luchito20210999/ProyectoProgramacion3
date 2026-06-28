package pe.edu.pucp.softprog.proyectopro3rs.resources;

import jakarta.ws.rs.Consumes;
import jakarta.ws.rs.GET;
import jakarta.ws.rs.POST;
import jakarta.ws.rs.Path;
import jakarta.ws.rs.Produces;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;
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

    public BokunResource() {
        webhookBO = new WebhookBokunBOImpl();
    }

    @POST
    @Path("/webhook")
    @Consumes(MediaType.WILDCARD)
    public Response procesarWebhook(String payload) {
        try {
            webhookBO.procesarWebhook(payload);

            return Response.ok()
                    .entity(Map.of(
                            "status", "success",
                            "message", "Webhook procesado correctamente"
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
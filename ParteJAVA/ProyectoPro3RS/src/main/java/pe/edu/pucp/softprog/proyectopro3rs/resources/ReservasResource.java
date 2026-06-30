package pe.edu.pucp.softprog.proyectopro3rs.resources;

import jakarta.ws.rs.Consumes;
import jakarta.ws.rs.DELETE;
import jakarta.ws.rs.GET;
import jakarta.ws.rs.POST;
import jakarta.ws.rs.PUT;
import jakarta.ws.rs.Path;
import jakarta.ws.rs.PathParam;
import jakarta.ws.rs.Produces;
import jakarta.ws.rs.core.Context;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;
import jakarta.ws.rs.core.UriInfo;
import pe.edu.pucp.proyectopro3.bo.reservas.ReservaBO;
import pe.edu.pucp.proyectopro3.bo.reservas.ReservaBOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.dto.ReservaDetalleDTO;
import pe.edu.pucp.proyectopro3.modelo.reservas.Reserva;

import java.net.URI;
import java.util.List;
import java.util.Map;

@Path("/v1/reservas")
@Consumes(MediaType.APPLICATION_JSON)
@Produces(MediaType.APPLICATION_JSON)
public class ReservasResource {
    private final ReservaBO reservaBO;

    @Context
    private UriInfo uriInfo;

    public ReservasResource() {
        this.reservaBO = new ReservaBOImpl();
    }

    @GET
    public Response listar() {
        try {
            List<ReservaDetalleDTO> reservas = this.reservaBO.listarDetalle();
            return Response.ok(reservas).build();
        } catch (RuntimeException ex) {
            return error(Response.Status.INTERNAL_SERVER_ERROR, ex.getMessage());
        }
    }

    @GET
    @Path("{id}")
    public Response obtener(@PathParam("id") int id) {
        try {
            ReservaDetalleDTO reserva = this.reservaBO.obtenerDetalle(id);
            if (reserva == null) {
                return error(Response.Status.NOT_FOUND, "Reserva: " + id + ", no encontrada");
            }
            return Response.ok(reserva).build();
        } catch (IllegalArgumentException ex) {
            return error(Response.Status.BAD_REQUEST, ex.getMessage());
        } catch (RuntimeException ex) {
            return error(Response.Status.INTERNAL_SERVER_ERROR, ex.getMessage());
        }
    }

    @POST
    public Response crear(Reserva reserva) {
        if (reserva == null) {
            return error(Response.Status.BAD_REQUEST, "La reserva no es valida");
        }

        try {
            this.reservaBO.guardar(reserva, Estado.Nuevo);
            URI location = uriInfo.getAbsolutePathBuilder()
                    .path(String.valueOf(reserva.getIdReserva()))
                    .build();

            return Response.created(location)
                    .entity(reserva)
                    .build();
        } catch (IllegalArgumentException ex) {
            return error(Response.Status.BAD_REQUEST, ex.getMessage());
        } catch (RuntimeException ex) {
            return error(Response.Status.INTERNAL_SERVER_ERROR, ex.getMessage());
        }
    }

    @PUT
    @Path("{id}")
    public Response actualizar(@PathParam("id") int id, Reserva reserva) {
        if (reserva == null) {
            return error(Response.Status.BAD_REQUEST, "La reserva no es valida");
        }

        try {
            if (this.reservaBO.obtener(id) == null) {
                return error(Response.Status.NOT_FOUND, "Reserva: " + id + ", no encontrada");
            }
            reserva.setIdReserva(id);
            this.reservaBO.guardar(reserva, Estado.Modificado);
            return Response.ok(reserva).build();
        } catch (IllegalArgumentException ex) {
            return error(Response.Status.BAD_REQUEST, ex.getMessage());
        } catch (RuntimeException ex) {
            return error(Response.Status.INTERNAL_SERVER_ERROR, ex.getMessage());
        }
    }

    @DELETE
    @Path("{id}")
    public Response eliminar(@PathParam("id") int id) {
        try {
            if (this.reservaBO.obtener(id) == null) {
                return error(Response.Status.NOT_FOUND, "Reserva: " + id + ", no encontrada");
            }
            this.reservaBO.eliminar(id);
            return Response.noContent().build();
        } catch (IllegalArgumentException ex) {
            return error(Response.Status.BAD_REQUEST, ex.getMessage());
        } catch (RuntimeException ex) {
            return error(Response.Status.INTERNAL_SERVER_ERROR, ex.getMessage());
        }
    }

    private Response error(Response.Status status, String mensaje) {
        return Response.status(status)
                .entity(Map.of("error", mensaje == null ? status.getReasonPhrase() : mensaje))
                .build();
    }
}

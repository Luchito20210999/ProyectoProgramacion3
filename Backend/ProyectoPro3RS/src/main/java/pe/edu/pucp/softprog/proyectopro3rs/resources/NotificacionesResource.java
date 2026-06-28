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
import pe.edu.pucp.proyectopro3.bo.notificaciones.NotificacionBO;
import pe.edu.pucp.proyectopro3.bo.notificaciones.NotificacionBOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.notificaciones.Notificacion;

import java.net.URI;
import java.util.List;
import java.util.Map;

@Path("/v1/notificaciones")
@Consumes(MediaType.APPLICATION_JSON)
@Produces(MediaType.APPLICATION_JSON)
public class NotificacionesResource {
    private final NotificacionBO notificacionBO;

    @Context
    private UriInfo uriInfo;

    public NotificacionesResource() {
        this.notificacionBO = new NotificacionBOImpl();
    }

    @GET
    public Response listar() {
        try {
            List<Notificacion> notificaciones = this.notificacionBO.listar();
            return Response.ok(notificaciones).build();
        } catch (RuntimeException ex) {
            return error(Response.Status.INTERNAL_SERVER_ERROR, ex.getMessage());
        }
    }

    @GET
    @Path("{id}")
    public Response obtener(@PathParam("id") int id) {
        try {
            Notificacion notificacion = this.notificacionBO.obtener(id);
            if (notificacion == null) {
                return error(Response.Status.NOT_FOUND, "Notificacion: " + id + ", no encontrada");
            }
            return Response.ok(notificacion).build();
        } catch (IllegalArgumentException ex) {
            return error(Response.Status.BAD_REQUEST, ex.getMessage());
        } catch (RuntimeException ex) {
            return error(Response.Status.INTERNAL_SERVER_ERROR, ex.getMessage());
        }
    }

    @POST
    public Response crear(Notificacion notificacion) {
        if (notificacion == null) {
            return error(Response.Status.BAD_REQUEST, "La notificacion no es valida");
        }

        try {
            this.notificacionBO.guardar(notificacion, Estado.Nuevo);
            URI location = uriInfo.getAbsolutePathBuilder()
                    .path(String.valueOf(notificacion.getIdNotificacion()))
                    .build();

            return Response.created(location)
                    .entity(notificacion)
                    .build();
        } catch (IllegalArgumentException ex) {
            return error(Response.Status.BAD_REQUEST, ex.getMessage());
        } catch (RuntimeException ex) {
            return error(Response.Status.INTERNAL_SERVER_ERROR, ex.getMessage());
        }
    }

    @PUT
    @Path("{id}")
    public Response actualizar(@PathParam("id") int id, Notificacion notificacion) {
        if (notificacion == null) {
            return error(Response.Status.BAD_REQUEST, "La notificacion no es valida");
        }

        try {
            if (this.notificacionBO.obtener(id) == null) {
                return error(Response.Status.NOT_FOUND, "Notificacion: " + id + ", no encontrada");
            }
            notificacion.setIdNotificacion(id);
            this.notificacionBO.guardar(notificacion, Estado.Modificado);
            return Response.ok(notificacion).build();
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
            if (this.notificacionBO.obtener(id) == null) {
                return error(Response.Status.NOT_FOUND, "Notificacion: " + id + ", no encontrada");
            }
            this.notificacionBO.eliminar(id);
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

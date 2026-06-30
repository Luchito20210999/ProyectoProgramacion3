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
import pe.edu.pucp.proyectopro3.bo.auditoria.AuditoriaRegistro;
import pe.edu.pucp.proyectopro3.bo.reclamos.ReclamoBO;
import pe.edu.pucp.proyectopro3.bo.reclamos.ReclamoBOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.dto.ReclamoDetalleDTO;
import pe.edu.pucp.proyectopro3.modelo.reclamos.Reclamo;

import java.net.URI;
import java.util.List;
import java.util.Map;

@Path("/v1/reclamos")
@Consumes(MediaType.APPLICATION_JSON)
@Produces(MediaType.APPLICATION_JSON)
public class ReclamosResource {
    private final ReclamoBO reclamoBO;
    private final AuditoriaRegistro auditoriaRegistro;

    @Context
    private UriInfo uriInfo;

    public ReclamosResource() {
        this.reclamoBO = new ReclamoBOImpl();
        this.auditoriaRegistro = new AuditoriaRegistro();
    }

    @GET
    public Response listar() {
        try {
            List<ReclamoDetalleDTO> reclamos = this.reclamoBO.listarDetalle();
            return Response.ok(reclamos).build();
        } catch (RuntimeException ex) {
            return error(Response.Status.INTERNAL_SERVER_ERROR, ex.getMessage());
        }
    }

    @GET
    @Path("{id}")
    public Response obtener(@PathParam("id") int id) {
        try {
            ReclamoDetalleDTO reclamo = this.reclamoBO.obtenerDetalle(id);
            if (reclamo == null) {
                return error(Response.Status.NOT_FOUND, "Reclamo: " + id + ", no encontrado");
            }
            return Response.ok(reclamo).build();
        } catch (IllegalArgumentException ex) {
            return error(Response.Status.NOT_FOUND, ex.getMessage());
        } catch (RuntimeException ex) {
            return error(Response.Status.INTERNAL_SERVER_ERROR, ex.getMessage());
        }
    }

    @POST
    public Response crear(Reclamo reclamo) {
        if (reclamo == null) {
            return error(Response.Status.BAD_REQUEST, "El reclamo no es valido");
        }

        try {
            this.reclamoBO.registrarReclamo(reclamo, reclamo.getIdReserva());
            auditoriaRegistro.registrar(
                    "CREAR_RECLAMO",
                    "Registro de reclamo " + reclamo.getIdReclamo(),
                    "Modulo Reclamos",
                    reclamo.getIdUsuario());

            URI location = uriInfo.getAbsolutePathBuilder()
                    .path(String.valueOf(reclamo.getIdReclamo()))
                    .build();

            return Response.created(location)
                    .entity(reclamo)
                    .build();
        } catch (IllegalArgumentException ex) {
            return error(Response.Status.BAD_REQUEST, ex.getMessage());
        } catch (RuntimeException ex) {
            return error(Response.Status.INTERNAL_SERVER_ERROR, ex.getMessage());
        }
    }

    @PUT
    @Path("{id}")
    public Response actualizar(@PathParam("id") int id, Reclamo reclamo) {
        if (reclamo == null) {
            return error(Response.Status.BAD_REQUEST, "El reclamo no es valido");
        }

        try {
            this.reclamoBO.obtener(id);
            reclamo.setIdReclamo(id);
            this.reclamoBO.guardar(reclamo, Estado.Modificado);
            auditoriaRegistro.registrar(
                    "ACTUALIZAR_RECLAMO",
                    "Actualizacion de reclamo " + id,
                    "Modulo Reclamos",
                    reclamo.getIdUsuario());
            return Response.ok(reclamo).build();
        } catch (IllegalArgumentException ex) {
            return error(Response.Status.NOT_FOUND, ex.getMessage());
        } catch (RuntimeException ex) {
            return error(Response.Status.INTERNAL_SERVER_ERROR, ex.getMessage());
        }
    }

    @DELETE
    @Path("{id}")
    public Response eliminar(@PathParam("id") int id) {
        try {
            Reclamo reclamo = this.reclamoBO.obtener(id);
            this.reclamoBO.eliminar(id);
            auditoriaRegistro.registrar(
                    "ELIMINAR_RECLAMO",
                    "Eliminacion de reclamo " + id,
                    "Modulo Reclamos",
                    reclamo.getIdUsuario());
            return Response.noContent().build();
        } catch (IllegalArgumentException ex) {
            return error(Response.Status.NOT_FOUND, ex.getMessage());
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

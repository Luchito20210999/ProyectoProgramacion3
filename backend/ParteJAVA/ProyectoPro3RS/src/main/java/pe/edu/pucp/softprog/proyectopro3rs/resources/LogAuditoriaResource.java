package pe.edu.pucp.softprog.proyectopro3rs.resources;


import jakarta.ws.rs.*;
import jakarta.ws.rs.core.Context;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;
import jakarta.ws.rs.core.UriInfo;
import pe.edu.pucp.proyectopro3.bo.auditoria.LogAuditoriaBO;
import pe.edu.pucp.proyectopro3.bo.auditoria.LogAuditoriaBOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.auditoria.LogAuditoria;
import pe.edu.pucp.proyectopro3.modelo.crm.Cliente;

import java.net.URI;
import java.util.List;
import java.util.Map;

@Path("/v1/auditoria")
@Consumes(MediaType.APPLICATION_JSON)
@Produces(MediaType.APPLICATION_JSON)
public class LogAuditoriaResource {
    private final LogAuditoriaBO logauditoriaBO;

    @Context
    private UriInfo uriInfo;

    public LogAuditoriaResource(){this.logauditoriaBO = new LogAuditoriaBOImpl();
    }

    @GET
    public List<LogAuditoria> listar() {
        return this.logauditoriaBO.listar();
    }

    @GET
    @Path("{id}")
    public Response obtener(@PathParam("id") int id) {
        LogAuditoria logAuditoria = this.logauditoriaBO.obtener(id);

        if (logAuditoria == null) {
            return Response.status(Response.Status.NOT_FOUND)
                    .entity(Map.of("error", "LogAuditoria: " + id + ", no encontrado"))
                    .build();
        }

        return Response.ok(logAuditoria).build();
    }

    @POST
    public Response crear(LogAuditoria logAuditoria) {
        if (logAuditoria == null) {
            return Response.status(Response.Status.BAD_REQUEST)
                    .entity("La Auditoria no es valido")
                    .build();
        }

        this.logauditoriaBO.guardar(logAuditoria, Estado.Nuevo);
        URI location = uriInfo.getAbsolutePathBuilder()
                .path(String.valueOf(logAuditoria.getIdLogAuditoria()))
                .build();

        return Response.created(location)
                .entity(logAuditoria)
                .build();
    }

    @PUT
    @Path("{id}")
    public Response actualizar(@PathParam("id") int id, LogAuditoria logAuditoria) {
        if (logAuditoria == null ) {
            return Response.status(Response.Status.BAD_REQUEST)
                    .entity(Map.of("error", "La auditoria fallo al actualizarse"))
                    .build();
        }

        if (this.logauditoriaBO.obtener(id) == null) {
            return Response.status(Response.Status.NOT_FOUND)
                    .entity("LogAuditoria: " + id + ", no encontrado")
                    .build();
        }

        this.logauditoriaBO.guardar(logAuditoria, Estado.Modificado);

        return Response.ok(logAuditoria).build();
    }

    @DELETE
    @Path("{id}")
    public Response eliminar(@PathParam("id") int id) {
        if (this.logauditoriaBO.obtener(id) == null) {
            return Response.status(Response.Status.NOT_FOUND)
                    .entity("LogAuditoria: " + id + ", no encontrada")
                    .build();
        }
        this.logauditoriaBO.eliminar(id);

        return Response.noContent().build();
    }
}

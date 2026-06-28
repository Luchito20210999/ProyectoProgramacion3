package pe.edu.pucp.softprog.proyectopro3rs.resources;

import jakarta.ws.rs.*;
import jakarta.ws.rs.core.Context;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;
import jakarta.ws.rs.core.UriInfo;
import pe.edu.pucp.proyectopro3.bo.reservas.ServicioBO;
import pe.edu.pucp.proyectopro3.bo.reservas.ServicioBOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.reservas.Servicio;

import java.net.URI;
import java.util.List;
import java.util.Map;

@Path("/v1/servicios")
@Consumes(MediaType.APPLICATION_JSON)
@Produces(MediaType.APPLICATION_JSON)
public class ServicioResource {
    private final ServicioBO servicioBO;

    @Context
    private UriInfo uriInfo;

    public ServicioResource() {
        this.servicioBO = new ServicioBOImpl();
    }

    @GET
    public List<Servicio> listar() {
        return this.servicioBO.listar();
    }

    @GET
    @Path("{id}")
    public Response obtener(@PathParam("id") int id) {
        Servicio s = this.servicioBO.obtener(id);

        if (s == null) {
            return Response.status(Response.Status.NOT_FOUND)
                    .entity(Map.of("error", "Servicio: " + id + ", no encontrado"))
                    .build();
        }

        return Response.ok(s).build();
    }

    @POST
    public Response crear(Servicio servicio) {
        if (servicio == null || servicio.getNombre() == null || servicio.getNombre().isBlank()) {
            return Response.status(Response.Status.BAD_REQUEST)
                    .entity("El servicio no es valido")
                    .build();
        }

        this.servicioBO.guardar(servicio, Estado.Nuevo);
        URI location = uriInfo.getAbsolutePathBuilder()
                .path(String.valueOf(servicio.getIdServicio()))
                .build();

        return Response.created(location)
                .entity(servicio)
                .build();
    }

    @PUT
    @Path("{id}")
    public Response actualizar(@PathParam("id") int id, Servicio servicio) {
        if (servicio == null || servicio.getNombre() == null || servicio.getNombre().isBlank()) {
            return Response.status(Response.Status.BAD_REQUEST)
                    .entity(Map.of("error", "El servicio no es valido"))
                    .build();
        }

        if (this.servicioBO.obtener(id) == null) {
            return Response.status(Response.Status.NOT_FOUND)
                    .entity("Servicio: " + id + ", no encontrado")
                    .build();
        }

        this.servicioBO.guardar(servicio, Estado.Modificado);

        return Response.ok(servicio).build();
    }

    @DELETE
    @Path("{id}")
    public Response eliminar(@PathParam("id") int id) {
        if (this.servicioBO.obtener(id) == null) {
            return Response.status(Response.Status.NOT_FOUND)
                    .entity("Servicio: " + id + ", no encontrado")
                    .build();
        }
        this.servicioBO.eliminar(id);

        return Response.noContent().build();
    }
}

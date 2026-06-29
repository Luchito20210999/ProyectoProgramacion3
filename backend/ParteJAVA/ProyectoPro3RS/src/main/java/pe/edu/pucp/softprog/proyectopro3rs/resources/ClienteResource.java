package pe.edu.pucp.softprog.proyectopro3rs.resources;

import jakarta.ws.rs.*;
import jakarta.ws.rs.core.Context;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;
import jakarta.ws.rs.core.UriInfo;
import pe.edu.pucp.proyectopro3.bo.crm.ClienteBO;
import pe.edu.pucp.proyectopro3.bo.crm.ClienteBOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.crm.Cliente;

import java.net.URI;
import java.util.List;
import java.util.Map;

@Path("/v1/clientes")
@Consumes(MediaType.APPLICATION_JSON)
@Produces(MediaType.APPLICATION_JSON)
public class ClienteResource {
    private final ClienteBO clienteBO;

    @Context
    private UriInfo uriInfo;

    public ClienteResource(){this.clienteBO = new ClienteBOImpl();}

    @GET
    public List<Cliente> listar() {
        return this.clienteBO.listar();
    }

    @GET
    @Path("{id}")
    public Response obtener(@PathParam("id") int id) {
        Cliente cliente = this.clienteBO.obtener(id);

        if (cliente == null) {
            return Response.status(Response.Status.NOT_FOUND)
                    .entity(Map.of("error", "Cliente: " + id + ", no encontrado"))
                    .build();
        }

        return Response.ok(cliente).build();
    }

    @POST
    public Response crear(Cliente cliente) {
        if (cliente == null || cliente.getNombres() == null || cliente.getNombres().isBlank()) {
            return Response.status(Response.Status.BAD_REQUEST)
                    .entity("El cliente no es valido")
                    .build();
        }

        this.clienteBO.guardar(cliente, Estado.Nuevo);
        URI location = uriInfo.getAbsolutePathBuilder()
                .path(String.valueOf(cliente.getIdCliente()))
                .build();

        return Response.created(location)
                .entity(cliente)
                .build();
    }

    @PUT
    @Path("{id}")
    public Response actualizar(@PathParam("id") int id, Cliente cliente) {
        if (cliente == null || cliente.getNombres() == null || cliente.getNombres().isBlank()) {
            return Response.status(Response.Status.BAD_REQUEST)
                    .entity(Map.of("error", "El cliente no es valido"))
                    .build();
        }

        if (this.clienteBO.obtener(id) == null) {
            return Response.status(Response.Status.NOT_FOUND)
                    .entity("cliente: " + id + ", no encontrado")
                    .build();
        }

        this.clienteBO.guardar(cliente, Estado.Modificado);

        return Response.ok(cliente).build();
    }

    @DELETE
    @Path("{id}")
    public Response eliminar(@PathParam("id") int id) {
        if (this.clienteBO.obtener(id) == null) {
            return Response.status(Response.Status.NOT_FOUND)
                    .entity("Cliente: " + id + ", no encontrada")
                    .build();
        }
        this.clienteBO.eliminar(id);

        return Response.noContent().build();
    }
}

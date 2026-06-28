package pe.edu.pucp.softprog.proyectopro3rs.resources;

import jakarta.ws.rs.*;
import jakarta.ws.rs.core.Context;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;
import jakarta.ws.rs.core.UriInfo;
import pe.edu.pucp.proyectopro3.bo.auth.UsuarioBO;
import pe.edu.pucp.proyectopro3.bo.auth.UsuarioBOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.auth.Usuario;

import java.net.URI;
import java.util.List;
import java.util.Map;

@Path("/v1/usuarios")
@Consumes(MediaType.APPLICATION_JSON)
@Produces(MediaType.APPLICATION_JSON)

public class UsuariosResource {
    private final UsuarioBO usuarioBO;

    @Context
    private UriInfo uriInfo;

    public UsuariosResource(){this.usuarioBO = new UsuarioBOImpl();}

    @GET
    public List<Usuario> listar(){return usuarioBO.listar();}

    @GET
    @Path("{id}")
    public Response obtener(@PathParam("id") int id) {
        Usuario usuario = this.usuarioBO.obtener(id);

        if (usuario == null) {
            return Response.status(Response.Status.NOT_FOUND)
                    .entity(Map.of("error", "Usuario: " + id + ", no encontrado"))
                    .build();
        }

        return Response.ok(usuario).build();
    }

    @POST
    public Response crearUsuario(Usuario usuario) {
        if (usuario == null || usuario.getNombres() == null || usuario.getNombres().isBlank()) {
            return Response.status(Response.Status.BAD_REQUEST)
                    .entity("El usuario no es valido")
                    .build();
        }

        this.usuarioBO.guardar(usuario, Estado.Nuevo);
        URI location = uriInfo.getAbsolutePathBuilder()
                .path(String.valueOf(usuario.getIdUsuario()))
                .build();

        return Response.created(location)
                .entity(usuario)
                .build();
    }

    @PUT
    @Path("{id}")
    public Response actualizar(@PathParam("id") int id, Usuario usuario) {
        if (usuario == null || usuario.getNombres() == null || usuario.getNombres().isBlank()) {
            return Response.status(Response.Status.BAD_REQUEST)
                    .entity(Map.of("error", "El cliente no es valido"))
                    .build();
        }

        if (this.usuarioBO.obtener(id) == null) {
            return Response.status(Response.Status.NOT_FOUND)
                    .entity("Usuario: " + id + ", no encontrado")
                    .build();
        }

        this.usuarioBO.guardar(usuario, Estado.Modificado);

        return Response.ok(usuario).build();
    }
    @DELETE
    @Path("{id}")
    public Response eliminar(@PathParam("id") int id) {
        if (this.usuarioBO.obtener(id) == null) {
            return Response.status(Response.Status.NOT_FOUND)
                    .entity("Usuario: " + id + ", no encontrada")
                    .build();
        }
        this.usuarioBO.eliminar(id);

        return Response.noContent().build();
    }

//    @POST
//    @Path("login")
//    public Response login(Usuario cuenta) {
//        if (cuenta == null || cuenta.getCorreo() == null || cuenta.getContrasena() == null) {
//            return Response.status(Response.Status.BAD_REQUEST)
//                    .entity(Map.of("error", "Credenciales incompletas"))
//                    .build();
//        }
//
//        Usuario usuario = this.usuarioBO.listar().stream()
//                .filter(u -> cuenta.getCorreo().equalsIgnoreCase(u.getCorreo()))
//                .findFirst()
//                .orElse(null);
//
//        String tipoUsuario = cuenta.getTipoUsuario();
//        if ((tipoUsuario == null || tipoUsuario.isBlank()) && usuario != null) {
//            tipoUsuario = usuario.getTipoUsuario();
//        }
//
//        boolean success = tipoUsuario != null && !tipoUsuario.isBlank()
//                && this.usuarioBO.login(
//                cuenta.getCorreo(),
//                cuenta.getContrasena(),
//                tipoUsuario);
//
//        if (success) {
//            usuario.setContrasena(null);
//            return Response.status(Response.Status.OK)
//                    .entity(usuario)
//                    .build();
//        }
//
//        return Response.status(401)
//                .entity("Usuario o password incorrectos")
//                .build();
//    }

    @POST
    @Path("login")
    public Response login(Usuario cuenta) {
        boolean success =
                this.usuarioBO.login(
                        cuenta.getCorreo(),
                        cuenta.getContrasena(),
                        cuenta.getTipoUsuario());

        if (success) {
            return Response.status(Response.Status.OK)
                    .entity("Login exitoso")
                    .build();
        }

        return Response.status(401)
                .entity("Usuario o password incorrectos")
                .build();
    }
}

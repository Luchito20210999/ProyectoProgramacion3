package pe.edu.pucp.proyectopro3.app.pruebas.auth;

import pe.edu.pucp.proyectopro3.bo.auth.UsuarioBO;
import pe.edu.pucp.proyectopro3.bo.auth.UsuarioBOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.auth.Usuario;
import pe.edu.pucp.proyectopro3.modelo.crm.TipoDocumento;

import java.util.List;

public class UsuarioPrueba {

    public static void ejecutar() {
        System.out.println("========== PRUEBA: UsuarioBO ==========");
        UsuarioBO usuarioBO = new UsuarioBOImpl();

        // 1. LISTAR
        System.out.println("[LISTAR] Consultando todos los usuarios...");
        List<Usuario> usuarios = usuarioBO.listar();
        System.out.println("  Total encontrados: " + usuarios.size());
        for (Usuario u : usuarios) {
            System.out.println("  > ID: " + u.getIdUsuario()
                    + " | " + u.getNombres() + " " + u.getApellidos()
                    + " | Correo: " + u.getCorreo());
        }

        // 2. CREAR USUARIO (método de dominio)
        System.out.println("[CREAR] Creando nuevo usuario...");
        Usuario nuevo = new Usuario();
        nuevo.setNombres("Carlos");
        nuevo.setApellidos("García Test");
        nuevo.setTipoDocumento(TipoDocumento.DNI);
        nuevo.setNumeroDocumento("87654321");
        nuevo.setCorreo("carlos.garcia@test.com");
        nuevo.setContrasena("password123");
        nuevo.setNumeroContacto("911222333");

        usuarioBO.guardar(nuevo, Estado.Nuevo);
        System.out.println("  Usuario creado con ID: " + nuevo.getIdUsuario());

        // 3. OBTENER
        System.out.println("[OBTENER] Buscando usuario ID: " + nuevo.getIdUsuario() + "...");
        Usuario encontrado = usuarioBO.obtener(nuevo.getIdUsuario());
        if (encontrado != null) {
            System.out.println("  Encontrado: " + encontrado.getNombres()
                    + " " + encontrado.getApellidos());
        } else {
            System.out.println("  No encontrado.");
        }

        // 4. MODIFICAR
        System.out.println("[MODIFICAR] Actualizando usuario...");
        nuevo.setApellidos("García Modificado");
        usuarioBO.guardar(nuevo, Estado.Modificado);
        System.out.println("  Usuario actualizado correctamente.");

        // 5. ELIMINAR
        System.out.println("[ELIMINAR] Eliminando usuario ID: " + nuevo.getIdUsuario() + "...");
        usuarioBO.eliminar(nuevo.getIdUsuario());
        System.out.println("  Usuario eliminado correctamente.");

        System.out.println("========== FIN: UsuarioBO ==========\n");
    }
}

package pe.edu.pucp.proyectopro3.bo.auth;

import pe.edu.pucp.proyectopro3.bo.BaseBO;
import pe.edu.pucp.proyectopro3.dao.auth.UsuarioDAO;
import pe.edu.pucp.proyectopro3.dao.auth.UsuarioDAOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.auth.Usuario;

import java.util.List;
import java.util.Objects;

public class UsuarioBOImpl extends BaseBO implements UsuarioBO {
    private final UsuarioDAO usuarioDao;

    public UsuarioBOImpl() {
        this.usuarioDao = new UsuarioDAOImpl();
    }

    // ====================================================================
    // MÉTODOS DE DOMINIO (UsuariosGestionable)
    // ====================================================================

    @Override
    public boolean login(String username, String password, String tipoUsuario) {
        validarTextoObligatorio(username, "username");
        validarTextoObligatorio(password, "password");
        validarTextoObligatorio(password, "tipoUsuario");

        return usuarioDao.login(username,password,tipoUsuario);
    }

    // ====================================================================
    // MÉTODOS CRUD HEREDADOS (Gestionable)
    // ====================================================================

    @Override
    public List<Usuario> listar() {
        return this.usuarioDao.leerTodos();
    }

    @Override
    public Usuario obtener(int id) {
        validarIdPositivo(id, "id de usuario");
        return this.usuarioDao.leer(id);
    }

    @Override
    public void eliminar(int id) {
        validarIdPositivo(id, "id de usuario");
        if (!this.usuarioDao.eliminar(id)) {
            throw new IllegalStateException("No se pudo eliminar el usuario.");
        }
    }

    @Override
    public void guardar(Usuario modelo, Estado estado) {
        validarUsuario(modelo);
        validarEstado(estado);

        if (estado == Estado.Nuevo) {
            String contrasenaPlana = modelo.getContrasena();
            if (contrasenaPlana != null && !contrasenaPlana.isBlank()) {
                String hash = org.mindrot.jbcrypt.BCrypt.hashpw(contrasenaPlana, org.mindrot.jbcrypt.BCrypt.gensalt());
                modelo.setContrasena(hash);
            }
            Integer id = this.usuarioDao.crear(modelo);
            if (id == null || id <= 0) {
                throw new IllegalStateException("No se pudo registrar el nuevo usuario");
            }
            modelo.setIdUsuario(id);
        } else if (estado == Estado.Modificado) {
            validarIdPositivo(modelo.getIdUsuario(), "id de usuario");
            if (!this.usuarioDao.actualizar(modelo)) {
                throw new IllegalStateException("No se pudo actualizar el usuario con id: " + modelo.getIdUsuario());
            }
        }
    }

    private String hashSHA256(String input) {
        try {
            java.security.MessageDigest digest = java.security.MessageDigest.getInstance("SHA-256");
            byte[] hash = digest.digest(input.getBytes(java.nio.charset.StandardCharsets.UTF_8));
            StringBuilder hexString = new StringBuilder();
            for (byte b : hash) {
                String hex = Integer.toHexString(0xff & b);
                if (hex.length() == 1) hexString.append('0');
                hexString.append(hex);
            }
            return hexString.toString();
        } catch (Exception ex) {
            return input;
        }
    }

    // ====================================================================
    // VALIDACIONES PRIVADAS
    // ====================================================================

    private void validarUsuario(Usuario u) {
        Objects.requireNonNull(u, "El objeto Usuario no puede ser nulo");
        validarTextoObligatorio(u.getNombres(), "nombres del usuario");
        validarTextoObligatorio(u.getApellidos(), "apellidos del usuario");
        validarTextoObligatorio(u.getNumeroDocumento(), "número de documento");
        validarTextoObligatorio(u.getContrasena(), "contraseña del usuario");

        // Validación adicional de negocio: correo electrónico
        if (u.getCorreo() != null && !u.getCorreo().contains("@")) {
            throw new IllegalArgumentException("El formato del correo electrónico es inválido");
        }
    }
}

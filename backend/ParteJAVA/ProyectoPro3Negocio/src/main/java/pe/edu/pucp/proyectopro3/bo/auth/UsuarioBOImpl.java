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
        validarTextoObligatorio(tipoUsuario, "tipoUsuario");

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
        String contrasenaPlana = modelo.getContrasena();
        if (contrasenaPlana != null && !contrasenaPlana.isBlank() && !esHashBCrypt(contrasenaPlana)) {
            String hash = org.mindrot.jbcrypt.BCrypt.hashpw(contrasenaPlana, org.mindrot.jbcrypt.BCrypt.gensalt());
            modelo.setContrasena(hash);
        }
        if (estado == Estado.Nuevo) {
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

    private boolean esHashBCrypt(String valor) {
        return valor.startsWith("$2a$") || valor.startsWith("$2b$") || valor.startsWith("$2y$");
    }
}

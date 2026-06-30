package pe.edu.pucp.proyectopro3.bo.notificaciones;

import pe.edu.pucp.proyectopro3.bo.auth.UsuarioBO;
import pe.edu.pucp.proyectopro3.bo.auth.UsuarioBOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.auth.Usuario;
import pe.edu.pucp.proyectopro3.modelo.notificaciones.Notificacion;
import pe.edu.pucp.proyectopro3.modelo.notificaciones.TipoEvento;

import java.util.Date;
import java.util.HashSet;
import java.util.List;
import java.util.Locale;
import java.util.Set;

public class NotificacionRegistro {
    private final NotificacionBO notificacionBO;
    private final UsuarioBO usuarioBO;

    public NotificacionRegistro() {
        this.notificacionBO = new NotificacionBOImpl();
        this.usuarioBO = new UsuarioBOImpl();
    }

    public void registrarEventoOperativo(TipoEvento tipoEvento, String mensaje, int idUsuarioOperador) {
        Set<Integer> destinatarios = new HashSet<>();

        if (esUsuarioOperativo(idUsuarioOperador)) {
            destinatarios.add(idUsuarioOperador);
        }

        if (destinatarios.isEmpty()) {
            for (Usuario usuario : obtenerAdministradoresActivos()) {
                destinatarios.add(usuario.getIdUsuario());
            }
        }

        for (Integer idUsuario : destinatarios) {
            registrar(tipoEvento, mensaje, idUsuario);
        }
    }

    private void registrar(TipoEvento tipoEvento, String mensaje, int idUsuario) {
        Notificacion notificacion = new Notificacion();
        notificacion.setMensaje(mensaje);
        notificacion.setTipoEvento(tipoEvento);
        notificacion.setTipoNotificacion(tipoEvento.name());
        notificacion.setFechaEnvio(new Date());
        notificacion.setLeido(false);
        notificacion.setIdUsuario(idUsuario);

        this.notificacionBO.guardar(notificacion, Estado.Nuevo);
    }

    private boolean esUsuarioOperativo(int idUsuario) {
        if (idUsuario <= 0) {
            return false;
        }

        try {
            Usuario usuario = this.usuarioBO.obtener(idUsuario);
            return usuario != null
                    && usuario.getActivo()
                    && esRolOperativo(usuario.getTipoUsuario());
        } catch (RuntimeException ex) {
            return false;
        }
    }

    private List<Usuario> obtenerAdministradoresActivos() {
        return this.usuarioBO.listar().stream()
                .filter(usuario -> usuario.getActivo() && esAdministrador(usuario.getTipoUsuario()))
                .toList();
    }

    private boolean esRolOperativo(String tipoUsuario) {
        String rol = normalizarRol(tipoUsuario);
        return "OPERADOR".equals(rol) || "ADMINISTRADOR".equals(rol);
    }

    private boolean esAdministrador(String tipoUsuario) {
        return "ADMINISTRADOR".equals(normalizarRol(tipoUsuario));
    }

    private String normalizarRol(String tipoUsuario) {
        if (tipoUsuario == null) {
            return "";
        }

        String rol = tipoUsuario.trim().toUpperCase(Locale.ROOT);
        return "ADMIN".equals(rol) ? "ADMINISTRADOR" : rol;
    }
}

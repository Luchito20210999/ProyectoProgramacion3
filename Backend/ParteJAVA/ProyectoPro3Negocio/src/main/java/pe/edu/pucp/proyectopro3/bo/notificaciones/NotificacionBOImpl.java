package pe.edu.pucp.proyectopro3.bo.notificaciones;

import pe.edu.pucp.proyectopro3.bo.BaseBO;
import pe.edu.pucp.proyectopro3.dao.notificaciones.NotificacionDAO;
import pe.edu.pucp.proyectopro3.dao.notificaciones.NotificacionDAOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.notificaciones.Notificacion;

import java.util.List;
import java.util.Objects;


public class NotificacionBOImpl extends BaseBO implements NotificacionBO {
    private final NotificacionDAO notificacionDao;

    public NotificacionBOImpl() {
        this.notificacionDao = new NotificacionDAOImpl();
    }

    @Override
    public List<Notificacion> listar() {
        return this.notificacionDao.leerTodos();
    }

    @Override
    public Notificacion obtener(int id) {
        validarIdPositivo(id, "id de notificación");
        return this.notificacionDao.leer(id);
    }

    @Override
    public void eliminar(int id) {
        validarIdPositivo(id, "id de notificación");
        if (!this.notificacionDao.eliminar(id)) {
            throw new IllegalStateException("No se pudo eliminar la notificación con id: " + id);
        }
    }

    @Override
    public void guardar(Notificacion modelo, Estado estado) {
        validarEstado(estado);
        validarNotificacion(modelo);

        if (estado == Estado.Nuevo) {
            int id = this.notificacionDao.crear(modelo);
            if (id <= 0) throw new IllegalStateException("Error al registrar la notificación");
            modelo.setIdNotificacion(id);
        } else if (estado == Estado.Modificado) {
            validarIdPositivo(modelo.getIdNotificacion(), "id de notificación");
            if (!this.notificacionDao.actualizar(modelo)) {
                throw new IllegalStateException("No se pudo actualizar la notificación.");
            }
        }
    }

    private void validarNotificacion(Notificacion modelo) {
        Objects.requireNonNull(modelo, "El objeto Notificación no puede ser nulo");

        validarTextoObligatorio(modelo.getMensaje(), "mensaje de la notificación");

        if (modelo.getFechaEnvio() == null) {
            throw new IllegalArgumentException("La fecha de envío es obligatoria");
        }

        if (modelo.getIdUsuario() <= 0) {
            throw new IllegalArgumentException("La notificación debe estar asociada a un Usuario válido.");
        }
    }
}

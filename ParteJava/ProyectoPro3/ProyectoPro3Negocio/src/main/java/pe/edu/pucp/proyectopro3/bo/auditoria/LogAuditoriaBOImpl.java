package pe.edu.pucp.proyectopro3.bo.auditoria;

import pe.edu.pucp.proyectopro3.bo.BaseBO;
import pe.edu.pucp.proyectopro3.dao.auditoria.LogAuditoriaDAO;
import pe.edu.pucp.proyectopro3.dao.auditoria.LogAuditoriaDAOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.auditoria.LogAuditoria;

import java.util.List;
import java.util.Objects;

public class LogAuditoriaBOImpl extends BaseBO implements LogAuditoriaBO {
    private final LogAuditoriaDAO logDao;

    public LogAuditoriaBOImpl() {
        this.logDao = new LogAuditoriaDAOImpl();
    }

    @Override
    public List<LogAuditoria> listar() {
        return this.logDao.leerTodos();
    }

    @Override
    public LogAuditoria obtener(int id) {
        validarIdPositivo(id, "id de log de auditoría");
        return this.logDao.leer(id);
    }

    @Override
    public void eliminar(int id) {
        throw new UnsupportedOperationException("No se permite eliminar registros de auditoría.");
    }

    @Override
    public void guardar(LogAuditoria modelo, Estado estado) {
        validarEstado(estado);

        if (estado == Estado.Nuevo) {
            validarLog(modelo);
            int id = this.logDao.crear(modelo);
            if (id <= 0) {
                throw new IllegalStateException("Error al registrar el log de auditoría");
            }
            modelo.setIdLogAuditoria(id);
        }

    }

    private void validarLog(LogAuditoria log) {
        Objects.requireNonNull(log, "El objeto de auditoría no puede ser nulo");
        validarTextoObligatorio(log.getAccion(), "acción realizada");
        validarTextoObligatorio(log.getDescripcion(), "descripción del evento");
        validarTextoObligatorio(log.getOrigenAccion(), "origen de la acción (IP/Dispositivo)");

        if (log.getFechaRegistro() == null) {
            throw new IllegalArgumentException("La fecha del evento es obligatoria");
        }
    }
}

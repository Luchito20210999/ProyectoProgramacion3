package pe.edu.pucp.proyectopro3.bo.auditoria;

import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.auditoria.LogAuditoria;

import java.util.Date;

public class AuditoriaRegistro {
    private final LogAuditoriaBO logAuditoriaBO;

    public AuditoriaRegistro() {
        this.logAuditoriaBO = new LogAuditoriaBOImpl();
    }

    public void registrar(String accion, String descripcion, String origenAccion, int idUsuario) {
        LogAuditoria log = new LogAuditoria();
        log.setAccion(accion);
        log.setDescripcion(descripcion);
        log.setOrigenAccion(origenAccion);
        log.setFechaRegistro(new Date());
        log.setIdUsuario(idUsuario);

        this.logAuditoriaBO.guardar(log, Estado.Nuevo);
    }
}
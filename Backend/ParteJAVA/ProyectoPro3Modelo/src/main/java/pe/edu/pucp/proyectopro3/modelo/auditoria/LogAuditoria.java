package pe.edu.pucp.proyectopro3.modelo.auditoria;

import java.util.Date;

public class LogAuditoria {
    private int idLogAuditoria;
    private String descripcion;
    private String accion;
    private Date fechaRegistro;
    private String origenAccion;
    private int idUsuario;

    public LogAuditoria(int idLogAuditoria, String descripcion, String accion, Date fechaRegistro, String origenAccion,  int idUsuario) {
        this.idLogAuditoria = idLogAuditoria;
        this.descripcion = descripcion;
        this.accion = accion;
        this.fechaRegistro = fechaRegistro;
        this.origenAccion = origenAccion;
        this.idUsuario = idUsuario;
    }

    public LogAuditoria() {

    }

    public int getIdLogAuditoria() {
        return idLogAuditoria;
    }

    public void setIdLogAuditoria(int idLogAuditoria) {
        this.idLogAuditoria = idLogAuditoria;
    }

    public Date getFechaRegistro() {
        return fechaRegistro;
    }

    public void setFechaRegistro(Date fechaRegistro) {
        this.fechaRegistro = fechaRegistro;
    }

    public String getDescripcion() {
        return descripcion;
    }

    public void setDescripcion(String descripcion) {
        this.descripcion = descripcion;
    }

    public int getIdUsuario() {
        return idUsuario;
    }

    public void setIdUsuario(int idUsuario) {
        this.idUsuario = idUsuario;
    }

    public String getAccion() {
        return accion;
    }

    public void setAccion(String accion) {
        this.accion = accion;
    }

    public String getOrigenAccion() {
        return origenAccion;
    }

    public void setOrigenAccion(String origenAccion) {
        this.origenAccion = origenAccion;
    }
}

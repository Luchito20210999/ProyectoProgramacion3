package pe.edu.pucp.proyectopro3.modelo.auth;

import pe.edu.pucp.proyectopro3.modelo.auditoria.LogAuditoria;
import pe.edu.pucp.proyectopro3.modelo.crm.TipoDocumento;
import pe.edu.pucp.proyectopro3.modelo.notificaciones.Notificacion;

import java.util.List;

public class Usuario {
    private int idUsuario;
    private String nombres;
    private String apellidos;
    private TipoDocumento tipoDocumento;
    private String numeroDocumento;
    private String correo;
    private String contrasena;
    private String numeroContacto;
    private String tipoUsuario;
    private List<Notificacion> notificacions;
    private List<LogAuditoria> logsAuditorias;

    public Usuario(int idUsuario, String nombres, String apellidos, TipoDocumento tipoDocumento, String numeroDocumento, String correo, String contrasena, String numeroContacto, String tipoUsuario) {
        this.idUsuario = idUsuario;
        this.nombres = nombres;
        this.apellidos = apellidos;
        this.tipoDocumento = tipoDocumento;
        this.numeroDocumento = numeroDocumento;
        this.correo = correo;
        this.contrasena = contrasena;
        this.numeroContacto = numeroContacto;
        this.tipoUsuario = tipoUsuario;
    }

    public Usuario() {

    }

    public int getIdUsuario() {
        return idUsuario;
    }

    public void setIdUsuario(int idUsuario) {
        this.idUsuario = idUsuario;
    }

    public String getNombres() {
        return nombres;
    }

    public void setNombres(String nombres) {
        this.nombres = nombres;
    }

    public String getApellidos() {
        return apellidos;
    }

    public void setApellidos(String apellidos) {
        this.apellidos = apellidos;
    }

    public TipoDocumento getTipoDocumento() {
        return tipoDocumento;
    }

    public void setTipoDocumento(TipoDocumento tipoDocumento) {
        this.tipoDocumento = tipoDocumento;
    }

    public String getNumeroDocumento() {
        return numeroDocumento;
    }

    public void setNumeroDocumento(String numeroDocumento) {
        this.numeroDocumento = numeroDocumento;
    }

    public String getCorreo() {
        return correo;
    }

    public void setCorreo(String correo) {
        this.correo = correo;
    }

    public String getContrasena() {
        return contrasena;
    }

    public void setContrasena(String contrasena) {
        this.contrasena = contrasena;
    }

    public String getNumeroContacto() {
        return numeroContacto;
    }

    public void setNumeroContacto(String numeroContacto) {
        this.numeroContacto = numeroContacto;
    }

    public String getTipoUsuario() {
        return tipoUsuario;
    }

    public void setTipoUsuario(String tipoUsuario) {
        this.tipoUsuario = tipoUsuario;
    }

    public List<Notificacion> getNotificacions() {
        return notificacions;
    }

    public void setNotificacions(List<Notificacion> notificacions) {
        this.notificacions = notificacions;
    }

    public List<LogAuditoria> getLogsAuditorias() {
        return logsAuditorias;
    }

    public void setLogsAuditorias(List<LogAuditoria> logsAuditorias) {
        this.logsAuditorias = logsAuditorias;
    }
}

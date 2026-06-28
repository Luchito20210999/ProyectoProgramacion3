package pe.edu.pucp.proyectopro3.modelo.reclamos;

import java.util.Date;

public class Reclamo {
    private int idReclamo;
    private Date fechaReclamo;
    private String descripcion;
    private EstadoReclamo estadoReclamo;
    private String motivoResolucion;
    private Date fechaResolucion;
    private int idUsuario;
    private int idReserva;

    public Reclamo(int idReclamo, Date fechaReclamo, String descripcion, EstadoReclamo estadoReclamo, String motivoResolucion, Date fechaResolucion, int idReserva) {
        this.idReclamo = idReclamo;
        this.fechaReclamo = fechaReclamo;
        this.descripcion = descripcion;
        this.estadoReclamo = estadoReclamo;
        this.motivoResolucion = motivoResolucion;
        this.fechaResolucion = fechaResolucion;
        this.idReserva = idReserva;
    }

    public Reclamo() {

    }

    public int getIdReclamo() {
        return idReclamo;
    }

    public void setIdReclamo(int idReclamo) {
        this.idReclamo = idReclamo;
    }

    public Date getFechaReclamo() {
        return fechaReclamo;
    }

    public void setFechaReclamo(Date fechaReclamo) {
        this.fechaReclamo = fechaReclamo;
    }

    public String getDescripcion() {
        return descripcion;
    }

    public void setDescripcion(String descripcion) {
        this.descripcion = descripcion;
    }

    public EstadoReclamo getEstadoReclamo() {
        return estadoReclamo;
    }

    public void setEstadoReclamo(EstadoReclamo estadoReclamo) {
        this.estadoReclamo = estadoReclamo;
    }

    public String getMotivoResolucion() {
        return motivoResolucion;
    }

    public void setMotivoResolucion(String motivoResolucion) {
        this.motivoResolucion = motivoResolucion;
    }

    public Date getFechaResolucion() {
        return fechaResolucion;
    }

    public void setFechaResolucion(Date fechaResolucion) {
        this.fechaResolucion = fechaResolucion;
    }

    public int getIdReserva() {
        return idReserva;
    }

    public void setIdReserva(int idReserva) {
        this.idReserva = idReserva;
    }

    public int getIdUsuario() { return idUsuario; }

    public void setIdUsuario(int idUsuario) { this.idUsuario = idUsuario; }
}

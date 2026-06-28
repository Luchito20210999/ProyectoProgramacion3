package pe.edu.pucp.proyectopro3.modelo.reportes;

import java.util.Date;

public abstract class Reporte {
    private int idReporte;
    private Date fechaGeneracion;
    private Date fechaInicioFiltro;
    private Date fechaFinFiltro;

    public Reporte(int idReporte, Date fechaGeneracion, Date fechaInicioFiltro, Date fechaFinFiltro) {
        this.idReporte = idReporte;
        this.fechaGeneracion = fechaGeneracion;
        this.fechaInicioFiltro = fechaInicioFiltro;
        this.fechaFinFiltro = fechaFinFiltro;
    }

    public Reporte() {

    }

    public int getIdReporte() {
        return idReporte;
    }

    public void setIdReporte(int idReporte) {
        this.idReporte = idReporte;
    }

    public Date getFechaGeneracion() {
        return fechaGeneracion;
    }

    public void setFechaGeneracion(Date fechaGeneracion) {
        this.fechaGeneracion = fechaGeneracion;
    }

    public Date getFechaInicioFiltro() {
        return fechaInicioFiltro;
    }

    public void setFechaInicioFiltro(Date fechaInicioFiltro) {
        this.fechaInicioFiltro = fechaInicioFiltro;
    }

    public Date getFechaFinFiltro() {
        return fechaFinFiltro;
    }

    public void setFechaFinFiltro(Date fechaFinFiltro) {
        this.fechaFinFiltro = fechaFinFiltro;
    }
}

package pe.edu.pucp.proyectopro3.modelo.reportes;

import pe.edu.pucp.proyectopro3.modelo.reclamos.Reclamo;

import java.util.Date;
import java.util.List;

public class ReporteReclamo extends Reporte{
    private List<Reclamo> detalleReclamos;
    private int cantidadReservas;
    private int cantidadReclamos;
    private double porcentajeIncidencias;
    private int totalProcede;
    private int totalNoProcede;
    private int totalPendientes;

    public ReporteReclamo() {

    }

    public List<Reclamo> getDetalleReclamos() {
        return detalleReclamos;
    }

    public void setDetalleReclamos(List<Reclamo> detalleReclamos) {
        this.detalleReclamos = detalleReclamos;
    }

    public int getCantidadReservas() {
        return cantidadReservas;
    }

    public void setCantidadReservas(int cantidadReservas) {
        this.cantidadReservas = cantidadReservas;
    }

    public int getCantidadReclamos() {
        return cantidadReclamos;
    }

    public void setCantidadReclamos(int cantidadReclamos) {
        this.cantidadReclamos = cantidadReclamos;
    }

    public double getPorcentajeIncidencias() {
        return porcentajeIncidencias;
    }

    public void setPorcentajeIncidencias(double porcentajeIncidencias) {
        this.porcentajeIncidencias = porcentajeIncidencias;
    }

    public int getTotalProcede() {
        return totalProcede;
    }

    public void setTotalProcede(int totalProcede) {
        this.totalProcede = totalProcede;
    }

    public int getTotalNoProcede() {
        return totalNoProcede;
    }

    public void setTotalNoProcede(int totalNoProcede) {
        this.totalNoProcede = totalNoProcede;
    }

    public int getTotalPendientes() {
        return totalPendientes;
    }

    public void setTotalPendientes(int totalPendientes) {
        this.totalPendientes = totalPendientes;
    }

    public ReporteReclamo(int idReporte, Date fechaGeneracion, Date fechaInicioFiltro, Date fechaFinFiltro, List<Reclamo> detalleReclamos, int cantidadReservas, int cantidadReclamos, double porcentajeIncidencias, int totalProcede, int totalNoProcede, int totalPendientes) {
        super(idReporte, fechaGeneracion, fechaInicioFiltro, fechaFinFiltro);
        this.detalleReclamos = detalleReclamos;
        this.cantidadReservas = cantidadReservas;
        this.cantidadReclamos = cantidadReclamos;
        this.porcentajeIncidencias = porcentajeIncidencias;
        this.totalProcede = totalProcede;
        this.totalNoProcede = totalNoProcede;
        this.totalPendientes = totalPendientes;



    }
}

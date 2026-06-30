package pe.edu.pucp.proyectopro3.modelo.reportes;

import pe.edu.pucp.proyectopro3.modelo.reservas.Reserva;

import java.util.Date;
import java.util.List;

public class ReporteVentas extends Reporte {
    private List<Reserva> detalleVentas;
    private int totalVentas;
    private double montoTotalGenerado;

    public ReporteVentas(int idReporte, Date fechaGeneracion, Date fechaInicioFiltro, Date fechaFinFiltro, List<Reserva> detalleVentas, int totalVentas, double montoTotalGenerado) {
        super(idReporte, fechaGeneracion, fechaInicioFiltro, fechaFinFiltro);
        this.detalleVentas = detalleVentas;
        this.totalVentas = totalVentas;
        this.montoTotalGenerado = montoTotalGenerado;
    }

    public ReporteVentas() {
        super();
    }

    public List<Reserva> getDetalleVentas() {
        return detalleVentas;
    }

    public void setDetalleVentas(List<Reserva> detalleVentas) {
        this.detalleVentas = detalleVentas;
    }

    public int getTotalVentas() {
        return totalVentas;
    }

    public void setTotalVentas(int totalVentas) {
        this.totalVentas = totalVentas;
    }

    public double getMontoTotalGenerado() {
        return montoTotalGenerado;
    }

    public void setMontoTotalGenerado(double montoTotalGenerado) {
        this.montoTotalGenerado = montoTotalGenerado;
    }
}

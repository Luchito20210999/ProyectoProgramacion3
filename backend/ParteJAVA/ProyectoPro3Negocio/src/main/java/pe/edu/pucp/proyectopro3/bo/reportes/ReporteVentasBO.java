package pe.edu.pucp.proyectopro3.bo.reportes;

import pe.edu.pucp.proyectopro3.bo.Gestionable;
import pe.edu.pucp.proyectopro3.modelo.reportes.Reporte;
import pe.edu.pucp.proyectopro3.modelo.reportes.ReporteVentas;

import java.util.Date;

public interface ReporteVentasBO extends Gestionable<ReporteVentas> {
    Reporte generarReporte(Date fechaInicio, Date fechaFin);
    void exportarDashboard();
}

package pe.edu.pucp.proyectopro3.bo.reportes;

import pe.edu.pucp.proyectopro3.bo.Gestionable;
import pe.edu.pucp.proyectopro3.modelo.reportes.Reporte;
import pe.edu.pucp.proyectopro3.modelo.reportes.ReporteReclamo;

import java.util.Date;

public interface ReporteReclamoBO extends Gestionable<ReporteReclamo> {
    Reporte generarReporte(Date fechaInicio, Date fechaFin);
    void exportarDashboard();
}

package pe.edu.pucp.proyectopro3.modelo.reportes;

import java.util.Date;

public interface Consultable {
    Reporte generarReporte(Date fechaInicio, Date fechaFin);
    void exportarDashboard();
}

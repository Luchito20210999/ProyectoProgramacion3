package pe.edu.pucp.proyectopro3.app.pruebas.reportes;

import pe.edu.pucp.proyectopro3.bo.reportes.ReporteVentasBO;
import pe.edu.pucp.proyectopro3.bo.reportes.ReporteVentasBOImpl;
import pe.edu.pucp.proyectopro3.bo.reportes.ReporteReclamoBO;
import pe.edu.pucp.proyectopro3.bo.reportes.ReporteReclamoBOImpl;
import pe.edu.pucp.proyectopro3.modelo.reportes.Reporte;
import pe.edu.pucp.proyectopro3.modelo.reportes.ReporteVentas;
import pe.edu.pucp.proyectopro3.modelo.reportes.ReporteReclamo;

import java.util.Calendar;
import java.util.Date;
import java.util.List;

public class ReportePrueba {

    public static void ejecutar() {
        System.out.println("========== PRUEBA: ReporteVentasBO ==========");
        ejecutarReporteVentas();
        System.out.println("========== PRUEBA: ReporteReclamoBO ==========");
        ejecutarReporteReclamo();
    }

    private static void ejecutarReporteVentas() {
        ReporteVentasBO ventasBO = new ReporteVentasBOImpl();

        System.out.println("[LISTAR] Consultando reportes de ventas...");
        List<ReporteVentas> reportes = ventasBO.listar();
        System.out.println("  Total encontrados: " + reportes.size());

        System.out.println("[GENERAR] Generando reporte de ventas...");
        Calendar cal = Calendar.getInstance();
        cal.add(Calendar.MONTH, -1);
        Date fechaInicio = cal.getTime();
        Date fechaFin = new Date();

        Reporte reporte = ventasBO.generarReporte(fechaInicio, fechaFin);
        if (reporte instanceof ReporteVentas rv) {
            System.out.println("  Total ventas: " + rv.getTotalVentas());
            System.out.println("  Monto total: " + rv.getMontoTotalGenerado());
            System.out.println("  ID reporte: " + rv.getIdReporte());
        }

        System.out.println("[ELIMINAR] Eliminando reporte generado...");
        ventasBO.eliminar(reporte.getIdReporte());
        System.out.println("  Eliminado correctamente.");
        System.out.println();
    }

    private static void ejecutarReporteReclamo() {
        ReporteReclamoBO reclamoBO = new ReporteReclamoBOImpl();

        System.out.println("[LISTAR] Consultando reportes de reclamos...");
        List<ReporteReclamo> reportes = reclamoBO.listar();
        System.out.println("  Total encontrados: " + reportes.size());

        System.out.println("[GENERAR] Generando reporte de reclamos...");
        Calendar cal = Calendar.getInstance();
        cal.add(Calendar.MONTH, -1);
        Date fechaInicio = cal.getTime();
        Date fechaFin = new Date();

        Reporte reporte = reclamoBO.generarReporte(fechaInicio, fechaFin);
        if (reporte instanceof ReporteReclamo rr) {
            System.out.println("  Cantidad reclamos: " + rr.getCantidadReclamos());
            System.out.println("  % incidencias: " + rr.getPorcentajeIncidencias());
            System.out.println("  Procede: " + rr.getTotalProcede());
            System.out.println("  No procede: " + rr.getTotalNoProcede());
            System.out.println("  Pendientes: " + rr.getTotalPendientes());
        }

        System.out.println("[ELIMINAR] Eliminando reporte generado...");
        reclamoBO.eliminar(reporte.getIdReporte());
        System.out.println("  Eliminado correctamente.");
        System.out.println("========== FIN: Reportes ==========\n");
    }
}

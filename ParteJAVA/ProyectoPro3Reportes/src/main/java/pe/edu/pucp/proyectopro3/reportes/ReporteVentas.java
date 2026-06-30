package pe.edu.pucp.proyectopro3.reportes;

import java.io.IOException;
import jakarta.servlet.ServletException;
import jakarta.servlet.annotation.WebServlet;
import jakarta.servlet.http.HttpServlet;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import java.io.FileNotFoundException;
import java.io.InputStream;
import java.sql.Connection;
import java.sql.SQLException;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.Map;
import net.sf.jasperreports.engine.JRException;
import net.sf.jasperreports.engine.JasperExportManager;
import net.sf.jasperreports.engine.JasperFillManager;
import net.sf.jasperreports.engine.JasperPrint;

import pe.edu.pucp.proyectopro3.db.DBFactoryProvider;

@WebServlet(name = "ReporteVentas",urlPatterns = {"/reportes/ventas"})
public class ReporteVentas extends HttpServlet {
    private final String NOMBRE_REPORTE = "Reportes/Reporte_Ventas.jasper";

    protected void processRequest(HttpServletRequest request, HttpServletResponse response)
        throws IOException {

        response.setContentType("application/pdf");

        InputStream reporte = getClass().getClassLoader().getResourceAsStream(NOMBRE_REPORTE);

        if (reporte == null) {
            throw new FileNotFoundException("No se encontro el reporte: " + NOMBRE_REPORTE);
        }

        Map<String, Object> parametros = new HashMap<>();

        Date inicio = obtenerFecha(request.getParameter("fechaInicio"), "2000-01-01");
        Date fin = finDelDia(obtenerFecha(request.getParameter("fechaFin"), "2099-12-31"));
        String servicio = limpiar(request.getParameter("servicio"));
        String canal = limpiar(request.getParameter("canal"));

        parametros.put("P_FECHA_INICIO", inicio);
        parametros.put("P_FECHA_FIN", fin);
        parametros.put("P_NOMBRE_SERVICIO", servicio);
        parametros.put("P_ESTADO", servicio);
        parametros.put("P_CANAL", canal);

        try (Connection conn = DBFactoryProvider.getManager().getConnection()) {
            JasperPrint jp = JasperFillManager.fillReport(reporte, parametros, conn);
            JasperExportManager.exportReportToPdfStream(jp, response.getOutputStream());
        }
        catch (SQLException | ClassNotFoundException | JRException ex) {
            response.sendError(HttpServletResponse.SC_INTERNAL_SERVER_ERROR,
                    "Error al generar el reporte: " + ex.getMessage());
        }
    }

    private Date obtenerFecha(String valor, String respaldo) {
        SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd");
        try {
            return sdf.parse(valor == null || valor.isBlank() ? respaldo : valor);
        } catch (ParseException ex) {
            try {
                return sdf.parse(respaldo);
            } catch (ParseException ignored) {
                return new Date();
            }
        }
    }

    private String limpiar(String valor) {
        return valor == null ? "" : valor.trim();
    }

    private Date finDelDia(Date fecha) {
        return new Date(fecha.getTime() + 86_399_999L);
    }

    @Override
    protected void doGet(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        processRequest(request, response);
    }

    @Override
    public String getServletInfo() {
        return "Servlet para generar el reporte de venta";
    }
}

package pe.edu.pucp.proyectopro3.reportes;

import com.itextpdf.text.BaseColor;
import com.itextpdf.text.Chunk;
import com.itextpdf.text.Document;
import com.itextpdf.text.DocumentException;
import com.itextpdf.text.Element;
import com.itextpdf.text.Font;
import com.itextpdf.text.PageSize;
import com.itextpdf.text.Paragraph;
import com.itextpdf.text.Phrase;
import com.itextpdf.text.pdf.PdfPCell;
import com.itextpdf.text.pdf.PdfPTable;
import com.itextpdf.text.pdf.PdfWriter;
import jakarta.servlet.ServletException;
import jakarta.servlet.annotation.WebServlet;
import jakarta.servlet.http.HttpServlet;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import pe.edu.pucp.proyectopro3.db.DBFactoryProvider;

import java.io.IOException;
import java.sql.Connection;
import java.sql.Date;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.text.ParseException;
import java.text.SimpleDateFormat;

@WebServlet(name = "ReporteCalidad", urlPatterns = {"/reportes/calidad"})
public class ReporteCalidad extends HttpServlet {
    private static final Font TITULO = new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD);
    private static final Font SUBTITULO = new Font(Font.FontFamily.HELVETICA, 11, Font.NORMAL);
    private static final Font CABECERA = new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.WHITE);
    private static final Font TEXTO = new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL);
    private static final BaseColor AZUL = new BaseColor(14, 165, 233);

    protected void processRequest(HttpServletRequest request, HttpServletResponse response)
            throws IOException {
        response.setContentType("application/pdf");
        response.setHeader("Content-Disposition", "inline; filename=Reporte_Calidad.pdf");

        Date fechaInicio = obtenerFecha(request.getParameter("fechaInicio"), "2000-01-01");
        Date fechaFin = obtenerFecha(request.getParameter("fechaFin"), "2099-12-31");
        String servicio = limpiar(request.getParameter("servicio"));

        try (Connection conn = DBFactoryProvider.getManager().getConnection()) {
            DatosResumen resumen = obtenerResumen(conn, fechaInicio, fechaFin, servicio);
            generarPdf(response, conn, fechaInicio, fechaFin, servicio, resumen);
        } catch (SQLException | ClassNotFoundException | DocumentException ex) {
            response.sendError(HttpServletResponse.SC_INTERNAL_SERVER_ERROR,
                    "Error al generar el reporte de calidad: " + ex.getMessage());
        }
    }

    private void generarPdf(HttpServletResponse response, Connection conn, Date fechaInicio,
            Date fechaFin, String servicio, DatosResumen resumen)
            throws DocumentException, SQLException, IOException {
        Document document = new Document(PageSize.A4.rotate(), 28, 28, 28, 28);
        PdfWriter.getInstance(document, response.getOutputStream());
        document.open();

        document.add(new Paragraph("Reporte de Calidad Operativa", TITULO));
        document.add(new Paragraph("Indicadores de incidencias y atencion de reclamos.", SUBTITULO));
        document.add(new Paragraph("Periodo: " + fechaInicio + " al " + fechaFin
                + " | Servicio: " + (servicio.isBlank() ? "Todos" : servicio), SUBTITULO));
        document.add(Chunk.NEWLINE);

        PdfPTable resumenTabla = new PdfPTable(5);
        resumenTabla.setWidthPercentage(100);
        resumenTabla.setWidths(new float[]{1.2f, 1.2f, 1.3f, 1.2f, 1.2f});
        agregarResumen(resumenTabla, "Reservas", String.valueOf(resumen.cantidadReservas));
        agregarResumen(resumenTabla, "Reclamos", String.valueOf(resumen.cantidadReclamos));
        agregarResumen(resumenTabla, "% Incidencias", String.format("%.2f%%", resumen.porcentajeIncidencias));
        agregarResumen(resumenTabla, "Procede", String.valueOf(resumen.totalProcede));
        agregarResumen(resumenTabla, "Pendientes", String.valueOf(resumen.totalPendientes));
        document.add(resumenTabla);
        document.add(Chunk.NEWLINE);

        PdfPTable detalle = new PdfPTable(7);
        detalle.setWidthPercentage(100);
        detalle.setWidths(new float[]{0.6f, 1.4f, 1.9f, 1.8f, 1.1f, 1.3f, 2.6f});
        agregarCabecera(detalle, "Nro");
        agregarCabecera(detalle, "Reserva");
        agregarCabecera(detalle, "Cliente");
        agregarCabecera(detalle, "Servicio");
        agregarCabecera(detalle, "Fecha");
        agregarCabecera(detalle, "Estado");
        agregarCabecera(detalle, "Resolucion");

        agregarDetalle(conn, detalle, fechaInicio, fechaFin, servicio);
        document.add(detalle);
        document.close();
    }

    private DatosResumen obtenerResumen(Connection conn, Date fechaInicio, Date fechaFin, String servicio)
            throws SQLException {
        DatosResumen resumen = new DatosResumen();
        resumen.cantidadReservas = consultarEntero(conn,
                """
                SELECT COUNT(DISTINCT r.id_reserva)
                FROM Reserva r
                LEFT JOIN Detalle_Reserva dr ON dr.id_reserva = r.id_reserva
                LEFT JOIN Servicio s ON s.id_servicio = dr.id_servicio
                WHERE DATE(r.fecha_registro) BETWEEN ? AND ?
                  AND (? = '' OR s.nombre = ?)
                """, fechaInicio, fechaFin, servicio);

        String sqlReclamos =
                """
                SELECT
                    COUNT(DISTINCT rec.id_reclamo) AS total,
                    SUM(CASE WHEN rec.estado_reclamo = 'PROCEDE' THEN 1 ELSE 0 END) AS procede,
                    SUM(CASE WHEN rec.estado_reclamo = 'NO_PROCEDE' THEN 1 ELSE 0 END) AS no_procede,
                    SUM(CASE WHEN rec.estado_reclamo IN ('PENDIENTE', 'EN_ATENCION') THEN 1 ELSE 0 END) AS pendientes
                FROM Reclamo rec
                INNER JOIN Reserva r ON r.id_reserva = rec.id_reserva
                LEFT JOIN Detalle_Reserva dr ON dr.id_reserva = r.id_reserva
                LEFT JOIN Servicio s ON s.id_servicio = dr.id_servicio
                WHERE DATE(rec.fecha_reclamo) BETWEEN ? AND ?
                  AND (? = '' OR s.nombre = ?)
                """;

        try (PreparedStatement ps = conn.prepareStatement(sqlReclamos)) {
            ps.setDate(1, fechaInicio);
            ps.setDate(2, fechaFin);
            ps.setString(3, servicio);
            ps.setString(4, servicio);
            try (ResultSet rs = ps.executeQuery()) {
                if (rs.next()) {
                    resumen.cantidadReclamos = rs.getInt("total");
                    resumen.totalProcede = rs.getInt("procede");
                    resumen.totalNoProcede = rs.getInt("no_procede");
                    resumen.totalPendientes = rs.getInt("pendientes");
                }
            }
        }

        resumen.porcentajeIncidencias = resumen.cantidadReservas > 0
                ? (resumen.cantidadReclamos * 100.0) / resumen.cantidadReservas
                : 0;
        return resumen;
    }

    private int consultarEntero(Connection conn, String sql, Date fechaInicio, Date fechaFin, String servicio)
            throws SQLException {
        try (PreparedStatement ps = conn.prepareStatement(sql)) {
            ps.setDate(1, fechaInicio);
            ps.setDate(2, fechaFin);
            ps.setString(3, servicio);
            ps.setString(4, servicio);
            try (ResultSet rs = ps.executeQuery()) {
                return rs.next() ? rs.getInt(1) : 0;
            }
        }
    }

    private void agregarDetalle(Connection conn, PdfPTable tabla, Date fechaInicio, Date fechaFin, String servicio)
            throws SQLException {
        String sql =
                """
                SELECT
                    rec.id_reclamo,
                    COALESCE(NULLIF(r.codigo_bokun, ''), CONCAT('RES-', r.id_reserva)) AS codigo_reserva,
                    CONCAT(c.nombres, ' ', c.apellidos) AS cliente,
                    COALESCE(GROUP_CONCAT(DISTINCT s.nombre ORDER BY s.nombre SEPARATOR ', '), 'Sin servicio') AS servicio,
                    DATE(rec.fecha_reclamo) AS fecha_reclamo,
                    rec.estado_reclamo,
                    COALESCE(NULLIF(rec.motivo_resolucion, ''), rec.descripcion) AS resolucion
                FROM Reclamo rec
                INNER JOIN Reserva r ON r.id_reserva = rec.id_reserva
                INNER JOIN Cliente c ON c.id_cliente = r.id_cliente
                LEFT JOIN Detalle_Reserva dr ON dr.id_reserva = r.id_reserva
                LEFT JOIN Servicio s ON s.id_servicio = dr.id_servicio
                WHERE DATE(rec.fecha_reclamo) BETWEEN ? AND ?
                  AND (? = '' OR s.nombre = ?)
                GROUP BY rec.id_reclamo, r.codigo_bokun, r.id_reserva, c.nombres, c.apellidos,
                         rec.fecha_reclamo, rec.estado_reclamo, rec.motivo_resolucion, rec.descripcion
                ORDER BY rec.fecha_reclamo DESC, rec.id_reclamo DESC
                """;

        int nro = 1;
        try (PreparedStatement ps = conn.prepareStatement(sql)) {
            ps.setDate(1, fechaInicio);
            ps.setDate(2, fechaFin);
            ps.setString(3, servicio);
            ps.setString(4, servicio);
            try (ResultSet rs = ps.executeQuery()) {
                while (rs.next()) {
                    agregarCelda(tabla, String.valueOf(nro++));
                    agregarCelda(tabla, rs.getString("codigo_reserva"));
                    agregarCelda(tabla, rs.getString("cliente"));
                    agregarCelda(tabla, rs.getString("servicio"));
                    agregarCelda(tabla, String.valueOf(rs.getDate("fecha_reclamo")));
                    agregarCelda(tabla, rs.getString("estado_reclamo"));
                    agregarCelda(tabla, rs.getString("resolucion"));
                }
            }
        }

        if (nro == 1) {
            PdfPCell celda = new PdfPCell(new Phrase("No se encontraron reclamos para los filtros seleccionados.", TEXTO));
            celda.setColspan(7);
            celda.setPadding(8);
            celda.setHorizontalAlignment(Element.ALIGN_CENTER);
            tabla.addCell(celda);
        }
    }

    private void agregarResumen(PdfPTable tabla, String etiqueta, String valor) {
        PdfPCell celda = new PdfPCell();
        celda.setPadding(8);
        celda.addElement(new Phrase(etiqueta, new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.GRAY)));
        celda.addElement(new Phrase(valor, new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD)));
        tabla.addCell(celda);
    }

    private void agregarCabecera(PdfPTable tabla, String texto) {
        PdfPCell celda = new PdfPCell(new Phrase(texto, CABECERA));
        celda.setBackgroundColor(AZUL);
        celda.setPadding(6);
        tabla.addCell(celda);
    }

    private void agregarCelda(PdfPTable tabla, String texto) {
        PdfPCell celda = new PdfPCell(new Phrase(texto == null ? "" : texto, TEXTO));
        celda.setPadding(5);
        tabla.addCell(celda);
    }

    private Date obtenerFecha(String valor, String respaldo) {
        SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd");
        try {
            return new Date(sdf.parse(valor == null || valor.isBlank() ? respaldo : valor).getTime());
        } catch (ParseException ex) {
            try {
                return new Date(sdf.parse(respaldo).getTime());
            } catch (ParseException ignored) {
                return new Date(System.currentTimeMillis());
            }
        }
    }

    private String limpiar(String valor) {
        return valor == null ? "" : valor.trim();
    }

    @Override
    protected void doGet(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        processRequest(request, response);
    }

    @Override
    public String getServletInfo() {
        return "Servlet para generar el reporte de calidad";
    }

    private static class DatosResumen {
        int cantidadReservas;
        int cantidadReclamos;
        double porcentajeIncidencias;
        int totalProcede;
        int totalNoProcede;
        int totalPendientes;
    }
}

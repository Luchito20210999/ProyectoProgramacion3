package pe.edu.pucp.proyectopro3.dao.reportes;

import pe.edu.pucp.proyectopro3.dao.DefaultBaseDAO;
import pe.edu.pucp.proyectopro3.modelo.reportes.ReporteReclamo;

import java.sql.*;

public class ReporteReclamoDAOImpl extends DefaultBaseDAO<ReporteReclamo> implements ReporteReclamoDAO {

    @Override
    protected PreparedStatement comandoCrear(Connection conn, ReporteReclamo modelo) throws SQLException {

        String sql = "{call sp_InsertReporteReclamo(?, ?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setDate(1, new java.sql.Date(modelo.getFechaGeneracion().getTime()));
        cmd.setDate(2, new java.sql.Date(modelo.getFechaInicioFiltro().getTime()));
        cmd.setDate(3, new java.sql.Date(modelo.getFechaFinFiltro().getTime()));
        cmd.setInt(4, modelo.getCantidadReclamos());
        cmd.setDouble(5, modelo.getPorcentajeIncidencias());
        cmd.setInt(6, modelo.getTotalProcede());
        cmd.setInt(7, modelo.getTotalNoProcede());
        cmd.setInt(8, modelo.getTotalPendientes());

        return cmd;
    }

    @Override
    protected PreparedStatement comandoActualizar(Connection conn, ReporteReclamo modelo) throws SQLException {
        // SQL directo para actualizar todos los campos incluyendo el nuevo
        String sql = "UPDATE ReporteReclamo SET fechaGeneracion=?, fechaInicioFiltro=?, fechaFinFiltro=?, " +
                "cantidadReclamos=?, porcentajeAtendidos=?, totalProcedentes=?, totalNoProcedentes=?, " +
                "totalPendientes=? WHERE idReporteReclamo=?";
        PreparedStatement cmd = conn.prepareStatement(sql);

        cmd.setDate(1, new java.sql.Date(modelo.getFechaGeneracion().getTime()));
        cmd.setDate(2, new java.sql.Date(modelo.getFechaInicioFiltro().getTime()));
        cmd.setDate(3, new java.sql.Date(modelo.getFechaFinFiltro().getTime()));
        cmd.setInt(4, modelo.getCantidadReclamos());
        cmd.setDouble(5, modelo.getPorcentajeIncidencias());
        cmd.setInt(6, modelo.getTotalProcede());
        cmd.setInt(7, modelo.getTotalNoProcede());
        cmd.setInt(8, modelo.getTotalPendientes()); // <-- Campo agregado
        cmd.setInt(9, modelo.getIdReporte());

        return cmd;
    }

    @Override
    protected PreparedStatement comandoEliminar(Connection conn, Integer id) throws SQLException {
        String sql = "DELETE FROM ReporteReclamo WHERE idReporteReclamo = ?";
        PreparedStatement cmd = conn.prepareStatement(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeer(Connection conn, Integer id) throws SQLException {
        String sql = "SELECT * FROM ReporteReclamo WHERE idReporteReclamo = ?";
        PreparedStatement cmd = conn.prepareStatement(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeerTodos(Connection conn) throws SQLException {
        String sql = "{call sp_ListReporteReclamo()}";
        return conn.prepareCall(sql);
    }

    @Override
    protected ReporteReclamo mapearModelo(ResultSet rs) throws SQLException {
        ReporteReclamo reporte = new ReporteReclamo();

        reporte.setIdReporte(rs.getInt("idReporteReclamo"));
        reporte.setFechaGeneracion(rs.getDate("fechaGeneracion"));
        reporte.setFechaInicioFiltro(rs.getDate("fechaInicioFiltro"));
        reporte.setFechaFinFiltro(rs.getDate("fechaFinFiltro"));
        reporte.setCantidadReclamos(rs.getInt("cantidadReclamos"));
        reporte.setPorcentajeIncidencias(rs.getDouble("porcentajeAtendidos"));
        reporte.setTotalProcede(rs.getInt("totalProcedentes"));
        reporte.setTotalNoProcede(rs.getInt("totalNoProcedentes"));
        reporte.setTotalPendientes(rs.getInt("totalPendientes")); // <-- Campo agregado

        return reporte;
    }

}

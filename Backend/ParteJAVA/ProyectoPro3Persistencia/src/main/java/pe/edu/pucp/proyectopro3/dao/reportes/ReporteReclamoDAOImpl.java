package pe.edu.pucp.proyectopro3.dao.reportes;

import pe.edu.pucp.proyectopro3.dao.DefaultBaseDAO;
import pe.edu.pucp.proyectopro3.modelo.reportes.ReporteReclamo;

import java.sql.*;

public class ReporteReclamoDAOImpl extends DefaultBaseDAO<ReporteReclamo> implements ReporteReclamoDAO {
    private static final int DEFAULT_ID_USUARIO = 1;

    @Override
    protected PreparedStatement comandoCrear(Connection conn, ReporteReclamo modelo) throws SQLException {
        String sql = "{call sp_InsertReporte_Reclamo(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setDate(1, new java.sql.Date(modelo.getFechaGeneracion().getTime()));
        cmd.setDate(2, new java.sql.Date(modelo.getFechaInicioFiltro().getTime()));
        cmd.setDate(3, new java.sql.Date(modelo.getFechaFinFiltro().getTime()));
        cmd.setInt(4, modelo.getCantidadReservas());
        cmd.setInt(5, modelo.getCantidadReclamos());
        cmd.setDouble(6, modelo.getPorcentajeIncidencias());
        cmd.setInt(7, modelo.getTotalProcede());
        cmd.setInt(8, modelo.getTotalNoProcede());
        cmd.setInt(9, modelo.getTotalPendientes());
        cmd.setInt(10, DEFAULT_ID_USUARIO);
        cmd.registerOutParameter(11, Types.INTEGER);

        return cmd;
    }

    @Override
    protected PreparedStatement comandoActualizar(Connection conn, ReporteReclamo modelo) throws SQLException {
        String sql = "UPDATE Reporte_Reclamo SET fecha_generacion=?, fecha_inicio_filtro=?, "
                + "fecha_fin_filtro=?, cantidad_reservas=?, cantidad_reclamos=?, "
                + "porcentaje_incidencias=?, total_procede=?, total_no_procede=?, "
                + "total_pendientes=? WHERE id_reporte_reclamo=?";
        PreparedStatement cmd = conn.prepareStatement(sql);

        cmd.setDate(1, new java.sql.Date(modelo.getFechaGeneracion().getTime()));
        cmd.setDate(2, new java.sql.Date(modelo.getFechaInicioFiltro().getTime()));
        cmd.setDate(3, new java.sql.Date(modelo.getFechaFinFiltro().getTime()));
        cmd.setInt(4, modelo.getCantidadReservas());
        cmd.setInt(5, modelo.getCantidadReclamos());
        cmd.setDouble(6, modelo.getPorcentajeIncidencias());
        cmd.setInt(7, modelo.getTotalProcede());
        cmd.setInt(8, modelo.getTotalNoProcede());
        cmd.setInt(9, modelo.getTotalPendientes());
        cmd.setInt(10, modelo.getIdReporte());

        return cmd;
    }

    @Override
    protected PreparedStatement comandoEliminar(Connection conn, Integer id) throws SQLException {
        String sql = "{call sp_DeleteReporte_Reclamo(?)}";
        CallableStatement cmd = conn.prepareCall(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeer(Connection conn, Integer id) throws SQLException {
        String sql = "{call sp_ListReporteReclamoById(?)}";
        CallableStatement cmd = conn.prepareCall(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeerTodos(Connection conn) throws SQLException {
        String sql = "{call spListReporteReclamo()}";
        return conn.prepareCall(sql);
    }

    @Override
    protected Integer extraerIdDesdeCallable(CallableStatement cmd) throws SQLException {
        return cmd.getInt(11);
    }

    @Override
    protected Integer extraerIdDesdeGeneratedKeys(ResultSet rs) throws SQLException {
        return rs.getInt(1);
    }

    @Override
    protected ReporteReclamo mapearModelo(ResultSet rs) throws SQLException {
        ReporteReclamo reporte = new ReporteReclamo();

        reporte.setIdReporte(rs.getInt("id_reporte_reclamo"));
        reporte.setFechaGeneracion(rs.getDate("fecha_generacion"));
        reporte.setFechaInicioFiltro(rs.getDate("fecha_inicio_filtro"));
        reporte.setFechaFinFiltro(rs.getDate("fecha_fin_filtro"));
        reporte.setCantidadReservas(rs.getInt("cantidad_reservas"));
        reporte.setCantidadReclamos(rs.getInt("cantidad_reclamos"));
        reporte.setPorcentajeIncidencias(rs.getDouble("porcentaje_incidencias"));
        reporte.setTotalProcede(rs.getInt("total_procede"));
        reporte.setTotalNoProcede(rs.getInt("total_no_procede"));
        reporte.setTotalPendientes(rs.getInt("total_pendientes"));

        return reporte;
    }
}

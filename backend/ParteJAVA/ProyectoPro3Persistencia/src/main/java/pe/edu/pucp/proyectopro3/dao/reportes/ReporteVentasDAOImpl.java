package pe.edu.pucp.proyectopro3.dao.reportes;

import pe.edu.pucp.proyectopro3.dao.DefaultBaseDAO;
import pe.edu.pucp.proyectopro3.modelo.reportes.ReporteVentas;

import java.sql.*;

public class ReporteVentasDAOImpl extends DefaultBaseDAO<ReporteVentas> implements ReporteVentasDAO {
    private static final int DEFAULT_ID_USUARIO = 1;

    @Override
    protected PreparedStatement comandoCrear(Connection conn, ReporteVentas modelo) throws SQLException {
        String sql = "{call sp_InsertReporte_Ventas(?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setDate(1, new java.sql.Date(modelo.getFechaGeneracion().getTime()));
        cmd.setDate(2, new java.sql.Date(modelo.getFechaInicioFiltro().getTime()));
        cmd.setDate(3, new java.sql.Date(modelo.getFechaFinFiltro().getTime()));
        cmd.setInt(4, modelo.getTotalVentas());
        cmd.setDouble(5, modelo.getMontoTotalGenerado());
        cmd.setInt(6, DEFAULT_ID_USUARIO);
        cmd.registerOutParameter(7, Types.INTEGER);

        return cmd;
    }

    @Override
    protected PreparedStatement comandoActualizar(Connection conn, ReporteVentas modelo) throws SQLException {
        String sql = "UPDATE Reporte_Ventas SET fecha_generacion=?, fecha_inicio_filtro=?, "
                + "fecha_fin_filtro=?, cantidad_registros=?, monto_total=? WHERE id_reporte_ventas=?";
        PreparedStatement cmd = conn.prepareStatement(sql);

        cmd.setDate(1, new java.sql.Date(modelo.getFechaGeneracion().getTime()));
        cmd.setDate(2, new java.sql.Date(modelo.getFechaInicioFiltro().getTime()));
        cmd.setDate(3, new java.sql.Date(modelo.getFechaFinFiltro().getTime()));
        cmd.setInt(4, modelo.getTotalVentas());
        cmd.setDouble(5, modelo.getMontoTotalGenerado());
        cmd.setInt(6, modelo.getIdReporte());

        return cmd;
    }

    @Override
    protected PreparedStatement comandoEliminar(Connection conn, Integer id) throws SQLException {
        String sql = "{call sp_DeleteReporte_Ventas(?)}";
        CallableStatement cmd = conn.prepareCall(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeer(Connection conn, Integer id) throws SQLException {
        String sql = "{call sp_ListReporteVentasById(?)}";
        CallableStatement cmd = conn.prepareCall(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeerTodos(Connection conn) throws SQLException {
        String sql = "{call spListReporteVentas()}";
        return conn.prepareCall(sql);
    }

    @Override
    protected Integer extraerIdDesdeCallable(CallableStatement cmd) throws SQLException {
        return cmd.getInt(7);
    }

    @Override
    protected Integer extraerIdDesdeGeneratedKeys(ResultSet rs) throws SQLException {
        return rs.getInt(1);
    }

    @Override
    protected ReporteVentas mapearModelo(ResultSet rs) throws SQLException {
        ReporteVentas reporte = new ReporteVentas();

        reporte.setIdReporte(rs.getInt("id_reporte_ventas"));
        reporte.setFechaGeneracion(rs.getDate("fecha_generacion"));
        reporte.setFechaInicioFiltro(rs.getDate("fecha_inicio_filtro"));
        reporte.setFechaFinFiltro(rs.getDate("fecha_fin_filtro"));
        reporte.setTotalVentas(rs.getInt("cantidad_registros"));
        reporte.setMontoTotalGenerado(rs.getDouble("monto_total"));

        return reporte;
    }
}

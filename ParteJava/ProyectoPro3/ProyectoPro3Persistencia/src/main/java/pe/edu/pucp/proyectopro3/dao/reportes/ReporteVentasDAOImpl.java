package pe.edu.pucp.proyectopro3.dao.reportes;

import pe.edu.pucp.proyectopro3.dao.DefaultBaseDAO;
import pe.edu.pucp.proyectopro3.modelo.reportes.ReporteVentas;

import java.sql.*;


public class ReporteVentasDAOImpl extends DefaultBaseDAO<ReporteVentas> implements ReporteVentasDAO {

    @Override
    protected PreparedStatement comandoCrear(Connection conn, ReporteVentas modelo) throws SQLException {
        // Procedimiento: sp_InsertReporteVentas(_fGen, _fIni, _fFin, _tVen, _mTot)
        String sql = "{call sp_InsertReporteVentas(?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setDate(1, new java.sql.Date(modelo.getFechaGeneracion().getTime()));
        cmd.setDate(2, new java.sql.Date(modelo.getFechaInicioFiltro().getTime()));
        cmd.setDate(3, new java.sql.Date(modelo.getFechaFinFiltro().getTime()));
        cmd.setInt(4, modelo.getTotalVentas());
        cmd.setDouble(5, modelo.getMontoTotalGenerado());

        return cmd;
    }

    @Override
    protected PreparedStatement comandoActualizar(Connection conn, ReporteVentas modelo) throws SQLException {
        // Los reportes suelen ser históricos y no modificables, pero el framework requiere el método [cite: 40]
        String sql = "UPDATE ReporteVentas SET fechaGeneracion=?, fechaInicioFiltro=?, " +
                "fechaFinFiltro=?, totalVentas=?, montoTotalGenerado=? WHERE idReporteVentas=?";
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
        // [cite: 42]
        String sql = "DELETE FROM ReporteVentas WHERE idReporteVentas = ?";
        PreparedStatement cmd = conn.prepareStatement(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeer(Connection conn, Integer id) throws SQLException {
        // Búsqueda individual por ID [cite: 44]
        String sql = "SELECT * FROM ReporteVentas WHERE idReporteVentas = ?";
        PreparedStatement cmd = conn.prepareStatement(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeerTodos(Connection conn) throws SQLException {
        // Procedimiento: sp_ListReporteVentas()
        String sql = "{call sp_ListReporteVentas()}";
        return conn.prepareCall(sql);
    }

    @Override
    protected ReporteVentas mapearModelo(ResultSet rs) throws SQLException {
        // Mapeo de columnas según la tabla ReporteVentas
        ReporteVentas reporte = new ReporteVentas();

        reporte.setIdReporte(rs.getInt("idReporteVentas"));
        reporte.setFechaGeneracion(rs.getDate("fechaGeneracion"));
        reporte.setFechaInicioFiltro(rs.getDate("fechaInicioFiltro"));
        reporte.setFechaFinFiltro(rs.getDate("fechaFinFiltro"));
        reporte.setTotalVentas(rs.getInt("totalVentas"));
        reporte.setMontoTotalGenerado(rs.getDouble("montoTotalGenerado"));

        return reporte;
    }
}

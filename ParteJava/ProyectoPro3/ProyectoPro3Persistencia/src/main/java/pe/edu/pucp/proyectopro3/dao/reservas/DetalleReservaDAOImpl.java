package pe.edu.pucp.proyectopro3.dao.reservas;

import pe.edu.pucp.proyectopro3.dao.DefaultBaseDAO;
import pe.edu.pucp.proyectopro3.modelo.reservas.DetalleReserva;
import java.sql.*;

public class DetalleReservaDAOImpl extends DefaultBaseDAO<DetalleReserva> implements DetalleReservaDAO {

    @Override
    protected PreparedStatement comandoCrear(Connection conn, DetalleReserva modelo) throws SQLException {
        String sql = "{call sp_InsertDetalleReserva(?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setInt(1, modelo.getIdReserva());
        cmd.setInt(2, modelo.getIdServicio());
        cmd.setInt(3, modelo.getCantidad());
        cmd.setDouble(4, modelo.getSubtotal());

        return cmd;
    }

    @Override
    protected PreparedStatement comandoActualizar(Connection conn, DetalleReserva modelo) throws SQLException {
        String sql = "{call sp_UpdateDetalleReserva(?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setInt(1, modelo.getIdDetalle());
        cmd.setInt(2, modelo.getIdReserva());
        cmd.setInt(3, modelo.getIdServicio());
        cmd.setInt(4, modelo.getCantidad());
        cmd.setDouble(5, modelo.getSubtotal());

        return cmd;
    }

    @Override
    protected PreparedStatement comandoEliminar(Connection conn, Integer id) throws SQLException {
        String sql = "{call sp_DeleteDetalleReserva(?)}";
        CallableStatement cmd = conn.prepareCall(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeer(Connection conn, Integer id) throws SQLException {
        // Búsqueda por ID del detalle
        String sql = "SELECT * FROM DetalleReserva WHERE idDetalleReserva = ?";
        PreparedStatement cmd = conn.prepareStatement(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeerTodos(Connection conn) throws SQLException {
        String sql = "{call sp_ListDetalleReserva()}";
        return conn.prepareCall(sql);
    }

    @Override
    protected DetalleReserva mapearModelo(ResultSet rs) throws SQLException {
        DetalleReserva detalle = new DetalleReserva();

        detalle.setIdDetalle(rs.getInt("idDetalleReserva"));
        detalle.setIdReserva(rs.getInt("idReserva"));
        detalle.setIdServicio(rs.getInt("idServicio"));
        detalle.setCantidad(rs.getInt("cantidad"));
        detalle.setSubtotal(rs.getDouble("subtotal"));

        return detalle;
    }
}

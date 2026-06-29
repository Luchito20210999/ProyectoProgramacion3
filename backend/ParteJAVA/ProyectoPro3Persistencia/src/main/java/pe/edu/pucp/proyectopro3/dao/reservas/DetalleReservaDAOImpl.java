package pe.edu.pucp.proyectopro3.dao.reservas;

import pe.edu.pucp.proyectopro3.dao.DefaultBaseDAO;
import pe.edu.pucp.proyectopro3.modelo.reservas.DetalleReserva;

import java.sql.*;

public class DetalleReservaDAOImpl extends DefaultBaseDAO<DetalleReserva> implements DetalleReservaDAO {

    @Override
    protected PreparedStatement comandoCrear(Connection conn, DetalleReserva modelo) throws SQLException {
        String sql = "{call sp_InsertDetalle_Reserva(?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        setCamposDetalleReserva(cmd, modelo, 1);
        cmd.registerOutParameter(5, Types.INTEGER);

        return cmd;
    }

    @Override
    protected PreparedStatement comandoActualizar(Connection conn, DetalleReserva modelo) throws SQLException {
        String sql = "{call sp_UpdateDetalle_Reserva(?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setInt(1, modelo.getIdDetalle());
        setCamposDetalleReserva(cmd, modelo, 2);

        return cmd;
    }

    @Override
    protected PreparedStatement comandoEliminar(Connection conn, Integer id) throws SQLException {
        String sql = "{call sp_DeleteDetalle_Reserva(?)}";
        CallableStatement cmd = conn.prepareCall(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeer(Connection conn, Integer id) throws SQLException {
        String sql = "{call sp_ListDetalleReservaById(?)}";
        CallableStatement cmd = conn.prepareCall(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeerTodos(Connection conn) throws SQLException {
        String sql = "{call spListDetalleReserva()}";
        return conn.prepareCall(sql);
    }

    @Override
    protected Integer extraerIdDesdeCallable(CallableStatement cmd) throws SQLException {
        return cmd.getInt(5);
    }

    @Override
    protected Integer extraerIdDesdeGeneratedKeys(ResultSet rs) throws SQLException {
        return rs.getInt(1);
    }

    @Override
    protected DetalleReserva mapearModelo(ResultSet rs) throws SQLException {
        DetalleReserva detalle = new DetalleReserva();

        detalle.setIdDetalle(rs.getInt("id_detalle_reserva"));
        detalle.setIdReserva(rs.getInt("id_reserva"));
        detalle.setIdServicio(rs.getInt("id_servicio"));
        detalle.setCantidad(rs.getInt("cantidad"));
        detalle.setSubtotal(rs.getDouble("subtotal"));

        return detalle;
    }

    private void setCamposDetalleReserva(CallableStatement cmd, DetalleReserva modelo, int inicio) throws SQLException {
        cmd.setInt(inicio, modelo.getIdReserva());
        cmd.setInt(inicio + 1, modelo.getIdServicio());
        cmd.setInt(inicio + 2, modelo.getCantidad());
        cmd.setDouble(inicio + 3, modelo.getSubtotal());
    }
}

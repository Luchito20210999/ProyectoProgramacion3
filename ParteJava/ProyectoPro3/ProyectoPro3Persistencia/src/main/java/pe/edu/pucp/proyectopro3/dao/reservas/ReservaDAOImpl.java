package pe.edu.pucp.proyectopro3.dao.reservas;

import pe.edu.pucp.proyectopro3.dao.DefaultBaseDAO;
import pe.edu.pucp.proyectopro3.modelo.reclamos.EstadoReclamo;
import pe.edu.pucp.proyectopro3.modelo.reservas.EstadoReserva;
import pe.edu.pucp.proyectopro3.modelo.reservas.Reserva;

import java.sql.*;

public class ReservaDAOImpl extends DefaultBaseDAO<Reserva> implements ReservaDAO {

    @Override
    protected PreparedStatement comandoCrear(Connection conn, Reserva modelo) throws SQLException {
        // Procedimiento: sp_InsertReserva(_fReg, _est, _cant, _tot, _fMod, _can, _imp, _idCli)
        String sql = "{call sp_InsertReserva(?, ?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setDate(1, (Date) modelo.getFechaRegistro());
        cmd.setString(2, modelo.getEstadoReserva().name());
        cmd.setInt(3, modelo.getCantidadBoletos());
        cmd.setDouble(4, modelo.getMontoTotal());
        cmd.setDate(5, new java.sql.Date(modelo.getFechaUltimaModificacion().getTime()));
        cmd.setString(6, modelo.getCanalVenta());
        cmd.setDouble(7, modelo.getMontoImpuestos());
        cmd.setInt(8, modelo.getIdCliente());
        return cmd;
    }

    @Override
    protected PreparedStatement comandoActualizar(Connection conn, Reserva modelo) throws SQLException {
        // Procedimiento: sp_UpdateReserva(_id, _fReg, _est, _cant, _tot, _fMod, _can, _imp, _idCli)
        String sql = "{call sp_UpdateReserva(?, ?, ?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setInt(1, modelo.getIdReserva());
        cmd.setDate(2, new java.sql.Date(modelo.getFechaRegistro().getTime()));
        cmd.setString(3, modelo.getEstadoReserva().name());
        cmd.setInt(4, modelo.getCantidadBoletos());
        cmd.setDouble(5, modelo.getMontoTotal());
        cmd.setDate(6, new java.sql.Date(modelo.getFechaUltimaModificacion().getTime()));
        cmd.setString(7, modelo.getCanalVenta());
        cmd.setDouble(8, modelo.getMontoImpuestos());
        cmd.setInt(9, modelo.getIdCliente());

        return cmd;
    }

    @Override
    protected PreparedStatement comandoEliminar(Connection conn, Integer id) throws SQLException {
        String sql = "{call sp_DeleteReserva(?)}";
        CallableStatement cmd = conn.prepareCall(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeer(Connection conn, Integer id) throws SQLException {
        // Búsqueda individual por ID (asumiendo SELECT estándar o un SP de búsqueda)
        String sql = "SELECT * FROM Reserva WHERE idReserva = ?";
        PreparedStatement cmd = conn.prepareStatement(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeerTodos(Connection conn) throws SQLException {
        String sql = "{call sp_ListReservas()}";
        return conn.prepareCall(sql);
    }

    @Override
    protected Reserva mapearModelo(ResultSet rs) throws SQLException {
        Reserva reserva = new Reserva();

        reserva.setIdReserva(rs.getInt("idReserva"));
        reserva.setFechaRegistro(rs.getDate("fechaRegistro"));
        reserva.setEstadoReserva(EstadoReserva.valueOf(rs.getString("estadoReserva")));
        String estadoStr = rs.getString("estadoReserva");
        reserva.setCantidadBoletos(rs.getInt("cantidadBoletos"));
        reserva.setMontoTotal(rs.getDouble("montoTotal"));
        reserva.setFechaUltimaModificacion(rs.getDate("fechaUltimaModificacion"));
        reserva.setCanalVenta(rs.getString("canalVenta"));
        reserva.setMontoImpuestos(rs.getDouble("montoImpuestos"));
        reserva.setIdCliente(rs.getInt("idCliente"));

        return reserva;
    }
}

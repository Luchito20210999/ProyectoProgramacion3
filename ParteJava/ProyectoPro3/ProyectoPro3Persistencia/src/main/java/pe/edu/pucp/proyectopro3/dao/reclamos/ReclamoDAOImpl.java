package pe.edu.pucp.proyectopro3.dao.reclamos;

import pe.edu.pucp.proyectopro3.dao.DefaultBaseDAO;
import pe.edu.pucp.proyectopro3.modelo.reclamos.EstadoReclamo;
import pe.edu.pucp.proyectopro3.modelo.reclamos.Reclamo;

import java.sql.*;

public class ReclamoDAOImpl extends DefaultBaseDAO<Reclamo> implements ReclamoDAO {
    @Override
    protected PreparedStatement comandoCrear(Connection conn, Reclamo modelo) throws SQLException {
        // Usamos el procedimiento sp_InsertReclamo definido en tu SQL [cite: 1]
        String sql = "{call sp_InsertReclamo(?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setDate(1, new java.sql.Date(modelo.getFechaReclamo().getTime()));
        cmd.setString(2, modelo.getDescripcion());
        cmd.setString(3, modelo.getEstadoReclamo().name());
        cmd.setString(4, modelo.getMotivoResolucion());
        cmd.setDate(5, new java.sql.Date(modelo.getFechaResolucion().getTime()));
        cmd.setInt(6,modelo.getIdReserva());

        return cmd;
    }

    @Override
    protected PreparedStatement comandoActualizar(Connection conn, Reclamo modelo) throws SQLException {
        // Usamos sp_UpdateReclamo [cite: 1]
        String sql = "{call sp_UpdateReclamo(?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setInt(1, modelo.getIdReclamo());
        cmd.setDate(2, new java.sql.Date(modelo.getFechaReclamo().getTime()));
        cmd.setString(3, modelo.getDescripcion());
        cmd.setString(4, modelo.getEstadoReclamo().name());
        cmd.setString(5, modelo.getMotivoResolucion());
        cmd.setDate(6,  new java.sql.Date(modelo.getFechaResolucion().getTime()));

        return cmd;
    }

    @Override
    protected PreparedStatement comandoEliminar(Connection conn, Integer id) throws SQLException {
        String sql = "{call sp_DeleteReclamo(?)}";
        CallableStatement cmd = conn.prepareCall(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeer(Connection conn, Integer id) throws SQLException {
        // Nota: Asegúrate de tener un SP que busque por ID individualmente
        String sql = "SELECT * FROM Reclamo WHERE idReclamo = ?";
        PreparedStatement cmd = conn.prepareStatement(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeerTodos(Connection conn) throws SQLException {
        String sql = "{call sp_ListReclamos()}";
        return conn.prepareCall(sql);
    }

    @Override
    protected Reclamo mapearModelo(ResultSet rs) throws SQLException {
        Reclamo reclamo = new Reclamo();
        reclamo.setIdReclamo(rs.getInt("idReclamo"));
        reclamo.setFechaReclamo(rs.getDate("fechaReclamo"));
        reclamo.setDescripcion(rs.getString("descripcion"));
        reclamo.setEstadoReclamo(EstadoReclamo.valueOf(rs.getString("estadoReclamo")));
        reclamo.setMotivoResolucion(rs.getString("motivoResolucion"));
        reclamo.setFechaResolucion(rs.getDate("fechaResolucion"));
        reclamo.setIdReserva(rs.getInt("idReserva"));
        return reclamo;
    }
}


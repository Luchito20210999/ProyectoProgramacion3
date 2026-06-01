package pe.edu.pucp.proyectopro3.dao.reclamos;

import pe.edu.pucp.proyectopro3.dao.DefaultBaseDAO;
import pe.edu.pucp.proyectopro3.modelo.reclamos.EstadoReclamo;
import pe.edu.pucp.proyectopro3.modelo.reclamos.Reclamo;

import java.sql.*;
import java.util.Date;

public class ReclamoDAOImpl extends DefaultBaseDAO<Reclamo> implements ReclamoDAO {

    @Override
    protected PreparedStatement comandoCrear(Connection conn, Reclamo modelo) throws SQLException {
        String sql = "{call sp_InsertReclamo(?, ?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        setCamposReclamo(cmd, modelo, 1);
        cmd.registerOutParameter(8, Types.INTEGER);

        return cmd;
    }

    @Override
    protected PreparedStatement comandoActualizar(Connection conn, Reclamo modelo) throws SQLException {
        String sql = "{call sp_UpdateReclamo(?, ?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setInt(1, modelo.getIdReclamo());
        setCamposReclamo(cmd, modelo, 2);

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
        String sql = "{call sp_ListReclamoById(?)}";
        CallableStatement cmd = conn.prepareCall(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeerTodos(Connection conn) throws SQLException {
        String sql = "{call spListReclamos()}";
        return conn.prepareCall(sql);
    }

    @Override
    protected Integer extraerIdDesdeCallable(CallableStatement cmd) throws SQLException {
        return cmd.getInt(8);
    }

    @Override
    protected Integer extraerIdDesdeGeneratedKeys(ResultSet rs) throws SQLException {
        return rs.getInt(1);
    }

    @Override
    protected Reclamo mapearModelo(ResultSet rs) throws SQLException {
        Reclamo reclamo = new Reclamo();

        reclamo.setIdReclamo(rs.getInt("id_reclamo"));
        reclamo.setFechaReclamo(rs.getTimestamp("fecha_reclamo"));
        reclamo.setDescripcion(rs.getString("descripcion"));
        reclamo.setEstadoReclamo(EstadoReclamo.valueOf(rs.getString("estado_reclamo")));
        reclamo.setMotivoResolucion(rs.getString("motivo_resolucion"));
        reclamo.setFechaResolucion(rs.getDate("fecha_resolucion"));
        reclamo.setIdUsuario(rs.getInt("id_usuario"));
        reclamo.setIdReserva(rs.getInt("id_reserva"));

        return reclamo;
    }

    private void setCamposReclamo(CallableStatement cmd, Reclamo modelo, int inicio) throws SQLException {
        setTimestamp(cmd, inicio, modelo.getFechaReclamo());
        cmd.setString(inicio + 1, modelo.getDescripcion());
        cmd.setString(inicio + 2, modelo.getEstadoReclamo().name());
        cmd.setString(inicio + 3, modelo.getMotivoResolucion());
        setDate(cmd, inicio + 4, modelo.getFechaResolucion());
        setIdUsuario(cmd, inicio + 5, modelo.getIdUsuario());
        cmd.setInt(inicio + 6, modelo.getIdReserva());
    }

    private void setTimestamp(CallableStatement cmd, int index, Date value) throws SQLException {
        if (value == null) {
            cmd.setNull(index, Types.TIMESTAMP);
        } else {
            cmd.setTimestamp(index, new Timestamp(value.getTime()));
        }
    }

    private void setDate(CallableStatement cmd, int index, Date value) throws SQLException {
        if (value == null) {
            cmd.setNull(index, Types.DATE);
        } else {
            cmd.setDate(index, new java.sql.Date(value.getTime()));
        }
    }

    private void setIdUsuario(CallableStatement cmd, int index, int idUsuario) throws SQLException {
        if (idUsuario <= 0) {
            cmd.setNull(index, Types.INTEGER);
        } else {
            cmd.setInt(index, idUsuario);
        }
    }
}

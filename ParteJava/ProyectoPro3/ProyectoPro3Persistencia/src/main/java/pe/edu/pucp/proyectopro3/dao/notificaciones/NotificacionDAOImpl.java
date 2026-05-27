package pe.edu.pucp.proyectopro3.dao.notificaciones;

import pe.edu.pucp.proyectopro3.dao.DefaultBaseDAO;
import pe.edu.pucp.proyectopro3.modelo.notificaciones.Notificacion;
import pe.edu.pucp.proyectopro3.modelo.notificaciones.TipoEvento;

import java.sql.*;

public class NotificacionDAOImpl extends DefaultBaseDAO<Notificacion> implements NotificacionDAO {

    @Override
    protected PreparedStatement comandoCrear(Connection conn, Notificacion modelo) throws SQLException {
        String sql = "{call sp_InsertNotificacion(?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setString(1, modelo.getMensaje());
        cmd.setString(2, modelo.getTipoNotificacion());
        cmd.setTimestamp(3, new java.sql.Timestamp(modelo.getFechaEnvio().getTime()));
        cmd.setBoolean(4, modelo.isLeido());
        cmd.setInt(5, modelo.getIdUsuario());
        cmd.setString(6,modelo.getTipoEvento().name());

        return cmd;
    }

    @Override
    protected PreparedStatement comandoActualizar(Connection conn, Notificacion modelo) throws SQLException {
        String sql = "{call sp_UpdateNotificacion(?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setInt(1, modelo.getIdNotificacion());
        cmd.setString(2, modelo.getMensaje());
        cmd.setString(3, modelo.getTipoNotificacion());
        cmd.setTimestamp(4, new java.sql.Timestamp(modelo.getFechaEnvio().getTime()));
        cmd.setBoolean(5, modelo.isLeido());
        cmd.setInt(6, modelo.getIdUsuario());
        cmd.setString(7,modelo.getTipoEvento().name());

        return cmd;
    }

    @Override
    protected PreparedStatement comandoEliminar(Connection conn, Integer id) throws SQLException {
        String sql = "{call sp_DeleteNotificacion(?)}";
        CallableStatement cmd = conn.prepareCall(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeer(Connection conn, Integer id) throws SQLException {
        String sql = "SELECT * FROM Notificacion WHERE idNotificacion = ?";
        PreparedStatement cmd = conn.prepareStatement(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeerTodos(Connection conn) throws SQLException {
        String sql = "{call sp_ListNotificacion()}";
        return conn.prepareCall(sql);
    }

    @Override
    protected Notificacion mapearModelo(ResultSet rs) throws SQLException {
        Notificacion notificacion = new Notificacion();

        notificacion.setIdNotificacion(rs.getInt("id_notificacion"));
        notificacion.setMensaje(rs.getString("mensaje"));
        notificacion.setTipoNotificacion(rs.getString("tipo_notificacion"));
        notificacion.setFechaEnvio(rs.getDate("fecha_envio"));
        notificacion.setLeido(rs.getBoolean("leido"));
        notificacion.setIdUsuario(rs.getInt("id_usuario"));
        notificacion.setTipoEvento(TipoEvento.valueOf(rs.getString("tipo_evento")));

        return notificacion;
    }
}

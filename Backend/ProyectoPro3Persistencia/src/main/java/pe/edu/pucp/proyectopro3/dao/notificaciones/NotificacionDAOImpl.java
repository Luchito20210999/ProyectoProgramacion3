package pe.edu.pucp.proyectopro3.dao.notificaciones;

import pe.edu.pucp.proyectopro3.dao.DefaultBaseDAO;
import pe.edu.pucp.proyectopro3.modelo.notificaciones.Notificacion;

import java.sql.*;

public class NotificacionDAOImpl extends DefaultBaseDAO<Notificacion> implements NotificacionDAO {

    @Override
    protected PreparedStatement comandoCrear(Connection conn, Notificacion modelo) throws SQLException {
        String sql = "{call sp_InsertNotificacion(?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        setCamposNotificacion(cmd, modelo, 1);
        cmd.registerOutParameter(6, Types.INTEGER);

        return cmd;
    }

    @Override
    protected PreparedStatement comandoActualizar(Connection conn, Notificacion modelo) throws SQLException {
        String sql = "{call sp_UpdateNotificacion(?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setInt(1, modelo.getIdNotificacion());
        setCamposNotificacion(cmd, modelo, 2);

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
        String sql = "{call sp_ListNotificacionById(?)}";
        CallableStatement cmd = conn.prepareCall(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeerTodos(Connection conn) throws SQLException {
        String sql = "{call spListNotificaciones()}";
        return conn.prepareCall(sql);
    }

    @Override
    protected Integer extraerIdDesdeCallable(CallableStatement cmd) throws SQLException {
        return cmd.getInt(6);
    }

    @Override
    protected Integer extraerIdDesdeGeneratedKeys(ResultSet rs) throws SQLException {
        return rs.getInt(1);
    }

    @Override
    protected Notificacion mapearModelo(ResultSet rs) throws SQLException {
        Notificacion notificacion = new Notificacion();

        notificacion.setIdNotificacion(rs.getInt("id_notificacion"));
        notificacion.setMensaje(rs.getString("mensaje"));
        notificacion.setTipoNotificacion(rs.getString("tipo_notificacion"));
        notificacion.setFechaEnvio(rs.getTimestamp("fecha_envio"));
        notificacion.setLeido("Y".equalsIgnoreCase(rs.getString("leido")));
        notificacion.setIdUsuario(rs.getInt("id_usuario"));

        return notificacion;
    }

    private void setCamposNotificacion(CallableStatement cmd, Notificacion modelo, int inicio) throws SQLException {
        cmd.setString(inicio, modelo.getMensaje());
        cmd.setString(inicio + 1, obtenerTipoNotificacion(modelo));
        cmd.setTimestamp(inicio + 2, new java.sql.Timestamp(modelo.getFechaEnvio().getTime()));
        cmd.setString(inicio + 3, modelo.isLeido() ? "Y" : "N");
        cmd.setInt(inicio + 4, modelo.getIdUsuario());
    }

    private String obtenerTipoNotificacion(Notificacion modelo) {
        if (modelo.getTipoNotificacion() != null && !modelo.getTipoNotificacion().isBlank()) {
            return modelo.getTipoNotificacion();
        }
        if (modelo.getTipoEvento() != null) {
            return modelo.getTipoEvento().name();
        }
        return "GENERAL";
    }
}

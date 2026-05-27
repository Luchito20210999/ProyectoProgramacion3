package pe.edu.pucp.proyectopro3.dao.auditoria;

import pe.edu.pucp.proyectopro3.dao.DefaultBaseDAO;
import pe.edu.pucp.proyectopro3.modelo.auditoria.LogAuditoria;

import java.sql.*;

public class LogAuditoriaDAOImpl extends DefaultBaseDAO<LogAuditoria> implements LogAuditoriaDAO {

    @Override
    protected PreparedStatement comandoCrear(Connection conn, LogAuditoria modelo) throws SQLException {
        String sql = "{call sp_InsertLogAuditoria(?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);
        cmd.setString(1, modelo.getDescripcion());
        cmd.setString(2, modelo.getAccion());
        cmd.setTimestamp(3, new java.sql.Timestamp(modelo.getFechaRegistro().getTime()));
        cmd.setString(4, modelo.getOrigenAccion());
        cmd.setInt(5, modelo.getIdUsuario());

        return cmd;
    }

    @Override
    protected PreparedStatement comandoActualizar(Connection conn, LogAuditoria modelo) throws SQLException {
        throw new UnsupportedOperationException("No se permite actualizar registros de auditoría.");
    }

    @Override
    protected PreparedStatement comandoEliminar(Connection conn, Integer id) throws SQLException {
        throw new UnsupportedOperationException("No se permite actualizar registros de auditoría.");
    }

    @Override
    protected PreparedStatement comandoLeer(Connection conn, Integer id) throws SQLException {
        String sql = "SELECT * FROM LogAuditoria WHERE idLogAuditoria = ?";
        PreparedStatement cmd = conn.prepareStatement(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeerTodos(Connection conn) throws SQLException {
        // sp_ListLogAuditoria()
        String sql = "{call sp_ListLogAuditoria()}";
        return conn.prepareCall(sql);
    }

    @Override
    protected LogAuditoria mapearModelo(ResultSet rs) throws SQLException {
        LogAuditoria log = new LogAuditoria();

        log.setIdLogAuditoria(rs.getInt("idLogAuditoria"));
        log.setDescripcion(rs.getString("descripcion"));
        log.setAccion(rs.getString("accion"));
        log.setFechaRegistro(rs.getTimestamp("fecha_registro"));
        log.setOrigenAccion(rs.getString("origenAccion"));
        log.setIdUsuario(rs.getInt("id_usuario"));
        int idUsu = rs.getInt("id_usuario");
        if (!rs.wasNull()) {
            log.setIdUsuario(idUsu);
        }

        return log;
    }
}
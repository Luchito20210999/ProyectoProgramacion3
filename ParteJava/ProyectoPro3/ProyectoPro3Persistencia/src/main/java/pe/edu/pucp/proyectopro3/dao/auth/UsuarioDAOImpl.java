package pe.edu.pucp.proyectopro3.dao.auth;

import pe.edu.pucp.proyectopro3.dao.DefaultBaseDAO;
import pe.edu.pucp.proyectopro3.modelo.auth.Usuario;
import pe.edu.pucp.proyectopro3.modelo.crm.TipoDocumento;

import java.sql.*;

public class UsuarioDAOImpl extends DefaultBaseDAO<Usuario> implements UsuarioDAO {

    @Override
    protected PreparedStatement comandoCrear(Connection conn, Usuario modelo) throws SQLException {
        String sql = "{call sp_InsertUsuario(?, ?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setString(1, modelo.getNombres());
        cmd.setString(2, modelo.getApellidos());
        cmd.setString(3, modelo.getTipoDocumento().name());
        cmd.setString(4, modelo.getNumeroDocumento());
        cmd.setString(5, modelo.getCorreo());
        cmd.setString(6, modelo.getContrasena());
        cmd.setString(7, modelo.getNumeroContacto());
        cmd.registerOutParameter(8, Types.INTEGER);

        return cmd;
    }

    @Override
    protected PreparedStatement comandoActualizar(Connection conn, Usuario modelo) throws SQLException {
        String sql = "{call sp_UpdateUsuario(?, ?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setInt(1, modelo.getIdUsuario());
        cmd.setString(2, modelo.getNombres());
        cmd.setString(3, modelo.getApellidos());
        cmd.setString(4, modelo.getTipoDocumento().name());
        cmd.setString(5, modelo.getNumeroDocumento());
        cmd.setString(6, modelo.getCorreo());
        cmd.setString(7, modelo.getContrasena());
        cmd.setString(8, modelo.getNumeroContacto());

        return cmd;
    }

    @Override
    protected PreparedStatement comandoEliminar(Connection conn, Integer id) throws SQLException {
        String sql = "{call sp_DeleteUsuario(?)}";
        CallableStatement cmd = conn.prepareCall(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeer(Connection conn, Integer id) throws SQLException {
        String sql = "SELECT * FROM Usuario WHERE id_usuario = ?";
        PreparedStatement cmd = conn.prepareStatement(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeerTodos(Connection conn) throws SQLException {
        String sql = "{call sp_ListUsuarios()}";
        return conn.prepareCall(sql);
    }

    @Override
    protected Integer extraerIdDesdeCallable(CallableStatement cmd) throws SQLException {
        return cmd.getInt(8);
    }

    @Override
    protected Usuario mapearModelo(ResultSet rs) throws SQLException {
        Usuario user = new Usuario();

        user.setIdUsuario(rs.getInt("id_usuario"));
        user.setNombres(rs.getString("nombres"));
        user.setApellidos(rs.getString("apellidos"));
        user.setTipoDocumento(TipoDocumento.valueOf(rs.getString("tipo_documento")));
        user.setNumeroDocumento(rs.getString("numero_documento"));
        user.setCorreo(rs.getString("correo"));
        user.setContrasena(rs.getString("contrasena"));
        user.setNumeroContacto(rs.getString("numero_contacto"));

        int idUsu = rs.getInt("id_usuario");
        if (!rs.wasNull()) {
            user.setIdUsuario(idUsu);
        }

        return user;
    }
}

package pe.edu.pucp.proyectopro3.dao.auth;

import pe.edu.pucp.proyectopro3.dao.DefaultBaseDAO;
import pe.edu.pucp.proyectopro3.modelo.auth.Usuario;
import pe.edu.pucp.proyectopro3.modelo.crm.TipoDocumento;

import java.sql.*;

public class UsuarioDAOImpl extends DefaultBaseDAO<Usuario> implements UsuarioDAO {
    @Override
    public boolean login(String username, String password, String tipoUsuario) {
        return ejecutarComando(conn -> {
            try (PreparedStatement cmd = this.comandoLogin(conn, username, password, tipoUsuario)) {
                if (cmd instanceof CallableStatement callableCmd) {
                    callableCmd.execute();
                    return callableCmd.getBoolean("p_valido");
                }
                return false;
            }
        });
    }

    @Override
    protected PreparedStatement comandoCrear(Connection conn, Usuario modelo) throws SQLException {
        // sp_InsertUsuario(_nom, _ape, _tipoDoc, _numDoc, _corr, _pass, _tel, _tipoUsu, OUT _id_generado)
        String sql = "{call sp_InsertUsuario(?, ?, ?, ?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        setCamposUsuario(cmd, modelo, 1);

        cmd.registerOutParameter(10, Types.INTEGER);

        return cmd;
    }

    @Override
    protected PreparedStatement comandoActualizar(Connection conn, Usuario modelo) throws SQLException {
        // sp_UpdateUsuario(_id, _nom, _ape, _tipoDoc, _numDoc, _corr, _pass, _tel, _tipoUsu)
        String sql = "{call sp_UpdateUsuario(?, ?, ?, ?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setInt(1, modelo.getIdUsuario());
        setCamposUsuario(cmd, modelo, 2);

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
        String sql = "{call sp_ListUsuarioById(?)}";
        CallableStatement cmd = conn.prepareCall(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeerTodos(Connection conn) throws SQLException {
        String sql = "{call spListUsuarios()}";
        return conn.prepareCall(sql);
    }

    protected PreparedStatement comandoLogin(Connection conn,
                                             String username,
                                             String password,
                                             String tipoUsuario) throws SQLException {
        String sql = "{call sp_LoginUsuario(?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);
        cmd.setString("p_correo", username);
        cmd.setString("p_contrasena", password);
        cmd.setString("p_tipo_usuario", tipoUsuario);
        cmd.registerOutParameter("p_valido", Types.BOOLEAN);
        return cmd;
    }

    @Override
    protected Integer extraerIdDesdeCallable(CallableStatement cmd) throws SQLException {
        return cmd.getInt(10);
    }

    @Override
    protected Usuario mapearModelo(ResultSet rs) throws SQLException {
        Usuario usuario = new Usuario();

        usuario.setIdUsuario(rs.getInt("id_usuario"));
        usuario.setNombres(rs.getString("nombres"));
        usuario.setApellidos(rs.getString("apellidos"));
        usuario.setTipoDocumento(TipoDocumento.valueOf(rs.getString("tipo_documento")));
        usuario.setNumeroDocumento(rs.getString("numero_documento"));
        usuario.setCorreo(rs.getString("correo"));
        usuario.setContrasena(rs.getString("contrasena"));
        usuario.setNumeroContacto(rs.getString("numero_contacto"));
        usuario.setTipoUsuario(rs.getString("tipo_usuario"));
        usuario.setActivo(rs.getBoolean("activo"));

        return usuario;
    }

    @Override
    protected Integer extraerIdDesdeGeneratedKeys(ResultSet rs) throws SQLException {
        return rs.getInt(1);
    }

    private void setCamposUsuario(CallableStatement cmd, Usuario modelo, int inicio) throws SQLException {
        cmd.setString(inicio, modelo.getNombres());
        cmd.setString(inicio + 1, modelo.getApellidos());
        cmd.setString(inicio + 2, modelo.getTipoDocumento().name());
        cmd.setString(inicio + 3, modelo.getNumeroDocumento());
        cmd.setString(inicio + 4, modelo.getCorreo());
        cmd.setString(inicio + 5, modelo.getContrasena());
        cmd.setString(inicio + 6, modelo.getNumeroContacto());
        cmd.setString(inicio + 7, modelo.getTipoUsuario());
        cmd.setBoolean(inicio + 8, modelo.getActivo());
    }
}

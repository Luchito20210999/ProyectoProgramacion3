package pe.edu.pucp.proyectopro3.dao.auth;

import pe.edu.pucp.proyectopro3.dao.DefaultBaseDAO;
import pe.edu.pucp.proyectopro3.modelo.auth.Administrador;
import pe.edu.pucp.proyectopro3.modelo.auth.Analista;
import pe.edu.pucp.proyectopro3.modelo.auth.Operador;
import pe.edu.pucp.proyectopro3.modelo.auth.Usuario;
import pe.edu.pucp.proyectopro3.modelo.crm.TipoDocumento;

import java.sql.*;

public class UsuarioDAOImpl extends DefaultBaseDAO<Usuario> implements UsuarioDAO {

    @Override
    protected PreparedStatement comandoCrear(Connection conn, Usuario modelo) throws SQLException {
        // sp_InsertUsuario(_nom, _ape, _tipoDoc, _numDoc, _corr, _pass, _tel, _tipoUsu, OUT _id_generado)
        String sql = "{call sp_InsertUsuario(?, ?, ?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        setCamposUsuario(cmd, modelo, 1);

        cmd.registerOutParameter(9, Types.INTEGER);

        return cmd;
    }

    @Override
    protected PreparedStatement comandoActualizar(Connection conn, Usuario modelo) throws SQLException {
        // sp_UpdateUsuario(_id, _nom, _ape, _tipoDoc, _numDoc, _corr, _pass, _tel, _tipoUsu)
        String sql = "{call sp_UpdateUsuario(?, ?, ?, ?, ?, ?, ?, ?, ?)}";
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

    @Override
    protected Integer extraerIdDesdeCallable(CallableStatement cmd) throws SQLException {
        return cmd.getInt(9);
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
        cmd.setString(inicio + 7, obtenerTipoUsuario(modelo));
    }

    private String obtenerTipoUsuario(Usuario usuario) {
        if (usuario instanceof Administrador) {
            return "ADMINISTRADOR";
        } else if (usuario instanceof Operador) {
            return "OPERADOR";
        } else if (usuario instanceof Analista) {
            return "ANALISTA";
        }

        return "USUARIO";
    }
}

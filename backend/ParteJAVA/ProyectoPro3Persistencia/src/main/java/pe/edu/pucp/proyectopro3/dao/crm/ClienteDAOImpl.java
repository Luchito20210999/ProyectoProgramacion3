package pe.edu.pucp.proyectopro3.dao.crm;

import pe.edu.pucp.proyectopro3.dao.DefaultBaseDAO;
import pe.edu.pucp.proyectopro3.modelo.crm.Cliente;
import pe.edu.pucp.proyectopro3.modelo.crm.TipoDocumento;

import java.sql.*;

public class ClienteDAOImpl extends DefaultBaseDAO<Cliente> implements ClienteDAO {

    @Override
    protected PreparedStatement comandoCrear(Connection conn, Cliente modelo) throws SQLException {
        // sp_InsertCliente(_nom, _ape, _tipoDoc, _numDoc, _corr, _nac, _fReg, _tel, _fNac, OUT _id_generado)
        String sql = "{call sp_InsertCliente(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        setCamposCliente(cmd, modelo, 1);

        // Parámetro OUT: _id_generado
        cmd.registerOutParameter(11, Types.INTEGER);

        return cmd;
    }

    @Override
    protected PreparedStatement comandoActualizar(Connection conn, Cliente modelo) throws SQLException {
        // sp_UpdateCliente(_id, _nom, _ape, _tipoDoc, _numDoc, _corr, _nac, _fReg, _tel, _fNac)
        String sql = "{call sp_UpdateCliente(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setInt(1, modelo.getIdCliente());

        // Los campos empiezan desde la posición 2 porque la posición 1 es el ID
        setCamposCliente(cmd, modelo, 2);

        return cmd;
    }

    @Override
    protected PreparedStatement comandoEliminar(Connection conn, Integer id) throws SQLException {
        String sql = "{call sp_DeleteCliente(?)}";
        CallableStatement cmd = conn.prepareCall(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeer(Connection conn, Integer id) throws SQLException {
        String sql = "{call sp_ListClienteById(?)}";
        CallableStatement cmd = conn.prepareCall(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeerTodos(Connection conn) throws SQLException {
        String sql = "{call spListClientes()}";
        return conn.prepareCall(sql);
    }

    @Override
    protected Cliente mapearModelo(ResultSet rs) throws SQLException {
        Cliente cliente = new Cliente();

        cliente.setIdCliente(rs.getInt("id_cliente"));
        cliente.setNombres(rs.getString("nombres"));
        cliente.setApellidos(rs.getString("apellidos"));
        cliente.setTipoDocumento(TipoDocumento.valueOf(rs.getString("tipo_documento")));
        cliente.setNumeroDocumento(rs.getString("numero_documento"));
        cliente.setCorreo(rs.getString("correo"));
        cliente.setNacionalidad(rs.getString("nacionalidad"));
        cliente.setFechaRegistro(rs.getDate("fecha_registro"));
        cliente.setNumeroContacto(rs.getString("numero_contacto"));
        cliente.setFechaNacimiento(rs.getDate("fecha_nacimiento"));
        cliente.setActivo(rs.getBoolean("activo"));

        return cliente;
    }

    @Override
    protected Integer extraerIdDesdeCallable(CallableStatement cmd) throws SQLException {
        // En sp_InsertCliente, el parámetro OUT _id_generado está en la posición 10
        return cmd.getInt(11);
    }

    @Override
    protected Integer extraerIdDesdeGeneratedKeys(ResultSet rs) throws SQLException {
        return rs.getInt(1);
    }

    private void setCamposCliente(CallableStatement cmd, Cliente modelo, int inicio) throws SQLException {
        cmd.setString(inicio, modelo.getNombres());
        cmd.setString(inicio + 1, modelo.getApellidos());
        cmd.setString(inicio + 2, modelo.getTipoDocumento().name());
        cmd.setString(inicio + 3, modelo.getNumeroDocumento());
        cmd.setString(inicio + 4, modelo.getCorreo());
        cmd.setString(inicio + 5, modelo.getNacionalidad());
        cmd.setDate(inicio + 6, new java.sql.Date(modelo.getFechaRegistro().getTime()));
        cmd.setString(inicio + 7, modelo.getNumeroContacto());
        cmd.setDate(inicio + 8, new java.sql.Date(modelo.getFechaNacimiento().getTime()));
        cmd.setBoolean(inicio + 9, modelo.getActivo());
    }
}

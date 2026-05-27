package pe.edu.pucp.proyectopro3.dao.crm;

import pe.edu.pucp.proyectopro3.dao.DefaultBaseDAO;
import pe.edu.pucp.proyectopro3.modelo.crm.Cliente;
import pe.edu.pucp.proyectopro3.modelo.crm.TipoDocumento;
import pe.edu.pucp.proyectopro3.modelo.reclamos.EstadoReclamo;

import java.sql.*;

public class ClienteDAOImpl extends DefaultBaseDAO<Cliente> implements ClienteDAO {

    @Override
    protected PreparedStatement comandoCrear(Connection conn, Cliente modelo) throws SQLException {
        // sp_InsertCliente(_nom, _ape, _tipoDoc, _numDoc, _nac, _corr, _fReg, _tel, _fNac)
        String sql = "{call sp_InsertCliente(?, ?, ?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setString(1, modelo.getNombres());
        cmd.setString(2, modelo.getApellidos());
        cmd.setString(3, modelo.getTipoDocumento().name()); // Conversión de Enum a String
        cmd.setString(4, modelo.getNumeroContacto());
        cmd.setString(5, modelo.getCorreo());
        cmd.setString(6, modelo.getNacionalidad());
        cmd.setDate(7, new java.sql.Date(modelo.getFechaRegistro().getTime()));
        cmd.setString(8, modelo.getNumeroContacto());
        cmd.setDate(9, new java.sql.Date(modelo.getFechaNacimiento().getTime()));

        return cmd;
    }

    @Override
    protected PreparedStatement comandoActualizar(Connection conn, Cliente modelo) throws SQLException {
        // sp_UpdateCliente(_id, _nom, _ape, _tipoDoc, _numDoc, _nac, _corr, _fReg, _tel, _fNac)
        String sql = "{call sp_UpdateCliente(?, ?, ?, ?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setInt(1, modelo.getIdCliente());
        cmd.setString(2, modelo.getNombres());
        cmd.setString(3, modelo.getApellidos());
        cmd.setString(4, modelo.getTipoDocumento().name());
        cmd.setString(5, modelo.getNumeroDocumento());
        cmd.setString(6, modelo.getCorreo());
        cmd.setString(7, modelo.getNacionalidad());
        cmd.setDate(8, new java.sql.Date(modelo.getFechaRegistro().getTime()));
        cmd.setString(9, modelo.getNumeroContacto());
        cmd.setDate(10, new java.sql.Date(modelo.getFechaNacimiento().getTime()));

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
        // Al no haber un SP específico para "buscar por ID", usamos un SELECT estándar
        String sql = "SELECT * FROM Cliente WHERE id_cliente = ?";
        PreparedStatement cmd = conn.prepareStatement(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeerTodos(Connection conn) throws SQLException {
        String sql = "{call sp_ListClientes()}";
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

        return cliente;
    }
}

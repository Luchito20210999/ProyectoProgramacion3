package pe.edu.pucp.proyectopro3.dao.reservas;

import pe.edu.pucp.proyectopro3.dao.DefaultBaseDAO;
import pe.edu.pucp.proyectopro3.modelo.reservas.Servicio;

import java.sql.*;

public class ServicioDAOImpl extends DefaultBaseDAO<Servicio> implements ServicioDAO {

    @Override
    protected PreparedStatement comandoCrear(Connection conn, Servicio modelo) throws SQLException {
        // sp_InsertServicio(_nom, _desc, _pre, _dur, _idioma, _cap, _recojo, _ciu)
        String sql = "{call sp_InsertServicio(?, ?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setString(1, modelo.getNombre());
        cmd.setString(2, modelo.getDescripcion());
        cmd.setDouble(3, modelo.getPrecioUSD());
        cmd.setDouble(4, modelo.getDuracionHoras());
        cmd.setString(5, modelo.getIdiomaGuia()); // Campo corregido
        cmd.setInt(6, modelo.getCapacidadMaxima());
        cmd.setBoolean(7, modelo.isIncluyeRecojo());
        cmd.setString(8, modelo.getCiudadDestino()); // Campo corregido

        return cmd;
    }

    @Override
    protected PreparedStatement comandoActualizar(Connection conn, Servicio modelo) throws SQLException {
        // sp_UpdateServicio(_id, _nom, _desc, _pre, _dur, _idioma, _cap, _recojo, _ciu)
        String sql = "{call sp_UpdateServicio(?, ?, ?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setInt(1, modelo.getIdServicio());
        cmd.setString(2, modelo.getNombre());
        cmd.setString(3, modelo.getDescripcion());
        cmd.setDouble(4, modelo.getPrecioUSD());
        cmd.setDouble(5, modelo.getDuracionHoras());
        cmd.setString(6, modelo.getIdiomaGuia());
        cmd.setInt(7, modelo.getCapacidadMaxima());
        cmd.setBoolean(8, modelo.isIncluyeRecojo());
        cmd.setString(9, modelo.getCiudadDestino());

        return cmd;
    }

    @Override
    protected PreparedStatement comandoEliminar(Connection conn, Integer id) throws SQLException {
        String sql = "{call sp_DeleteServicio(?)}";
        CallableStatement cmd = conn.prepareCall(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeer(Connection conn, Integer id) throws SQLException {
        String sql = "SELECT * FROM Servicio WHERE idServicio = ?";
        PreparedStatement cmd = conn.prepareStatement(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeerTodos(Connection conn) throws SQLException {
        String sql = "{call sp_ListServicios()}";
        return conn.prepareCall(sql);
    }

    @Override
    protected Servicio mapearModelo(ResultSet rs) throws SQLException {
        Servicio servicio = new Servicio();

        servicio.setIdServicio(rs.getInt("idServicio"));
        servicio.setNombre(rs.getString("nombre"));
        servicio.setDescripcion(rs.getString("descripcion"));
        servicio.setPrecioUSD(rs.getDouble("precioUSD")); // Según SQL
        servicio.setDuracionHoras(rs.getDouble("duracionHoras")); // Según SQL
        servicio.setIdiomaGuia(rs.getString("idiomaGuia")); // Campo corregido
        servicio.setCapacidadMaxima(rs.getInt("capacidadMaxima"));
        servicio.setIncluyeRecojo(rs.getBoolean("incluyeRecojo"));
        servicio.setCiudadDestino(rs.getString("ciudadDestino")); // Campo corregido

        return servicio;
    }
}
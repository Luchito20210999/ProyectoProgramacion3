package pe.edu.pucp.proyectopro3.dao.reservas;

import pe.edu.pucp.proyectopro3.dao.DefaultBaseDAO;
import pe.edu.pucp.proyectopro3.modelo.reservas.Servicio;

import java.sql.*;

public class ServicioDAOImpl extends DefaultBaseDAO<Servicio> implements ServicioDAO {

    @Override
    protected PreparedStatement comandoCrear(Connection conn, Servicio modelo) throws SQLException {
        String sql = "{call sp_InsertServicio(?, ?, ?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        setCamposServicio(cmd, modelo, 1);
        cmd.registerOutParameter(9, Types.INTEGER);

        return cmd;
    }

    @Override
    protected PreparedStatement comandoActualizar(Connection conn, Servicio modelo) throws SQLException {
        String sql = "{call sp_UpdateServicio(?, ?, ?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setInt(1, modelo.getIdServicio());
        setCamposServicio(cmd, modelo, 2);

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
        String sql = "{call sp_ListServicioById(?)}";
        CallableStatement cmd = conn.prepareCall(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeerTodos(Connection conn) throws SQLException {
        String sql = "{call spListServicios()}";
        return conn.prepareCall(sql);
    }

    @Override
    protected Integer extraerIdDesdeCallable(CallableStatement cmd) throws SQLException {
        return cmd.getInt(9);
    }

    @Override
    protected Integer extraerIdDesdeGeneratedKeys(ResultSet rs) throws SQLException {
        return rs.getInt(1);
    }

    @Override
    protected Servicio mapearModelo(ResultSet rs) throws SQLException {
        Servicio servicio = new Servicio();

        servicio.setIdServicio(rs.getInt("id_servicio"));
        servicio.setNombre(rs.getString("nombre"));
        servicio.setDescripcion(rs.getString("descripcion"));
        servicio.setPrecioUSD(rs.getDouble("precio_usd"));
        servicio.setDuracionHoras(rs.getDouble("duracion_horas"));
        servicio.setIdiomaGuia(rs.getString("idioma_guia"));
        servicio.setCapacidadMaxima(rs.getInt("capacidad_maxima"));
        servicio.setIncluyeRecojo("Y".equalsIgnoreCase(rs.getString("incluye_recojo")));
        servicio.setCiudadDestino(rs.getString("ciudad_destino"));

        return servicio;
    }

    private void setCamposServicio(CallableStatement cmd, Servicio modelo, int inicio) throws SQLException {
        cmd.setString(inicio, modelo.getNombre());
        cmd.setString(inicio + 1, modelo.getDescripcion());
        cmd.setDouble(inicio + 2, modelo.getPrecioUSD());
        cmd.setDouble(inicio + 3, modelo.getDuracionHoras());
        cmd.setString(inicio + 4, modelo.getIdiomaGuia());
        cmd.setInt(inicio + 5, modelo.getCapacidadMaxima());
        cmd.setString(inicio + 6, modelo.isIncluyeRecojo() ? "Y" : "N");
        cmd.setString(inicio + 7, modelo.getCiudadDestino());
    }
}

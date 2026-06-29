package pe.edu.pucp.proyectopro3.dao.reservas;

import pe.edu.pucp.proyectopro3.dao.DefaultBaseDAO;
import pe.edu.pucp.proyectopro3.modelo.dto.ReservaDetalleDTO;
import pe.edu.pucp.proyectopro3.modelo.reservas.EstadoReserva;
import pe.edu.pucp.proyectopro3.modelo.reservas.Reserva;

import java.sql.*;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

public class ReservaDAOImpl extends DefaultBaseDAO<Reserva> implements ReservaDAO {

    @Override
    protected PreparedStatement comandoCrear(Connection conn, Reserva modelo) throws SQLException {
        String sql = "{call sp_InsertReserva(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        setCamposReserva(cmd, modelo, 1);
        cmd.registerOutParameter(11, Types.INTEGER);

        return cmd;
    }

    @Override
    protected PreparedStatement comandoActualizar(Connection conn, Reserva modelo) throws SQLException {
        String sql = "{call sp_UpdateReserva(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)}";
        CallableStatement cmd = conn.prepareCall(sql);

        cmd.setInt(1, modelo.getIdReserva());
        setCamposReserva(cmd, modelo, 2);

        return cmd;
    }

    @Override
    protected PreparedStatement comandoEliminar(Connection conn, Integer id) throws SQLException {
        String sql = "{call sp_DeleteReserva(?)}";
        CallableStatement cmd = conn.prepareCall(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeer(Connection conn, Integer id) throws SQLException {
        String sql = "{call sp_ListReservaById(?)}";
        CallableStatement cmd = conn.prepareCall(sql);
        cmd.setInt(1, id);
        return cmd;
    }

    @Override
    protected PreparedStatement comandoLeerTodos(Connection conn) throws SQLException {
        String sql = "{call spListReservas()}";
        return conn.prepareCall(sql);
    }

    @Override
    public List<ReservaDetalleDTO> listarDetalle() {
        return ejecutarComando(conn -> {
            String sql = "{call spListReservasDetalle()}";
            try (CallableStatement cmd = conn.prepareCall(sql);
                 ResultSet rs = cmd.executeQuery()) {
                List<ReservaDetalleDTO> reservas = new ArrayList<>();
                while (rs.next()) {
                    reservas.add(mapearDetalle(rs));
                }
                return reservas;
            }
        });
    }

    @Override
    public ReservaDetalleDTO obtenerDetalle(int idReserva) {
        return ejecutarComando(conn -> {
            String sql = "{call sp_ListReservaDetalleById(?)}";
            try (CallableStatement cmd = conn.prepareCall(sql)) {
                cmd.setInt(1, idReserva);
                try (ResultSet rs = cmd.executeQuery()) {
                    return rs.next() ? mapearDetalle(rs) : null;
                }
            }
        });
    }

    @Override
    protected Integer extraerIdDesdeCallable(CallableStatement cmd) throws SQLException {
        return cmd.getInt(11);
    }

    @Override
    protected Integer extraerIdDesdeGeneratedKeys(ResultSet rs) throws SQLException {
        return rs.getInt(1);
    }

    @Override
    protected Reserva mapearModelo(ResultSet rs) throws SQLException {
        Reserva reserva = new Reserva();

        reserva.setIdReserva(rs.getInt("id_reserva"));
        reserva.setFechaRegistro(rs.getTimestamp("fecha_registro"));
        reserva.setEstadoReserva(EstadoReserva.valueOf(rs.getString("estado_reserva")));
        reserva.setCantidadBoletos(rs.getInt("cantidad_boletos"));
        reserva.setMontoTotal(rs.getDouble("monto_total"));
        reserva.setFechaUltimaModificacion(rs.getTimestamp("fecha_ultima_modif"));
        reserva.setCanalVenta(rs.getString("canal_venta"));
        reserva.setMontoImpuestos(rs.getDouble("monto_impuestos"));
        reserva.setCodigoBokun(rs.getString("codigo_bokun"));
        reserva.setIdUsuario(rs.getInt("id_usuario"));
        reserva.setIdCliente(rs.getInt("id_cliente"));

        return reserva;
    }

    private ReservaDetalleDTO mapearDetalle(ResultSet rs) throws SQLException {
        ReservaDetalleDTO reserva = new ReservaDetalleDTO();

        reserva.setIdReserva(rs.getInt("id_reserva"));
        reserva.setFechaRegistro(rs.getTimestamp("fecha_registro"));
        reserva.setEstadoReserva(rs.getString("estado_reserva"));
        reserva.setCantidadBoletos(rs.getInt("cantidad_boletos"));
        reserva.setMontoTotal(rs.getDouble("monto_total"));
        reserva.setMontoImpuestos(rs.getDouble("monto_impuestos"));
        reserva.setCodigoBokun(rs.getString("codigo_bokun"));
        reserva.setCodigoReserva(rs.getString("codigo_reserva"));
        reserva.setIdUsuario(rs.getInt("id_usuario"));
        reserva.setIdCliente(rs.getInt("id_cliente"));
        reserva.setCliente(rs.getString("cliente"));
        reserva.setClienteTipoDocumento(rs.getString("cliente_tipo_documento"));
        reserva.setClienteNumeroDocumento(rs.getString("cliente_numero_documento"));
        reserva.setClienteCorreo(rs.getString("cliente_correo"));
        reserva.setClienteNacionalidad(rs.getString("cliente_nacionalidad"));
        reserva.setIdServicio(rs.getInt("id_servicio"));
        reserva.setServicio(rs.getString("servicio"));
        reserva.setCiudadDestino(rs.getString("ciudad_destino"));
        reserva.setServicioPrecioUSD(rs.getDouble("servicio_precio_usd"));
        reserva.setServicioDuracionHoras(rs.getDouble("servicio_duracion_horas"));
        reserva.setServicioCapacidadMaxima(rs.getInt("servicio_capacidad_maxima"));
        reserva.setServicioIdiomaGuia(rs.getString("servicio_idioma_guia"));
        reserva.setServicioIncluyeRecojo(rs.getBoolean("servicio_incluye_recojo"));

        return reserva;
    }

    private void setCamposReserva(CallableStatement cmd, Reserva modelo, int inicio) throws SQLException {
        setTimestamp(cmd, inicio, modelo.getFechaRegistro());
        cmd.setString(inicio + 1, modelo.getEstadoReserva().name());
        cmd.setInt(inicio + 2, modelo.getCantidadBoletos());
        cmd.setDouble(inicio + 3, modelo.getMontoTotal());
        setTimestamp(cmd, inicio + 4, modelo.getFechaUltimaModificacion());
        cmd.setString(inicio + 5, modelo.getCanalVenta());
        cmd.setDouble(inicio + 6, modelo.getMontoImpuestos());
        cmd.setString(inicio + 7, modelo.getCodigoBokun());
        setIdUsuario(cmd, inicio + 8, modelo.getIdUsuario());
        cmd.setInt(inicio + 9, modelo.getIdCliente());
    }

    private void setTimestamp(CallableStatement cmd, int index, Date value) throws SQLException {
        if (value == null) {
            cmd.setNull(index, Types.TIMESTAMP);
        } else {
            cmd.setTimestamp(index, new Timestamp(value.getTime()));
        }
    }

    private void setIdUsuario(CallableStatement cmd, int index, int idUsuario) throws SQLException {
        if (idUsuario <= 0) {
            cmd.setNull(index, Types.INTEGER);
        } else {
            cmd.setInt(index, idUsuario);
        }
    }
}

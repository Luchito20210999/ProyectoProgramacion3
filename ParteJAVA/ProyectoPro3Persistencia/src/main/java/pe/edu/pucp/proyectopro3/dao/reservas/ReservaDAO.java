package pe.edu.pucp.proyectopro3.dao.reservas;

import pe.edu.pucp.proyectopro3.dao.Persistible;
import pe.edu.pucp.proyectopro3.modelo.dto.ReservaDetalleDTO;
import pe.edu.pucp.proyectopro3.modelo.reservas.Reserva;

import java.util.List;

public interface ReservaDAO extends Persistible<Reserva, Integer> {
    List<ReservaDetalleDTO> listarDetalle();
    ReservaDetalleDTO obtenerDetalle(int idReserva);
}

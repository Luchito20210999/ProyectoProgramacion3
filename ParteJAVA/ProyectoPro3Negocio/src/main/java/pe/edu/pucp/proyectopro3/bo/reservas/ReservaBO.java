package pe.edu.pucp.proyectopro3.bo.reservas;

import pe.edu.pucp.proyectopro3.bo.Gestionable;
import pe.edu.pucp.proyectopro3.modelo.dto.ReservaDetalleDTO;
import pe.edu.pucp.proyectopro3.modelo.reservas.Reserva;

import java.util.List;

public interface ReservaBO extends Gestionable<Reserva> {
    public Reserva consultarReserva(int idReserva);
    public void modificarReserva(int idReserva);
    public void anularReserva(int idReserva);
    public List<ReservaDetalleDTO> listarDetalle();
    public ReservaDetalleDTO obtenerDetalle(int idReserva);
}

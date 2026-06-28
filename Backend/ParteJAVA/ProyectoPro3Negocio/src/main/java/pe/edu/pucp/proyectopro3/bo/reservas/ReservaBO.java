package pe.edu.pucp.proyectopro3.bo.reservas;

import pe.edu.pucp.proyectopro3.bo.Gestionable;
import pe.edu.pucp.proyectopro3.modelo.reservas.Reserva;

public interface ReservaBO extends Gestionable<Reserva> {
    public Reserva consultarReserva(int idReserva);
    public void modificarReserva(int idReserva);
    public void anularReserva(int idReserva);
}
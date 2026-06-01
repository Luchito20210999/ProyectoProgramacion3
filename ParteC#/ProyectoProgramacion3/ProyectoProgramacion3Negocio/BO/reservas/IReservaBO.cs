using ProyectoProgramacion3Model.Model.reservas;

namespace ProyectoProgramacion3Negocio.BO.reservas;

public interface IReservaBO : IGestionable<Reserva>
{
    Reserva ConsultarReserva(int idReserva);
    void ModificarReserva(int idReserva);
    void AnularReserva(int idReserva);
}

using ProyectoProgramacion3Web.Servicios.Base;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Servicios.Reservas;

public interface IReservasServiceClient
{
    List<ReservaItem> Listar();
    ReservaItem? Obtener(int id);
    void Guardar(ReservaItem modelo, Estado estado);
}

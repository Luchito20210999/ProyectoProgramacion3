using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Servicios.Base;

public interface IServiceClient<TViewModel> where TViewModel : class
{
    List<TViewModel> Listar();
    TViewModel? Obtener(int id);
    void Guardar(TViewModel modelo, Estado estado);
    void Eliminar(int id);
}

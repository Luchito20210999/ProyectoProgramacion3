using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Servicios.Auditoria;

public interface IAuditoriaServiceClient
{
    List<AuditoriaItem> Listar();
    void Registrar(string accion, string descripcion, string origenAccion, int idUsuario);
}

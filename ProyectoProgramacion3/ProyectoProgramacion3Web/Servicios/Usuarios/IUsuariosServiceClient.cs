using ProyectoProgramacion3Web.Servicios.Base;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Servicios.Usuarios;

public interface IUsuariosServiceClient : IServiceClient<UsuarioItem>
{
    UsuarioItem? Login(string correo, string contrasena);
}

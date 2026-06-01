using ProyectoProgramacion3Model.Model.auth;

namespace ProyectoProgramacion3Negocio.BO.auth;

public interface IUsuarioBO : IGestionable<Usuario>
{
    void CrearUsuario(Usuario usuario);
    void EditarUsuario(int id);
    void EliminarUsuario(int id);
    void BuscarUsuario(int id);
}

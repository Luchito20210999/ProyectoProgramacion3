using ProyectoProgramacion3Model.Model.crm;
namespace ProyectoProgramacion3Model.Model.auth;

public class Administrador : Usuario
{
    public Administrador()
    {
    }

    public Administrador(int idUsuario, string nombres, string apellidos, TipoDocumento tipoDocumento, 
        string numeroDocumento, string numeroContacto, string correo, string contrasena) :
        base(idUsuario, nombres, apellidos, tipoDocumento, numeroDocumento, numeroContacto, correo, contrasena)
    {
    }

    public void buscarUsuario(int id)
    {
    }

    public void crearUsuario(Usuario u)
    {
    }

    public void editarUsuario(int id)
    {
    }

    public void eliminarUsuario(int id)
    {
    }
}

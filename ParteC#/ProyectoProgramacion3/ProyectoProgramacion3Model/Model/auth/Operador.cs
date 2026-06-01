using ProyectoProgramacion3Model.Model.crm;
namespace ProyectoProgramacion3Model.Model.auth;

public class Operador: Usuario
{
    public Operador()
    {
    }
    public Operador(int idUsuario, string nombres, string apellidos, TipoDocumento tipoDocumento, 
        string numeroDocumento, string numeroContacto, string correo, string contrasena) :
        base(idUsuario, nombres, apellidos, tipoDocumento, numeroDocumento, numeroContacto, correo, contrasena)
    {
    }
}

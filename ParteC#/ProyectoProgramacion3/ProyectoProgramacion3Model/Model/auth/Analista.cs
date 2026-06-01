using ProyectoProgramacion3Model.Model.crm;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoProgramacion3Model.Model.auth;
public class Analista : Usuario
{
    public Analista()
    {
    }
    public Analista(int idUsuario, string nombres, string apellidos, TipoDocumento tipoDocumento,
        string numeroDocumento, string numeroContacto, string correo, string contrasena) :
        base(idUsuario, nombres, apellidos, tipoDocumento, numeroDocumento, numeroContacto, correo, contrasena)
    {
    }
}

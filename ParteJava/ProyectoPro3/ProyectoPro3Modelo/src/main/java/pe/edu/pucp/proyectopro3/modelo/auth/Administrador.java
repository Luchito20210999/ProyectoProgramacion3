package pe.edu.pucp.proyectopro3.modelo.auth;

import pe.edu.pucp.proyectopro3.modelo.crm.TipoDocumento;

public class Administrador extends Usuario{
    public Administrador(int idUsuario, String nombres, String apellidos, TipoDocumento tipoDocumento, String numeroDocumento, String numeroContacto, String correo, String contrasena) {
        super(idUsuario, nombres, apellidos, tipoDocumento, numeroDocumento, correo, contrasena, numeroContacto);
    }

}

package pe.edu.pucp.proyectopro3.bo.auth;

import pe.edu.pucp.proyectopro3.bo.Gestionable;
import pe.edu.pucp.proyectopro3.modelo.auth.Usuario;

public interface UsuarioBO extends Gestionable<Usuario>{
    boolean login(String username, String password, String tipoUsuario);
}

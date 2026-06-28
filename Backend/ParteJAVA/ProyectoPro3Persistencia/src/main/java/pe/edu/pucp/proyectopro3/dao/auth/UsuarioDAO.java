package pe.edu.pucp.proyectopro3.dao.auth;

import pe.edu.pucp.proyectopro3.dao.Persistible;
import pe.edu.pucp.proyectopro3.modelo.auth.Usuario;

public interface UsuarioDAO extends Persistible<Usuario,Integer> {
    boolean login(String username, String password, String tipoUsuario);
    Usuario leerPorCorreo(String correo);
}

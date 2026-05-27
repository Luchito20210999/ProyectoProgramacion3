package pe.edu.pucp.proyectopro3.modelo.auth;

public interface UsuariosGestionable {
    void crearUsuario(Usuario u);
    void editarUsuario(int id);
    void eliminarUsuario(int id);
    void buscarUsuario(int id);
}

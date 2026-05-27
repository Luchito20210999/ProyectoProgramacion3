package pe.edu.pucp.proyectopro3.modelo.auth;

public class GestorUsuario implements Autenticable{
    @Override
    public boolean iniciarSesion(String correo, String contrasena) {
        //En proceso
        return false;
    }

    @Override
    public void cerrarSesion() {

    }

    @Override
    public void recuperarContrasena(String correo) {

    }
}

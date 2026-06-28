package pe.edu.pucp.proyectopro3.modelo.reclamos;

public interface ReclamoGestionable {
    void registrarReclamo(Reclamo r, int idReserva);

    Reclamo consultarReclamo(int idReclamo);

    void eliminarReclamo(int idReclamo);
}

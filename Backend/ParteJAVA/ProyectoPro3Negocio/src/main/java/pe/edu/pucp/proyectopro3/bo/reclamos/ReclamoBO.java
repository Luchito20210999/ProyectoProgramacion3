package pe.edu.pucp.proyectopro3.bo.reclamos;

import pe.edu.pucp.proyectopro3.bo.Gestionable;
import pe.edu.pucp.proyectopro3.modelo.reclamos.Reclamo;

public interface ReclamoBO extends Gestionable<Reclamo> {
    void atenderReclamo(int idReclamo);

    void evaluarProcedencia(int idReclamo, boolean procede);

    void registrarReclamo(Reclamo r, int idReserva);

    Reclamo consultarReclamo(int idReclamo);

    void eliminarReclamo(int idReclamo);
}

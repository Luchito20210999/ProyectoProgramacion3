package pe.edu.pucp.proyectopro3.bo.reclamos;

import pe.edu.pucp.proyectopro3.bo.Gestionable;
import pe.edu.pucp.proyectopro3.modelo.dto.ReclamoDetalleDTO;
import pe.edu.pucp.proyectopro3.modelo.reclamos.Reclamo;

import java.util.List;

public interface ReclamoBO extends Gestionable<Reclamo> {
    void atenderReclamo(int idReclamo);

    void evaluarProcedencia(int idReclamo, boolean procede);

    void registrarReclamo(Reclamo r, int idReserva);

    Reclamo consultarReclamo(int idReclamo);

    void eliminarReclamo(int idReclamo);

    List<ReclamoDetalleDTO> listarDetalle();

    ReclamoDetalleDTO obtenerDetalle(int idReclamo);
}

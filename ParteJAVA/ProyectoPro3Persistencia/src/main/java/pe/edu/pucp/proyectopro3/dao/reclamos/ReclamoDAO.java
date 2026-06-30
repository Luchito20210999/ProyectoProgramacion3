package pe.edu.pucp.proyectopro3.dao.reclamos;

import pe.edu.pucp.proyectopro3.dao.Persistible;
import pe.edu.pucp.proyectopro3.modelo.dto.ReclamoDetalleDTO;
import pe.edu.pucp.proyectopro3.modelo.reclamos.Reclamo;

import java.util.List;

public interface ReclamoDAO extends Persistible<Reclamo, Integer> {
    List<ReclamoDetalleDTO> listarDetalle();
    ReclamoDetalleDTO obtenerDetalle(int idReclamo);
}

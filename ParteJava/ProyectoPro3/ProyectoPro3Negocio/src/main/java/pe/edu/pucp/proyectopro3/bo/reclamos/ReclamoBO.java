package pe.edu.pucp.proyectopro3.bo.reclamos;

import pe.edu.pucp.proyectopro3.bo.Gestionable;
import pe.edu.pucp.proyectopro3.modelo.reclamos.Atendible;
import pe.edu.pucp.proyectopro3.modelo.reclamos.Reclamo;
import pe.edu.pucp.proyectopro3.modelo.reclamos.ReclamoGestionable;

public interface ReclamoBO extends Gestionable<Reclamo>, ReclamoGestionable, Atendible {
}

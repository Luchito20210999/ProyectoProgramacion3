package pe.edu.pucp.proyectopro3.modelo.reclamos;

public interface Atendible {
    void atenderReclamo(int idReclamo);

    void evaluarProcedencia(int idReclamo, boolean procede);
}

package pe.edu.pucp.proyectopro3.bo.reclamos;


import pe.edu.pucp.proyectopro3.bo.BaseBO;
import pe.edu.pucp.proyectopro3.dao.reclamos.ReclamoDAO;
import pe.edu.pucp.proyectopro3.dao.reclamos.ReclamoDAOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.reclamos.EstadoReclamo;
import pe.edu.pucp.proyectopro3.modelo.reclamos.Reclamo;

import java.util.Date;
import java.util.List;
import java.util.Objects;

public class ReclamoBOImpl extends BaseBO implements ReclamoBO {
    private final ReclamoDAO reclamoDao;

    public ReclamoBOImpl() {
        this.reclamoDao = new ReclamoDAOImpl();
    }

    @Override
    public void registrarReclamo(Reclamo r, int idReserva) {
        validarReclamo(r);
        validarIdPositivo(idReserva, "id de reserva");

        // 1. Vinculamos el reclamo a la reserva
        // Asumiendo que agregaste 'idReserva' a la clase Reclamo como llave foránea
        r.setIdReserva(idReserva);

        // 2. Por regla de negocio, todo reclamo nuevo nace como PENDIENTE
        r.setEstadoReclamo(EstadoReclamo.PENDIENTE);
        r.setFechaReclamo(new Date());

        // 3. Guardamos en base de datos
        int idGenerado = this.reclamoDao.crear(r);
        if (idGenerado <= 0) {
            throw new IllegalStateException("Error al registrar el reclamo en el sistema.");
        }
        r.setIdReclamo(idGenerado);
    }

    @Override
    public Reclamo consultarReclamo(int idReclamo) {
        validarIdPositivo(idReclamo, "id de reclamo");
        Reclamo reclamo = this.reclamoDao.leer(idReclamo);
        if (reclamo == null) {
            throw new IllegalArgumentException("No existe un reclamo con el ID " + idReclamo);
        }
        return reclamo;
    }

    @Override
    public void eliminarReclamo(int idReclamo) {
        // En algunos sistemas, un reclamo que ya está en atención no se puede eliminar.
        // Aquí podríamos añadir esa validación antes de borrarlo.
        validarIdPositivo(idReclamo, "id de reclamo");
        if (!this.reclamoDao.eliminar(idReclamo)) {
            throw new IllegalStateException("No se pudo eliminar el reclamo.");
        }
    }

    /*Atendible*/

    @Override
    public void atenderReclamo(int idReclamo) {
        Reclamo reclamo = consultarReclamo(idReclamo);

        // Regla de negocio: Solo un reclamo PENDIENTE puede pasar a EN_ATENCION
        if (reclamo.getEstadoReclamo() != EstadoReclamo.PENDIENTE) {
            throw new IllegalStateException("Solo se pueden atender reclamos en estado PENDIENTE.");
        }

        reclamo.setEstadoReclamo(EstadoReclamo.EN_ATENCION);

        if (!this.reclamoDao.actualizar(reclamo)) {
            throw new IllegalStateException("Error al actualizar el estado a En Atención.");
        }
    }

    @Override
    public void evaluarProcedencia(int idReclamo, boolean procede) {
        Reclamo reclamo = consultarReclamo(idReclamo);

        // Regla de negocio: Solo se evalúa si ya estaba en atención
        if (reclamo.getEstadoReclamo() != EstadoReclamo.EN_ATENCION) {
            throw new IllegalStateException("El reclamo debe estar EN_ATENCION para ser evaluado.");
        }

        // Asignamos el estado final dependiendo del boolean 'procede'
        reclamo.setEstadoReclamo(procede ? EstadoReclamo.PROCEDE : EstadoReclamo.NO_PROCEDE);

        // Registramos la fecha en la que se resolvió
        reclamo.setFechaResolucion(new Date());

        // Validamos que el trabajador haya escrito un motivo
        validarTextoObligatorio(reclamo.getMotivoResolucion(), "motivo de resolución");

        if (!this.reclamoDao.actualizar(reclamo)) {
            throw new IllegalStateException("Error al registrar la evaluación del reclamo.");
        }
    }

    // ====================================================================
    // MÉTODOS CRUD HEREDADOS (Gestionable)
    // ====================================================================

    @Override
    public void guardar(Reclamo modelo, Estado estado) {
        // En esta entidad, preferimos usar registrarReclamo() para nuevos,
        // pero podemos mantener este método para actualizaciones generales.
        if (estado == Estado.Nuevo) {
            throw new UnsupportedOperationException("Para reclamos nuevos, use registrarReclamo()");
        } else if (estado == Estado.Modificado) {
            validarReclamo(modelo);
            if (!this.reclamoDao.actualizar(modelo)) {
                throw new IllegalStateException("Error al actualizar el reclamo");
            }
        }
    }

    @Override
    public List<Reclamo> listar() {
        return this.reclamoDao.leerTodos();
    }

    @Override
    public Reclamo obtener(int id) {
        return consultarReclamo(id); // Reutilizamos el método de negocio
    }

    @Override
    public void eliminar(int id) {
        eliminarReclamo(id); // Reutilizamos el método de negocio
    }

    // ====================================================================
    // VALIDACIONES PRIVADAS
    // ====================================================================

    private void validarReclamo(Reclamo r) {
        Objects.requireNonNull(r, "El reclamo no puede ser nulo");
        validarTextoObligatorio(r.getDescripcion(), "descripción del reclamo");
    }
}

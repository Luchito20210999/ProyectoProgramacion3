package pe.edu.pucp.proyectopro3.bo.reclamos;


import pe.edu.pucp.proyectopro3.bo.BaseBO;
import pe.edu.pucp.proyectopro3.bo.notificaciones.NotificacionRegistro;
import pe.edu.pucp.proyectopro3.dao.reclamos.ReclamoDAO;
import pe.edu.pucp.proyectopro3.dao.reclamos.ReclamoDAOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.dto.ReclamoDetalleDTO;
import pe.edu.pucp.proyectopro3.modelo.notificaciones.TipoEvento;
import pe.edu.pucp.proyectopro3.modelo.reclamos.EstadoReclamo;
import pe.edu.pucp.proyectopro3.modelo.reclamos.Reclamo;

import java.util.Date;
import java.util.List;
import java.util.Objects;

public class ReclamoBOImpl extends BaseBO implements ReclamoBO {
    private final ReclamoDAO reclamoDao;
    private final NotificacionRegistro notificacionRegistro;

    public ReclamoBOImpl() {
        this.reclamoDao = new ReclamoDAOImpl();
        this.notificacionRegistro = new NotificacionRegistro();
    }

    @Override
    public void registrarReclamo(Reclamo r, int idReserva) {
        validarReclamo(r);
        validarIdPositivo(idReserva, "id de reserva");

        // 1. Vinculamos el reclamo a la reserva
        // Asumiendo que agregaste 'idReserva' a la clase Reclamo como llave forÃ¡nea
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
        notificarEventoReclamo(
                r,
                "Reclamo #" + r.getIdReclamo() + " pendiente para la reserva " + r.getIdReserva() + ".");
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
        validarIdPositivo(idReclamo, "id de reclamo");
        Reclamo reclamo = consultarReclamo(idReclamo);
        if (reclamo.getEstadoReclamo() != EstadoReclamo.PENDIENTE) {
            throw new IllegalStateException("La eliminacion solo sera permitida bajo el estado 'Pendiente'.");
        }
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
            throw new IllegalStateException("Error al actualizar el estado a En AtenciÃ³n.");
        }
    }

    @Override
    public void evaluarProcedencia(int idReclamo, boolean procede) {
        Reclamo reclamo = consultarReclamo(idReclamo);

        // Regla de negocio: Solo se evalÃºa si ya estaba en atenciÃ³n
        if (reclamo.getEstadoReclamo() != EstadoReclamo.EN_ATENCION) {
            throw new IllegalStateException("El reclamo debe estar EN_ATENCION para ser evaluado.");
        }

        // Asignamos el estado final dependiendo del boolean 'procede'
        reclamo.setEstadoReclamo(procede ? EstadoReclamo.PROCEDE : EstadoReclamo.NO_PROCEDE);

        // Registramos la fecha en la que se resolviÃ³
        reclamo.setFechaResolucion(new Date());

        // Validamos que el trabajador haya escrito un motivo
        validarTextoObligatorio(reclamo.getMotivoResolucion(), "motivo de resoluciÃ³n");

        if (!this.reclamoDao.actualizar(reclamo)) {
            throw new IllegalStateException("Error al registrar la evaluaciÃ³n del reclamo.");
        }
    }

    // ====================================================================
    // MÃ‰TODOS CRUD HEREDADOS (Gestionable)
    // ====================================================================

    @Override
    public void guardar(Reclamo modelo, Estado estado) {
        // En esta entidad, preferimos usar registrarReclamo() para nuevos,
        // pero podemos mantener este mÃ©todo para actualizaciones generales.
        if (estado == Estado.Nuevo) {
            throw new UnsupportedOperationException("Para reclamos nuevos, use registrarReclamo()");
        } else if (estado == Estado.Modificado) {
            validarReclamo(modelo);
            if (!this.reclamoDao.actualizar(modelo)) {
                throw new IllegalStateException("Error al actualizar el reclamo");
            }
            notificarEventoReclamo(
                    modelo,
                    "Reclamo #" + modelo.getIdReclamo() + " actualizado a estado "
                            + modelo.getEstadoReclamo() + ".");
        }
    }

    @Override
    public List<Reclamo> listar() {
        return this.reclamoDao.leerTodos();
    }

    @Override
    public List<ReclamoDetalleDTO> listarDetalle() {
        return this.reclamoDao.listarDetalle();
    }

    @Override
    public Reclamo obtener(int id) {
        return consultarReclamo(id); // Reutilizamos el mÃ©todo de negocio
    }

    @Override
    public ReclamoDetalleDTO obtenerDetalle(int idReclamo) {
        validarIdPositivo(idReclamo, "id de reclamo");
        return this.reclamoDao.obtenerDetalle(idReclamo);
    }

    @Override
    public void eliminar(int id) {
        eliminarReclamo(id); // Reutilizamos el mÃ©todo de negocio
    }

    // ====================================================================
    // VALIDACIONES PRIVADAS
    // ====================================================================

    private void validarReclamo(Reclamo r) {
        Objects.requireNonNull(r, "El reclamo no puede ser nulo");
        validarTextoObligatorio(r.getDescripcion(), "descripciÃ³n del reclamo");
    }
    private void notificarEventoReclamo(Reclamo reclamo, String mensaje) {
        try {
            this.notificacionRegistro.registrarEventoOperativo(
                    TipoEvento.RECLAMO_PENDIENTE,
                    mensaje,
                    reclamo.getIdUsuario());
        } catch (RuntimeException ex) {
            System.err.println("No se pudo registrar la notificacion de reclamo: " + ex.getMessage());
        }
    }
}


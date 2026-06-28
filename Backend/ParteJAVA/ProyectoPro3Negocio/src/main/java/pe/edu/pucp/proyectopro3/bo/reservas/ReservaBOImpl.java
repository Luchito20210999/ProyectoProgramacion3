package pe.edu.pucp.proyectopro3.bo.reservas;

import pe.edu.pucp.proyectopro3.bo.BaseBO;
import pe.edu.pucp.proyectopro3.dao.TransactionsManager;
import pe.edu.pucp.proyectopro3.dao.reservas.DetalleReservaDAO;
import pe.edu.pucp.proyectopro3.dao.reservas.DetalleReservaDAOImpl;
import pe.edu.pucp.proyectopro3.dao.reservas.ReservaDAO;
import pe.edu.pucp.proyectopro3.dao.reservas.ReservaDAOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.reservas.DetalleReserva;
import pe.edu.pucp.proyectopro3.modelo.reservas.Reserva;

import java.util.Date;
import java.util.List;
import java.util.Objects;


public class ReservaBOImpl extends BaseBO implements ReservaBO {
    private final ReservaDAO reservaDao;
    private final DetalleReservaDAO detalleDao;

    public ReservaBOImpl() {
        this.reservaDao = new ReservaDAOImpl();
        this.detalleDao = new DetalleReservaDAOImpl();
    }


    @Override
    public Reserva consultarReserva(int idReserva) {
        validarIdPositivo(idReserva, "id de reserva");
        Reserva reserva = this.reservaDao.leer(idReserva);

        if (reserva == null) {
            throw new IllegalArgumentException("Error: No existe una reserva con el ID " + idReserva);
        }
        return reserva;
    }

    @Override
    public void modificarReserva(int idReserva) {
        validarIdPositivo(idReserva, "id de reserva");
        Reserva reserva = consultarReserva(idReserva);
        reserva.setFechaUltimaModificacion(new Date());

        if (!this.reservaDao.actualizar(reserva)) {
            throw new IllegalStateException("No se pudo registrar la modificación de la reserva");
        }
    }

    @Override
    public void anularReserva(int idReserva) {
        validarIdPositivo(idReserva, "id de reserva");
        Reserva reserva = consultarReserva(idReserva);

        reserva.setFechaUltimaModificacion(new Date());

        if (!this.reservaDao.actualizar(reserva)) {
            throw new IllegalStateException("No se pudo anular la reserva con ID: " + idReserva);
        }
    }

    @Override
    public void guardar(Reserva modelo, Estado estado) {
        validarReserva(modelo);
        validarEstado(estado);

        try {
            TransactionsManager.iniciarTransaccion();

            if (estado == Estado.Nuevo) {
                // 1. Guardar Cabecera
                int id = this.reservaDao.crear(modelo);
                if (id <= 0) throw new IllegalStateException("Error al crear cabecera de reserva");
                modelo.setIdReserva(id);

                // 2. Guardar Detalles vinculados al ID de la reserva
                for (DetalleReserva det : modelo.getDetalles()) {
                    det.setIdReserva(id);
                    this.detalleDao.crear(det);
                }
            } else if (estado == Estado.Modificado) {
                validarIdPositivo(modelo.getIdReserva(), "id de reserva");
                if (!this.reservaDao.actualizar(modelo)) {
                    throw new IllegalStateException("Error al actualizar la reserva");
                }
            }

            TransactionsManager.commitTransaccion();
        } catch (Exception ex) {
            TransactionsManager.rollbackTransaccion();
            throw new RuntimeException("Error en la operación de guardado: " + ex.getMessage(), ex);
        }
    }

    @Override
    public List<Reserva> listar() {
        return this.reservaDao.leerTodos();
    }

    @Override
    public Reserva obtener(int id) {
        validarIdPositivo(id, "id");
        return this.reservaDao.leer(id);
    }

    @Override
    public void eliminar(int id) {
        validarIdPositivo(id, "id");
        if (!this.reservaDao.eliminar(id)) {
            throw new IllegalStateException("No se pudo eliminar la reserva con id: " + id);
        }
    }



    private void validarReserva(Reserva r) {
        Objects.requireNonNull(r, "La reserva es obligatoria");
        validarIdPositivo(r.getIdCliente(), "id del cliente");

        if (r.getDetalles() == null || r.getDetalles().isEmpty()) {
            throw new IllegalArgumentException("La reserva debe tener al menos un detalle de servicio");
        }

        if (r.getMontoTotal() < 0) {
            throw new IllegalArgumentException("El monto total no puede ser negativo");
        }
    }
}

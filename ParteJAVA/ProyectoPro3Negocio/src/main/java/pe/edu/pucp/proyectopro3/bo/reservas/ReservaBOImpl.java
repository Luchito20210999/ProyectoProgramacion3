package pe.edu.pucp.proyectopro3.bo.reservas;

import pe.edu.pucp.proyectopro3.bo.BaseBO;
import pe.edu.pucp.proyectopro3.bo.notificaciones.NotificacionRegistro;
import pe.edu.pucp.proyectopro3.dao.TransactionsManager;
import pe.edu.pucp.proyectopro3.dao.crm.ClienteDAO;
import pe.edu.pucp.proyectopro3.dao.crm.ClienteDAOImpl;
import pe.edu.pucp.proyectopro3.dao.reservas.DetalleReservaDAO;
import pe.edu.pucp.proyectopro3.dao.reservas.DetalleReservaDAOImpl;
import pe.edu.pucp.proyectopro3.dao.reservas.ReservaDAO;
import pe.edu.pucp.proyectopro3.dao.reservas.ReservaDAOImpl;
import pe.edu.pucp.proyectopro3.dao.reservas.ServicioDAO;
import pe.edu.pucp.proyectopro3.dao.reservas.ServicioDAOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.crm.Cliente;
import pe.edu.pucp.proyectopro3.modelo.dto.ReservaDetalleDTO;
import pe.edu.pucp.proyectopro3.modelo.notificaciones.TipoEvento;
import pe.edu.pucp.proyectopro3.modelo.reservas.DetalleReserva;
import pe.edu.pucp.proyectopro3.modelo.reservas.EstadoReserva;
import pe.edu.pucp.proyectopro3.modelo.reservas.Reserva;
import pe.edu.pucp.proyectopro3.modelo.reservas.Servicio;

import java.util.Date;
import java.util.List;
import java.util.Locale;
import java.util.Objects;


public class ReservaBOImpl extends BaseBO implements ReservaBO {
    private static final double TASA_IMPUESTO = 0.18;
    private static final double FACTOR_TARIFA_EXTRANJERO = 1.20;

    private final ReservaDAO reservaDao;
    private final DetalleReservaDAO detalleDao;
    private final ServicioDAO servicioDao;
    private final ClienteDAO clienteDao;
    private final NotificacionRegistro notificacionRegistro;

    public ReservaBOImpl() {
        this.reservaDao = new ReservaDAOImpl();
        this.detalleDao = new DetalleReservaDAOImpl();
        this.servicioDao = new ServicioDAOImpl();
        this.clienteDao = new ClienteDAOImpl();
        this.notificacionRegistro = new NotificacionRegistro();
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
        calcularMontos(modelo);
        boolean esNuevaReserva = estado == Estado.Nuevo;
        boolean esAnulacion = estado == Estado.Modificado
                && modelo.getEstadoReserva() == EstadoReserva.RECHAZADO;

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

                for (DetalleReserva det : modelo.getDetalles()) {
                    if (det.getIdDetalle() > 0) {
                        det.setIdReserva(modelo.getIdReserva());
                        this.detalleDao.actualizar(det);
                    }
                }
            }

            TransactionsManager.commitTransaccion();
            notificarEventoReserva(modelo, esNuevaReserva, esAnulacion);
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
    public List<ReservaDetalleDTO> listarDetalle() {
        return this.reservaDao.listarDetalle();
    }

    @Override
    public Reserva obtener(int id) {
        validarIdPositivo(id, "id");
        return this.reservaDao.leer(id);
    }

    @Override
    public ReservaDetalleDTO obtenerDetalle(int idReserva) {
        validarIdPositivo(idReserva, "id de reserva");
        return this.reservaDao.obtenerDetalle(idReserva);
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

    private void calcularMontos(Reserva reserva) {
        double subtotalGeneral = 0.0;
        int cantidadTotal = 0;
        Cliente cliente = this.clienteDao.leer(reserva.getIdCliente());

        if (cliente == null) {
            throw new IllegalArgumentException("No existe el cliente con ID " + reserva.getIdCliente());
        }

        boolean clienteExtranjero = esClienteExtranjero(cliente);

        for (DetalleReserva detalle : reserva.getDetalles()) {
            Objects.requireNonNull(detalle, "El detalle de reserva no puede ser nulo");
            validarIdPositivo(detalle.getIdServicio(), "id del servicio");

            if (detalle.getCantidad() <= 0) {
                throw new IllegalArgumentException("La cantidad del detalle debe ser mayor que cero");
            }

            Servicio servicio = this.servicioDao.leer(detalle.getIdServicio());
            if (servicio == null) {
                throw new IllegalArgumentException("No existe el servicio con ID " + detalle.getIdServicio());
            }

            if (servicio.getCapacidadMaxima() > 0 && detalle.getCantidad() > servicio.getCapacidadMaxima()) {
                throw new IllegalArgumentException("La reserva excede la capacidad maxima del servicio "
                        + servicio.getNombre());
            }

            double precioAplicado = servicio.getPrecioUSD();
            if (clienteExtranjero) {
                precioAplicado = precioAplicado * FACTOR_TARIFA_EXTRANJERO;
            }

            double subtotal = redondear(detalle.getCantidad() * precioAplicado);
            detalle.setSubtotal(subtotal);
            subtotalGeneral += subtotal;
            cantidadTotal += detalle.getCantidad();
        }

        double impuestos = 0.0;
        reserva.setCantidadBoletos(cantidadTotal);
        reserva.setMontoImpuestos(impuestos);
        reserva.setMontoTotal(redondear(subtotalGeneral));
    }

    private double redondear(double valor) {
        return Math.round(valor * 100.0) / 100.0;
    }

    private boolean esClienteExtranjero(Cliente cliente) {
        String nacionalidad = cliente.getNacionalidad();

        if (nacionalidad == null || nacionalidad.trim().isEmpty()) {
            return false;
        }

        String valor = nacionalidad.trim().toUpperCase(Locale.ROOT);
        return !valor.equals("PE")
                && !valor.equals("PERU")
                && !valor.equals("PERUANO")
                && !valor.equals("PERUANA");
    }

    private void notificarEventoReserva(Reserva reserva, boolean esNuevaReserva, boolean esAnulacion) {
        if (!esNuevaReserva && !esAnulacion) {
            return;
        }

        try {
            TipoEvento tipoEvento = esAnulacion
                    ? TipoEvento.ANULACION_RESERVA
                    : TipoEvento.NUEVA_RESERVA;
            String accion = esAnulacion ? "anulada" : "registrada";
            String codigo = codigoReserva(reserva);
            this.notificacionRegistro.registrarEventoOperativo(
                    tipoEvento,
                    "Reserva " + codigo + " " + accion + ".",
                    reserva.getIdUsuario());
        } catch (RuntimeException ex) {
            System.err.println("No se pudo registrar la notificacion de reserva: " + ex.getMessage());
        }
    }

    private String codigoReserva(Reserva reserva) {
        if (reserva.getCodigoBokun() != null && !reserva.getCodigoBokun().isBlank()) {
            return reserva.getCodigoBokun();
        }
        return "RES-" + reserva.getIdReserva();
    }
}

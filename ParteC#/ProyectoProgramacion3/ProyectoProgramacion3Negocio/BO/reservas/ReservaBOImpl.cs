using ProyectoProgramacion3Model.Model;
using ProyectoProgramacion3Model.Model.reservas;
using ProyectoProgramacion3Persistencia.DAO;
using ProyectoProgramacion3Persistencia.DAO.reservas;

namespace ProyectoProgramacion3Negocio.BO.reservas;

public class ReservaBOImpl : BaseBO, IReservaBO
{
    private readonly IReservaDao reservaDao;
    private readonly IDetalleReservaDao detalleDao;

    public ReservaBOImpl()
    {
        reservaDao = new ReservaDaoImpl();
        detalleDao = new DetalleReservaDaoImpl();
    }

    public Reserva ConsultarReserva(int idReserva)
    {
        ValidarIdPositivo(idReserva, "id de reserva");
        return reservaDao.Leer(idReserva)
            ?? throw new ArgumentException($"Error: No existe una reserva con el ID {idReserva}");
    }

    public void ModificarReserva(int idReserva)
    {
        ValidarIdPositivo(idReserva, "id de reserva");
        var reserva = ConsultarReserva(idReserva);
        reserva.fechaUltimaModificacion = DateTime.Now;

        if (!reservaDao.Actualizar(reserva))
        {
            throw new InvalidOperationException("No se pudo registrar la modificacion de la reserva");
        }
    }

    public void AnularReserva(int idReserva)
    {
        ValidarIdPositivo(idReserva, "id de reserva");
        var reserva = ConsultarReserva(idReserva);
        reserva.fechaUltimaModificacion = DateTime.Now;

        if (!reservaDao.Actualizar(reserva))
        {
            throw new InvalidOperationException($"No se pudo anular la reserva con ID: {idReserva}");
        }
    }

    public void Guardar(Reserva modelo, Estado estado)
    {
        ValidarReserva(modelo);
        ValidarEstado(estado);

        try
        {
            TransactionsManager.IniciarTransaccion();

            if (estado == Estado.Nuevo)
            {
                var id = reservaDao.Crear(modelo);
                if (id <= 0)
                {
                    throw new InvalidOperationException("Error al crear cabecera de reserva");
                }

                modelo.idReserva = id;

                foreach (var detalle in modelo.detalles)
                {
                    detalle.idReserva = id;
                    var detalleId = detalleDao.Crear(detalle);
                    if (detalleId <= 0)
                    {
                        throw new InvalidOperationException("Error al crear detalle de reserva");
                    }

                    detalle.idDetalleReserva = detalleId;
                }
            }
            else if (estado == Estado.Modificado)
            {
                ValidarIdPositivo(modelo.idReserva, "id de reserva");
                if (!reservaDao.Actualizar(modelo))
                {
                    throw new InvalidOperationException("Error al actualizar la reserva");
                }
            }

            TransactionsManager.CommitTransaccion();
        }
        catch (Exception ex)
        {
            if (TransactionsManager.HayTransaccionActiva())
            {
                TransactionsManager.RollbackTransaccion();
            }

            throw new InvalidOperationException($"Error en la operacion de guardado: {ex.Message}", ex);
        }
    }

    public List<Reserva> Listar() => reservaDao.LeerTodos();

    public Reserva? Obtener(int id)
    {
        ValidarIdPositivo(id, "id");
        return reservaDao.Leer(id);
    }

    public void Eliminar(int id)
    {
        ValidarIdPositivo(id, "id");
        if (!reservaDao.Eliminar(id))
        {
            throw new InvalidOperationException($"No se pudo eliminar la reserva con id: {id}");
        }
    }

    private static void ValidarReserva(Reserva reserva)
    {
        ArgumentNullException.ThrowIfNull(reserva);
        ValidarIdPositivo(reserva.idCliente, "id del cliente");

        if (reserva.detalles is null || reserva.detalles.Count == 0)
        {
            throw new ArgumentException("La reserva debe tener al menos un detalle de servicio");
        }

        if (reserva.montoTotal < 0)
        {
            throw new ArgumentException("El monto total no puede ser negativo");
        }
    }
}

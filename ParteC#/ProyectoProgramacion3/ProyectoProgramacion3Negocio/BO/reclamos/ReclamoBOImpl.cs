using ProyectoProgramacion3Model.Model;
using ProyectoProgramacion3Model.Model.reclamos;
using ProyectoProgramacion3Persistencia.DAO.reclamos;

namespace ProyectoProgramacion3Negocio.BO.reclamos;

public class ReclamoBOImpl : BaseBO, IReclamoBO
{
    private readonly IReclamoDao reclamoDao;

    public ReclamoBOImpl()
    {
        reclamoDao = new ReclamoDaoImpl();
    }

    public void RegistrarReclamo(Reclamo reclamo, int idReserva)
    {
        ValidarReclamo(reclamo);
        ValidarIdPositivo(idReserva, "id de reserva");

        reclamo.idReserva = idReserva;
        reclamo.estadoReclamo = EstadoReclamo.PENDIENTE;
        reclamo.fechaReclamo = DateOnly.FromDateTime(DateTime.Today);

        var id = reclamoDao.Crear(reclamo);
        if (id <= 0)
        {
            throw new InvalidOperationException("Error al registrar el reclamo en el sistema.");
        }

        reclamo.idReclamo = id;
    }

    public Reclamo ConsultarReclamo(int idReclamo)
    {
        ValidarIdPositivo(idReclamo, "id de reclamo");
        return reclamoDao.Leer(idReclamo)
            ?? throw new ArgumentException($"No existe un reclamo con el ID {idReclamo}");
    }

    public void EliminarReclamo(int idReclamo)
    {
        ValidarIdPositivo(idReclamo, "id de reclamo");
        if (!reclamoDao.Eliminar(idReclamo))
        {
            throw new InvalidOperationException("No se pudo eliminar el reclamo.");
        }
    }

    public void AtenderReclamo(int idReclamo)
    {
        var reclamo = ConsultarReclamo(idReclamo);

        if (reclamo.estadoReclamo != EstadoReclamo.PENDIENTE)
        {
            throw new InvalidOperationException("Solo se pueden atender reclamos en estado PENDIENTE.");
        }

        reclamo.estadoReclamo = EstadoReclamo.EN_ATENCION;
        if (!reclamoDao.Actualizar(reclamo))
        {
            throw new InvalidOperationException("Error al actualizar el estado a En Atencion.");
        }
    }

    public void EvaluarProcedencia(int idReclamo, bool procede)
    {
        var reclamo = ConsultarReclamo(idReclamo);

        if (reclamo.estadoReclamo != EstadoReclamo.EN_ATENCION)
        {
            throw new InvalidOperationException("El reclamo debe estar EN_ATENCION para ser evaluado.");
        }

        reclamo.estadoReclamo = procede ? EstadoReclamo.PROCEDE : EstadoReclamo.NO_PROCEDE;
        reclamo.fechaResolucion = DateOnly.FromDateTime(DateTime.Today);
        ValidarTextoObligatorio(reclamo.motivoResolucion, "motivo de resolucion");

        if (!reclamoDao.Actualizar(reclamo))
        {
            throw new InvalidOperationException("Error al registrar la evaluacion del reclamo.");
        }
    }

    public void Guardar(Reclamo modelo, Estado estado)
    {
        if (estado == Estado.Nuevo)
        {
            throw new NotSupportedException("Para reclamos nuevos, use RegistrarReclamo()");
        }

        if (estado == Estado.Modificado)
        {
            ValidarReclamo(modelo);
            if (!reclamoDao.Actualizar(modelo))
            {
                throw new InvalidOperationException("Error al actualizar el reclamo");
            }
        }
    }

    public List<Reclamo> Listar() => reclamoDao.LeerTodos();
    public Reclamo? Obtener(int id) => ConsultarReclamo(id);
    public void Eliminar(int id) => EliminarReclamo(id);

    private static void ValidarReclamo(Reclamo reclamo)
    {
        ArgumentNullException.ThrowIfNull(reclamo);
        ValidarTextoObligatorio(reclamo.descripcion, "descripcion del reclamo");
    }
}

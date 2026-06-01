using ProyectoProgramacion3Model.Model;
using ProyectoProgramacion3Model.Model.notificaciones;
using ProyectoProgramacion3Persistencia.DAO.notificaciones;

namespace ProyectoProgramacion3Negocio.BO.notificaciones;

public class NotificacionBOImpl : BaseBO, INotificacionBO
{
    private readonly INotificacionDao notificacionDao;

    public NotificacionBOImpl()
    {
        notificacionDao = new NotificacionDaoImpl();
    }

    public List<Notificacion> Listar() => notificacionDao.LeerTodos();

    public Notificacion? Obtener(int id)
    {
        ValidarIdPositivo(id, "id de notificacion");
        return notificacionDao.Leer(id);
    }

    public void Eliminar(int id)
    {
        ValidarIdPositivo(id, "id de notificacion");
        if (!notificacionDao.Eliminar(id))
        {
            throw new InvalidOperationException($"No se pudo eliminar la notificacion con id: {id}");
        }
    }

    public void Guardar(Notificacion modelo, Estado estado)
    {
        ValidarEstado(estado);
        ValidarNotificacion(modelo);

        if (estado == Estado.Nuevo)
        {
            var id = notificacionDao.Crear(modelo);
            if (id <= 0)
            {
                throw new InvalidOperationException("Error al registrar la notificacion");
            }

            modelo.idNotificacion = id;
        }
        else if (estado == Estado.Modificado)
        {
            ValidarIdPositivo(modelo.idNotificacion, "id de notificacion");
            if (!notificacionDao.Actualizar(modelo))
            {
                throw new InvalidOperationException("No se pudo actualizar la notificacion.");
            }
        }
    }

    private static void ValidarNotificacion(Notificacion modelo)
    {
        ArgumentNullException.ThrowIfNull(modelo);
        ValidarTextoObligatorio(modelo.mensaje, "mensaje de la notificacion");
        ValidarDateOnly(modelo.fechaEnvio, "fecha de envio");
        ValidarIdPositivo(modelo.idUsuario, "id de usuario de la notificacion");
    }
}

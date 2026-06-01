using ProyectoProgramacion3Model.Model;
using ProyectoProgramacion3Model.Model.reservas;
using ProyectoProgramacion3Persistencia.DAO.reservas;

namespace ProyectoProgramacion3Negocio.BO.reservas;

public class ServicioBOImpl : BaseBO, IServicioBO
{
    private readonly IServicioDao servicioDao;

    public ServicioBOImpl()
    {
        servicioDao = new ServicioDaoImpl();
    }

    public List<Servicio> Listar() => servicioDao.LeerTodos();

    public Servicio? Obtener(int id)
    {
        ValidarIdPositivo(id, "id del servicio");
        return servicioDao.Leer(id);
    }

    public void Eliminar(int id)
    {
        ValidarIdPositivo(id, "id del servicio");
        if (!servicioDao.Eliminar(id))
        {
            throw new InvalidOperationException($"No se pudo eliminar el servicio con id: {id}");
        }
    }

    public void Guardar(Servicio modelo, Estado estado)
    {
        ValidarServicio(modelo);
        ValidarEstado(estado);

        if (estado == Estado.Nuevo)
        {
            var id = servicioDao.Crear(modelo);
            if (id <= 0)
            {
                throw new InvalidOperationException("No se pudo registrar el nuevo servicio");
            }

            modelo.idServicio = id;
        }
        else if (estado == Estado.Modificado)
        {
            ValidarIdPositivo(modelo.idServicio, "id del servicio");
            if (!servicioDao.Actualizar(modelo))
            {
                throw new InvalidOperationException($"No se pudo actualizar el servicio con id: {modelo.idServicio}");
            }
        }
        else
        {
            throw new ArgumentException($"Estado no soportado: {estado}");
        }
    }

    private static void ValidarServicio(Servicio modelo)
    {
        ArgumentNullException.ThrowIfNull(modelo);
        ValidarTextoObligatorio(modelo.nombre, "nombre del servicio");
        ValidarTextoObligatorio(modelo.descripcion, "descripcion del servicio");

        if (modelo.precioUSD < 0)
        {
            throw new ArgumentException("El costo del servicio no puede ser negativo");
        }
    }
}

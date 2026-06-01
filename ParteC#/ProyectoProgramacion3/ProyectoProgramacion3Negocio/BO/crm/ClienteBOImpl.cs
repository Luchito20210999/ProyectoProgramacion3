using ProyectoProgramacion3Model.Model;
using ProyectoProgramacion3Model.Model.crm;
using ProyectoProgramacion3Persistencia.DAO.crm;

namespace ProyectoProgramacion3Negocio.BO.crm;

public class ClienteBOImpl : BaseBO, IClienteBO
{
    private readonly IClienteDao clienteDao;

    public ClienteBOImpl()
    {
        clienteDao = new ClienteDaoImpl();
    }

    public List<Cliente> Listar() => clienteDao.LeerTodos();

    public Cliente? Obtener(int id)
    {
        ValidarIdPositivo(id, "id del cliente");
        return clienteDao.Leer(id);
    }

    public void Eliminar(int id)
    {
        ValidarIdPositivo(id, "id del cliente");
        if (!clienteDao.Eliminar(id))
        {
            throw new InvalidOperationException($"No se pudo eliminar el cliente con id: {id}");
        }
    }

    public void Guardar(Cliente modelo, Estado estado)
    {
        ValidarCliente(modelo);
        ValidarEstado(estado);

        if (estado == Estado.Nuevo)
        {
            var id = clienteDao.Crear(modelo);
            if (id <= 0)
            {
                throw new InvalidOperationException("No se pudo registrar el nuevo cliente");
            }

            modelo.idCliente = id;
        }
        else if (estado == Estado.Modificado)
        {
            ValidarIdPositivo(modelo.idCliente, "id del cliente");
            if (!clienteDao.Actualizar(modelo))
            {
                throw new InvalidOperationException($"No se pudo actualizar el cliente con id: {modelo.idCliente}");
            }
        }
    }

    private static void ValidarCliente(Cliente modelo)
    {
        ArgumentNullException.ThrowIfNull(modelo);
        ValidarTextoObligatorio(modelo.nombres, "nombres del cliente");
        ValidarTextoObligatorio(modelo.apellidos, "apellidos del cliente");
        ValidarTextoObligatorio(modelo.numeroDocumento, "numero de documento");

        if (!string.IsNullOrWhiteSpace(modelo.correo) && !modelo.correo.Contains('@'))
        {
            throw new ArgumentException("El formato del correo electronico es invalido");
        }

        ValidarDateOnly(modelo.fechaRegistro, "fecha de registro");
    }
}

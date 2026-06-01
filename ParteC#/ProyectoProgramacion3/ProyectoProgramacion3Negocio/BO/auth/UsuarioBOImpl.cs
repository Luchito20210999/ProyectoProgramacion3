using ProyectoProgramacion3Model.Model;
using ProyectoProgramacion3Model.Model.auth;
using ProyectoProgramacion3Persistencia.DAO.auth;

namespace ProyectoProgramacion3Negocio.BO.auth;

public class UsuarioBOImpl : BaseBO, IUsuarioBO
{
    private readonly IUsuarioDao usuarioDao;

    public UsuarioBOImpl()
    {
        usuarioDao = new UsuarioDaoImpl();
    }

    public void CrearUsuario(Usuario usuario)
    {
        ValidarUsuario(usuario);
        var id = usuarioDao.Crear(usuario);
        if (id <= 0)
        {
            throw new InvalidOperationException("Error al registrar el usuario en el sistema.");
        }

        usuario.idUsuario = id;
    }

    public void EditarUsuario(int id)
    {
        ValidarIdPositivo(id, "id de usuario");
        var usuario = usuarioDao.Leer(id) ?? throw new ArgumentException($"No existe un usuario con el ID {id}");
        if (!usuarioDao.Actualizar(usuario))
        {
            throw new InvalidOperationException($"No se pudo actualizar el usuario con ID: {id}");
        }
    }

    public void EliminarUsuario(int id)
    {
        ValidarIdPositivo(id, "id de usuario");
        if (!usuarioDao.Eliminar(id))
        {
            throw new InvalidOperationException("No se pudo eliminar el usuario.");
        }
    }

    public void BuscarUsuario(int id)
    {
        ValidarIdPositivo(id, "id de usuario");
        if (usuarioDao.Leer(id) is null)
        {
            throw new ArgumentException($"No existe un usuario con el ID {id}");
        }
    }

    public List<Usuario> Listar() => usuarioDao.LeerTodos();

    public Usuario? Obtener(int id)
    {
        ValidarIdPositivo(id, "id de usuario");
        return usuarioDao.Leer(id);
    }

    public void Eliminar(int id) => EliminarUsuario(id);

    public void Guardar(Usuario modelo, Estado estado)
    {
        ValidarUsuario(modelo);
        ValidarEstado(estado);

        if (estado == Estado.Nuevo)
        {
            var id = usuarioDao.Crear(modelo);
            if (id <= 0)
            {
                throw new InvalidOperationException("No se pudo registrar el nuevo usuario");
            }

            modelo.idUsuario = id;
        }
        else if (estado == Estado.Modificado)
        {
            ValidarIdPositivo(modelo.idUsuario, "id de usuario");
            if (!usuarioDao.Actualizar(modelo))
            {
                throw new InvalidOperationException($"No se pudo actualizar el usuario con id: {modelo.idUsuario}");
            }
        }
    }

    private static void ValidarUsuario(Usuario usuario)
    {
        ArgumentNullException.ThrowIfNull(usuario);
        ValidarTextoObligatorio(usuario.nombres, "nombres del usuario");
        ValidarTextoObligatorio(usuario.apellidos, "apellidos del usuario");
        ValidarTextoObligatorio(usuario.numeroDocumento, "numero de documento");
        ValidarTextoObligatorio(usuario.contrasena, "contrasena del usuario");

        if (!string.IsNullOrWhiteSpace(usuario.correo) && !usuario.correo.Contains('@'))
        {
            throw new ArgumentException("El formato del correo electronico es invalido");
        }
    }
}

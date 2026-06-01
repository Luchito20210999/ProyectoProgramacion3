using System.Data;
using System.Data.Common;
using ProyectoProgramacion3Model.Model.auth;
using ProyectoProgramacion3Model.Model.crm;

namespace ProyectoProgramacion3Persistencia.DAO.auth;

public class UsuarioDaoImpl : DefaultBaseDao<Usuario>, IUsuarioDao
{
    protected override DbCommand ComandoCrear(DbConnection conn, Usuario modelo)
    {
        var cmd = CrearStoredProcedure(conn, "sp_InsertUsuario");
        SetCamposUsuario(cmd, modelo);
        CrearParametroSalida(cmd, "_id_generado", DbType.Int32);
        return cmd;
    }

    protected override DbCommand ComandoActualizar(DbConnection conn, Usuario modelo)
    {
        var cmd = CrearStoredProcedure(conn, "sp_UpdateUsuario");
        CrearParametro(cmd, "p_id_usuario", modelo.idUsuario);
        SetCamposUsuario(cmd, modelo);
        return cmd;
    }

    protected override DbCommand ComandoEliminar(DbConnection conn, int id)
    {
        var cmd = CrearStoredProcedure(conn, "sp_DeleteUsuario");
        CrearParametro(cmd, "p_id_usuario", id);
        return cmd;
    }

    protected override DbCommand ComandoLeer(DbConnection conn, int id)
    {
        var cmd = CrearStoredProcedure(conn, "sp_ListUsuarioById");
        CrearParametro(cmd, "p_id_usuario", id);
        return cmd;
    }

    protected override DbCommand ComandoLeerTodos(DbConnection conn)
    {
        return CrearStoredProcedure(conn, "spListUsuarios");
    }

    protected override Usuario MapearModelo(DbDataReader reader)
    {
        return new Usuario
        {
            idUsuario = LeerEntero(reader, "id_usuario"),
            nombres = LeerTexto(reader, "nombres"),
            apellidos = LeerTexto(reader, "apellidos"),
            tipoDocumento = Enum.Parse<TipoDocumento>(LeerTexto(reader, "tipo_documento")),
            numeroDocumento = LeerTexto(reader, "numero_documento"),
            correo = LeerTexto(reader, "correo"),
            contrasena = LeerTexto(reader, "contrasena"),
            numeroContacto = LeerTexto(reader, "numero_contacto")
        };
    }

    private void SetCamposUsuario(DbCommand cmd, Usuario modelo)
    {
        CrearParametro(cmd, "p_nombres", modelo.nombres);
        CrearParametro(cmd, "p_apellidos", modelo.apellidos);
        CrearParametro(cmd, "p_tipo_documento", modelo.tipoDocumento.ToString());
        CrearParametro(cmd, "p_numero_documento", modelo.numeroDocumento);
        CrearParametro(cmd, "p_correo", modelo.correo);
        CrearParametro(cmd, "p_contrasena", modelo.contrasena);
        CrearParametro(cmd, "p_numero_contacto", modelo.numeroContacto);
        CrearParametro(cmd, "p_tipo_usuario", ObtenerTipoUsuario(modelo));
    }

    private static string ObtenerTipoUsuario(Usuario usuario)
    {
        if (usuario is Administrador)
        {
            return "ADMINISTRADOR";
        }

        if (usuario is Operador)
        {
            return "OPERADOR";
        }

        if (usuario is Analista)
        {
            return "ANALISTA";
        }

        return "USUARIO";
    }
}

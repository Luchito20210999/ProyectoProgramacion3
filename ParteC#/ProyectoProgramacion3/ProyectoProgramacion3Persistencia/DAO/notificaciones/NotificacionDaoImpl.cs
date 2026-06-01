using System.Data;
using System.Data.Common;
using ProyectoProgramacion3Model.Model.notificaciones;

namespace ProyectoProgramacion3Persistencia.DAO.notificaciones;

public class NotificacionDaoImpl : DefaultBaseDao<Notificacion>, INotificacionDao
{
    protected override DbCommand ComandoCrear(DbConnection conn, Notificacion modelo)
    {
        var cmd = CrearStoredProcedure(conn, "sp_InsertNotificacion");
        SetCamposNotificacion(cmd, modelo);
        CrearParametroSalida(cmd, "_id_generado", DbType.Int32);
        return cmd;
    }

    protected override DbCommand ComandoActualizar(DbConnection conn, Notificacion modelo)
    {
        var cmd = CrearStoredProcedure(conn, "sp_UpdateNotificacion");
        CrearParametro(cmd, "p_id_notificacion", modelo.idNotificacion);
        SetCamposNotificacion(cmd, modelo);
        return cmd;
    }

    protected override DbCommand ComandoEliminar(DbConnection conn, int id)
    {
        var cmd = CrearStoredProcedure(conn, "sp_DeleteNotificacion");
        CrearParametro(cmd, "p_id_notificacion", id);
        return cmd;
    }

    protected override DbCommand ComandoLeer(DbConnection conn, int id)
    {
        var cmd = CrearStoredProcedure(conn, "sp_ListNotificacionById");
        CrearParametro(cmd, "p_id_notificacion", id);
        return cmd;
    }

    protected override DbCommand ComandoLeerTodos(DbConnection conn)
    {
        return CrearStoredProcedure(conn, "spListNotificaciones");
    }

    protected override Notificacion MapearModelo(DbDataReader reader)
    {
        return new Notificacion
        {
            idNotificacion = LeerEntero(reader, "id_notificacion"),
            mensaje = LeerTexto(reader, "mensaje"),
            tipoNotificacion = LeerTexto(reader, "tipo_notificacion"),
            fechaEnvio = LeerDateOnly(reader, "fecha_envio"),
            leido = LeerBoolYN(reader, "leido"),
            idUsuario = LeerEntero(reader, "id_usuario")
        };
    }

    private void SetCamposNotificacion(DbCommand cmd, Notificacion modelo)
    {
        CrearParametro(cmd, "p_mensaje", modelo.mensaje);
        CrearParametro(cmd, "p_tipo_notificacion", ObtenerTipoNotificacion(modelo));
        CrearParametro(cmd, "p_fecha_envio", DateOnlyParam(modelo.fechaEnvio), DbType.DateTime);
        CrearParametro(cmd, "p_leido", modelo.leido ? "Y" : "N");
        CrearParametro(cmd, "p_id_usuario", modelo.idUsuario);
    }

    private static string ObtenerTipoNotificacion(Notificacion modelo)
    {
        if (!string.IsNullOrWhiteSpace(modelo.tipoNotificacion))
        {
            return modelo.tipoNotificacion;
        }

        return modelo.tipoEvento.ToString() ?? "GENERAL";
    }
}

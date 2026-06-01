using System.Data;
using System.Data.Common;
using ProyectoProgramacion3Model.Model.Auditoria;

namespace ProyectoProgramacion3Persistencia.DAO.auditoria;

public class LogAuditoriaDaoImpl : DefaultBaseDao<LogAuditoria>, ILogAuditoriaDao
{
    protected override DbCommand ComandoCrear(DbConnection conn, LogAuditoria modelo)
    {
        var cmd = CrearStoredProcedure(conn, "sp_InsertLogAuditoria");
        CrearParametro(cmd, "p_descripcion", modelo.descripcion);
        CrearParametro(cmd, "p_accion", modelo.accion);
        CrearParametro(cmd, "p_fecha_registro", DateOnlyParam(modelo.fechaRegistro), DbType.DateTime);
        CrearParametro(cmd, "p_origenAccion", modelo.origenAccion);
        CrearParametro(cmd, "p_id_usuario", modelo.idUsuario);
        CrearParametroSalida(cmd, "_id_generado", DbType.Int32);
        return cmd;
    }

    protected override DbCommand ComandoActualizar(DbConnection conn, LogAuditoria modelo)
    {
        throw new NotSupportedException("No se permite actualizar registros de auditoria.");
    }

    protected override DbCommand ComandoEliminar(DbConnection conn, int id)
    {
        throw new NotSupportedException("No se permite eliminar registros de auditoria.");
    }

    protected override DbCommand ComandoLeer(DbConnection conn, int id)
    {
        var cmd = CrearStoredProcedure(conn, "sp_ListLogAuditoriaById");
        CrearParametro(cmd, "p_idLogAuditoria", id);
        return cmd;
    }

    protected override DbCommand ComandoLeerTodos(DbConnection conn)
    {
        return CrearStoredProcedure(conn, "spListLogAuditoria");
    }

    protected override LogAuditoria MapearModelo(DbDataReader reader)
    {
        return new LogAuditoria
        {
            idLogAuditoria = LeerEntero(reader, "idLogAuditoria"),
            descripcion = LeerTexto(reader, "descripcion"),
            accion = LeerTexto(reader, "accion"),
            fechaRegistro = LeerDateOnly(reader, "fecha_registro"),
            origenAccion = LeerTexto(reader, "origenAccion"),
            idUsuario = LeerEntero(reader, "id_usuario")
        };
    }
}

using System.Data;
using System.Data.Common;
using ProyectoProgramacion3Model.Model.reclamos;

namespace ProyectoProgramacion3Persistencia.DAO.reclamos;

public class ReclamoDaoImpl : DefaultBaseDao<Reclamo>, IReclamoDao
{
    protected override DbCommand ComandoCrear(DbConnection conn, Reclamo modelo)
    {
        var cmd = CrearStoredProcedure(conn, "sp_InsertReclamo");
        SetCamposReclamo(cmd, modelo);
        CrearParametroSalida(cmd, "_id_generado", DbType.Int32);
        return cmd;
    }

    protected override DbCommand ComandoActualizar(DbConnection conn, Reclamo modelo)
    {
        var cmd = CrearStoredProcedure(conn, "sp_UpdateReclamo");
        CrearParametro(cmd, "p_id_reclamo", modelo.idReclamo);
        SetCamposReclamo(cmd, modelo);
        return cmd;
    }

    protected override DbCommand ComandoEliminar(DbConnection conn, int id)
    {
        var cmd = CrearStoredProcedure(conn, "sp_DeleteReclamo");
        CrearParametro(cmd, "p_id_reclamo", id);
        return cmd;
    }

    protected override DbCommand ComandoLeer(DbConnection conn, int id)
    {
        var cmd = CrearStoredProcedure(conn, "sp_ListReclamoById");
        CrearParametro(cmd, "p_id_reclamo", id);
        return cmd;
    }

    protected override DbCommand ComandoLeerTodos(DbConnection conn)
    {
        return CrearStoredProcedure(conn, "spListReclamos");
    }

    protected override Reclamo MapearModelo(DbDataReader reader)
    {
        return new Reclamo
        {
            idReclamo = LeerEntero(reader, "id_reclamo"),
            fechaReclamo = LeerDateOnly(reader, "fecha_reclamo"),
            descripcion = LeerTexto(reader, "descripcion"),
            estadoReclamo = Enum.Parse<EstadoReclamo>(LeerTexto(reader, "estado_reclamo")),
            motivoResolucion = LeerTexto(reader, "motivo_resolucion"),
            fechaResolucion = LeerDateOnlyNullable(reader, "fecha_resolucion"),
            idUsuario = LeerEnteroNullable(reader, "id_usuario"),
            idReserva = LeerEntero(reader, "id_reserva")
        };
    }

    private void SetCamposReclamo(DbCommand cmd, Reclamo modelo)
    {
        CrearParametro(cmd, "p_fecha_reclamo", DateOnlyParam(modelo.fechaReclamo), DbType.DateTime);
        CrearParametro(cmd, "p_descripcion", modelo.descripcion);
        CrearParametro(cmd, "p_estado_reclamo", modelo.estadoReclamo.ToString());
        CrearParametro(cmd, "p_motivo_resolucion", modelo.motivoResolucion);
        CrearParametro(cmd, "p_fecha_resolucion", DateOnlyParam(modelo.fechaResolucion), DbType.Date);
        CrearParametro(cmd, "p_id_usuario", modelo.idUsuario);
        CrearParametro(cmd, "p_id_reserva", modelo.idReserva);
    }
}

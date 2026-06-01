using System.Data;
using System.Data.Common;
using ProyectoProgramacion3Model.Model.reportes;

namespace ProyectoProgramacion3Persistencia.DAO.reportes;

public class ReporteReclamoDaoImpl : DefaultBaseDao<ReporteReclamo>, IReporteReclamoDao
{
    private const int DefaultIdUsuario = 1;

    protected override DbCommand ComandoCrear(DbConnection conn, ReporteReclamo modelo)
    {
        var cmd = CrearStoredProcedure(conn, "sp_InsertReporte_Reclamo");
        CrearParametro(cmd, "p_fecha_generacion", DateOnlyParam(modelo.fechaGeneracion), DbType.Date);
        CrearParametro(cmd, "p_fecha_inicio_filtro", DateOnlyParam(modelo.fechaInicioFiltro), DbType.Date);
        CrearParametro(cmd, "p_fecha_fin_filtro", DateOnlyParam(modelo.fechaFinFiltro), DbType.Date);
        CrearParametro(cmd, "p_cantidad_reservas", modelo.cantidadReservas);
        CrearParametro(cmd, "p_cantidad_reclamos", modelo.cantidadReclamos);
        CrearParametro(cmd, "p_porcentaje_incidencias", modelo.porcentajeIncidencias);
        CrearParametro(cmd, "p_total_procede", modelo.totalProcede);
        CrearParametro(cmd, "p_total_no_procede", modelo.totalNoProcede);
        CrearParametro(cmd, "p_total_pendientes", modelo.totalPendientes);
        CrearParametro(cmd, "p_id_usuario", DefaultIdUsuario);
        CrearParametroSalida(cmd, "_id_generado", DbType.Int32);
        return cmd;
    }

    protected override DbCommand ComandoActualizar(DbConnection conn, ReporteReclamo modelo)
    {
        var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Reporte_Reclamo SET fecha_generacion=@fechaGeneracion, fecha_inicio_filtro=@fechaInicio, fecha_fin_filtro=@fechaFin, cantidad_reservas=@reservas, cantidad_reclamos=@reclamos, porcentaje_incidencias=@porcentaje, total_procede=@procede, total_no_procede=@noProcede, total_pendientes=@pendientes WHERE id_reporte_reclamo=@id";
        CrearParametro(cmd, "@fechaGeneracion", DateOnlyParam(modelo.fechaGeneracion), DbType.Date);
        CrearParametro(cmd, "@fechaInicio", DateOnlyParam(modelo.fechaInicioFiltro), DbType.Date);
        CrearParametro(cmd, "@fechaFin", DateOnlyParam(modelo.fechaFinFiltro), DbType.Date);
        CrearParametro(cmd, "@reservas", modelo.cantidadReservas);
        CrearParametro(cmd, "@reclamos", modelo.cantidadReclamos);
        CrearParametro(cmd, "@porcentaje", modelo.porcentajeIncidencias);
        CrearParametro(cmd, "@procede", modelo.totalProcede);
        CrearParametro(cmd, "@noProcede", modelo.totalNoProcede);
        CrearParametro(cmd, "@pendientes", modelo.totalPendientes);
        CrearParametro(cmd, "@id", modelo.idReporte);
        return cmd;
    }

    protected override DbCommand ComandoEliminar(DbConnection conn, int id)
    {
        var cmd = CrearStoredProcedure(conn, "sp_DeleteReporte_Reclamo");
        CrearParametro(cmd, "p_id_reporte_reclamo", id);
        return cmd;
    }

    protected override DbCommand ComandoLeer(DbConnection conn, int id)
    {
        var cmd = CrearStoredProcedure(conn, "sp_ListReporteReclamoById");
        CrearParametro(cmd, "p_id_reporte_reclamo", id);
        return cmd;
    }

    protected override DbCommand ComandoLeerTodos(DbConnection conn)
    {
        return CrearStoredProcedure(conn, "spListReporteReclamo");
    }

    protected override ReporteReclamo MapearModelo(DbDataReader reader)
    {
        return new ReporteReclamo
        {
            idReporte = LeerEntero(reader, "id_reporte_reclamo"),
            fechaGeneracion = LeerDateOnly(reader, "fecha_generacion"),
            fechaInicioFiltro = LeerDateOnly(reader, "fecha_inicio_filtro"),
            fechaFinFiltro = LeerDateOnly(reader, "fecha_fin_filtro"),
            cantidadReservas = LeerEntero(reader, "cantidad_reservas"),
            cantidadReclamos = LeerEntero(reader, "cantidad_reclamos"),
            porcentajeIncidencias = LeerDouble(reader, "porcentaje_incidencias"),
            totalProcede = LeerEntero(reader, "total_procede"),
            totalNoProcede = LeerEntero(reader, "total_no_procede"),
            totalPendientes = LeerEntero(reader, "total_pendientes")
        };
    }
}

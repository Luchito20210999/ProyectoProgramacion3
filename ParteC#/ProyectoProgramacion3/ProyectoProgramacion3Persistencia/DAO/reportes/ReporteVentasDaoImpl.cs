using System.Data;
using System.Data.Common;
using ProyectoProgramacion3Model.Model.reportes;

namespace ProyectoProgramacion3Persistencia.DAO.reportes;

public class ReporteVentasDaoImpl : DefaultBaseDao<ReporteVentas>, IReporteVentasDao
{
    private const int DefaultIdUsuario = 1;

    protected override DbCommand ComandoCrear(DbConnection conn, ReporteVentas modelo)
    {
        var cmd = CrearStoredProcedure(conn, "sp_InsertReporte_Ventas");
        CrearParametro(cmd, "p_fecha_generacion", DateOnlyParam(modelo.fechaGeneracion), DbType.Date);
        CrearParametro(cmd, "p_fecha_inicio_filtro", DateOnlyParam(modelo.fechaInicioFiltro), DbType.Date);
        CrearParametro(cmd, "p_fecha_fin_filtro", DateOnlyParam(modelo.fechaFinFiltro), DbType.Date);
        CrearParametro(cmd, "p_cantidad_registros", modelo.totalVentas);
        CrearParametro(cmd, "p_monto_total", modelo.montoTotalGenerado);
        CrearParametro(cmd, "p_id_usuario", DefaultIdUsuario);
        CrearParametroSalida(cmd, "_id_generado", DbType.Int32);
        return cmd;
    }

    protected override DbCommand ComandoActualizar(DbConnection conn, ReporteVentas modelo)
    {
        var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Reporte_Ventas SET fecha_generacion=@fechaGeneracion, fecha_inicio_filtro=@fechaInicio, fecha_fin_filtro=@fechaFin, cantidad_registros=@cantidad, monto_total=@monto WHERE id_reporte_ventas=@id";
        CrearParametro(cmd, "@fechaGeneracion", DateOnlyParam(modelo.fechaGeneracion), DbType.Date);
        CrearParametro(cmd, "@fechaInicio", DateOnlyParam(modelo.fechaInicioFiltro), DbType.Date);
        CrearParametro(cmd, "@fechaFin", DateOnlyParam(modelo.fechaFinFiltro), DbType.Date);
        CrearParametro(cmd, "@cantidad", modelo.totalVentas);
        CrearParametro(cmd, "@monto", modelo.montoTotalGenerado);
        CrearParametro(cmd, "@id", modelo.idReporte);
        return cmd;
    }

    protected override DbCommand ComandoEliminar(DbConnection conn, int id)
    {
        var cmd = CrearStoredProcedure(conn, "sp_DeleteReporte_Ventas");
        CrearParametro(cmd, "p_id_reporte_ventas", id);
        return cmd;
    }

    protected override DbCommand ComandoLeer(DbConnection conn, int id)
    {
        var cmd = CrearStoredProcedure(conn, "sp_ListReporteVentasById");
        CrearParametro(cmd, "p_id_reporte_ventas", id);
        return cmd;
    }

    protected override DbCommand ComandoLeerTodos(DbConnection conn)
    {
        return CrearStoredProcedure(conn, "spListReporteVentas");
    }

    protected override ReporteVentas MapearModelo(DbDataReader reader)
    {
        return new ReporteVentas
        {
            idReporte = LeerEntero(reader, "id_reporte_ventas"),
            fechaGeneracion = LeerDateOnly(reader, "fecha_generacion"),
            fechaInicioFiltro = LeerDateOnly(reader, "fecha_inicio_filtro"),
            fechaFinFiltro = LeerDateOnly(reader, "fecha_fin_filtro"),
            totalVentas = LeerEntero(reader, "cantidad_registros"),
            montoTotalGenerado = LeerDouble(reader, "monto_total")
        };
    }
}

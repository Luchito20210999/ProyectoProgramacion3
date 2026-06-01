using System.Data;
using System.Data.Common;
using ProyectoProgramacion3Model.Model.reservas;

namespace ProyectoProgramacion3Persistencia.DAO.reservas;

public class DetalleReservaDaoImpl : DefaultBaseDao<DetalleReserva>, IDetalleReservaDao
{
    protected override DbCommand ComandoCrear(DbConnection conn, DetalleReserva modelo)
    {
        var cmd = CrearStoredProcedure(conn, "sp_InsertDetalle_Reserva");
        SetCamposDetalle(cmd, modelo);
        CrearParametroSalida(cmd, "_id_generado", DbType.Int32);
        return cmd;
    }

    protected override DbCommand ComandoActualizar(DbConnection conn, DetalleReserva modelo)
    {
        var cmd = CrearStoredProcedure(conn, "sp_UpdateDetalle_Reserva");
        CrearParametro(cmd, "p_id_detalle_reserva", modelo.idDetalleReserva);
        SetCamposDetalle(cmd, modelo);
        return cmd;
    }

    protected override DbCommand ComandoEliminar(DbConnection conn, int id)
    {
        var cmd = CrearStoredProcedure(conn, "sp_DeleteDetalle_Reserva");
        CrearParametro(cmd, "p_id_detalle_reserva", id);
        return cmd;
    }

    protected override DbCommand ComandoLeer(DbConnection conn, int id)
    {
        var cmd = CrearStoredProcedure(conn, "sp_ListDetalleReservaById");
        CrearParametro(cmd, "p_id_detalle_reserva", id);
        return cmd;
    }

    protected override DbCommand ComandoLeerTodos(DbConnection conn)
    {
        return CrearStoredProcedure(conn, "spListDetalleReserva");
    }

    protected override DetalleReserva MapearModelo(DbDataReader reader)
    {
        return new DetalleReserva
        {
            idDetalleReserva = LeerEntero(reader, "id_detalle_reserva"),
            idReserva = LeerEntero(reader, "id_reserva"),
            idServicio = LeerEntero(reader, "id_servicio"),
            cantidad = LeerEntero(reader, "cantidad"),
            subtotal = LeerDouble(reader, "subtotal")
        };
    }

    private void SetCamposDetalle(DbCommand cmd, DetalleReserva modelo)
    {
        CrearParametro(cmd, "p_id_reserva", modelo.idReserva);
        CrearParametro(cmd, "p_id_servicio", modelo.idServicio);
        CrearParametro(cmd, "p_cantidad", modelo.cantidad);
        CrearParametro(cmd, "p_subtotal", modelo.subtotal);
    }
}

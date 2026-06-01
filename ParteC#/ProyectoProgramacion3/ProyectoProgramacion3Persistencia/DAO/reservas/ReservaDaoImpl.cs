using System.Data;
using System.Data.Common;
using ProyectoProgramacion3Model.Model.reservas;

namespace ProyectoProgramacion3Persistencia.DAO.reservas;

public class ReservaDaoImpl : DefaultBaseDao<Reserva>, IReservaDao
{
    protected override DbCommand ComandoCrear(DbConnection conn, Reserva modelo)
    {
        var cmd = CrearStoredProcedure(conn, "sp_InsertReserva");
        SetCamposReserva(cmd, modelo);
        CrearParametroSalida(cmd, "_id_generado", DbType.Int32);
        return cmd;
    }

    protected override DbCommand ComandoActualizar(DbConnection conn, Reserva modelo)
    {
        var cmd = CrearStoredProcedure(conn, "sp_UpdateReserva");
        CrearParametro(cmd, "p_id_reserva", modelo.idReserva);
        SetCamposReserva(cmd, modelo);
        return cmd;
    }

    protected override DbCommand ComandoEliminar(DbConnection conn, int id)
    {
        var cmd = CrearStoredProcedure(conn, "sp_DeleteReserva");
        CrearParametro(cmd, "p_id_reserva", id);
        return cmd;
    }

    protected override DbCommand ComandoLeer(DbConnection conn, int id)
    {
        var cmd = CrearStoredProcedure(conn, "sp_ListReservaById");
        CrearParametro(cmd, "p_id_reserva", id);
        return cmd;
    }

    protected override DbCommand ComandoLeerTodos(DbConnection conn)
    {
        return CrearStoredProcedure(conn, "spListReservas");
    }

    protected override Reserva MapearModelo(DbDataReader reader)
    {
        return new Reserva
        {
            idReserva = LeerEntero(reader, "id_reserva"),
            fechaRegistro = LeerDateTime(reader, "fecha_registro"),
            estadoReserva = Enum.Parse<EstadoReserva>(LeerTexto(reader, "estado_reserva")),
            cantidadBoletos = LeerEntero(reader, "cantidad_boletos"),
            montoTotal = LeerDouble(reader, "monto_total"),
            fechaUltimaModificacion = LeerDateTime(reader, "fecha_ultima_modif"),
            canalVenta = LeerTexto(reader, "canal_venta"),
            montoImpuestos = LeerDouble(reader, "monto_impuestos"),
            codigoBokun = LeerTexto(reader, "codigo_bokun"),
            idUsuario = LeerEnteroNullable(reader, "id_usuario"),
            idCliente = LeerEntero(reader, "id_cliente")
        };
    }

    private void SetCamposReserva(DbCommand cmd, Reserva modelo)
    {
        CrearParametro(cmd, "p_fecha_registro", modelo.fechaRegistro, DbType.DateTime);
        CrearParametro(cmd, "p_estado_reserva", modelo.estadoReserva.ToString());
        CrearParametro(cmd, "p_cantidad_boletos", modelo.cantidadBoletos);
        CrearParametro(cmd, "p_monto_total", modelo.montoTotal);
        CrearParametro(cmd, "p_fecha_ultima_modif", modelo.fechaUltimaModificacion, DbType.DateTime);
        CrearParametro(cmd, "p_canal_venta", modelo.canalVenta);
        CrearParametro(cmd, "p_monto_impuestos", modelo.montoImpuestos);
        CrearParametro(cmd, "p_codigo_bokun", modelo.codigoBokun);
        CrearParametro(cmd, "p_id_usuario", modelo.idUsuario);
        CrearParametro(cmd, "p_id_cliente", modelo.idCliente);
    }
}

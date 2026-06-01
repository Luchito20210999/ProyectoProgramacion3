using System.Data;
using System.Data.Common;
using ProyectoProgramacion3Model.Model.reservas;

namespace ProyectoProgramacion3Persistencia.DAO.reservas;

public class ServicioDaoImpl : DefaultBaseDao<Servicio>, IServicioDao
{
    protected override DbCommand ComandoCrear(DbConnection conn, Servicio modelo)
    {
        var cmd = CrearStoredProcedure(conn, "sp_InsertServicio");
        SetCamposServicio(cmd, modelo);
        CrearParametroSalida(cmd, "_id_generado", DbType.Int32);
        return cmd;
    }

    protected override DbCommand ComandoActualizar(DbConnection conn, Servicio modelo)
    {
        var cmd = CrearStoredProcedure(conn, "sp_UpdateServicio");
        CrearParametro(cmd, "p_id_servicio", modelo.idServicio);
        SetCamposServicio(cmd, modelo);
        return cmd;
    }

    protected override DbCommand ComandoEliminar(DbConnection conn, int id)
    {
        var cmd = CrearStoredProcedure(conn, "sp_DeleteServicio");
        CrearParametro(cmd, "p_id_servicio", id);
        return cmd;
    }

    protected override DbCommand ComandoLeer(DbConnection conn, int id)
    {
        var cmd = CrearStoredProcedure(conn, "sp_ListServicioById");
        CrearParametro(cmd, "p_id_servicio", id);
        return cmd;
    }

    protected override DbCommand ComandoLeerTodos(DbConnection conn)
    {
        return CrearStoredProcedure(conn, "spListServicios");
    }

    protected override Servicio MapearModelo(DbDataReader reader)
    {
        return new Servicio
        {
            idServicio = LeerEntero(reader, "id_servicio"),
            nombre = LeerTexto(reader, "nombre"),
            descripcion = LeerTexto(reader, "descripcion"),
            precioUSD = LeerDouble(reader, "precio_usd"),
            duracionHoras = LeerDouble(reader, "duracion_horas"),
            idiomaGuia = LeerTexto(reader, "idioma_guia"),
            capacidadMaxima = LeerEntero(reader, "capacidad_maxima"),
            incluyeRecojo = LeerBoolYN(reader, "incluye_recojo"),
            ciudadDestino = LeerTexto(reader, "ciudad_destino")
        };
    }

    private void SetCamposServicio(DbCommand cmd, Servicio modelo)
    {
        CrearParametro(cmd, "p_nombre", modelo.nombre);
        CrearParametro(cmd, "p_descripcion", modelo.descripcion);
        CrearParametro(cmd, "p_precio_usd", modelo.precioUSD);
        CrearParametro(cmd, "p_duracion_horas", modelo.duracionHoras);
        CrearParametro(cmd, "p_idioma_guia", modelo.idiomaGuia);
        CrearParametro(cmd, "p_capacidad_maxima", modelo.capacidadMaxima);
        CrearParametro(cmd, "p_incluye_recojo", modelo.incluyeRecojo ? "Y" : "N");
        CrearParametro(cmd, "p_ciudad_destino", modelo.ciudadDestino);
    }
}

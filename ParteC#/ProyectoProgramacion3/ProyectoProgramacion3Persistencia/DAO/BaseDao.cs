using ProyectoProgramacion3DBManager.DB;
using System.Data;
using System.Data.Common;

namespace ProyectoProgramacion3Persistencia.DAO;

public abstract class BaseDao<M, I> : IPersistible<M, I>
{
    public virtual I Crear(M modelo)
    {
        return EjecutarComando(conn => EjecutarComandoCrear(conn, modelo));
    }

    public virtual bool Actualizar(M modelo)
    {
        return EjecutarComando(conn => EjecutarComandoActualizar(conn, modelo));
    }

    public virtual bool Eliminar(I id)
    {
        return EjecutarComando(conn => EjecutarComandoEliminar(conn, id));
    }

    public virtual M? Leer(I id)
    {
        return EjecutarComando(conn =>
        {
            using var cmd = ComandoLeer(conn, id);
            AdjuntarTransaccionActiva(cmd);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapearModelo(reader) : default;
        });
    }

    public virtual List<M> LeerTodos()
    {
        return EjecutarComando(conn =>
        {
            using var cmd = ComandoLeerTodos(conn);
            AdjuntarTransaccionActiva(cmd);
            using var reader = cmd.ExecuteReader();
            var modelos = new List<M>();
            while (reader.Read())
            {
                modelos.Add(MapearModelo(reader));
            }
            return modelos;
        });
    }

    protected T EjecutarComando<T>(ComandoDao<T> comando)
    {
        var conexionTransaccional = TransactionsManager.ObtenerConexionActual();
        if (conexionTransaccional is not null)
        {
            return comando(conexionTransaccional);
        }

        using var conn = DbFactoryProvider.GetManager().GetConnection();
        conn.Open();
        return comando(conn);
    }

    protected virtual I EjecutarComandoCrear(DbConnection conn, M modelo)
    {
        using var cmd = ComandoCrear(conn, modelo);
        AdjuntarTransaccionActiva(cmd);
        return cmd.ExecuteNonQuery() > 0 ? ExtraerIdDespuesDeCrear(cmd, conn) : default!;
    }

    protected virtual bool EjecutarComandoActualizar(DbConnection conn, M modelo)
    {
        using var cmd = ComandoActualizar(conn, modelo);
        AdjuntarTransaccionActiva(cmd);
        return cmd.ExecuteNonQuery() > 0;
    }

    protected virtual bool EjecutarComandoEliminar(DbConnection conn, I id)
    {
        using var cmd = ComandoEliminar(conn, id);
        AdjuntarTransaccionActiva(cmd);
        return cmd.ExecuteNonQuery() > 0;
    }

    protected void AdjuntarTransaccionActiva(DbCommand cmd)
    {
        var transaccion = TransactionsManager.ObtenerTransaccionActual();
        if (transaccion is not null)
        {
            cmd.Transaction = transaccion;
        }
    }

    protected DbParameter CrearParametro(DbCommand cmd, string name, object? value, DbType? dbType = null)
    {
        return CrearParametro(cmd, name, value, ParameterDirection.Input, dbType);
    }

    protected DbParameter CrearParametroSalida(DbCommand cmd, string name, DbType dbType)
    {
        return CrearParametro(cmd, name, DBNull.Value, ParameterDirection.Output, dbType);
    }

    protected DbParameter CrearParametro(DbCommand cmd, string name, object? value, ParameterDirection direction, DbType? dbType = null)
    {
        var parameter = cmd.CreateParameter();
        parameter.ParameterName = name;
        parameter.Direction = direction;
        parameter.Value = value ?? DBNull.Value;
        if (dbType.HasValue)
        {
            parameter.DbType = dbType.Value;
        }

        cmd.Parameters.Add(parameter);
        return parameter;
    }

    protected DbCommand CrearStoredProcedure(DbConnection conn, string nombre)
    {
        var cmd = conn.CreateCommand();
        cmd.CommandText = nombre;
        cmd.CommandType = CommandType.StoredProcedure;
        return cmd;
    }

    protected int LeerEntero(DbDataReader reader, string columnName) => Convert.ToInt32(reader[columnName]);
    protected int? LeerEnteroNullable(DbDataReader reader, string columnName) => reader[columnName] is DBNull ? null : Convert.ToInt32(reader[columnName]);
    protected double LeerDouble(DbDataReader reader, string columnName) => Convert.ToDouble(reader[columnName]);
    protected string LeerTexto(DbDataReader reader, string columnName) => Convert.ToString(reader[columnName]) ?? string.Empty;
    protected bool LeerBoolYN(DbDataReader reader, string columnName) => "Y".Equals(LeerTexto(reader, columnName), StringComparison.OrdinalIgnoreCase);
    protected DateOnly LeerDateOnly(DbDataReader reader, string columnName) => DateOnly.FromDateTime(Convert.ToDateTime(reader[columnName]));
    protected DateOnly LeerDateOnlyNullable(DbDataReader reader, string columnName) => reader[columnName] is DBNull ? default : DateOnly.FromDateTime(Convert.ToDateTime(reader[columnName]));
    protected DateTime LeerDateTime(DbDataReader reader, string columnName) => Convert.ToDateTime(reader[columnName]);
    protected object DateOnlyParam(DateOnly value) => value == default ? DBNull.Value : value.ToDateTime(TimeOnly.MinValue);

    protected abstract DbCommand ComandoCrear(DbConnection conn, M modelo);
    protected abstract DbCommand ComandoActualizar(DbConnection conn, M modelo);
    protected abstract DbCommand ComandoEliminar(DbConnection conn, I id);
    protected abstract DbCommand ComandoLeer(DbConnection conn, I id);
    protected abstract DbCommand ComandoLeerTodos(DbConnection conn);
    protected abstract M MapearModelo(DbDataReader reader);
    protected abstract I ExtraerIdDespuesDeCrear(DbCommand cmd, DbConnection conn);
}

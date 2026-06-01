using System.Data;
using System.Data.Common;
using ProyectoProgramacion3Model.Model.crm;

namespace ProyectoProgramacion3Persistencia.DAO.crm;

public class ClienteDaoImpl : DefaultBaseDao<Cliente>, IClienteDao
{
    protected override DbCommand ComandoCrear(DbConnection conn, Cliente modelo)
    {
        var cmd = CrearStoredProcedure(conn, "sp_InsertCliente");
        SetCamposCliente(cmd, modelo);
        CrearParametroSalida(cmd, "_id_generado", DbType.Int32);
        return cmd;
    }

    protected override DbCommand ComandoActualizar(DbConnection conn, Cliente modelo)
    {
        var cmd = CrearStoredProcedure(conn, "sp_UpdateCliente");
        CrearParametro(cmd, "p_id_cliente", modelo.idCliente);
        SetCamposCliente(cmd, modelo);
        return cmd;
    }

    protected override DbCommand ComandoEliminar(DbConnection conn, int id)
    {
        var cmd = CrearStoredProcedure(conn, "sp_DeleteCliente");
        CrearParametro(cmd, "p_id_cliente", id);
        return cmd;
    }

    protected override DbCommand ComandoLeer(DbConnection conn, int id)
    {
        var cmd = CrearStoredProcedure(conn, "sp_ListClienteById");
        CrearParametro(cmd, "p_id_cliente", id);
        return cmd;
    }

    protected override DbCommand ComandoLeerTodos(DbConnection conn)
    {
        return CrearStoredProcedure(conn, "spListClientes");
    }

    protected override Cliente MapearModelo(DbDataReader reader)
    {
        return new Cliente
        {
            idCliente = LeerEntero(reader, "id_cliente"),
            nombres = LeerTexto(reader, "nombres"),
            apellidos = LeerTexto(reader, "apellidos"),
            tipoDocumento = Enum.Parse<TipoDocumento>(LeerTexto(reader, "tipo_documento")),
            numeroDocumento = LeerTexto(reader, "numero_documento"),
            correo = LeerTexto(reader, "correo"),
            nacionalidad = LeerTexto(reader, "nacionalidad"),
            fechaRegistro = LeerDateOnly(reader, "fecha_registro"),
            numeroContacto = LeerTexto(reader, "numero_contacto"),
            fechaNacimiento = LeerDateOnlyNullable(reader, "fecha_nacimiento")
        };
    }

    private void SetCamposCliente(DbCommand cmd, Cliente modelo)
    {
        CrearParametro(cmd, "p_nombres", modelo.nombres);
        CrearParametro(cmd, "p_apellidos", modelo.apellidos);
        CrearParametro(cmd, "p_tipo_documento", modelo.tipoDocumento.ToString());
        CrearParametro(cmd, "p_numero_documento", modelo.numeroDocumento);
        CrearParametro(cmd, "p_correo", modelo.correo);
        CrearParametro(cmd, "p_nacionalidad", modelo.nacionalidad);
        CrearParametro(cmd, "p_fecha_registro", DateOnlyParam(modelo.fechaRegistro), DbType.Date);
        CrearParametro(cmd, "p_numero_contacto", modelo.numeroContacto);
        CrearParametro(cmd, "p_fecha_nacimiento", DateOnlyParam(modelo.fechaNacimiento), DbType.Date);
    }
}

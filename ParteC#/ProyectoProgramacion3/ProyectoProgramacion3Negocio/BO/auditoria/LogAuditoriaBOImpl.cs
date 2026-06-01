using ProyectoProgramacion3Model.Model;
using ProyectoProgramacion3Model.Model.Auditoria;
using ProyectoProgramacion3Persistencia.DAO.auditoria;

namespace ProyectoProgramacion3Negocio.BO.auditoria;

public class LogAuditoriaBOImpl : BaseBO, ILogAuditoriaBO
{
    private readonly ILogAuditoriaDao logDao;

    public LogAuditoriaBOImpl()
    {
        logDao = new LogAuditoriaDaoImpl();
    }

    public List<LogAuditoria> Listar() => logDao.LeerTodos();

    public LogAuditoria? Obtener(int id)
    {
        ValidarIdPositivo(id, "id de log de auditoria");
        return logDao.Leer(id);
    }

    public void Eliminar(int id)
    {
        throw new NotSupportedException("No se permite eliminar registros de auditoria.");
    }

    public void Guardar(LogAuditoria modelo, Estado estado)
    {
        ValidarEstado(estado);

        if (estado == Estado.Nuevo)
        {
            ValidarLog(modelo);
            var id = logDao.Crear(modelo);
            if (id <= 0)
            {
                throw new InvalidOperationException("Error al registrar el log de auditoria");
            }

            modelo.idLogAuditoria = id;
        }
    }

    private static void ValidarLog(LogAuditoria log)
    {
        ArgumentNullException.ThrowIfNull(log);
        ValidarTextoObligatorio(log.accion, "accion realizada");
        ValidarTextoObligatorio(log.descripcion, "descripcion del evento");
        ValidarTextoObligatorio(log.origenAccion, "origen de la accion");
        ValidarDateOnly(log.fechaRegistro, "fecha del evento");
    }
}

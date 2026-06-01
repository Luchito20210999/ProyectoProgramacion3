using ProyectoProgramacion3Model.Model;
using ProyectoProgramacion3Model.Model.reclamos;
using ProyectoProgramacion3Model.Model.reportes;
using ProyectoProgramacion3Persistencia.DAO.reclamos;
using ProyectoProgramacion3Persistencia.DAO.reportes;
using ProyectoProgramacion3Persistencia.DAO.reservas;

namespace ProyectoProgramacion3Negocio.BO.reportes;

public class ReporteReclamoBOImpl : BaseBO, IReporteReclamoBO
{
    private readonly IReporteReclamoDao reporteReclamoDao;
    private readonly IReclamoDao reclamoDao;
    private readonly IReservaDao reservaDao;

    public ReporteReclamoBOImpl()
    {
        reporteReclamoDao = new ReporteReclamoDaoImpl();
        reclamoDao = new ReclamoDaoImpl();
        reservaDao = new ReservaDaoImpl();
    }

    public List<ReporteReclamo> Listar() => reporteReclamoDao.LeerTodos();

    public ReporteReclamo? Obtener(int id)
    {
        ValidarIdPositivo(id, "id del reporte de reclamos");
        return reporteReclamoDao.Leer(id);
    }

    public void Eliminar(int id)
    {
        ValidarIdPositivo(id, "id del reporte de reclamos");
        if (!reporteReclamoDao.Eliminar(id))
        {
            throw new InvalidOperationException($"No se pudo eliminar el reporte de reclamos con id: {id}");
        }
    }

    public void Guardar(ReporteReclamo modelo, Estado estado)
    {
        ValidarReporteReclamo(modelo);
        ValidarEstado(estado);

        if (estado == Estado.Nuevo)
        {
            var id = reporteReclamoDao.Crear(modelo);
            if (id <= 0)
            {
                throw new InvalidOperationException("No se pudo registrar el reporte de reclamos");
            }

            modelo.idReporte = id;
        }
        else if (estado == Estado.Modificado)
        {
            ValidarIdPositivo(modelo.idReporte, "id del reporte de reclamos");
            if (!reporteReclamoDao.Actualizar(modelo))
            {
                throw new InvalidOperationException($"No se pudo actualizar el reporte de reclamos con id: {modelo.idReporte}");
            }
        }
    }

    public Reporte GenerarReporte(DateOnly fechaInicio, DateOnly fechaFin)
    {
        ValidarRangoFechas(fechaInicio, fechaFin);

        var reclamosFiltrados = reclamoDao.LeerTodos()
            .Where(r => EstaEnRango(r.fechaReclamo, fechaInicio, fechaFin))
            .ToList();

        var cantidadReservas = reservaDao.LeerTodos()
            .Count(r => EstaEnRango(DateOnly.FromDateTime(r.fechaRegistro), fechaInicio, fechaFin));

        var totalProcede = reclamosFiltrados.Count(r => r.estadoReclamo == EstadoReclamo.PROCEDE);
        var totalNoProcede = reclamosFiltrados.Count(r => r.estadoReclamo == EstadoReclamo.NO_PROCEDE);
        var totalPendientes = reclamosFiltrados.Count(r => r.estadoReclamo is EstadoReclamo.PENDIENTE or EstadoReclamo.EN_ATENCION);
        var cantidadReclamos = reclamosFiltrados.Count;
        var porcentajeIncidencias = cantidadReservas > 0 ? cantidadReclamos * 100.0 / cantidadReservas : 0.0;

        var reporte = new ReporteReclamo
        {
            fechaGeneracion = DateOnly.FromDateTime(DateTime.Today),
            fechaInicioFiltro = fechaInicio,
            fechaFinFiltro = fechaFin,
            reclamos = reclamosFiltrados,
            cantidadReservas = cantidadReservas,
            cantidadReclamos = cantidadReclamos,
            porcentajeIncidencias = porcentajeIncidencias,
            totalProcede = totalProcede,
            totalNoProcede = totalNoProcede,
            totalPendientes = totalPendientes
        };

        Guardar(reporte, Estado.Nuevo);
        return reporte;
    }

    public void ExportarDashboard()
    {
        throw new NotSupportedException("Funcionalidad de exportacion de dashboard pendiente de implementacion.");
    }

    private static void ValidarReporteReclamo(ReporteReclamo modelo)
    {
        ArgumentNullException.ThrowIfNull(modelo);
        ValidarDateOnly(modelo.fechaGeneracion, "fecha de generacion");

        if (modelo.cantidadReclamos < 0 ||
            modelo.porcentajeIncidencias < 0 ||
            modelo.totalProcede < 0 ||
            modelo.totalNoProcede < 0 ||
            modelo.totalPendientes < 0)
        {
            throw new ArgumentException("Los indicadores del reporte no pueden ser negativos");
        }
    }

    private static void ValidarRangoFechas(DateOnly fechaInicio, DateOnly fechaFin)
    {
        ValidarDateOnly(fechaInicio, "fecha de inicio");
        ValidarDateOnly(fechaFin, "fecha de fin");

        if (fechaInicio > fechaFin)
        {
            throw new ArgumentException("La fecha de inicio no puede ser posterior a la fecha de fin");
        }
    }

    private static bool EstaEnRango(DateOnly fecha, DateOnly inicio, DateOnly fin)
    {
        return fecha >= inicio && fecha <= fin;
    }
}

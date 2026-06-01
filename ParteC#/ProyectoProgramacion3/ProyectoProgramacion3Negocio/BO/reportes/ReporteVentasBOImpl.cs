using ProyectoProgramacion3Model.Model;
using ProyectoProgramacion3Model.Model.reportes;
using ProyectoProgramacion3Model.Model.reservas;
using ProyectoProgramacion3Persistencia.DAO.reportes;
using ProyectoProgramacion3Persistencia.DAO.reservas;

namespace ProyectoProgramacion3Negocio.BO.reportes;

public class ReporteVentasBOImpl : BaseBO, IReporteVentasBO
{
    private readonly IReporteVentasDao reporteVentasDao;
    private readonly IReservaDao reservaDao;

    public ReporteVentasBOImpl()
    {
        reporteVentasDao = new ReporteVentasDaoImpl();
        reservaDao = new ReservaDaoImpl();
    }

    public List<ReporteVentas> Listar() => reporteVentasDao.LeerTodos();

    public ReporteVentas? Obtener(int id)
    {
        ValidarIdPositivo(id, "id del reporte de ventas");
        return reporteVentasDao.Leer(id);
    }

    public void Eliminar(int id)
    {
        ValidarIdPositivo(id, "id del reporte de ventas");
        if (!reporteVentasDao.Eliminar(id))
        {
            throw new InvalidOperationException($"No se pudo eliminar el reporte de ventas con id: {id}");
        }
    }

    public void Guardar(ReporteVentas modelo, Estado estado)
    {
        ValidarReporteVentas(modelo);
        ValidarEstado(estado);

        if (estado == Estado.Nuevo)
        {
            var id = reporteVentasDao.Crear(modelo);
            if (id <= 0)
            {
                throw new InvalidOperationException("No se pudo registrar el reporte de ventas");
            }

            modelo.idReporte = id;
        }
        else if (estado == Estado.Modificado)
        {
            ValidarIdPositivo(modelo.idReporte, "id del reporte de ventas");
            if (!reporteVentasDao.Actualizar(modelo))
            {
                throw new InvalidOperationException($"No se pudo actualizar el reporte de ventas con id: {modelo.idReporte}");
            }
        }
    }

    public Reporte GenerarReporte(DateOnly fechaInicio, DateOnly fechaFin)
    {
        ValidarRangoFechas(fechaInicio, fechaFin);

        var reservasFiltradas = reservaDao.LeerTodos()
            .Where(r => EstaEnRango(DateOnly.FromDateTime(r.fechaRegistro), fechaInicio, fechaFin))
            .ToList();

        var reporte = new ReporteVentas
        {
            fechaGeneracion = DateOnly.FromDateTime(DateTime.Today),
            fechaInicioFiltro = fechaInicio,
            fechaFinFiltro = fechaFin,
            detalleVentas = reservasFiltradas,
            totalVentas = reservasFiltradas.Count,
            montoTotalGenerado = reservasFiltradas.Sum(r => r.montoTotal)
        };

        Guardar(reporte, Estado.Nuevo);
        return reporte;
    }

    public void ExportarDashboard()
    {
        throw new NotSupportedException("Funcionalidad de exportacion de dashboard pendiente de implementacion.");
    }

    private static void ValidarReporteVentas(ReporteVentas modelo)
    {
        ArgumentNullException.ThrowIfNull(modelo);
        ValidarDateOnly(modelo.fechaGeneracion, "fecha de generacion");

        if (modelo.totalVentas < 0)
        {
            throw new ArgumentException("El total de ventas no puede ser negativo");
        }

        if (modelo.montoTotalGenerado < 0)
        {
            throw new ArgumentException("El monto total generado no puede ser negativo");
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

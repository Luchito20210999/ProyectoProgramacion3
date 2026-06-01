using ProyectoProgramacion3Model.Model.reportes;

namespace ProyectoProgramacion3Negocio.BO.reportes;

public interface IReporteVentasBO : IGestionable<ReporteVentas>
{
    Reporte GenerarReporte(DateOnly fechaInicio, DateOnly fechaFin);
    void ExportarDashboard();
}

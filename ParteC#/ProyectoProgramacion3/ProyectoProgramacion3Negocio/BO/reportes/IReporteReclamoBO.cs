using ProyectoProgramacion3Model.Model.reportes;

namespace ProyectoProgramacion3Negocio.BO.reportes;

public interface IReporteReclamoBO : IGestionable<ReporteReclamo>
{
    Reporte GenerarReporte(DateOnly fechaInicio, DateOnly fechaFin);
    void ExportarDashboard();
}

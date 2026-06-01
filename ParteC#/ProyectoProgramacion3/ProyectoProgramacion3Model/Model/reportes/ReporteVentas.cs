using ProyectoProgramacion3Model.Model.reservas;

namespace ProyectoProgramacion3Model.Model.reportes;

public class ReporteVentas : Reporte
{
    public List<Reserva> detalleVentas { get; set; } = new List<Reserva>();
    public int totalVentas { get; set; }
    public double montoTotalGenerado { get; set; }
    public ReporteVentas(int idReporte, DateOnly fechaGeneracion, DateOnly fechaInicioFiltro, DateOnly fechaFinFiltro, List<Reserva> detalleVentas, int totalVentas, double montoTotalGenerado)
        : base(idReporte, fechaGeneracion, fechaInicioFiltro, fechaFinFiltro)
    {
        this.detalleVentas = detalleVentas;
        this.totalVentas = totalVentas;
        this.montoTotalGenerado = montoTotalGenerado;
    }
    public ReporteVentas()
    {
    }
}

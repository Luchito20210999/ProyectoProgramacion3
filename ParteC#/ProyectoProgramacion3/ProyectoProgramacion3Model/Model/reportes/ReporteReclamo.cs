using ProyectoProgramacion3Model.Model.reclamos;
namespace ProyectoProgramacion3Model.Model.reportes;

public class  ReporteReclamo : Reporte 
{
    public List<Reclamo> reclamos { get; set; } = new List<Reclamo>();
    public int cantidadReservas { get; set; }
    public int cantidadReclamos { get; set; }
    public double porcentajeIncidencias { get; set; }
    public int totalProcede { get; set; }
    public int totalNoProcede { get; set; }
    public int totalPendientes { get; set; }
    public ReporteReclamo(int idReporte, DateOnly fechaGeneracion, DateOnly fechaInicioFiltro, DateOnly fechaFinFiltro, List<Reclamo> reclamos,int totalProcede, int totalNoProcede, int totalPendientes) 
        : base(idReporte, fechaGeneracion, fechaInicioFiltro, fechaFinFiltro)
    {
        this.reclamos = reclamos;
        this.cantidadReservas = reclamos.Count; // Asumiendo que cada reclamo corresponde a una reserva
        this.cantidadReclamos = reclamos.Count;
        this.porcentajeIncidencias = (cantidadReclamos / (double)cantidadReservas) * 100;
        this.totalProcede = totalProcede;
        this.totalNoProcede = totalNoProcede;
        this.totalPendientes = totalPendientes;
    }
    public ReporteReclamo()
    {
    }
}

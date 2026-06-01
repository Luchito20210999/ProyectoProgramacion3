namespace ProyectoProgramacion3Model.Model.reportes;
public abstract class Reporte
{
    public int idReporte { get; set; }
    public DateOnly fechaGeneracion { get; set; }
    public DateOnly fechaInicioFiltro { get; set; }
    public DateOnly fechaFinFiltro { get; set; }
    public Reporte(int idReporte, DateOnly fechaGeneracion, DateOnly fechaInicioFiltro, DateOnly fechaFinFiltro)
    {
        this.idReporte = idReporte;
        this.fechaGeneracion = fechaGeneracion;
        this.fechaInicioFiltro = fechaInicioFiltro;
        this.fechaFinFiltro = fechaFinFiltro;
    }
    public Reporte() 
    { 
    }
}
namespace ProyectoProgramacion3Model.Model.reclamos;
public class Reclamo{
    public int idReclamo { get; set; }
    public DateOnly fechaReclamo { get; set; }
    public string descripcion { get; set; } = string.Empty;
    public EstadoReclamo estadoReclamo { get; set; }
    public string motivoResolucion { get; set; } = string.Empty;
    public DateOnly fechaResolucion { get; set; }
    public int? idUsuario { get; set; }
    public int idReserva { get; set; }

    public Reclamo(int idReclamo, DateOnly fechaReclamo, string descripcion, EstadoReclamo estadoReclamo, string motivoResolucion, DateOnly fechaResolucion, int? idUsuario, int idReserva)
    {
        this.idReclamo = idReclamo;
        this.fechaReclamo = fechaReclamo;
        this.descripcion = descripcion;
        this.estadoReclamo = estadoReclamo;
        this.motivoResolucion = motivoResolucion;
        this.fechaResolucion = fechaResolucion;
        this.idUsuario = idUsuario;
        this.idReserva = idReserva;
    }
    public Reclamo()
    {
    }
}

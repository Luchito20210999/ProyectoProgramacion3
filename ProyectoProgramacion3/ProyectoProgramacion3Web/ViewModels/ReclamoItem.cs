namespace ProyectoProgramacion3Web.ViewModels;

public class ReclamoItem
{
    public int Id { get; set; }
    public int IdUsuario { get; set; }
    public int IdReserva { get; set; }
    public string CodigoReserva { get; set; } = string.Empty;
    public string Cliente { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.Today;
    public string Descripcion { get; set; } = string.Empty;
    public string Estado { get; set; } = "PENDIENTE";
    public string EstadoClase { get; set; } = "badge-pendiente";
    public DateTime? FechaResolucion { get; set; }
    public string MotivoResolucion { get; set; } = string.Empty;

    public ReclamoItem Clone()
    {
        return (ReclamoItem)MemberwiseClone();
    }
}

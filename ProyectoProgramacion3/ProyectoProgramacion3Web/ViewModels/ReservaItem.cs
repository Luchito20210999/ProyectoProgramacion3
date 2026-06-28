namespace ProyectoProgramacion3Web.ViewModels;

public class ReservaItem
{
    public int Id { get; set; }
    public int IdCliente { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Cliente { get; set; } = string.Empty;
    public string Servicio { get; set; } = string.Empty;
    public DateTime FechaServicio { get; set; }
    public int Pax { get; set; }
    public decimal Monto { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string EstadoClase { get; set; } = string.Empty;
}

namespace ProyectoProgramacion3Web.ViewModels;

public class NotificacionItem
{
    public int Id { get; set; }
    public int IdUsuario { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string FechaHora { get; set; } = string.Empty;
    public DateTime? FechaEnvio { get; set; }
    public bool Leido { get; set; }
    public string Icono { get; set; } = string.Empty;
    public string ColorClase { get; set; } = string.Empty;
    public bool TieneAccion { get; set; }
    public string UrlRedireccion { get; set; } = string.Empty;
}

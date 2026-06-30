namespace ProyectoProgramacion3Web.Servicios.Rest.Dtos.Notificaciones;

public sealed class NotificacionRestDto
{
    public int IdNotificacion { get; set; }
    public string? Mensaje { get; set; }
    public string? TipoNotificacion { get; set; }
    public DateTime? FechaEnvio { get; set; }
    public bool Leido { get; set; }
    public int IdUsuario { get; set; }
    public string? TipoEvento { get; set; }
}

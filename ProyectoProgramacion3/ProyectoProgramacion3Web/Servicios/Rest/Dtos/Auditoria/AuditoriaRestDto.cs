namespace ProyectoProgramacion3Web.Servicios.Rest.Dtos.Auditoria;

public sealed class AuditoriaRestDto
{
    public int IdLogAuditoria { get; set; }
    public string? Descripcion { get; set; }
    public string? Accion { get; set; }
    public DateTime? FechaRegistro { get; set; }
    public string? OrigenAccion { get; set; }
    public int IdUsuario { get; set; }
}

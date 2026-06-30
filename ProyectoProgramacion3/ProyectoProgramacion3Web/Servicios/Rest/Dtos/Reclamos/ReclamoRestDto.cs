namespace ProyectoProgramacion3Web.Servicios.Rest.Dtos.Reclamos;

public sealed class ReclamoRestDto
{
    public int IdReclamo { get; set; }
    public DateTime? FechaReclamo { get; set; }
    public string? Descripcion { get; set; }
    public string? EstadoReclamo { get; set; }
    public string? MotivoResolucion { get; set; }
    public DateTime? FechaResolucion { get; set; }
    public int IdUsuario { get; set; }
    public string? UsuarioResponsable { get; set; }
    public int IdReserva { get; set; }
    public string? CodigoReserva { get; set; }
    public int IdCliente { get; set; }
    public string? Cliente { get; set; }
}

namespace ProyectoProgramacion3Web.Servicios.Rest.Dtos.Clientes;

public sealed class ClienteRestDto
{
    public int IdCliente { get; set; }
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public string? TipoDocumento { get; set; }
    public string? NumeroDocumento { get; set; }
    public string? Correo { get; set; }
    public string? Nacionalidad { get; set; }
    public DateTime? FechaRegistro { get; set; }
    public string? NumeroContacto { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public bool Activo { get; set; } = true;
}

namespace ProyectoProgramacion3Web.Servicios.Rest.Dtos.Usuarios;

public sealed class UsuarioRestDto
{
    public int IdUsuario { get; set; }
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public string? TipoDocumento { get; set; }
    public string? NumeroDocumento { get; set; }
    public string? Correo { get; set; }
    public string? Contrasena { get; set; }
    public string? NumeroContacto { get; set; }
    public string? TipoUsuario { get; set; }
}

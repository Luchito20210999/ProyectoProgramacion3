namespace ProyectoProgramacion3Web.ViewModels;

public class UsuarioItem
{
    public int Id { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string TipoDocumento { get; set; } = "DNI";
    public string NumeroDocumento { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Tipo { get; set; } = "Operador";
    public string Estado { get; set; } = "Activo";
    public string NombreCompleto => $"{Nombres} {Apellidos}";
    public string EstadoClase => Estado == "Activo" ? "badge-activo" : "badge-inactivo";

    public UsuarioItem Clone()
    {
        return (UsuarioItem)MemberwiseClone();
    }
}

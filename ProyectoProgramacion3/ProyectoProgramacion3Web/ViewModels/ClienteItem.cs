namespace ProyectoProgramacion3Web.ViewModels;

public class ClienteItem
{
    public int Id { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string TipoDocumento { get; set; } = "DNI";
    public string NumeroDocumento { get; set; } = string.Empty;
    public string Nacionalidad { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; } = DateTime.Today;
    public string Correo { get; set; } = string.Empty;
    public string Contacto { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; } = DateTime.Today;

    public ClienteItem Clone()
    {
        return (ClienteItem)MemberwiseClone();
    }
}

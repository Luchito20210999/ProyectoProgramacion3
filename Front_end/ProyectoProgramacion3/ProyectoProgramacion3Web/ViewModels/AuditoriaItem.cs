namespace ProyectoProgramacion3Web.ViewModels;

public class AuditoriaItem
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public string Comando { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Ubicacion { get; set; } = string.Empty;
}

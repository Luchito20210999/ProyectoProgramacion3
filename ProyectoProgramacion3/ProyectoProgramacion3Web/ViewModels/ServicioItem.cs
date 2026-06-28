namespace ProyectoProgramacion3Web.ViewModels;

public class ServicioItem
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Duracion { get; set; }
    public int CapacidadMaxima { get; set; }
    public string IdiomaGuia { get; set; } = string.Empty;
    public string CiudadDestino { get; set; } = string.Empty;
    public string IncluyeRecojo { get; set; } = "No";
    public string Icono { get; set; } = string.Empty;

    public ServicioItem Clone()
    {
        return (ServicioItem)MemberwiseClone();
    }
}

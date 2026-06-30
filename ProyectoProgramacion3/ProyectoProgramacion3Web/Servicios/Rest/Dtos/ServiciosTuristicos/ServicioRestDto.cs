namespace ProyectoProgramacion3Web.Servicios.Rest.Dtos.ServiciosTuristicos;

public sealed class ServicioRestDto
{
    public int IdServicio { get; set; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public double PrecioUSD { get; set; }
    public double DuracionHoras { get; set; }
    public string? IdiomaGuia { get; set; }
    public int CapacidadMaxima { get; set; }
    public bool IncluyeRecojo { get; set; }
    public string? CiudadDestino { get; set; }
    public bool Activo { get; set; } = true;
}

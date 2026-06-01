namespace ProyectoProgramacion3Model.Model.reservas;
public class Servicio
{
    public int idServicio { get; set; }
    public string nombre { get; set; } = string.Empty;
    public string descripcion { get; set; } = string.Empty;
    public double precioUSD { get; set; }
    public double duracionHoras { get; set; }
    public string idiomaGuia { get; set; } = string.Empty;
    public int capacidadMaxima { get; set; }
    public bool incluyeRecojo { get; set; }
    public string ciudadDestino { get; set; } = string.Empty;
    public Servicio() { }
    public Servicio(int idServicio, string nombre, string descripcion, double precioUSD, double duracionHoras, string idiomaGuia, int capacidadMaxima, bool incluyeRecojo, string ciudadDestino)
    {
        this.idServicio = idServicio;
        this.nombre = nombre;
        this.descripcion = descripcion;
        this.precioUSD = precioUSD;
        this.duracionHoras = duracionHoras;
        this.idiomaGuia = idiomaGuia;
        this.capacidadMaxima = capacidadMaxima;
        this.incluyeRecojo = incluyeRecojo;
        this.ciudadDestino = ciudadDestino;
    }

}



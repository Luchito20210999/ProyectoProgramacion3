namespace ProyectoProgramacion3Web.ViewModels;

public class ReservaItem
{
    public int Id { get; set; }
    public int IdCliente { get; set; }
    public int IdServicio { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Cliente { get; set; } = string.Empty;
    public string ClienteTipoDocumento { get; set; } = string.Empty;
    public string ClienteNumeroDocumento { get; set; } = string.Empty;
    public string ClienteCorreo { get; set; } = string.Empty;
    public string ClienteNacionalidad { get; set; } = string.Empty;
    public string Servicio { get; set; } = string.Empty;
    public string CiudadDestino { get; set; } = string.Empty;
    public decimal ServicioPrecioUSD { get; set; }
    public decimal MontoImpuestos { get; set; }
    public double ServicioDuracionHoras { get; set; }
    public int ServicioCapacidadMaxima { get; set; }
    public string ServicioIdiomaGuia { get; set; } = string.Empty;
    public bool ServicioIncluyeRecojo { get; set; }
    public DateTime FechaServicio { get; set; }
    public int Pax { get; set; }
    public decimal Monto { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string EstadoClase { get; set; } = string.Empty;
}

using System.Text.Json.Serialization;

namespace ProyectoProgramacion3Web.Servicios.Rest.Dtos.Reservas;

public sealed class ReservaRestDto
{
    [JsonPropertyName("idReserva")]
    public int IdReserva { get; set; }

    [JsonPropertyName("codigoBokun")]
    public string CodigoBokun { get; set; } = string.Empty;

    [JsonPropertyName("codigoReserva")]
    public string CodigoReserva { get; set; } = string.Empty;

    [JsonPropertyName("estadoReserva")]
    public string EstadoReserva { get; set; } = string.Empty;

    [JsonPropertyName("fechaRegistro")]
    public string FechaRegistro { get; set; } = string.Empty;

    [JsonPropertyName("fechaUltimaModificacion")]
    public DateTime? FechaUltimaModificacion { get; set; }

    [JsonPropertyName("canalVenta")]
    public string CanalVenta { get; set; } = string.Empty;

    [JsonPropertyName("montoImpuestos")]
    public double MontoImpuestos { get; set; }

    [JsonPropertyName("cantidadBoletos")]
    public int CantidadBoletos { get; set; }

    [JsonPropertyName("montoTotal")]
    public double MontoTotal { get; set; }

    [JsonPropertyName("idCliente")]
    public int IdCliente { get; set; }

    [JsonPropertyName("idUsuario")]
    public int IdUsuario { get; set; }

    [JsonPropertyName("idServicio")]
    public int IdServicio { get; set; }

    [JsonPropertyName("cliente")]
    public string Cliente { get; set; } = string.Empty;

    [JsonPropertyName("clienteTipoDocumento")]
    public string ClienteTipoDocumento { get; set; } = string.Empty;

    [JsonPropertyName("clienteNumeroDocumento")]
    public string ClienteNumeroDocumento { get; set; } = string.Empty;

    [JsonPropertyName("clienteCorreo")]
    public string ClienteCorreo { get; set; } = string.Empty;

    [JsonPropertyName("clienteNacionalidad")]
    public string ClienteNacionalidad { get; set; } = string.Empty;

    [JsonPropertyName("servicio")]
    public string Servicio { get; set; } = string.Empty;

    [JsonPropertyName("ciudadDestino")]
    public string CiudadDestino { get; set; } = string.Empty;

    [JsonPropertyName("servicioPrecioUSD")]
    public double ServicioPrecioUSD { get; set; }

    [JsonPropertyName("servicioDuracionHoras")]
    public double ServicioDuracionHoras { get; set; }

    [JsonPropertyName("servicioCapacidadMaxima")]
    public int ServicioCapacidadMaxima { get; set; }

    [JsonPropertyName("servicioIdiomaGuia")]
    public string ServicioIdiomaGuia { get; set; } = string.Empty;

    [JsonPropertyName("servicioIncluyeRecojo")]
    public bool ServicioIncluyeRecojo { get; set; }

    [JsonPropertyName("detalles")]
    public List<DetalleReservaRestDto> Detalles { get; set; } = new();
}

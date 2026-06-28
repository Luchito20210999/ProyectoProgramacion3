using System.Text.Json.Serialization;

namespace ProyectoProgramacion3Web.Servicios.Rest.Dtos.Reservas;

public sealed class ReservaRestDto
{
    [JsonPropertyName("idReserva")]
    public int IdReserva { get; set; }

    [JsonPropertyName("codigoBokun")]
    public string CodigoBokun { get; set; } = string.Empty;

    [JsonPropertyName("estadoReserva")]
    public string EstadoReserva { get; set; } = string.Empty;

    [JsonPropertyName("fechaRegistro")]
    public string FechaRegistro { get; set; } = string.Empty;

    [JsonPropertyName("cantidadBoletos")]
    public int CantidadBoletos { get; set; }

    [JsonPropertyName("montoTotal")]
    public double MontoTotal { get; set; }

    [JsonPropertyName("idCliente")]
    public int IdCliente { get; set; }

    [JsonPropertyName("cliente")]
    public string Cliente { get; set; } = string.Empty;

    [JsonPropertyName("servicio")]
    public string Servicio { get; set; } = string.Empty;
}

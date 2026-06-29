using System.Text.Json.Serialization;

namespace ProyectoProgramacion3Web.Servicios.Rest.Dtos.Reservas;

public sealed class DetalleReservaRestDto
{
    [JsonPropertyName("idDetalle")]
    public int IdDetalle { get; set; }

    [JsonPropertyName("idReserva")]
    public int IdReserva { get; set; }

    [JsonPropertyName("idServicio")]
    public int IdServicio { get; set; }

    [JsonPropertyName("cantidad")]
    public int Cantidad { get; set; }

    [JsonPropertyName("subtotal")]
    public double Subtotal { get; set; }
}

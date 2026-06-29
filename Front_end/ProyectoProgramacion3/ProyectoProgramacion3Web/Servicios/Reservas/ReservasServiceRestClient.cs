using ProyectoProgramacion3Web.Servicios.Base;
using ProyectoProgramacion3Web.Servicios.Rest.Dtos.Reservas;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Servicios.Reservas;

public class ReservasServiceRestClient : RestServiceClient<ReservaItem, ReservaRestDto>, IReservasServiceClient
{
    protected override string ResourceSetting => "RestResources:Reservas";

    public ReservasServiceRestClient(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        : base(configuration, httpClientFactory)
    {
    }

    public List<ReservaItem> Listar()
    {
        return ListarPayload().Select(ToViewModel).ToList();
    }

    public ReservaItem? Obtener(int id) => ObtenerPayload(id.ToString(), "Obtener reserva") is { } dto ? ToViewModel(dto) : null;

    public void Guardar(ReservaItem modelo, Estado estado) => GuardarPayload(ToRest(modelo), estado, modelo.Id.ToString());

    protected override ReservaItem ToViewModel(ReservaRestDto source)
    {
        var estado = source.EstadoReserva switch
        {
            "APROBADO" => "Confirmada",
            "PENDIENTE" => "Pendiente",
            "RECHAZADO" => "Anulada",
            "OBSERVADO" => "Pendiente",
            _ => "Pendiente"
        };

        var montoCalculado = CalcularMontoServicio(source);

        return new ReservaItem
        {
            Id = source.IdReserva,
            IdCliente = source.IdCliente,
            IdServicio = source.IdServicio,
            Codigo = ResolverCodigo(source),
            Cliente = string.IsNullOrWhiteSpace(source.Cliente) ? $"Cliente #{source.IdCliente}" : source.Cliente,
            ClienteTipoDocumento = source.ClienteTipoDocumento,
            ClienteNumeroDocumento = source.ClienteNumeroDocumento,
            ClienteCorreo = source.ClienteCorreo,
            ClienteNacionalidad = source.ClienteNacionalidad,
            Servicio = string.IsNullOrWhiteSpace(source.Servicio) ? "Reserva Bokun" : source.Servicio,
            CiudadDestino = source.CiudadDestino,
            ServicioPrecioUSD = Convert.ToDecimal(source.ServicioPrecioUSD),
            ServicioDuracionHoras = source.ServicioDuracionHoras,
            ServicioCapacidadMaxima = source.ServicioCapacidadMaxima,
            ServicioIdiomaGuia = source.ServicioIdiomaGuia,
            ServicioIncluyeRecojo = source.ServicioIncluyeRecojo,
            FechaServicio = ParseFecha(source.FechaRegistro),
            Pax = source.CantidadBoletos,
            Monto = montoCalculado > 0 ? montoCalculado : Convert.ToDecimal(source.MontoTotal),
            MontoImpuestos = Convert.ToDecimal(source.MontoImpuestos),
            Estado = estado,
            EstadoClase = EstadoClase(estado)
        };
    }

    private static decimal CalcularMontoServicio(ReservaRestDto source)
    {
        if (source.ServicioPrecioUSD <= 0 || source.CantidadBoletos <= 0)
        {
            return 0;
        }

        var precio = Convert.ToDecimal(source.ServicioPrecioUSD);
        if (EsClienteExtranjero(source.ClienteNacionalidad))
        {
            precio *= 1.20m;
        }

        return decimal.Round(precio * source.CantidadBoletos, 2);
    }

    private static bool EsClienteExtranjero(string? nacionalidad)
    {
        if (string.IsNullOrWhiteSpace(nacionalidad))
        {
            return false;
        }

        var valor = nacionalidad.Trim().ToUpperInvariant();
        return valor != "PE" && valor != "PERU" && valor != "PERUANO" && valor != "PERUANA";
    }

    private static string ResolverCodigo(ReservaRestDto source)
    {
        if (!string.IsNullOrWhiteSpace(source.CodigoReserva))
        {
            return source.CodigoReserva;
        }

        return string.IsNullOrWhiteSpace(source.CodigoBokun) ? $"RES-{source.IdReserva}" : source.CodigoBokun;
    }

    protected override ReservaRestDto ToRest(ReservaItem source)
    {
        return new ReservaRestDto
        {
            IdReserva = source.Id,
            FechaRegistro = source.FechaServicio.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            FechaUltimaModificacion = DateTime.Now,
            EstadoReserva = EstadoRest(source.Estado),
            CantidadBoletos = source.Pax,
            MontoTotal = Convert.ToDouble(source.Monto),
            MontoImpuestos = 0,
            CanalVenta = EsCodigoBokun(source.Codigo) ? "Bokun" : "Interno",
            CodigoBokun = EsCodigoBokun(source.Codigo) ? source.Codigo : string.Empty,
            IdCliente = source.IdCliente,
            IdUsuario = 0,
            Detalles = source.IdServicio > 0
                ? new List<DetalleReservaRestDto>
                {
                    new()
                    {
                        IdReserva = source.Id,
                        IdServicio = source.IdServicio,
                        Cantidad = source.Pax,
                        Subtotal = Convert.ToDouble(source.Monto)
                    }
                }
                : new List<DetalleReservaRestDto>()
        };
    }

    private static string EstadoClase(string estado)
    {
        return estado switch
        {
            "Confirmada" => "badge-confirmada",
            "Pendiente" => "badge-pendiente",
            "Anulada" => "badge-anulada",
            _ => "badge-pendiente"
        };
    }

    private static string EstadoRest(string estado)
    {
        return estado switch
        {
            "Confirmada" => "APROBADO",
            "Pendiente" => "PENDIENTE",
            "Anulada" => "RECHAZADO",
            _ => "PENDIENTE"
        };
    }

    private static bool EsCodigoBokun(string codigo)
    {
        return !string.IsNullOrWhiteSpace(codigo) &&
               !codigo.StartsWith("RES-", StringComparison.OrdinalIgnoreCase);
    }

}

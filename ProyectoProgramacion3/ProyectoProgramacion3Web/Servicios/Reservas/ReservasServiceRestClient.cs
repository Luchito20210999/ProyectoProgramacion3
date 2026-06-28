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

        return new ReservaItem
        {
            Id = source.IdReserva,
            IdCliente = source.IdCliente,
            Codigo = string.IsNullOrWhiteSpace(source.CodigoBokun) ? $"RES-{source.IdReserva}" : source.CodigoBokun,
            Cliente = string.IsNullOrWhiteSpace(source.Cliente) ? $"Cliente #{source.IdCliente}" : source.Cliente,
            Servicio = string.IsNullOrWhiteSpace(source.Servicio) ? "Reserva Bokun" : source.Servicio,
            FechaServicio = ParseFecha(source.FechaRegistro),
            Pax = source.CantidadBoletos,
            Monto = Convert.ToDecimal(source.MontoTotal),
            Estado = estado,
            EstadoClase = EstadoClase(estado)
        };
    }

    protected override ReservaRestDto ToRest(ReservaItem source)
    {
        throw new NotSupportedException("Las reservas se sincronizan desde Bokun y se exponen como consulta.");
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

}

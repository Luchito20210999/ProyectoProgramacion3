using ProyectoProgramacion3Web.Servicios.Base;
using ProyectoProgramacion3Web.Servicios.Rest.Dtos.Reclamos;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Servicios.Reclamos;

public class ReclamosServiceRestClient : RestServiceClient<ReclamoItem, ReclamoRestDto>, IReclamosServiceClient
{
    protected override string ResourceSetting => "RestResources:Reclamos";

    public ReclamosServiceRestClient(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        : base(configuration, httpClientFactory)
    {
    }

    public List<ReclamoItem> Listar() => ListarPayload().Select(ToViewModel).OrderByDescending(r => r.Fecha).ToList();
    public ReclamoItem? Obtener(int id) => ObtenerPayload(id.ToString(), "Obtener reclamo") is { } dto ? ToViewModel(dto) : null;
    public void Guardar(ReclamoItem modelo, Estado estado) => GuardarPayload(ToRest(modelo), estado, modelo.Id.ToString());
    public void Eliminar(int id) => EliminarPayload(id.ToString());

    protected override ReclamoItem ToViewModel(ReclamoRestDto source)
    {
        string estado = source.EstadoReclamo ?? "PENDIENTE";
        return new ReclamoItem
        {
            Id = source.IdReclamo,
            IdUsuario = source.IdUsuario,
            IdReserva = source.IdReserva,
            CodigoReserva = source.IdReserva > 0 ? $"RES-{source.IdReserva}" : string.Empty,
            Cliente = source.IdUsuario > 0 ? $"Usuario {source.IdUsuario}" : "Cliente",
            Fecha = source.FechaReclamo ?? DateTime.Today,
            Descripcion = source.Descripcion ?? string.Empty,
            Estado = estado,
            EstadoClase = EstadoClase(estado),
            FechaResolucion = source.FechaResolucion,
            MotivoResolucion = source.MotivoResolucion ?? string.Empty
        };
    }

    protected override ReclamoRestDto ToRest(ReclamoItem source)
    {
        return new ReclamoRestDto
        {
            IdReclamo = source.Id,
            FechaReclamo = source.Fecha == default ? DateTime.Today : source.Fecha,
            Descripcion = source.Descripcion,
            EstadoReclamo = source.Estado,
            MotivoResolucion = source.MotivoResolucion,
            FechaResolucion = source.FechaResolucion,
            IdReserva = source.IdReserva > 0 ? source.IdReserva : ExtraerIdReserva(source.CodigoReserva),
            IdUsuario = source.IdUsuario
        };
    }

    private static int ExtraerIdReserva(string codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo)) return 0;
        string digits = new(codigo.Where(char.IsDigit).ToArray());
        return int.TryParse(digits, out int id) ? id : 0;
    }

    private static string EstadoClase(string estado)
    {
        return estado switch
        {
            "PENDIENTE" => "badge-pendiente",
            "EN_ATENCION" => "badge-atencion",
            "PROCEDE" => "badge-procede",
            "NO_PROCEDE" => "badge-noprocede",
            _ => "badge-pendiente"
        };
    }

}

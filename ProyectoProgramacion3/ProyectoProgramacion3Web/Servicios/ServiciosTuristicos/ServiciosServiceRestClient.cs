using ProyectoProgramacion3Web.Servicios.Base;
using ProyectoProgramacion3Web.Servicios.Rest.Dtos.ServiciosTuristicos;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Servicios.ServiciosTuristicos;

public class ServiciosServiceRestClient : RestServiceClient<ServicioItem, ServicioRestDto>, IServiciosServiceClient
{
    protected override string ResourceSetting => "RestResources:Servicios";

    public ServiciosServiceRestClient(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        : base(configuration, httpClientFactory)
    {
    }

    public List<ServicioItem> Listar() => ListarPayload().Select(ToViewModel).OrderBy(s => s.Id).ToList();
    public ServicioItem? Obtener(int id) => ObtenerPayload(id.ToString(), "Obtener servicio") is { } dto ? ToViewModel(dto) : null;
    public void Guardar(ServicioItem modelo, Estado estado) => GuardarPayload(ToRest(modelo), estado, modelo.Id.ToString());
    public void Eliminar(int id) => EliminarPayload(id.ToString());

    protected override ServicioItem ToViewModel(ServicioRestDto source)
    {
        return new ServicioItem
        {
            Id = source.IdServicio,
            Nombre = source.Nombre ?? string.Empty,
            Descripcion = source.Descripcion ?? string.Empty,
            Precio = Convert.ToDecimal(source.PrecioUSD),
            Duracion = Convert.ToInt32(source.DuracionHoras),
            CapacidadMaxima = source.CapacidadMaxima,
            IdiomaGuia = source.IdiomaGuia ?? string.Empty,
            CiudadDestino = source.CiudadDestino ?? string.Empty,
            IncluyeRecojo = source.IncluyeRecojo ? "Si" : "No",
            Estado = source.Activo ? "Activo" : "Inactivo"
        };
    }

    protected override ServicioRestDto ToRest(ServicioItem source)
    {
        return new ServicioRestDto
        {
            IdServicio = source.Id,
            Nombre = source.Nombre,
            Descripcion = source.Descripcion,
            PrecioUSD = Convert.ToDouble(source.Precio),
            DuracionHoras = source.Duracion,
            IdiomaGuia = source.IdiomaGuia,
            CapacidadMaxima = source.CapacidadMaxima,
            IncluyeRecojo = source.IncluyeRecojo.StartsWith("S", StringComparison.OrdinalIgnoreCase),
            CiudadDestino = source.CiudadDestino,
            Activo = string.Equals(source.Estado, "Activo", StringComparison.OrdinalIgnoreCase)
        };
    }
}

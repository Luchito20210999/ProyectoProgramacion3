using ProyectoProgramacion3Web.Servicios.Base;
using ProyectoProgramacion3Web.Servicios.Rest.Dtos.Clientes;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Servicios.Clientes;

public class ClientesServiceRestClient : RestServiceClient<ClienteItem, ClienteRestDto>, IClientesServiceClient
{
    protected override string ResourceSetting => "RestResources:Clientes";

    public ClientesServiceRestClient(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        : base(configuration, httpClientFactory)
    {
    }

    public List<ClienteItem> Listar() => ListarPayload().Select(ToViewModel).OrderBy(c => c.Id).ToList();
    public ClienteItem? Obtener(int id) => ObtenerPayload(id.ToString(), "Obtener cliente") is { } dto ? ToViewModel(dto) : null;
    public void Guardar(ClienteItem modelo, Estado estado) => GuardarPayload(ToRest(modelo), estado, modelo.Id.ToString());
    public void Eliminar(int id) => EliminarPayload(id.ToString());

    protected override ClienteItem ToViewModel(ClienteRestDto source)
    {
        return new ClienteItem
        {
            Id = source.IdCliente,
            Nombres = source.Nombres ?? string.Empty,
            Apellidos = source.Apellidos ?? string.Empty,
            TipoDocumento = source.TipoDocumento ?? "DNI",
            NumeroDocumento = source.NumeroDocumento ?? string.Empty,
            Nacionalidad = source.Nacionalidad ?? string.Empty,
            FechaNacimiento = source.FechaNacimiento ?? DateTime.Today,
            Correo = source.Correo ?? string.Empty,
            Contacto = source.NumeroContacto ?? string.Empty,
            FechaRegistro = source.FechaRegistro ?? DateTime.Today,
            Estado = source.Activo ? "Activo" : "Inactivo"
        };
    }

    protected override ClienteRestDto ToRest(ClienteItem source)
    {
        return new ClienteRestDto
        {
            IdCliente = source.Id,
            Nombres = source.Nombres,
            Apellidos = source.Apellidos,
            TipoDocumento = NormalizarTipoDocumento(source.TipoDocumento),
            NumeroDocumento = source.NumeroDocumento,
            Nacionalidad = source.Nacionalidad,
            FechaNacimiento = source.FechaNacimiento,
            Correo = source.Correo,
            NumeroContacto = source.Contacto,
            FechaRegistro = source.FechaRegistro == default ? DateTime.Today : source.FechaRegistro,
            Activo = string.Equals(source.Estado, "Activo", StringComparison.OrdinalIgnoreCase)
        };
    }

    private static string NormalizarTipoDocumento(string? tipoDocumento)
    {
        return tipoDocumento switch
        {
            "Pasaporte" => "PASAPORTE",
            "CE" => "CARNET_DE_EXTRANJERIA",
            _ => tipoDocumento ?? "DNI"
        };
    }

}

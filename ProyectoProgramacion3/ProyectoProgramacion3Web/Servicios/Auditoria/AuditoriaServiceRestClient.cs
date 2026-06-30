using System.Net.Http.Json;
using ProyectoProgramacion3Web.Servicios.Base;
using ProyectoProgramacion3Web.Servicios.Rest.Dtos.Auditoria;
using ProyectoProgramacion3Web.Servicios.Usuarios;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Servicios.Auditoria;

public class AuditoriaServiceRestClient : RestServiceClient<AuditoriaItem, AuditoriaRestDto>, IAuditoriaServiceClient
{
    protected override string ResourceSetting => "RestResources:Auditoria";
    private readonly IUsuariosServiceClient _usuariosServiceClient;
    private Dictionary<int, UsuarioItem>? _usuariosPorId;

    public AuditoriaServiceRestClient(
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        IUsuariosServiceClient usuariosServiceClient)
        : base(configuration, httpClientFactory)
    {
        _usuariosServiceClient = usuariosServiceClient;
    }

    public List<AuditoriaItem> Listar() => ListarPayload().Select(ToViewModel).ToList();

    public void Registrar(string accion, string descripcion, string origenAccion, int idUsuario)
    {
        var payload = new AuditoriaRestDto
        {
            Accion = accion,
            Descripcion = descripcion,
            OrigenAccion = origenAccion,
            FechaRegistro = DateTime.Now,
            IdUsuario = idUsuario
        };

        using var client = CreateClient();
        using var response = client.PostAsJsonAsync(string.Empty, payload, SerializerOptions).GetAwaiter().GetResult();
        EnsureSuccess(response, "Registrar auditoria");
    }

    protected override AuditoriaItem ToViewModel(AuditoriaRestDto source)
    {
        return new AuditoriaItem
        {
            Id = source.IdLogAuditoria,
            Fecha = source.FechaRegistro ?? DateTime.Now,
            Usuario = NombreUsuario(source.IdUsuario),
            Comando = source.Accion ?? string.Empty,
            Descripcion = source.Descripcion ?? string.Empty,
            Ubicacion = source.OrigenAccion ?? string.Empty
        };
    }

    protected override AuditoriaRestDto ToRest(AuditoriaItem source)
    {
        throw new NotSupportedException("Auditoria se consume como consulta.");
    }

    private string NombreUsuario(int idUsuario)
    {
        if (idUsuario <= 0)
        {
            return "Sistema";
        }

        _usuariosPorId ??= _usuariosServiceClient.Listar().ToDictionary(u => u.Id);
        if (!_usuariosPorId.TryGetValue(idUsuario, out var usuario))
        {
            return $"Usuario {idUsuario}";
        }

        return $"{usuario.Tipo} - {usuario.NombreCompleto}".Trim();
    }

}

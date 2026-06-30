using System.Net;
using System.Net.Http.Json;
using ProyectoProgramacion3Web.Servicios.Base;
using ProyectoProgramacion3Web.Servicios.Rest.Dtos.Usuarios;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Servicios.Usuarios;

public class UsuariosServiceRestClient : RestServiceClient<UsuarioItem, UsuarioRestDto>, IUsuariosServiceClient
{
    protected override string ResourceSetting => "RestResources:Usuarios";

    public UsuariosServiceRestClient(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        : base(configuration, httpClientFactory)
    {
    }

    public List<UsuarioItem> Listar() => ListarPayload().Select(ToViewModel).OrderBy(u => u.Id).ToList();
    public UsuarioItem? Obtener(int id) => ObtenerPayload(id.ToString(), "Obtener usuario") is { } dto ? ToViewModel(dto) : null;
    
    public void Guardar(UsuarioItem modelo, Estado estado) => GuardarPayload(ToRest(modelo), estado, modelo.Id.ToString());

    public void Eliminar(int id) => EliminarPayload(id.ToString());

    public UsuarioItem? Login(string correo, string contrasena)
    {
        if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasena))
        {
            return null;
        }

        var correoNormalizado = correo.Trim();
        using var client = CreateClient();
        var usuarioExistente = ListarPayload()
            .FirstOrDefault(u => u.Activo && string.Equals(u.Correo, correoNormalizado, StringComparison.OrdinalIgnoreCase));

        if (usuarioExistente is null || string.IsNullOrWhiteSpace(usuarioExistente.TipoUsuario))
        {
            return null;
        }

        var payload = new UsuarioRestDto
        {
            Correo = correoNormalizado,
            Contrasena = contrasena,
            TipoUsuario = usuarioExistente.TipoUsuario
        };

        using var response = client.PostAsJsonAsync("login", payload, SerializerOptions).GetAwaiter().GetResult();
        if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        EnsureSuccess(response, "Login");
        return ToViewModel(usuarioExistente);
    }

    protected override UsuarioItem ToViewModel(UsuarioRestDto source)
    {
        return new UsuarioItem
        {
            Id = source.IdUsuario,
            Nombres = source.Nombres ?? string.Empty,
            Apellidos = source.Apellidos ?? string.Empty,
            TipoDocumento = source.TipoDocumento ?? "DNI",
            NumeroDocumento = source.NumeroDocumento ?? string.Empty,
            Correo = source.Correo ?? string.Empty,
            Contrasena = source.Contrasena ?? string.Empty,
            Telefono = source.NumeroContacto ?? string.Empty,
            Tipo = source.TipoUsuario ?? "Operador",
            Estado = source.Activo ? "Activo" : "Inactivo"
        };
    }

    protected override UsuarioRestDto ToRest(UsuarioItem source)
    {
        return new UsuarioRestDto
        {
            IdUsuario = source.Id,
            Nombres = source.Nombres,
            Apellidos = source.Apellidos,
            TipoDocumento = source.TipoDocumento == "CE" ? "CARNET_DE_EXTRANJERIA" : source.TipoDocumento,
            NumeroDocumento = source.NumeroDocumento,
            Correo = source.Correo,
            NumeroContacto = source.Telefono,
            Contrasena = source.Contrasena,
            TipoUsuario = source.Tipo,
            Activo = string.Equals(source.Estado, "Activo", StringComparison.OrdinalIgnoreCase)
        };
    }

}

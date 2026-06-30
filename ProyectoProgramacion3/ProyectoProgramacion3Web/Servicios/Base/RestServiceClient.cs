using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Servicios.Base;

public abstract class RestServiceClient<TViewModel, TRestDto>
    where TViewModel : class
{
    protected IConfiguration Configuration { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected abstract string ResourceSetting { get; }

    protected static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new CustomDateTimeConverter(), new CustomNullableDateTimeConverter() }
    };

    protected RestServiceClient(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        Configuration = configuration;
        HttpClientFactory = httpClientFactory;
    }

    protected abstract TViewModel ToViewModel(TRestDto source);
    protected abstract TRestDto ToRest(TViewModel source);

    protected HttpClient CreateClient()
    {
        var endpoint = Configuration[ResourceSetting]?.Trim();
        if (string.IsNullOrWhiteSpace(endpoint))
        {
            throw new InvalidOperationException($"No se encontro configuracion para '{ResourceSetting}'.");
        }

        var client = HttpClientFactory.CreateClient();
        client.BaseAddress = new Uri(endpoint.EndsWith('/') ? endpoint : $"{endpoint}/", UriKind.Absolute);
        return client;
    }

    protected List<TRestDto> ListarPayload()
    {
        using var client = CreateClient();
        using var response = client.GetAsync(string.Empty).GetAwaiter().GetResult();
        EnsureSuccess(response, "Listar");
        return response.Content.ReadFromJsonAsync<List<TRestDto>>(SerializerOptions).GetAwaiter().GetResult() ?? [];
    }

    protected TRestDto? ObtenerPayload(string path, string operation)
    {
        using var client = CreateClient();
        using var response = client.GetAsync(path).GetAwaiter().GetResult();
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return default;
        }

        EnsureSuccess(response, operation);
        return response.Content.ReadFromJsonAsync<TRestDto>(SerializerOptions).GetAwaiter().GetResult();
    }

    protected void GuardarPayload(TRestDto payload, Estado estado, string idPath)
    {
        using var client = CreateClient();
        using var response = estado switch
        {
            Estado.Nuevo => client.PostAsJsonAsync(string.Empty, payload, SerializerOptions).GetAwaiter().GetResult(),
            Estado.Modificado => client.PutAsJsonAsync(idPath, payload, SerializerOptions).GetAwaiter().GetResult(),
            Estado.Eliminado => client.DeleteAsync(idPath).GetAwaiter().GetResult(),
            _ => throw new InvalidOperationException($"Estado no soportado: {estado}")
        };

        EnsureSuccess(response, "Guardar");
    }

    protected void EliminarPayload(string path)
    {
        using var client = CreateClient();
        using var response = client.DeleteAsync(path).GetAwaiter().GetResult();
        EnsureSuccess(response, "Eliminar");
    }

    protected static void EnsureSuccess(HttpResponseMessage response, string operation)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        throw new HttpRequestException($"{operation} fallo con estado {(int)response.StatusCode}: {body}");
    }

    protected static DateTime ParseFecha(string? source)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            return DateTime.Today;
        }

        // Clean time zone ID inside brackets
        source = Regex.Replace(source, @"\[[^\]]+\]$", "");

        return DateTimeOffset.TryParse(source, out var dto)
            ? dto.LocalDateTime
            : DateTime.TryParse(source, out var date) ? date : DateTime.Today;
    }
}

public class CustomDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return DateTime.Today;
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var str = reader.GetString();
            if (string.IsNullOrWhiteSpace(str))
            {
                return DateTime.Today;
            }

            // Remove trailing brackets like [UTC]
            str = Regex.Replace(str, @"\[[^\]]+\]$", "");

            if (DateTimeOffset.TryParse(str, out var dto))
            {
                return dto.LocalDateTime;
            }
            if (DateTime.TryParse(str, out var dt))
            {
                return dt;
            }
        }

        return DateTime.Today;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
    }
}

public class CustomNullableDateTimeConverter : JsonConverter<DateTime?>
{
    private readonly CustomDateTimeConverter _inner = new();

    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }
        return _inner.Read(ref reader, typeof(DateTime), options);
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
        }
        else
        {
            _inner.Write(writer, value.Value, options);
        }
    }
}

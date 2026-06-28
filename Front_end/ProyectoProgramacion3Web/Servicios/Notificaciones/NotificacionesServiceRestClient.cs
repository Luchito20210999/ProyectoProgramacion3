using ProyectoProgramacion3Web.Servicios.Base;
using ProyectoProgramacion3Web.Servicios.Rest.Dtos.Notificaciones;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Servicios.Notificaciones;

public class NotificacionesServiceRestClient : RestServiceClient<NotificacionItem, NotificacionRestDto>, INotificacionesServiceClient
{
    protected override string ResourceSetting => "RestResources:Notificaciones";

    public NotificacionesServiceRestClient(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        : base(configuration, httpClientFactory)
    {
    }

    public List<NotificacionItem> Listar() => ListarPayload().Select(ToViewModel).OrderByDescending(n => n.Id).ToList();
    public NotificacionItem? Obtener(int id) => ObtenerPayload(id.ToString(), "Obtener notificacion") is { } dto ? ToViewModel(dto) : null;
    public void Guardar(NotificacionItem modelo, Estado estado) => GuardarPayload(ToRest(modelo), estado, modelo.Id.ToString());
    public void Eliminar(int id) => EliminarPayload(id.ToString());

    protected override NotificacionItem ToViewModel(NotificacionRestDto source)
    {
        string tipo = source.TipoEvento ?? source.TipoNotificacion ?? "GENERAL";
        return new NotificacionItem
        {
            Id = source.IdNotificacion,
            IdUsuario = source.IdUsuario,
            Tipo = tipo,
            Titulo = tipo.Replace("_", " "),
            Descripcion = source.Mensaje ?? string.Empty,
            FechaHora = (source.FechaEnvio ?? DateTime.Now).ToString("dd/MM HH:mm"),
            FechaEnvio = source.FechaEnvio,
            Leido = source.Leido,
            Icono = tipo.Contains("RECLAMO") ? "!" : "i",
            ColorClase = tipo.Contains("RECLAMO") ? "circle-amarillo" : "circle-celeste",
            TieneAccion = tipo.Contains("RECLAMO"),
            UrlRedireccion = tipo.Contains("RECLAMO") ? "reclamos" : string.Empty
        };
    }

    protected override NotificacionRestDto ToRest(NotificacionItem source)
    {
        return new NotificacionRestDto
        {
            IdNotificacion = source.Id,
            Mensaje = source.Descripcion,
            TipoNotificacion = source.Tipo,
            TipoEvento = NormalizarTipoEvento(source.Tipo),
            FechaEnvio = source.FechaEnvio ?? DateTime.Now,
            Leido = source.Leido,
            IdUsuario = source.IdUsuario
        };
    }

    private static string NormalizarTipoEvento(string tipo)
    {
        return tipo is "NUEVA_RESERVA" or "ANULACION_RESERVA" or "RECLAMO_PENDIENTE" or "ERROR_INTEGRACION"
            ? tipo
            : "ERROR_INTEGRACION";
    }

}

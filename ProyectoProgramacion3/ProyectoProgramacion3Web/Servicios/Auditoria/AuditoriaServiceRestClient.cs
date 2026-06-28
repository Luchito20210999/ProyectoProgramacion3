using ProyectoProgramacion3Web.Servicios.Base;
using ProyectoProgramacion3Web.Servicios.Rest.Dtos.Auditoria;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Servicios.Auditoria;

public class AuditoriaServiceRestClient : RestServiceClient<AuditoriaItem, AuditoriaRestDto>, IAuditoriaServiceClient
{
    protected override string ResourceSetting => "RestResources:Auditoria";

    public AuditoriaServiceRestClient(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        : base(configuration, httpClientFactory)
    {
    }

    public List<AuditoriaItem> Listar() => ListarPayload().Select(ToViewModel).ToList();

    protected override AuditoriaItem ToViewModel(AuditoriaRestDto source)
    {
        return new AuditoriaItem
        {
            Id = source.IdLogAuditoria,
            Fecha = source.FechaRegistro ?? DateTime.Now,
            Usuario = source.IdUsuario > 0 ? $"Usuario {source.IdUsuario}" : "Sistema",
            Comando = source.Accion ?? string.Empty,
            Descripcion = source.Descripcion ?? string.Empty,
            Ubicacion = source.OrigenAccion ?? string.Empty
        };
    }

    protected override AuditoriaRestDto ToRest(AuditoriaItem source)
    {
        throw new NotSupportedException("Auditoria se consume como consulta.");
    }

}

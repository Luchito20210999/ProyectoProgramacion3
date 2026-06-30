using ProyectoProgramacion3Web.Servicios.Auditoria;

namespace ProyectoProgramacion3Web.Services;

public class AuditoriaFrontService
{
    private readonly IAuditoriaServiceClient _auditoriaServiceClient;
    private readonly SessionService _session;
    private readonly ILogger<AuditoriaFrontService> _logger;

    public AuditoriaFrontService(
        IAuditoriaServiceClient auditoriaServiceClient,
        SessionService session,
        ILogger<AuditoriaFrontService> logger)
    {
        _auditoriaServiceClient = auditoriaServiceClient;
        _session = session;
        _logger = logger;
    }

    public void Registrar(string accion, string modulo, string descripcion)
    {
        if (!_session.IsLoggedIn || _session.UserId <= 0)
        {
            return;
        }

        try
        {
            var rol = string.IsNullOrWhiteSpace(_session.Role) ? "Usuario" : _session.Role;
            var usuario = string.IsNullOrWhiteSpace(_session.Username)
                ? $"{rol} #{_session.UserId}"
                : $"{rol} {_session.Username}";

            _auditoriaServiceClient.Registrar(
                accion,
                $"{usuario} {descripcion}",
                modulo,
                _session.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se pudo registrar auditoria para {Accion}", accion);
        }
    }
}

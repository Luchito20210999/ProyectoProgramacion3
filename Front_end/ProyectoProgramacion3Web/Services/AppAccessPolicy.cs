namespace ProyectoProgramacion3Web.Services;

public class AppAccessPolicy
{
    private static readonly Dictionary<string, string[]> RouteRoles = new(StringComparer.OrdinalIgnoreCase)
    {
        ["bienvenida"] = new[] { "Administrador", "Operador", "Analista" },
        ["dashboard"] = new[] { "Administrador", "Analista" },
        ["reservas"] = new[] { "Administrador", "Operador" },
        ["clientes"] = new[] { "Administrador", "Operador" },
        ["servicios"] = new[] { "Administrador", "Operador" },
        ["reclamos"] = new[] { "Administrador", "Operador" },
        ["notificaciones"] = new[] { "Administrador", "Operador" },
        ["ventas"] = new[] { "Administrador", "Analista" },
        ["calidad"] = new[] { "Administrador", "Analista" },
        ["usuarios"] = new[] { "Administrador" },
        ["auditoria"] = new[] { "Administrador" },
        ["logout"] = new[] { "Administrador", "Operador", "Analista" }
    };

    public bool CanAccess(string role, string relativePath)
    {
        var route = NormalizeRoute(relativePath);
        if (string.IsNullOrEmpty(route) || !RouteRoles.TryGetValue(route, out var roles))
        {
            return true;
        }

        return roles.Contains(NormalizeRole(role), StringComparer.OrdinalIgnoreCase);
    }

    public bool CanAccessMenuItem(string role, string url)
    {
        return CanAccess(role, url);
    }

    public string GetDefaultRoute(string role)
    {
        return NormalizeRole(role) switch
        {
            "Operador" => "bienvenida",
            "Analista" => "dashboard",
            "Administrador" => "dashboard",
            _ => "bienvenida"
        };
    }

    public static string NormalizeRole(string? role)
    {
        return role?.Trim().ToUpperInvariant() switch
        {
            "ADMIN" => "Administrador",
            "ADMINISTRADOR" => "Administrador",
            "ADMNISTRADOR" => "Administrador",
            "OPERADOR" => "Operador",
            "ANALISTA" => "Analista",
            _ => role?.Trim() ?? string.Empty
        };
    }

    private static string NormalizeRoute(string relativePath)
    {
        return relativePath.Split('?', '#')[0].Trim().Trim('/');
    }
}

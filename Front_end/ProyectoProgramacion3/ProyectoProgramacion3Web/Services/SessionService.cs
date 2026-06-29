using System;
using System.Security.Claims;
using ProyectoProgramacion3Web.Servicios.Usuarios;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Services
{
    public class SessionService
    {
        private readonly IUsuariosServiceClient _usuariosServiceClient;

        public int UserId { get; private set; }
        public string Username { get; private set; } = string.Empty;
        public string Role { get; private set; } = string.Empty;
        public string Initials { get; private set; } = string.Empty;
        public bool IsLoggedIn { get; private set; } = false;

        public event Action? OnChange;

        public SessionService(IUsuariosServiceClient usuariosServiceClient)
        {
            _usuariosServiceClient = usuariosServiceClient;
        }

        public bool Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            try
            {
                var user = Authenticate(email, password);
                if (user != null)
                {
                    ApplyUser(user);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de login al consultar API: {ex.Message}");
            }
            return false;
        }

        public UsuarioItem? Authenticate(string email, string password)
        {
            return _usuariosServiceClient.Login(email, password);
        }

        public void ApplyUser(UsuarioItem user)
        {
            IsLoggedIn = true;
            UserId = user.Id;
            Username = $"{user.Nombres} {user.Apellidos}".Trim();
            Role = AppAccessPolicy.NormalizeRole(user.Tipo);

            var firstInit = string.IsNullOrEmpty(user.Nombres) ? "" : user.Nombres[0].ToString();
            var lastInit = string.IsNullOrEmpty(user.Apellidos) ? "" : user.Apellidos[0].ToString();
            Initials = (firstInit + lastInit).ToUpperInvariant();

            NotifyStateChanged();
        }

        public void RestoreFromClaims(ClaimsPrincipal principal)
        {
            if (IsLoggedIn || principal.Identity?.IsAuthenticated != true)
            {
                return;
            }

            var idValue = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(idValue, out var userId);

            UserId = userId;
            Username = principal.Identity.Name ?? principal.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
            Role = AppAccessPolicy.NormalizeRole(principal.FindFirstValue(ClaimTypes.Role));
            Initials = principal.FindFirstValue("initials") ?? BuildInitials(Username);
            IsLoggedIn = true;

            NotifyStateChanged();
        }

        public void Logout()
        {
            IsLoggedIn = false;
            UserId = 0;
            Username = string.Empty;
            Role = string.Empty;
            Initials = string.Empty;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        private static string BuildInitials(string name)
        {
            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return string.Concat(parts.Take(2).Select(part => part[0])).ToUpperInvariant();
        }
    }
}

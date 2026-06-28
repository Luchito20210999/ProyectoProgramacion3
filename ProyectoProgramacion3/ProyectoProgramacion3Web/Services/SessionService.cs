 using System;
using ProyectoProgramacion3Web.Servicios.Usuarios;

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
                var user = _usuariosServiceClient.Login(email, password);
                if (user != null)
                {
                    IsLoggedIn = true;
                    UserId = user.Id;
                    Username = $"{user.Nombres} {user.Apellidos}";
                    Role = AppAccessPolicy.NormalizeRole(user.Tipo);
                    
                    var firstInit = string.IsNullOrEmpty(user.Nombres) ? "" : user.Nombres[0].ToString();
                    var lastInit = string.IsNullOrEmpty(user.Apellidos) ? "" : user.Apellidos[0].ToString();
                    Initials = (firstInit + lastInit).ToUpperInvariant();
                    
                    NotifyStateChanged();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de login al consultar API: {ex.Message}");
            }
            return false;
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
    }
}

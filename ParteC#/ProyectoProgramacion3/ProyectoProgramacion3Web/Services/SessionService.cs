using System;

namespace ProyectoProgramacion3Web.Services
{
    public class SessionService
    {
        public string Username { get; private set; } = "Manuel Arango";
        public string Role { get; private set; } = "Operador";
        public string Initials { get; private set; } = "MA";
        public bool IsLoggedIn { get; private set; } = false;

        public event Action OnChange;

        public bool Login(string email, string password)
        {
            // Validación mínima de campos: no vacíos
            if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
            {
                IsLoggedIn = true;
                NotifyStateChanged();
                return true;
            }
            return false;
        }

        public void Logout()
        {
            IsLoggedIn = false;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}

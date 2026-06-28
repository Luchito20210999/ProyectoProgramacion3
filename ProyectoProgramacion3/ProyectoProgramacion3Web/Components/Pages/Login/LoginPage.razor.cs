using Microsoft.AspNetCore.Components;
using ProyectoProgramacion3Web.Services;

namespace ProyectoProgramacion3Web.Components.Pages.Login
{
    public partial class LoginPage
    {
        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private SessionService Session { get; set; } = default!;

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public bool IsLoginLoading { get; set; }

        public async Task HandleLogin()
        {
            if (IsLoginLoading)
            {
                return;
            }

            ErrorMessage = string.Empty;
            IsLoginLoading = true;
            await Task.Yield();

            bool success = Session.Login(Email, Password);
            if (success)
            {
                Navigation.NavigateTo("/bienvenida");
            }
            else
            {
                IsLoginLoading = false;
                ErrorMessage = "Usuario o contraseña incorrectos.";
            }
        }
    }
}

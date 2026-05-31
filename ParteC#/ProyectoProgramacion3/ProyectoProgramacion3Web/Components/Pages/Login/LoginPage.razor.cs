using Microsoft.AspNetCore.Components;
using ProyectoProgramacion3Web.Services;

namespace ProyectoProgramacion3Web.Components.Pages.Login
{
    public partial class LoginPage
    {
        [Inject]
        private NavigationManager Navigation { get; set; }

        [Inject]
        private SessionService Session { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }

        public void HandleLogin()
        {
            ErrorMessage = null;
            
            bool success = Session.Login(Email, Password);
            if (success)
            {
                Navigation.NavigateTo("/bienvenida");
            }
            else
            {
                ErrorMessage = "El correo electrónico y la contraseña son obligatorios.";
            }
        }
    }
}

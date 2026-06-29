using Microsoft.AspNetCore.Components;

namespace ProyectoProgramacion3Web.Components.Pages.Login
{
    public partial class LoginPage
    {
        public string ErrorMessage { get; set; } = string.Empty;

        [SupplyParameterFromQuery(Name = "error")]
        public string? Error { get; set; }

        protected override void OnParametersSet()
        {
            ErrorMessage = Error == "1" ? "Usuario o contrasena incorrectos." : string.Empty;
        }
    }
}

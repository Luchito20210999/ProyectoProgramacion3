using Microsoft.AspNetCore.Components;
using ProyectoProgramacion3Web.Services;

namespace ProyectoProgramacion3Web.Components.Layout
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        public SessionService Session { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        protected override void OnInitialized()
        {
            // Suscribirse a los cambios de estado de sesión
            Session.OnChange += StateHasChanged;
        }

        public void Dispose()
        {
            // Desuscribirse
            Session.OnChange -= StateHasChanged;
        }
    }
}

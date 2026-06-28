using Microsoft.AspNetCore.Components;
using ProyectoProgramacion3Web.Servicios.Notificaciones;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Components.Pages.Notificaciones
{
    public partial class Notificaciones : ComponentBase
    {
        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private INotificacionesServiceClient NotificacionesServiceClient { get; set; } = default!;

        public List<NotificacionItem> ListadoNotificaciones { get; set; } = new();

        protected override void OnInitialized()
        {
            CargarDatosIniciales();
        }

        private void CargarDatosIniciales()
        {
            ListadoNotificaciones = NotificacionesServiceClient.Listar();
        }

        public void MarcarTodasLeidas()
        {
            foreach (var notif in ListadoNotificaciones.Where(n => !n.Leido))
            {
                notif.Leido = true;
                NotificacionesServiceClient.Guardar(notif, Estado.Modificado);
            }
        }

        public void AtenderNotificacion(NotificacionItem notif)
        {
            if (notif == null) return;

            notif.Leido = true;
            NotificacionesServiceClient.Guardar(notif, Estado.Modificado);

            if (notif.TieneAccion && !string.IsNullOrEmpty(notif.UrlRedireccion))
            {
                Navigation.NavigateTo(notif.UrlRedireccion);
            }
        }
    }
}

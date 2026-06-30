using Microsoft.AspNetCore.Components;
using ProyectoProgramacion3Web.Services;
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

        [Inject]
        private SessionService Session { get; set; } = default!;

        public List<NotificacionItem> ListadoNotificaciones { get; set; } = new();

        protected override void OnInitialized()
        {
            CargarDatosIniciales();
        }

        private void CargarDatosIniciales()
        {
            var notificaciones = NotificacionesServiceClient.Listar();
            if (!EsAdministrador())
            {
                notificaciones = notificaciones
                    .Where(n => n.IdUsuario == Session.UserId)
                    .ToList();
            }

            ListadoNotificaciones = notificaciones;
        }

        public void MarcarTodasLeidas()
        {
            foreach (var notif in ListadoNotificaciones.Where(n => !n.Leido))
            {
                notif.Leido = true;
                NotificacionesServiceClient.Guardar(notif, Estado.Modificado);
            }
            CargarDatosIniciales();
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

        private bool EsAdministrador()
        {
            return AppAccessPolicy.NormalizeRole(Session.Role)
                .Equals("Administrador", StringComparison.OrdinalIgnoreCase);
        }
    }
}

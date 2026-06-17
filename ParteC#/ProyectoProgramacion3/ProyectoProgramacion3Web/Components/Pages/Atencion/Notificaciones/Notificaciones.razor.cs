using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace ProyectoProgramacion3Web.Components.Pages.Notificaciones
{
    public partial class Notificaciones : ComponentBase
    {
        [Inject]
        private NavigationManager Navigation { get; set; }

        // Listado de notificaciones en memoria
        public List<NotificacionItem> ListadoNotificaciones { get; set; } = new List<NotificacionItem>();

        protected override void OnInitialized()
        {
            CargarDatosIniciales();
        }

        private void CargarDatosIniciales()
        {
            ListadoNotificaciones = new List<NotificacionItem>
            {
                new NotificacionItem
                {
                    Id = 1,
                    Tipo = "NUEVA_RESERVA",
                    Titulo = "NUEVA RESERVA",
                    Descripcion = "Nueva reserva BK-10297 desde Bokun – Sofía López.",
                    FechaHora = "12/05 09:14",
                    Leido = false,
                    Icono = "✓",
                    ColorClase = "circle-verde",
                    TieneAccion = false
                },
                new NotificacionItem
                {
                    Id = 2,
                    Tipo = "RECLAMO_PENDIENTE",
                    Titulo = "RECLAMO PENDIENTE",
                    Descripcion = "Reclamo #1 sobre BK-10293 requiere atención.",
                    FechaHora = "12/05 07:00",
                    Leido = false,
                    Icono = "⚠",
                    ColorClase = "circle-amarillo",
                    TieneAccion = true,
                    UrlRedireccion = "reclamos?search=BK-10293"
                },
                new NotificacionItem
                {
                    Id = 3,
                    Tipo = "ANULACION_RESERVA",
                    Titulo = "ANULACION RESERVA",
                    Descripcion = "Reserva BK-10296 fue anulada por el cliente.",
                    FechaHora = "12/05 03:00",
                    Leido = true,
                    Icono = "×",
                    ColorClase = "circle-rojo",
                    TieneAccion = false
                },
                new NotificacionItem
                {
                    Id = 4,
                    Tipo = "RECLAMO_RESUELTO",
                    Titulo = "RECLAMO RESUELTO",
                    Descripcion = "Reclamo #3 resuelto como PROCEDE.",
                    FechaHora = "11/05 15:20",
                    Leido = true,
                    Icono = "ℹ",
                    ColorClase = "circle-celeste",
                    TieneAccion = false
                }
            };
        }

        // Marca todas las notificaciones visibles de la bandeja como leídas al mismo tiempo
        public void MarcarTodasLeidas()
        {
            foreach (var notif in ListadoNotificaciones)
            {
                notif.Leido = true;
            }
        }

        // Atiende una notificación marcándola como leída y redirigiendo al módulo de Reclamos
        public void AtenderNotificacion(NotificacionItem notif)
        {
            if (notif == null) return;

            notif.Leido = true;

            if (notif.TieneAccion && !string.IsNullOrEmpty(notif.UrlRedireccion))
            {
                Navigation.NavigateTo(notif.UrlRedireccion);
            }
        }
    }

    public class NotificacionItem
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string FechaHora { get; set; }
        public bool Leido { get; set; }
        public string Icono { get; set; }
        public string ColorClase { get; set; }
        public bool TieneAccion { get; set; }
        public string UrlRedireccion { get; set; }
    }
}

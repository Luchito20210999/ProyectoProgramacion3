using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using ProyectoProgramacion3Web.Services;

namespace ProyectoProgramacion3Web.Components.Navigation
{
    public partial class NavMenu
    {
        [Inject]
        private NavigationManager Navigation { get; set; }

        [Inject]
        private SessionService Session { get; set; }

        public List<MenuGroup> MenuGroups { get; set; }

                // Estado del modal de confirmación para cerrar sesión
        public bool IsConfirmOpen { get; set; }

        protected override void OnInitialized()
        {
            InitializeMenu();
        }

        // Abre el modal de confirmación de cierre de sesión
        public void SolicitarCerrarSesion()
        {
            IsConfirmOpen = true;
        }

        // Ejecuta el cierre de sesión definitivo redirigiendo al logout seguro
        public void AceptarCerrarSesion()
        {
            IsConfirmOpen = false;
            Navigation.NavigateTo("logout");
        }

        // Cierra el modal de confirmación y retiene la sesión
        public void CancelarCerrarSesion()
        {
            IsConfirmOpen = false;
        }

        private void InitializeMenu()
        {
            MenuGroups = new List<MenuGroup>
            {
                new MenuGroup
                {
                    Title = "Principal",
                    Items = new List<MenuItem>
                    {
                        new MenuItem { Text = "Bienvenida", Url = "bienvenida", Icon = "👋" },
                        new MenuItem { Text = "Dashboard", Url = "dashboard", Icon = "📊" }
                    }
                },
                new MenuGroup
                {
                    Title = "Operaciones",
                    Items = new List<MenuItem>
                    {
                        new MenuItem { Text = "Reservas", Url = "reservas", Icon = "📅" },
                        new MenuItem { Text = "Clientes", Url = "clientes", Icon = "👥" },
                        new MenuItem { Text = "Servicios", Url = "servicios", Icon = "💼" }
                    }
                },
                new MenuGroup
                {
                    Title = "Atención",
                    Items = new List<MenuItem>
                    {
                        new MenuItem { Text = "Reclamos", Url = "reclamos", Icon = "⚠️" },
                        new MenuItem { Text = "Notificaciones", Url = "notificaciones", Icon = "🔔" }
                    }
                },
                new MenuGroup
                {
                    Title = "Reportes",
                    Items = new List<MenuItem>
                    {
                        new MenuItem { Text = "Ventas", Url = "ventas", Icon = "📈" },
                        new MenuItem { Text = "Calidad", Url = "calidad", Icon = "⭐" }
                    }
                },
                new MenuGroup
                {
                    Title = "Administración",
                    Items = new List<MenuItem>
                    {
                        new MenuItem { Text = "Usuarios", Url = "usuarios", Icon = "👤" },
                        new MenuItem { Text = "Auditoría", Url = "auditoria", Icon = "🔍" },
                        new MenuItem { Text = "Cerrar sesión", Url = "logout", Icon = "🚪" }
                    }
                }
            };
        }
    }

    public class MenuGroup
    {
        public string Title { get; set; }
        public List<MenuItem> Items { get; set; }
    }

    public class MenuItem
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
    }
}

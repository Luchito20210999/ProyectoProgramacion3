using Microsoft.AspNetCore.Components;
using ProyectoProgramacion3Web.Services;

namespace ProyectoProgramacion3Web.Components.Navigation
{
    public partial class NavMenu : IDisposable
    {
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private SessionService Session { get; set; } = default!;
        [Inject] private AppAccessPolicy AccessPolicy { get; set; } = default!;

        public List<MenuGroup> MenuGroups { get; set; } = new();
        public bool IsConfirmOpen { get; set; }
        public bool IsMobileMenuOpen { get; set; }

        protected override void OnInitialized()
        {
            InitializeMenu();
            Session.OnChange += RefreshMenu;
        }

        public void SolicitarCerrarSesion()
        {
            CloseMobileMenu();
            IsConfirmOpen = true;
        }

        public void AceptarCerrarSesion()
        {
            IsConfirmOpen = false;
            Navigation.NavigateTo("logout");
        }

        public void CancelarCerrarSesion()
        {
            IsConfirmOpen = false;
        }

        public void ToggleMobileMenu()
        {
            IsMobileMenuOpen = !IsMobileMenuOpen;
        }

        public void CloseMobileMenu()
        {
            IsMobileMenuOpen = false;
        }

        private void InitializeMenu()
        {
            var allGroups = new List<MenuGroup>
            {
                new()
                {
                    Title = "Principal",
                    Items = new List<MenuItem>
                    {
                        new() { Text = "Bienvenida", Url = "bienvenida", Icon = "👋" },
                        new() { Text = "Dashboard", Url = "dashboard", Icon = "📊" }
                    }
                },
                new()
                {
                    Title = "Operaciones",
                    Items = new List<MenuItem>
                    {
                        new() { Text = "Reservas", Url = "reservas", Icon = "🧾" },
                        new() { Text = "Clientes", Url = "clientes", Icon = "👥" },
                        new() { Text = "Servicios", Url = "servicios", Icon = "💼" }
                    }
                },
                new()
                {
                    Title = "Atencion",
                    Items = new List<MenuItem>
                    {
                        new() { Text = "Reclamos", Url = "reclamos", Icon = "⚠️" },
                        new() { Text = "Notificaciones", Url = "notificaciones", Icon = "🔔" }
                    }
                },
                new()
                {
                    Title = "Reportes",
                    Items = new List<MenuItem>
                    {
                        new() { Text = "Ventas", Url = "ventas", Icon = "📈" },
                        new() { Text = "Calidad", Url = "calidad", Icon = "⭐" }
                    }
                },
                new()
                {
                    Title = "Administracion",
                    Items = new List<MenuItem>
                    {
                        new() { Text = "Usuarios", Url = "usuarios", Icon = "👤" },
                        new() { Text = "Auditoria", Url = "auditoria", Icon = "🔍" },
                        new() { Text = "Bokun", Url = "bokun-config", Icon = "🔗" },
                        new() { Text = "Cerrar sesion", Url = "logout", Icon = "🚪" }
                    }
                }
            };

            MenuGroups = allGroups
                .Select(group => new MenuGroup
                {
                    Title = group.Title,
                    Items = group.Items
                        .Where(item => AccessPolicy.CanAccessMenuItem(Session.Role, item.Url))
                        .ToList()
                })
                .Where(group => group.Items.Any())
                .ToList();
        }

        private void RefreshMenu()
        {
            InitializeMenu();
            StateHasChanged();
        }

        public void Dispose()
        {
            Session.OnChange -= RefreshMenu;
        }
    }

    public class MenuGroup
    {
        public string Title { get; set; } = string.Empty;
        public List<MenuItem> Items { get; set; } = new();
    }

    public class MenuItem
    {
        public string Text { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }
}

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using ProyectoProgramacion3Web.Services;

namespace ProyectoProgramacion3Web.Components.Layout
{
    public partial class MainLayout : LayoutComponentBase, IDisposable
    {
        private int loadingVersion;
        private IDisposable? locationChangingRegistration;

        [Inject]
        public SessionService Session { get; set; } = default!;

        [Inject]
        public NavigationManager Navigation { get; set; } = default!;

        [Inject]
        public AppAccessPolicy AccessPolicy { get; set; } = default!;

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        public bool IsPageLoading { get; set; }
        public bool CanRenderBody { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            Session.OnChange += StateHasChanged;
            locationChangingRegistration = Navigation.RegisterLocationChangingHandler(OnLocationChanging);
            Navigation.LocationChanged += OnLocationChanged;
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            Session.RestoreFromClaims(authState.User);
            ValidateRouteAccess();
        }

        public void Dispose()
        {
            Session.OnChange -= StateHasChanged;
            Navigation.LocationChanged -= OnLocationChanged;
            locationChangingRegistration?.Dispose();
        }

        private async ValueTask OnLocationChanging(LocationChangingContext context)
        {
            ShowPageLoading();
            _ = HidePageLoadingAsync(++loadingVersion);
            await Task.Yield();
        }

        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            ShowPageLoading();
            var version = ++loadingVersion;
            ValidateRouteAccess();
            _ = HidePageLoadingAsync(version);
        }

        private void ValidateRouteAccess()
        {
            var route = Navigation.ToBaseRelativePath(Navigation.Uri)
                .Split('?', '#')[0]
                .Trim('/');

            if (string.IsNullOrEmpty(route) ||
                string.Equals(route, "login", StringComparison.OrdinalIgnoreCase))
            {
                CanRenderBody = true;
                return;
            }

            if (string.Equals(route, "logout", StringComparison.OrdinalIgnoreCase))
            {
                CanRenderBody = true;
                return;
            }

            if (!Session.IsLoggedIn)
            {
                CanRenderBody = false;
                NavigateIfNeeded(string.Empty);
                return;
            }

            if (!AccessPolicy.CanAccess(Session.Role, route))
            {
                CanRenderBody = false;
                NavigateIfNeeded(AccessPolicy.GetDefaultRoute(Session.Role));
                return;
            }

            CanRenderBody = true;
        }

        private void ShowPageLoading()
        {
            IsPageLoading = true;
            _ = InvokeAsync(StateHasChanged);
        }

        private void NavigateIfNeeded(string targetRoute)
        {
            var currentRoute = Navigation.ToBaseRelativePath(Navigation.Uri)
                .Split('?', '#')[0]
                .Trim('/');
            var normalizedTarget = targetRoute.Trim('/');

            if (!string.Equals(currentRoute, normalizedTarget, StringComparison.OrdinalIgnoreCase))
            {
                Navigation.NavigateTo(normalizedTarget, replace: true);
            }
        }

        private async Task HidePageLoadingAsync(int version)
        {
            await Task.Delay(350);

            if (version == loadingVersion)
            {
                IsPageLoading = false;
                await InvokeAsync(StateHasChanged);
            }
        }
    }
}

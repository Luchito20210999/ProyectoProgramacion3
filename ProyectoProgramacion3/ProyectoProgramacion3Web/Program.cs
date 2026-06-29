using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using ProyectoProgramacion3Web.Components;
using ProyectoProgramacion3Web.Services;
using ProyectoProgramacion3Web.Servicios.Auditoria;
using ProyectoProgramacion3Web.Servicios.Clientes;
using ProyectoProgramacion3Web.Servicios.Notificaciones;
using ProyectoProgramacion3Web.Servicios.Reclamos;
using ProyectoProgramacion3Web.Servicios.Reservas;
using ProyectoProgramacion3Web.Servicios.ServiciosTuristicos;
using ProyectoProgramacion3Web.Servicios.Usuarios;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var dataProtectionKeysPath = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "DataProtectionKeys");
Directory.CreateDirectory(dataProtectionKeysPath);
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionKeysPath));

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "SACR.Auth";
        options.LoginPath = "/";
        options.LogoutPath = "/logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<SessionService>();
builder.Services.AddSingleton<AppAccessPolicy>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IReservasServiceClient, ReservasServiceRestClient>();
builder.Services.AddScoped<IClientesServiceClient, ClientesServiceRestClient>();
builder.Services.AddScoped<IServiciosServiceClient, ServiciosServiceRestClient>();
builder.Services.AddScoped<IUsuariosServiceClient, UsuariosServiceRestClient>();
builder.Services.AddScoped<IReclamosServiceClient, ReclamosServiceRestClient>();
builder.Services.AddScoped<INotificacionesServiceClient, NotificacionesServiceRestClient>();
builder.Services.AddScoped<IAuditoriaServiceClient, AuditoriaServiceRestClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapPost("/auth/login", async (HttpContext httpContext, IUsuariosServiceClient usuariosServiceClient) =>
{
    var form = await httpContext.Request.ReadFormAsync();
    var email = form["email"].ToString();
    var password = form["password"].ToString();
    var rememberMe = string.Equals(form["rememberMe"].ToString(), "on", StringComparison.OrdinalIgnoreCase);

    var user = usuariosServiceClient.Login(email, password);
    if (user is null)
    {
        return Results.Redirect("/?error=1");
    }

    var fullName = user.NombreCompleto.Trim();
    var initials = BuildInitials(user.Nombres, user.Apellidos);
    var claims = new List<Claim>
    {
        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new(ClaimTypes.Name, string.IsNullOrWhiteSpace(fullName) ? user.Correo : fullName),
        new(ClaimTypes.Email, user.Correo),
        new(ClaimTypes.Role, AppAccessPolicy.NormalizeRole(user.Tipo)),
        new("initials", initials)
    };

    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    var properties = new AuthenticationProperties
    {
        IsPersistent = rememberMe,
        ExpiresUtc = rememberMe
            ? DateTimeOffset.UtcNow.AddDays(7)
            : DateTimeOffset.UtcNow.AddHours(8)
    };

    await httpContext.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme,
        new ClaimsPrincipal(identity),
        properties);

    return Results.Redirect("/bienvenida");
});

app.MapGet("/auth/logout", async (HttpContext httpContext) =>
{
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/");
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

static string BuildInitials(string nombres, string apellidos)
{
    var firstInit = string.IsNullOrWhiteSpace(nombres) ? string.Empty : nombres.Trim()[0].ToString();
    var lastInit = string.IsNullOrWhiteSpace(apellidos) ? string.Empty : apellidos.Trim()[0].ToString();
    return (firstInit + lastInit).ToUpperInvariant();
}

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

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

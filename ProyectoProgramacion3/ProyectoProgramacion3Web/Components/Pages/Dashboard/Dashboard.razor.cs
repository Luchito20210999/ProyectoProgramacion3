using Microsoft.AspNetCore.Components;
using ProyectoProgramacion3Web.Services;
using ProyectoProgramacion3Web.Servicios.Reclamos;
using ProyectoProgramacion3Web.Servicios.Reservas;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Components.Pages.Dashboard
{
    public partial class Dashboard : ComponentBase
    {
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private SessionService Session { get; set; } = default!;
        [Inject] private AppAccessPolicy AccessPolicy { get; set; } = default!;
        [Inject] private IReservasServiceClient ReservasServiceClient { get; set; } = default!;
        [Inject] private IReclamosServiceClient ReclamosServiceClient { get; set; } = default!;

        public int ReservasHoy { get; set; }
        public decimal VentasUSD { get; set; }
        public int ReclamosPendientes { get; set; }
        public double PorcentajeIncidencias { get; set; }
        public List<VentaDia> VentasSieteDias { get; set; } = new();
        public List<ReclamoEstado> ReclamosPorEstado { get; set; } = new();
        public List<Reserva> UltimasReservas { get; set; } = new();
        public string MaxEjeString { get; set; } = "$0";
        public string HighMedEjeString { get; set; } = "$0";
        public string LowMedEjeString { get; set; } = "$0";
        public string MinEjeString { get; set; } = "$0";
        public bool TieneDatosGrafico { get; set; }
        public bool PuedeVerReservas => AccessPolicy.CanAccessMenuItem(Session.Role, "reservas");

        protected override void OnInitialized()
        {
            CargarDatosDashboard();
        }

        private void CargarDatosDashboard()
        {
            var reservasUi = ReservasServiceClient.Listar().Select(MapearReserva).ToList();
            var reclamos = ReclamosServiceClient.Listar();

            ReservasHoy = reservasUi.Count(r => r.Fecha.Date == DateTime.Today);
            UltimasReservas = reservasUi.OrderByDescending(r => r.Fecha).Take(5).ToList();
            VentasUSD = reservasUi.Where(r => !EsAnulada(r.Estado)).Sum(r => r.Monto);
            ReclamosPendientes = reclamos.Count(r => r.Estado is "PENDIENTE" or "EN_ATENCION");
            PorcentajeIncidencias = reservasUi.Count > 0 ? (double)reclamos.Count / reservasUi.Count * 100 : 0;

            CargarGraficoSemanal(reservasUi);
            ReclamosPorEstado = reclamos
                .GroupBy(r => r.Estado)
                .Select(g => new ReclamoEstado
                {
                    Estado = g.Key,
                    Contador = g.Count(),
                    ColorClase = g.Key switch
                    {
                        "PENDIENTE" => "status-pendiente",
                        "EN_ATENCION" => "status-atencion",
                        "PROCEDE" => "status-procede",
                        "NO_PROCEDE" => "status-noprocede",
                        _ => "status-pendiente"
                    }
                })
                .ToList();
        }

        private void CargarGraficoSemanal(List<Reserva> reservas)
        {
            var ventasPorDia = new Dictionary<DayOfWeek, decimal>
            {
                { DayOfWeek.Monday, 0m }, { DayOfWeek.Tuesday, 0m }, { DayOfWeek.Wednesday, 0m },
                { DayOfWeek.Thursday, 0m }, { DayOfWeek.Friday, 0m }, { DayOfWeek.Saturday, 0m },
                { DayOfWeek.Sunday, 0m }
            };

            reservas.Where(r => !EsAnulada(r.Estado)).ToList().ForEach(res => ventasPorDia[res.Fecha.DayOfWeek] += res.Monto);
            decimal maxVentaDia = ventasPorDia.Values.DefaultIfEmpty(0m).Max();
            TieneDatosGrafico = maxVentaDia > 0;
            MaxEjeString = TieneDatosGrafico ? $"${maxVentaDia:N0}" : "$0";
            HighMedEjeString = TieneDatosGrafico ? $"${(maxVentaDia * 0.66m):N0}" : "$0";
            LowMedEjeString = TieneDatosGrafico ? $"${(maxVentaDia * 0.33m):N0}" : "$0";
            MinEjeString = "$0";

            VentasSieteDias = new List<VentaDia>
            {
                new() { Dia = "Lun", Monto = ventasPorDia[DayOfWeek.Monday] },
                new() { Dia = "Mar", Monto = ventasPorDia[DayOfWeek.Tuesday] },
                new() { Dia = "Mie", Monto = ventasPorDia[DayOfWeek.Wednesday] },
                new() { Dia = "Jue", Monto = ventasPorDia[DayOfWeek.Thursday] },
                new() { Dia = "Vie", Monto = ventasPorDia[DayOfWeek.Friday] },
                new() { Dia = "Sab", Monto = ventasPorDia[DayOfWeek.Saturday] },
                new() { Dia = "Dom", Monto = ventasPorDia[DayOfWeek.Sunday] }
            };

            VentasSieteDias.ForEach(v =>
            {
                v.PorcentajeAltura = maxVentaDia > 0 ? (int)((v.Monto / maxVentaDia) * 100) : 0;
                v.EsMaximo = maxVentaDia > 0 && v.Monto == maxVentaDia;
            });
        }

        public void VerTodasReservas()
        {
            if (PuedeVerReservas)
            {
                Navigation.NavigateTo("reservas");
            }
        }

        private static Reserva MapearReserva(ReservaItem reserva)
        {
            string estado = reserva.Estado;
            return new Reserva
            {
                Codigo = reserva.Codigo,
                Cliente = reserva.Cliente,
                Servicio = reserva.Servicio,
                Fecha = reserva.FechaServicio,
                Monto = reserva.Monto,
                Estado = estado,
                EstadoClase = EsAnulada(estado) ? "badge-anulada" : estado.Contains("PEND", StringComparison.OrdinalIgnoreCase) ? "badge-pendiente" : "badge-confirmada"
            };
        }

        private static bool EsAnulada(string estado)
        {
            return estado.Contains("ANUL", StringComparison.OrdinalIgnoreCase) ||
                   estado.Contains("CANCEL", StringComparison.OrdinalIgnoreCase);
        }

    }

    public class VentaDia
    {
        public string Dia { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public int PorcentajeAltura { get; set; }
        public bool EsMaximo { get; set; }
    }

    public class ReclamoEstado
    {
        public string Estado { get; set; } = string.Empty;
        public int Contador { get; set; }
        public string ColorClase { get; set; } = string.Empty;
    }

    public class Reserva
    {
        public string Codigo { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public string Servicio { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string EstadoClase { get; set; } = string.Empty;
    }
}

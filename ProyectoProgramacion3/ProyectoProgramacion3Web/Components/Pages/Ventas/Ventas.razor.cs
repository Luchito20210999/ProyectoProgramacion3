using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ProyectoProgramacion3Web.Servicios.Reservas;

namespace ProyectoProgramacion3Web.Components.Pages.Ventas
{
    public partial class Ventas : ComponentBase
    {
        [Inject] private IJSRuntime JS { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IReservasServiceClient ReservasServiceClient { get; set; } = default!;

        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string ServicioSeleccionado { get; set; } = "Todos";
        public string CanalSeleccionado { get; set; } = "Todos";
        public decimal TotalVentas { get; set; }
        public int ReservasCount { get; set; }
        public decimal TicketPromedio { get; set; }
        public int PaxAtendidos { get; set; }
        public decimal MaxMontoMes { get; set; }
        public List<VentaReservaItem> MasterReservas { get; set; } = new();
        public List<VentaReservaItem> ListadoFiltrado { get; set; } = new();
        public List<TopServicioItem> TopServicios { get; set; } = new();
        public List<VentasMesItem> VentasPorMes { get; set; } = new();

        protected override void OnInitialized()
        {
            CargarDatosIniciales();
            FiltrarDatos();
        }

        private void CargarDatosIniciales()
        {
            MasterReservas = ReservasServiceClient.Listar().Select(r => new VentaReservaItem
            {
                Codigo = r.Codigo,
                Cliente = r.Cliente,
                Servicio = string.IsNullOrWhiteSpace(r.Servicio) ? "Sin servicio" : r.Servicio,
                Fecha = r.FechaServicio,
                Pax = r.Pax,
                Monto = r.Monto,
                Canal = r.Codigo.StartsWith("BK", StringComparison.OrdinalIgnoreCase) ? "Bokun" : "Java"
            }).ToList();
        }

        public void FiltrarDatos()
        {
            var query = MasterReservas.AsQueryable();
            if (FechaInicio.HasValue) query = query.Where(r => r.Fecha.Date >= FechaInicio.Value.Date);
            if (FechaFin.HasValue) query = query.Where(r => r.Fecha.Date <= FechaFin.Value.Date);
            if (ServicioSeleccionado != "Todos") query = query.Where(r => r.Servicio == ServicioSeleccionado);
            if (CanalSeleccionado != "Todos") query = query.Where(r => r.Canal == CanalSeleccionado);

            ListadoFiltrado = query.OrderByDescending(r => r.Fecha).ToList();
            TotalVentas = ListadoFiltrado.Sum(r => r.Monto);
            ReservasCount = ListadoFiltrado.Count;
            TicketPromedio = ReservasCount > 0 ? TotalVentas / ReservasCount : 0m;
            PaxAtendidos = ListadoFiltrado.Sum(r => r.Pax);

            TopServicios = ListadoFiltrado.GroupBy(r => r.Servicio)
                .Select(g => new TopServicioItem { Servicio = g.Key, TotalMonto = g.Sum(r => r.Monto) })
                .OrderByDescending(t => t.TotalMonto)
                .Take(5)
                .ToList();

            string[] nombresMeses = { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };
            var tempMeses = new List<VentasMesItem>();
            decimal maxMonto = 0m;

            for (int i = 0; i < 12; i++)
            {
                decimal montoMes = ListadoFiltrado.Where(r => r.Fecha.Month == (i + 1)).Sum(r => r.Monto);
                if (montoMes > maxMonto) maxMonto = montoMes;
                tempMeses.Add(new VentasMesItem { Mes = nombresMeses[i], Monto = montoMes });
            }

            tempMeses.ForEach(item => item.PorcentajeAltura = maxMonto > 0 ? (double)(item.Monto / maxMonto) * 100 : 0);
            VentasPorMes = tempMeses;
            MaxMontoMes = maxMonto;
        }

        public async Task ExportarACsv()
        {
            var csv = new System.Text.StringBuilder("\uFEFF");
            csv.AppendLine("REPORTE DE VENTAS - SACR");
            csv.AppendLine($"Fecha de Generacion:;{DateTime.Now:dd/MM/yyyy HH:mm}");
            csv.AppendLine($"Filtros:;Servicio: {ServicioSeleccionado};Canal: {CanalSeleccionado}");
            csv.AppendLine();
            csv.AppendLine($"RESUMEN:;Total Ventas:;${TotalVentas:N2};Total Reservas:;{ReservasCount};Ticket Promedio:;${TicketPromedio:N2};Pax Atendidos:;{PaxAtendidos}");
            csv.AppendLine();
            csv.AppendLine("DETALLE TRANSACCIONAL DE RESERVAS");
            csv.AppendLine("Codigo Reserva;Cliente;Servicio;Fecha;Pax;Monto;Canal");
            ListadoFiltrado.ForEach(res => csv.AppendLine($"{res.Codigo};{res.Cliente};{res.Servicio};{res.Fecha:dd/MM/yyyy};{res.Pax};${res.Monto:N2};{res.Canal}"));

            var base64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(csv.ToString()));
            await JS.InvokeVoidAsync("downloadFileFromBase64", $"Reporte_Ventas_{DateTime.Now:yyyyMMdd_HHmmss}.csv", base64, "text/csv;charset=utf-8");
        }

    }

    public class VentaReservaItem
    {
        public string Codigo { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public string Servicio { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public int Pax { get; set; }
        public decimal Monto { get; set; }
        public string Canal { get; set; } = string.Empty;
    }

    public class TopServicioItem
    {
        public string Servicio { get; set; } = string.Empty;
        public decimal TotalMonto { get; set; }
    }

    public class VentasMesItem
    {
        public string Mes { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public double PorcentajeAltura { get; set; }
    }
}

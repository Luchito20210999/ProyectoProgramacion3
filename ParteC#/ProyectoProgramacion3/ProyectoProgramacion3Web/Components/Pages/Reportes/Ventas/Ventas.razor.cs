using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ProyectoProgramacion3Web.Components.Pages.Ventas
{
    /// <summary>
    /// Componente code-behind de la vista de Reporte de Ventas.
    /// Administra la carga de datos simulados y los filtros de ingresos de SACR.
    /// </summary>
    public partial class Ventas : ComponentBase
    {
        [Inject] private IJSRuntime JS { get; set; }
        [Inject] private NavigationManager Navigation { get; set; }

        // Filtros enlazados a la interfaz
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string ServicioSeleccionado { get; set; } = "Todos";
        public string CanalSeleccionado { get; set; } = "Todos";

        // KPIs e Indicadores Clave de Desempeño recalculados en tiempo real
        public decimal TotalVentas { get; set; }
        public int ReservasCount { get; set; }
        public decimal TicketPromedio { get; set; }
        public int PaxAtendidos { get; set; }
        public decimal MaxMontoMes { get; set; }

        // Colecciones para renderizar datos estructurados y gráficos
        public List<VentaReservaItem> MasterReservas { get; set; } = new();
        public List<VentaReservaItem> ListadoFiltrado { get; set; } = new();
        public List<TopServicioItem> TopServicios { get; set; } = new();
        public List<VentasMesItem> VentasPorMes { get; set; } = new();

        /// <summary>
        /// Inicializa el componente, gatilla el sembrado de datos y aplica los filtros predeterminados.
        /// </summary>
        protected override void OnInitialized()
        {
            CargarDatosIniciales();
            FiltrarDatos();
        }

        /// <summary>
        /// Genera de manera procedimental y determinista los 20 registros de reservas.
        /// </summary>
        private void CargarDatosIniciales()
        {
            MasterReservas = new List<VentaReservaItem>();
            string[] services = { "Machu Picchu Full Day", "City Tour Lima", "Valle Sagrado", "Islas Ballestas" };
            
            // Agregamos reservas especiales para fidelidad de pruebas y búsquedas específicas
            MasterReservas.Add(new VentaReservaItem { Codigo = "BK-10294", Cliente = "James Carter", Servicio = "Machu Picchu Full Day", Fecha = new DateTime(2026, 5, 15), Pax = 4, Monto = 480.00m, Canal = "Directo" });
            MasterReservas.Add(new VentaReservaItem { Codigo = "BK-10293", Cliente = "Lucía Fernández", Servicio = "City Tour Lima", Fecha = new DateTime(2026, 5, 14), Pax = 2, Monto = 120.00m, Canal = "Bokun" });
            MasterReservas.Add(new VentaReservaItem { Codigo = "BK-10295", Cliente = "Marta Ríos", Servicio = "Islas Ballestas", Fecha = new DateTime(2026, 5, 16), Pax = 1, Monto = 75.00m, Canal = "Bokun" });

            // Ciclo compacto de generación determinista para los 17 registros restantes (total 20 con los 3 específicos)
            for (int i = 1; i <= 17; i++)
            {
                int serviceIndex = i % services.Length;
                string svc = services[serviceIndex];
                int month = (i % 6) + 1;
                
                // Pax balanceado
                int pax = (i % 2 == 0) ? 2 : 3;
                
                // Variación controlada de montos
                decimal basePrice = (serviceIndex == 0 ? 120m : serviceIndex == 1 ? 60m : serviceIndex == 2 ? 90m : 50m) * pax;
                if (i % 3 == 0) basePrice *= 1.2m;
                else if (i % 2 == 0) basePrice *= 0.8m;
                decimal monto = Math.Round(basePrice, 2);

                if (i == 17) monto = 500.00m; // Último elemento fijado en exactamente $500.00

                MasterReservas.Add(new VentaReservaItem
                {
                    Codigo = $"BK-{(20000 + i * 17) % 90000}",
                    Cliente = GetMockClienteName(i),
                    Servicio = svc,
                    Fecha = new DateTime(2026, month, (i % 28) + 1),
                    Pax = pax,
                    Monto = monto,
                    Canal = (i % 2 == 0) ? "Bokun" : "Directo"
                });
            }
        }

        /// <summary>
        /// Retorna un nombre ficticio determinista según índice para simulación de clientes.
        /// </summary>
        private string GetMockClienteName(int index)
        {
            string[] names = {
                "Lucía Fernández", "James Carter", "Marta Ríos", "Laura Benavides", "Carlos Mendoza",
                "Sophie Dubois", "James Wilson", "Mariana Costa", "Kenji Sato", "Pedro Núñez"
            };
            return names[index % names.Length];
        }

        /// <summary>
        /// Aplica los criterios de filtrado dinámico seleccionados por el usuario
        /// y recalcula las sumatorias, tops y porcentajes de las gráficas de barra en tiempo real.
        /// </summary>
        public void FiltrarDatos()
        {
            var query = MasterReservas.AsQueryable();
            if (FechaInicio.HasValue) query = query.Where(r => r.Fecha.Date >= FechaInicio.Value.Date);
            if (FechaFin.HasValue) query = query.Where(r => r.Fecha.Date <= FechaFin.Value.Date);
            if (ServicioSeleccionado != "Todos") query = query.Where(r => r.Servicio == ServicioSeleccionado);
            if (CanalSeleccionado != "Todos") query = query.Where(r => r.Canal == CanalSeleccionado);

            ListadoFiltrado = query.OrderByDescending(r => r.Fecha).ToList();
            
            // Recalcular métricas principales
            TotalVentas = ListadoFiltrado.Sum(r => r.Monto);
            ReservasCount = ListadoFiltrado.Count;
            TicketPromedio = ReservasCount > 0 ? TotalVentas / ReservasCount : 0m;
            PaxAtendidos = ListadoFiltrado.Sum(r => r.Pax);

            // Generar ranking de top servicios (máximo 5 items)
            TopServicios = ListadoFiltrado.GroupBy(r => r.Servicio)
                .Select(g => new TopServicioItem { Servicio = g.Key, TotalMonto = g.Sum(r => r.Monto) })
                .OrderByDescending(t => t.TotalMonto).Take(5).ToList();

            // Calcular montos de ventas distribuidos por mes (Jan-Jun 2026) para los gráficos
            string[] nombresMeses = { "Ene", "Feb", "Mar", "Abr", "May", "Jun" };
            var tempMeses = new List<VentasMesItem>();
            decimal maxMonto = 0m;

            for (int i = 0; i < 6; i++)
            {
                decimal montoMes = ListadoFiltrado.Where(r => r.Fecha.Month == (i + 1)).Sum(r => r.Monto);
                if (montoMes > maxMonto) maxMonto = montoMes;
                tempMeses.Add(new VentasMesItem { Mes = nombresMeses[i], Monto = montoMes });
            }

            // Normalizar las alturas de las barras en un rango porcentual (0-100)
            tempMeses.ForEach(item => item.PorcentajeAltura = maxMonto > 0 ? (double)(item.Monto / maxMonto) * 100 : 0);
            VentasPorMes = tempMeses;
            MaxMontoMes = maxMonto;
        }

        /// <summary>
        /// Genera y descarga el reporte estructurado en formato CSV con codificación UTF-8 y BOM
        /// para garantizar total compatibilidad con caracteres en español en Microsoft Excel.
        /// </summary>
        public async Task ExportarAExcel()
        {
            var csv = new System.Text.StringBuilder("\uFEFF"); // Agregar BOM
            csv.AppendLine("REPORTE DE VENTAS - SACR");
            csv.AppendLine($"Fecha de Generación:;{DateTime.Now:dd/MM/yyyy HH:mm}");
            csv.AppendLine($"Filtros:;Servicio: {ServicioSeleccionado};Canal: {CanalSeleccionado}");
            csv.AppendLine();
            csv.AppendLine($"RESUMEN:;Total Ventas:;${TotalVentas:N2};Total Reservas:;{ReservasCount};Ticket Promedio:;${TicketPromedio:N2};Pax Atendidos:;{PaxAtendidos}");
            csv.AppendLine();
            csv.AppendLine("RANKING DE SERVICIOS MAS RENTABLES");
            csv.AppendLine("Servicio;Monto Total");
            TopServicios.ForEach(ts => csv.AppendLine($"{ts.Servicio};${ts.TotalMonto:N2}"));
            csv.AppendLine();
            csv.AppendLine("DETALLE TRANSACCIONAL DE RESERVAS");
            csv.AppendLine("Código Reserva;Cliente;Servicio;Fecha;Pax;Monto;Canal");
            ListadoFiltrado.ForEach(res => csv.AppendLine($"{res.Codigo};{res.Cliente};{res.Servicio};{res.Fecha:dd/MM/yyyy};{res.Pax};${res.Monto:N2};{res.Canal}"));

            var base64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(csv.ToString()));
            await JS.InvokeVoidAsync("downloadFileFromBase64", $"Reporte_Ventas_{DateTime.Now:yyyyMMdd_HHmmss}.csv", base64, "text/csv;charset=utf-8");
        }
    }

    public class VentaReservaItem
    {
        public string Codigo { get; set; }
        public string Cliente { get; set; }
        public string Servicio { get; set; }
        public DateTime Fecha { get; set; }
        public int Pax { get; set; }
        public decimal Monto { get; set; }
        public string Canal { get; set; }
    }

    public class TopServicioItem
    {
        public string Servicio { get; set; }
        public decimal TotalMonto { get; set; }
    }

    public class VentasMesItem
    {
        public string Mes { get; set; }
        public decimal Monto { get; set; }
        public double PorcentajeAltura { get; set; }
    }
}

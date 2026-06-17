using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace ProyectoProgramacion3Web.Components.Pages.Dashboard
{
    /// <summary>
    /// Componente code-behind de la vista de Dashboard de SACR.
    /// Presenta KPIs en tiempo real de reservas, ventas acumuladas, incidencias de calidad y el histórico semanal de facturación.
    /// </summary>
    public partial class Dashboard : ComponentBase
    {
        [Inject] private NavigationManager Navigation { get; set; }

        // Métricas de Desempeño (KPIs) a renderizar en las tarjetas superiores
        public int ReservasHoy { get; set; }
        public decimal VentasUSD { get; set; }
        public int ReclamosPendientes { get; set; }
        public double PorcentajeIncidencias { get; set; }

        // Colecciones de Datos para el gráfico de barras y la tabla resumen
        public List<VentaDia> VentasSieteDias { get; set; } = new();
        public List<ReclamoEstado> ReclamosPorEstado { get; set; } = new();
        public List<Reserva> UltimasReservas { get; set; } = new();

        // Datos y ticks de escala de Eje Y para el gráfico semanal
        public string MaxEjeString { get; set; }
        public string HighMedEjeString { get; set; }
        public string LowMedEjeString { get; set; }
        public string MinEjeString { get; set; }
        public bool TieneDatosGrafico { get; set; }

        /// <summary>
        /// Inicializa el componente y gatilla la carga de métricas y datos estadísticos del Dashboard.
        /// </summary>
        protected override void OnInitialized()
        {
            CargarDatosDashboard();
        }

        /// <summary>
        /// Realiza el cálculo dinámico de métricas consolidadas a partir de las reservas cargadas en memoria,
        /// estructurando la escala del gráfico y la distribución de reclamos.
        /// </summary>
        private void CargarDatosDashboard()
        {
            // Métricas base fijas para simular datos del panel
            ReservasHoy = 2;
            ReclamosPendientes = 8;
            PorcentajeIncidencias = 3.2;

            // Sembrado de últimas reservas registradas en el sistema
            UltimasReservas = new List<Reserva>
            {
                new Reserva { Codigo = "BK-84920", Cliente = "Laura Benavides", Servicio = "Camino Inca 4 Días", Fecha = new DateTime(2026, 05, 30), Monto = 580.00m, Estado = "Confirmada", EstadoClase = "badge-confirmada" },
                new Reserva { Codigo = "BK-10492", Cliente = "Carlos Mendoza", Servicio = "City Tour Cusco Premium", Fecha = new DateTime(2026, 05, 30), Monto = 120.00m, Estado = "Confirmada", EstadoClase = "badge-confirmada" },
                new Reserva { Codigo = "BK-39502", Cliente = "Sophie Dubois", Servicio = "Machu Picchu Mágico Full Day", Fecha = new DateTime(2026, 05, 29), Monto = 340.00m, Estado = "Pendiente", EstadoClase = "badge-pendiente" },
                new Reserva { Codigo = "BK-92049", Cliente = "James Wilson", Servicio = "Líneas de Nazca Sobrevuelo", Fecha = new DateTime(2026, 05, 29), Monto = 210.00m, Estado = "Confirmada", EstadoClase = "badge-confirmada" },
                new Reserva { Codigo = "BK-48201", Cliente = "Mariana Costa", Servicio = "Valle Sagrado VIP + Almuerzo", Fecha = new DateTime(2026, 05, 28), Monto = 160.00m, Estado = "Anulada", EstadoClase = "badge-anulada" }
            };

            // Cálculo agregador de ventas a partir de reservas válidas
            VentasUSD = UltimasReservas.Where(r => r.Estado != "Anulada").Sum(r => r.Monto);

            // Estructuración del acumulador diario de facturación semanal (de lunes a domingo)
            var ventasPorDia = new Dictionary<DayOfWeek, decimal> { { DayOfWeek.Monday, 0m }, { DayOfWeek.Tuesday, 0m }, { DayOfWeek.Wednesday, 0m }, { DayOfWeek.Thursday, 0m }, { DayOfWeek.Friday, 0m }, { DayOfWeek.Saturday, 0m }, { DayOfWeek.Sunday, 0m } };
            UltimasReservas.Where(r => r.Estado != "Anulada").ToList().ForEach(res => ventasPorDia[res.Fecha.DayOfWeek] += res.Monto);

            decimal maxVentaDia = ventasPorDia.Values.Max();
            TieneDatosGrafico = maxVentaDia > 0;

            if (TieneDatosGrafico)
            {
                // Configurar etiquetas de escala en el Eje Y del gráfico
                MaxEjeString = $"${maxVentaDia:N0}";
                HighMedEjeString = $"${(maxVentaDia * 0.66m):N0}";
                LowMedEjeString = $"${(maxVentaDia * 0.33m):N0}";
                MinEjeString = "$0";

                // Construcción de la tendencia en barra para los 7 días
                VentasSieteDias = new List<VentaDia>
                {
                    new VentaDia { Dia = "Lun", Monto = ventasPorDia[DayOfWeek.Monday] },
                    new VentaDia { Dia = "Mar", Monto = ventasPorDia[DayOfWeek.Tuesday] },
                    new VentaDia { Dia = "Mié", Monto = ventasPorDia[DayOfWeek.Wednesday] },
                    new VentaDia { Dia = "Jue", Monto = ventasPorDia[DayOfWeek.Thursday] },
                    new VentaDia { Dia = "Vie", Monto = ventasPorDia[DayOfWeek.Friday] },
                    new VentaDia { Dia = "Sáb", Monto = ventasPorDia[DayOfWeek.Saturday] },
                    new VentaDia { Dia = "Dom", Monto = ventasPorDia[DayOfWeek.Sunday] }
                };

                // Calcular altura porcentual (0-100) y destacar el máximo del periodo
                VentasSieteDias.ForEach(v => {
                    v.PorcentajeAltura = (int)((v.Monto / maxVentaDia) * 100);
                    v.EsMaximo = v.Monto == maxVentaDia;
                });
            }
            else
            {
                MaxEjeString = HighMedEjeString = LowMedEjeString = MinEjeString = "$0";
            }

            // Datos de distribución por estado de reclamos
            ReclamosPorEstado = new List<ReclamoEstado>
            {
                new ReclamoEstado { Estado = "Pendiente", Contador = 8, ColorClase = "status-pendiente" },
                new ReclamoEstado { Estado = "En atención", Contador = 5, ColorClase = "status-atencion" },
                new ReclamoEstado { Estado = "Procede", Contador = 14, ColorClase = "status-procede" },
                new ReclamoEstado { Estado = "No procede", Contador = 3, ColorClase = "status-noprocede" }
            };
        }

        /// <summary>
        /// Redirige al usuario al módulo central de Reservas del sistema SACR.
        /// </summary>
        public void VerTodasReservas()
        {
            Navigation.NavigateTo("reservas");
        }
    }

    public class VentaDia
    {
        public string Dia { get; set; }
        public decimal Monto { get; set; }
        public int PorcentajeAltura { get; set; }
        public bool EsMaximo { get; set; }
    }

    public class ReclamoEstado
    {
        public string Estado { get; set; }
        public int Contador { get; set; }
        public string ColorClase { get; set; }
    }

    public class Reserva
    {
        public string Codigo { get; set; }
        public string Cliente { get; set; }
        public string Servicio { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; }
        public string EstadoClase { get; set; }
    }
}

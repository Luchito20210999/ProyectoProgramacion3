using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ProyectoProgramacion3Web.Components.Pages.Calidad
{
    /// <summary>
    /// Componente code-behind de la vista de Reporte de Calidad.
    /// Gestiona la visualización de reclamos, incidencias y la tasa de resolución del servicio SACR.
    /// </summary>
    public partial class Calidad : ComponentBase
    {
        [Inject] private IJSRuntime JS { get; set; }
        [Inject] private NavigationManager Navigation { get; set; }

        // Filtros enlazados
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string ServicioSeleccionado { get; set; } = "Todos";

        // KPIs de Calidad recalculados dinámicamente
        public int ReservasCount { get; set; }
        public int ReclamosCount { get; set; }
        public decimal IncidenciasRate { get; set; }
        public double TiempoPromedioAtencion { get; set; }
        public double MaxReclamosCount { get; set; }

        // Distribución de procedencia de Reclamos (Procede, No procede, Pendiente)
        public int ProcedeCount { get; set; }
        public double ProcedePercent { get; set; }
        public int NoProcedeCount { get; set; }
        public double NoProcedePercent { get; set; }
        public int PendientesCount { get; set; }
        public double PendientesPercent { get; set; }

        // Colecciones de Datos Maestros y de Renderizado
        public List<CalidadReservaItem> MasterReservas { get; set; } = new();
        public List<CalidadReclamoItem> MasterReclamos { get; set; } = new();
        public List<CalidadReclamoItem> ListadoFiltrado { get; set; } = new();
        public List<ReclamoServicioItem> ReclamosPorServicio { get; set; } = new();

        /// <summary>
        /// Inicializa el componente, carga el sembrado de datos y aplica los filtros predeterminados.
        /// </summary>
        protected override void OnInitialized()
        {
            CargarDatosIniciales();
            FiltrarDatos();
        }

        /// <summary>
        /// Genera de manera determinista los 312 registros de reservas y los 26 de reclamos para mantener la coherencia
        /// matemática de la tasa de incidencias (8.3%) y el tiempo de atención promedio (2.4 días).
        /// </summary>
        private void CargarDatosIniciales()
        {
            MasterReservas = new List<CalidadReservaItem>();
            string[] services = { "City Tour Lima", "Islas Ballestas", "Machu Picchu Full Day", "Valle Sagrado" };
            int[] counts = { 112, 90, 66, 44 }; // Total: 312 reservas

            // Sembrado de 312 reservas distribuidas de forma compacta
            int index = 1;
            for (int s = 0; s < services.Length; s++)
            {
                for (int i = 0; i < counts[s]; i++)
                {
                    MasterReservas.Add(new CalidadReservaItem
                    {
                        Codigo = $"BK-{10000 + index++}",
                        Servicio = services[s],
                        Fecha = new DateTime(2026, (i % 2 == 0 ? 4 : 5), 1 + (i * 3) % 28)
                    });
                }
            }

            // Sembrado de 26 Reclamos detallados
            MasterReclamos = new List<CalidadReclamoItem>
            {
                new CalidadReclamoItem { Id = 1, CodigoReserva = "BK-10288", Servicio = "Machu Picchu Full Day", Fecha = new DateTime(2026, 5, 9), Procedencia = "En atención", Resolucion = "—", DiasAtencion = 0 },
                new CalidadReclamoItem { Id = 2, CodigoReserva = "BK-10101", Servicio = "Machu Picchu Full Day", Fecha = new DateTime(2026, 5, 10), Procedencia = "Procede", Resolucion = "Guía asignado tarde", DiasAtencion = 1 },
                new CalidadReclamoItem { Id = 3, CodigoReserva = "BK-10102", Servicio = "Machu Picchu Full Day", Fecha = new DateTime(2026, 5, 12), Procedencia = "Procede", Resolucion = "Compensación almuerzo", DiasAtencion = 1 },
                new CalidadReclamoItem { Id = 4, CodigoReserva = "BK-10103", Servicio = "Machu Picchu Full Day", Fecha = new DateTime(2026, 5, 13), Procedencia = "Procede", Resolucion = "Reintegro por entradas", DiasAtencion = 2 },
                new CalidadReclamoItem { Id = 5, CodigoReserva = "BK-10104", Servicio = "Machu Picchu Full Day", Fecha = new DateTime(2026, 5, 15), Procedencia = "Procede", Resolucion = "Reembolso boleto Consettur", DiasAtencion = 2 },
                new CalidadReclamoItem { Id = 6, CodigoReserva = "BK-10105", Servicio = "Machu Picchu Full Day", Fecha = new DateTime(2026, 5, 18), Procedencia = "Procede", Resolucion = "Explicación arqueólogo ext.", DiasAtencion = 3 },
                new CalidadReclamoItem { Id = 7, CodigoReserva = "BK-10106", Servicio = "Machu Picchu Full Day", Fecha = new DateTime(2026, 5, 20), Procedencia = "Procede", Resolucion = "Servicio reprogramado VIP", DiasAtencion = 4 },
                new CalidadReclamoItem { Id = 8, CodigoReserva = "BK-10107", Servicio = "Machu Picchu Full Day", Fecha = new DateTime(2026, 5, 22), Procedencia = "Procede", Resolucion = "Reembolso retraso tren", DiasAtencion = 10 },
                new CalidadReclamoItem { Id = 9, CodigoReserva = "BK-10108", Servicio = "Machu Picchu Full Day", Fecha = new DateTime(2026, 5, 23), Procedencia = "No procede", Resolucion = "Cliente canceló tarde", DiasAtencion = 1 },
                new CalidadReclamoItem { Id = 10, CodigoReserva = "BK-10109", Servicio = "Machu Picchu Full Day", Fecha = new DateTime(2026, 5, 25), Procedencia = "No procede", Resolucion = "Pérdida tren por pasajero", DiasAtencion = 3 },

                new CalidadReclamoItem { Id = 11, CodigoReserva = "BK-10260", Servicio = "Islas Ballestas", Fecha = new DateTime(2026, 5, 2), Procedencia = "Procede", Resolucion = "Reembolso emitido", DiasAtencion = 3 },
                new CalidadReclamoItem { Id = 12, CodigoReserva = "BK-10201", Servicio = "Islas Ballestas", Fecha = new DateTime(2026, 5, 3), Procedencia = "Procede", Resolucion = "Cambio de embarcación", DiasAtencion = 1 },
                new CalidadReclamoItem { Id = 13, CodigoReserva = "BK-10202", Servicio = "Islas Ballestas", Fecha = new DateTime(2026, 5, 5), Procedencia = "Procede", Resolucion = "Mal clima, reprogramado", DiasAtencion = 2 },
                new CalidadReclamoItem { Id = 14, CodigoReserva = "BK-10203", Servicio = "Islas Ballestas", Fecha = new DateTime(2026, 5, 7), Procedencia = "No procede", Resolucion = "Pasajero llegó tarde", DiasAtencion = 1 },
                new CalidadReclamoItem { Id = 15, CodigoReserva = "BK-10204", Servicio = "Islas Ballestas", Fecha = new DateTime(2026, 5, 8), Procedencia = "No procede", Resolucion = "Mareo fortuito del cliente", DiasAtencion = 4 },
                new CalidadReclamoItem { Id = 16, CodigoReserva = "BK-10205", Servicio = "Islas Ballestas", Fecha = new DateTime(2026, 5, 10), Procedencia = "Pendiente", Resolucion = "—", DiasAtencion = 0 },

                new CalidadReclamoItem { Id = 17, CodigoReserva = "BK-10293", Servicio = "City Tour Lima", Fecha = new DateTime(2026, 5, 11), Procedencia = "Pendiente", Resolucion = "—", DiasAtencion = 0 },
                new CalidadReclamoItem { Id = 18, CodigoReserva = "BK-10301", Servicio = "City Tour Lima", Fecha = new DateTime(2026, 5, 12), Procedencia = "Procede", Resolucion = "Tránsito intenso, retraso", DiasAtencion = 1 },
                new CalidadReclamoItem { Id = 19, CodigoReserva = "BK-10302", Servicio = "City Tour Lima", Fecha = new DateTime(2026, 5, 14), Procedencia = "Procede", Resolucion = "Falta audífonos interactivos", DiasAtencion = 1 },
                new CalidadReclamoItem { Id = 20, CodigoReserva = "BK-10303", Servicio = "City Tour Lima", Fecha = new DateTime(2026, 5, 15), Procedencia = "No procede", Resolucion = "Museo cerrado por feriado", DiasAtencion = 2 },
                new CalidadReclamoItem { Id = 21, CodigoReserva = "BK-10304", Servicio = "City Tour Lima", Fecha = new DateTime(2026, 5, 17), Procedencia = "No procede", Resolucion = "Pasajero no esperó al bus", DiasAtencion = 2 },

                new CalidadReclamoItem { Id = 22, CodigoReserva = "BK-10220", Servicio = "Valle Sagrado", Fecha = new DateTime(2026, 4, 20), Procedencia = "No procede", Resolucion = "Promo no aplicaba", DiasAtencion = 2 },
                new CalidadReclamoItem { Id = 23, CodigoReserva = "BK-10401", Servicio = "Valle Sagrado", Fecha = new DateTime(2026, 4, 22), Procedencia = "Procede", Resolucion = "Almuerzo buffet incompleto", DiasAtencion = 4 },
                new CalidadReclamoItem { Id = 24, CodigoReserva = "BK-10402", Servicio = "Valle Sagrado", Fecha = new DateTime(2026, 4, 25), Procedencia = "En atención", Resolucion = "—", DiasAtencion = 0 },

                new CalidadReclamoItem { Id = 25, CodigoReserva = "BK-10501", Servicio = "Islas Ballestas", Fecha = new DateTime(2026, 5, 4), Procedencia = "Procede", Resolucion = "Asiento defectuoso en bus", DiasAtencion = 2 },
                new CalidadReclamoItem { Id = 26, CodigoReserva = "BK-10502", Servicio = "Islas Ballestas", Fecha = new DateTime(2026, 5, 5), Procedencia = "No procede", Resolucion = "Retraso por control policial", DiasAtencion = 1 }
            };

            // Mapear dinámicamente las clases del badge según procedencia
            MasterReclamos.ForEach(rec => rec.ProcedenciaClase = rec.Procedencia switch
            {
                "Procede" => "badge-procede",
                "No procede" => "badge-noprocede",
                "En atención" => "badge-atencion",
                _ => "badge-pendiente"
            });
        }

        /// <summary>
        /// Filtra la base de datos de reservas y de reclamos según rango de fechas y servicio seleccionado,
        /// recalculando todas las métricas de calidad y la distribución del gráfico de barras en tiempo real.
        /// </summary>
        public void FiltrarDatos()
        {
            // 1. Filtrar Reservas
            var queryRes = MasterReservas.AsQueryable();
            if (FechaInicio.HasValue) queryRes = queryRes.Where(r => r.Fecha.Date >= FechaInicio.Value.Date);
            if (FechaFin.HasValue) queryRes = queryRes.Where(r => r.Fecha.Date <= FechaFin.Value.Date);
            if (ServicioSeleccionado != "Todos") queryRes = queryRes.Where(r => r.Servicio == ServicioSeleccionado);
            ReservasCount = queryRes.Count();

            // 2. Filtrar Reclamos
            var queryRec = MasterReclamos.AsQueryable();
            if (FechaInicio.HasValue) queryRec = queryRec.Where(r => r.Fecha.Date >= FechaInicio.Value.Date);
            if (FechaFin.HasValue) queryRec = queryRec.Where(r => r.Fecha.Date <= FechaFin.Value.Date);
            if (ServicioSeleccionado != "Todos") queryRec = queryRec.Where(r => r.Servicio == ServicioSeleccionado);
            ListadoFiltrado = queryRec.OrderByDescending(r => r.Fecha).ToList();
            ReclamosCount = ListadoFiltrado.Count;

            // 3. Recalcular KPIs de tasa y días promedio de atención
            IncidenciasRate = ReservasCount > 0 ? ((decimal)ReclamosCount / ReservasCount) * 100 : 0m;
            var resueltos = ListadoFiltrado.Where(r => r.Procedencia == "Procede" || r.Procedencia == "No procede").ToList();
            TiempoPromedioAtencion = resueltos.Any() ? resueltos.Average(r => r.DiasAtencion) : 0;

            // 4. Calcular volumen de procedencias para progress-bars
            ProcedeCount = ListadoFiltrado.Count(r => r.Procedencia == "Procede");
            NoProcedeCount = ListadoFiltrado.Count(r => r.Procedencia == "No procede");
            PendientesCount = ListadoFiltrado.Count(r => r.Procedencia == "Pendiente" || r.Procedencia == "En atención");

            ProcedePercent = ReclamosCount > 0 ? ((double)ProcedeCount / ReclamosCount) * 100 : 0;
            NoProcedePercent = ReclamosCount > 0 ? ((double)NoProcedeCount / ReclamosCount) * 100 : 0;
            PendientesPercent = ReclamosCount > 0 ? ((double)PendientesCount / ReclamosCount) * 100 : 0;

            // 5. Generar distribución vertical de reclamos por servicio para el gráfico
            string[] servicios = { "City Tour Lima", "Machu Picchu Full Day", "Islas Ballestas", "Valle Sagrado" };
            var listTemp = new List<ReclamoServicioItem>();
            double maxCount = 0;

            foreach (var s in servicios)
            {
                double cnt = ListadoFiltrado.Count(r => r.Servicio == s);
                if (cnt > maxCount) maxCount = cnt;
                listTemp.Add(new ReclamoServicioItem
                {
                    Servicio = s,
                    ServicioCorto = s == "Machu Picchu Full Day" ? "Machu P." : (s == "City Tour Lima" ? "City T." : s.Split(' ')[0]),
                    Cantidad = cnt
                });
            }

            // Normalizar escala de barras (0-100%)
            listTemp.ForEach(item => item.PorcentajeAltura = maxCount > 0 ? (item.Cantidad / maxCount) * 100 : 0);
            ReclamosPorServicio = listTemp;
            MaxReclamosCount = maxCount;
        }

        /// <summary>
        /// Invoca el subsistema de impresión nativo del navegador, el cual activa la hoja de estilos de impresión
        /// (@media print) del CSS local para descargar/guardar un PDF corporativo perfectamente estructurado.
        /// </summary>
        public async Task ExportarAPDF()
        {
            await JS.InvokeVoidAsync("window.print");
        }
    }

    public class CalidadReclamoItem
    {
        public int Id { get; set; }
        public string CodigoReserva { get; set; }
        public string Servicio { get; set; }
        public DateTime Fecha { get; set; }
        public string Procedencia { get; set; }
        public string Resolucion { get; set; }
        public double DiasAtencion { get; set; }
        public string ProcedenciaClase { get; set; }
    }

    public class CalidadReservaItem
    {
        public string Codigo { get; set; }
        public string Servicio { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class ReclamoServicioItem
    {
        public string Servicio { get; set; }
        public string ServicioCorto { get; set; }
        public double Cantidad { get; set; }
        public double PorcentajeAltura { get; set; }
    }
}

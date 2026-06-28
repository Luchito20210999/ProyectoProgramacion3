using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ProyectoProgramacion3Web.Servicios.Reclamos;
using ProyectoProgramacion3Web.Servicios.Reservas;

namespace ProyectoProgramacion3Web.Components.Pages.Calidad
{
    public partial class Calidad : ComponentBase
    {
        [Inject] private IJSRuntime JS { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IReservasServiceClient ReservasServiceClient { get; set; } = default!;
        [Inject] private IReclamosServiceClient ReclamosServiceClient { get; set; } = default!;

        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string ServicioSeleccionado { get; set; } = "Todos";
        public int ReservasCount { get; set; }
        public int ReclamosCount { get; set; }
        public decimal IncidenciasRate { get; set; }
        public double TiempoPromedioAtencion { get; set; }
        public double MaxReclamosCount { get; set; }
        public int ProcedeCount { get; set; }
        public double ProcedePercent { get; set; }
        public int NoProcedeCount { get; set; }
        public double NoProcedePercent { get; set; }
        public int PendientesCount { get; set; }
        public double PendientesPercent { get; set; }
        public List<CalidadReservaItem> MasterReservas { get; set; } = new();
        public List<CalidadReclamoItem> MasterReclamos { get; set; } = new();
        public List<CalidadReclamoItem> ListadoFiltrado { get; set; } = new();
        public List<ReclamoServicioItem> ReclamosPorServicio { get; set; } = new();

        protected override void OnInitialized()
        {
            CargarDatosIniciales();
            FiltrarDatos();
        }

        private void CargarDatosIniciales()
        {
            MasterReservas = ReservasServiceClient.Listar().Select(r => new CalidadReservaItem
            {
                Codigo = r.Codigo,
                Servicio = string.IsNullOrWhiteSpace(r.Servicio) ? "Sin servicio" : r.Servicio,
                Fecha = r.FechaServicio
            }).ToList();

            MasterReclamos = ReclamosServiceClient.Listar().Select(r =>
            {
                string procedencia = MapearProcedencia(r.Estado);
                string servicio = BuscarServicioReserva(r.CodigoReserva);
                return new CalidadReclamoItem
                {
                    Id = r.Id,
                    CodigoReserva = r.CodigoReserva,
                    Servicio = servicio,
                    Fecha = r.Fecha,
                    Procedencia = procedencia,
                    Resolucion = r.MotivoResolucion,
                    DiasAtencion = CalcularDiasAtencion(r.Fecha, r.FechaResolucion),
                    ProcedenciaClase = ProcedenciaClase(procedencia)
                };
            }).ToList();
        }

        public void FiltrarDatos()
        {
            var queryRes = MasterReservas.AsQueryable();
            if (FechaInicio.HasValue) queryRes = queryRes.Where(r => r.Fecha.Date >= FechaInicio.Value.Date);
            if (FechaFin.HasValue) queryRes = queryRes.Where(r => r.Fecha.Date <= FechaFin.Value.Date);
            if (ServicioSeleccionado != "Todos") queryRes = queryRes.Where(r => r.Servicio == ServicioSeleccionado);
            ReservasCount = queryRes.Count();

            var queryRec = MasterReclamos.AsQueryable();
            if (FechaInicio.HasValue) queryRec = queryRec.Where(r => r.Fecha.Date >= FechaInicio.Value.Date);
            if (FechaFin.HasValue) queryRec = queryRec.Where(r => r.Fecha.Date <= FechaFin.Value.Date);
            if (ServicioSeleccionado != "Todos") queryRec = queryRec.Where(r => r.Servicio == ServicioSeleccionado);
            ListadoFiltrado = queryRec.OrderByDescending(r => r.Fecha).ToList();
            ReclamosCount = ListadoFiltrado.Count;

            IncidenciasRate = ReservasCount > 0 ? ((decimal)ReclamosCount / ReservasCount) * 100 : 0m;
            var resueltos = ListadoFiltrado.Where(r => r.Procedencia == "Procede" || r.Procedencia == "No procede").ToList();
            TiempoPromedioAtencion = resueltos.Any() ? resueltos.Average(r => r.DiasAtencion) : 0;

            ProcedeCount = ListadoFiltrado.Count(r => r.Procedencia == "Procede");
            NoProcedeCount = ListadoFiltrado.Count(r => r.Procedencia == "No procede");
            PendientesCount = ListadoFiltrado.Count(r => r.Procedencia == "Pendiente" || r.Procedencia == "En atencion");
            ProcedePercent = ReclamosCount > 0 ? ((double)ProcedeCount / ReclamosCount) * 100 : 0;
            NoProcedePercent = ReclamosCount > 0 ? ((double)NoProcedeCount / ReclamosCount) * 100 : 0;
            PendientesPercent = ReclamosCount > 0 ? ((double)PendientesCount / ReclamosCount) * 100 : 0;

            var agrupado = ListadoFiltrado.GroupBy(r => r.Servicio).Select(g => new ReclamoServicioItem
            {
                Servicio = g.Key,
                ServicioCorto = g.Key.Length > 10 ? g.Key[..10] : g.Key,
                Cantidad = g.Count()
            }).ToList();

            MaxReclamosCount = agrupado.Any() ? agrupado.Max(r => r.Cantidad) : 0;
            agrupado.ForEach(item => item.PorcentajeAltura = MaxReclamosCount > 0 ? (item.Cantidad / MaxReclamosCount) * 100 : 0);
            ReclamosPorServicio = agrupado;
        }

        public async Task ExportarACsv()
        {
            var csv = new System.Text.StringBuilder("\uFEFF");
            csv.AppendLine("REPORTE DE CALIDAD OPERATIVA - SACR");
            csv.AppendLine($"Fecha de Generacion:;{DateTime.Now:dd/MM/yyyy HH:mm}");
            csv.AppendLine($"Filtros:;Servicio: {ServicioSeleccionado};Desde: {(FechaInicio.HasValue ? FechaInicio.Value.ToString("dd/MM/yyyy") : "Inicio")};Hasta: {(FechaFin.HasValue ? FechaFin.Value.ToString("dd/MM/yyyy") : "Fin")}");
            csv.AppendLine();
            csv.AppendLine($"RESUMEN:;Total Reservas:;{ReservasCount};Total Reclamos:;{ReclamosCount};Porcentaje Incidencias:;{IncidenciasRate:F1}%;Tiempo Promedio Atencion:;{TiempoPromedioAtencion:F1} dias");
            csv.AppendLine($"PROCEDENCIA:;Procede:;{ProcedeCount};No Procede:;{NoProcedeCount};Pendientes/En Atencion:;{PendientesCount}");
            csv.AppendLine();
            csv.AppendLine("DETALLE DE RECLAMOS");
            csv.AppendLine("Nro;Codigo Reserva;Servicio;Fecha;Procedencia;Resolucion");

            for (int i = 0; i < ListadoFiltrado.Count; i++)
            {
                var reclamo = ListadoFiltrado[i];
                csv.AppendLine(string.Join(";",
                    i + 1,
                    Csv(reclamo.CodigoReserva),
                    Csv(reclamo.Servicio),
                    reclamo.Fecha.ToString("dd/MM/yyyy"),
                    Csv(reclamo.Procedencia),
                    Csv(reclamo.Resolucion)));
            }

            var base64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(csv.ToString()));
            await JS.InvokeVoidAsync("downloadFileFromBase64", $"Reporte_Calidad_{DateTime.Now:yyyyMMdd_HHmmss}.csv", base64, "text/csv;charset=utf-8");
        }

        private string BuscarServicioReserva(string codigoReserva)
        {
            if (string.IsNullOrWhiteSpace(codigoReserva)) return "Sin servicio";
            string digits = new(codigoReserva.Where(char.IsDigit).ToArray());
            return MasterReservas.FirstOrDefault(r => r.Codigo.Contains(digits, StringComparison.OrdinalIgnoreCase))?.Servicio ?? "Sin servicio";
        }

        private static string MapearProcedencia(string? estado)
        {
            return estado switch
            {
                "PROCEDE" => "Procede",
                "NO_PROCEDE" => "No procede",
                "EN_ATENCION" => "En atencion",
                _ => "Pendiente"
            };
        }

        private static string ProcedenciaClase(string procedencia)
        {
            return procedencia switch
            {
                "Procede" => "badge-procede",
                "No procede" => "badge-noprocede",
                "En atencion" => "badge-atencion",
                _ => "badge-pendiente"
            };
        }

        private static double CalcularDiasAtencion(DateTime? inicio, DateTime? fin)
        {
            if (!inicio.HasValue || !fin.HasValue) return 0;
            return Math.Max(0, (fin.Value.Date - inicio.Value.Date).TotalDays);
        }

        private static string Csv(string? value)
        {
            value ??= string.Empty;
            return value.Contains(';') || value.Contains('"') || value.Contains('\n')
                ? $"\"{value.Replace("\"", "\"\"")}\""
                : value;
        }

    }

    public class CalidadReclamoItem
    {
        public int Id { get; set; }
        public string CodigoReserva { get; set; } = string.Empty;
        public string Servicio { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Procedencia { get; set; } = string.Empty;
        public string Resolucion { get; set; } = string.Empty;
        public double DiasAtencion { get; set; }
        public string ProcedenciaClase { get; set; } = string.Empty;
    }

    public class CalidadReservaItem
    {
        public string Codigo { get; set; } = string.Empty;
        public string Servicio { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
    }

    public class ReclamoServicioItem
    {
        public string Servicio { get; set; } = string.Empty;
        public string ServicioCorto { get; set; } = string.Empty;
        public double Cantidad { get; set; }
        public double PorcentajeAltura { get; set; }
    }
}

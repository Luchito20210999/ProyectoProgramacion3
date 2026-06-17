using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace ProyectoProgramacion3Web.Components.Pages.Reservas
{
    public partial class Reservas : ComponentBase
    {
        // Campos de enlazamiento de filtros
        public string SearchText { get; set; } = string.Empty;
        public string SelectedEstado { get; set; } = "TODOS";
        public DateTime? SelectedFecha { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "search")]
        public string SearchTextQuery { get; set; }

        // Colecciones de Datos
        public List<ReservaItem> ListadoReservas { get; set; } = new List<ReservaItem>();
        public List<ReservaItem> ListadoFiltrado { get; set; } = new List<ReservaItem>();

        // Métricas KPIs dinámicas
        public int TotalCount { get; set; }
        public int ConfirmadasCount { get; set; }
        public int PendientesCount { get; set; }
        public int AnuladasCount { get; set; }

        // Estado del Modal CRUD
        public bool IsModalOpen { get; set; }
        public string ModalMode { get; set; } = "Ver"; // "Ver", "Modificar", "Nuevo"
        public string ModalTitle { get; set; } = "Detalle de reserva";
        public ReservaItem FormReserva { get; set; } = new ReservaItem();

        // Control visual del formulario en modal
        public bool IsFormDisabled => ModalMode == "Ver";
        public bool ShowGuardarBtn => ModalMode != "Ver";
        public bool IsCodigoReadOnly => ModalMode != "Nuevo";

        protected override void OnInitialized()
        {
            CargarDatosIniciales();
            FiltrarListado();
        }

        protected override void OnParametersSet()
        {
            if (!string.IsNullOrEmpty(SearchTextQuery))
            {
                SearchText = SearchTextQuery;
                FiltrarListado();
            }
        }

        private void CargarDatosIniciales()
        {
            ListadoReservas = new List<ReservaItem>
            {
                new ReservaItem { Codigo = "BK-84920", Cliente = "Laura Benavides", Servicio = "Camino Inca 4 Días", FechaServicio = new DateTime(2026, 05, 30), Pax = 2, Monto = 580.00m, Estado = "Confirmada", EstadoClase = "badge-confirmada" },
                new ReservaItem { Codigo = "BK-10492", Cliente = "Carlos Mendoza", Servicio = "City Tour Cusco Premium", FechaServicio = new DateTime(2026, 05, 30), Pax = 1, Monto = 120.00m, Estado = "Confirmada", EstadoClase = "badge-confirmada" },
                new ReservaItem { Codigo = "BK-39502", Cliente = "Sophie Dubois", Servicio = "Machu Picchu Mágico Full Day", FechaServicio = new DateTime(2026, 05, 29), Pax = 3, Monto = 340.00m, Estado = "Pendiente", EstadoClase = "badge-pendiente" },
                new ReservaItem { Codigo = "BK-92049", Cliente = "James Wilson", Servicio = "Líneas de Nazca Sobrevuelo", FechaServicio = new DateTime(2026, 05, 29), Pax = 2, Monto = 210.00m, Estado = "Confirmada", EstadoClase = "badge-confirmada" },
                new ReservaItem { Codigo = "BK-48201", Cliente = "Mariana Costa", Servicio = "Valle Sagrado VIP + Almuerzo", FechaServicio = new DateTime(2026, 05, 28), Pax = 4, Monto = 160.00m, Estado = "Anulada", EstadoClase = "badge-anulada" },
                new ReservaItem { Codigo = "BK-55291", Cliente = "Kenji Sato", Servicio = "Lago Titicaca y Uros Full Day", FechaServicio = new DateTime(2026, 06, 02), Pax = 2, Monto = 90.00m, Estado = "Pendiente", EstadoClase = "badge-pendiente" }
            };
        }

        // Ejecuta el filtrado en base a los inputs del usuario y recalcula los KPIs en tiempo real
        public void FiltrarListado()
        {
            var query = ListadoReservas.AsQueryable();

            // Filtro por texto (Código, Cliente o Servicio)
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string lowerSearch = SearchText.ToLower();
                query = query.Where(r => 
                    r.Codigo.ToLower().Contains(lowerSearch) || 
                    r.Cliente.ToLower().Contains(lowerSearch) || 
                    r.Servicio.ToLower().Contains(lowerSearch)
                );
            }

            // Filtro por Estado
            if (SelectedEstado != "TODOS")
            {
                query = query.Where(r => r.Estado.ToUpper() == SelectedEstado);
            }

            // Filtro por Fecha de Servicio exacta
            if (SelectedFecha.HasValue)
            {
                query = query.Where(r => r.FechaServicio.Date == SelectedFecha.Value.Date);
            }

            ListadoFiltrado = query.ToList();

            // Recalcular KPIs sobre la lista filtrada actualmente visible
            TotalCount = ListadoFiltrado.Count;
            ConfirmadasCount = ListadoFiltrado.Count(r => r.Estado == "Confirmada");
            PendientesCount = ListadoFiltrado.Count(r => r.Estado == "Pendiente");
            AnuladasCount = ListadoFiltrado.Count(r => r.Estado == "Anulada");
        }

        public void LimpiarFiltros()
        {
            SearchText = string.Empty;
            SelectedEstado = "TODOS";
            SelectedFecha = null;
            FiltrarListado();
        }

        // Simula la sincronización con Bokun sin agregar filas nuevas
        public void SincronizarBokun()
        {
            // Simular consulta a la API de Bokun (no se detectan nuevos registros)
            Console.WriteLine("Sincronización con Bokun completada con éxito. Todos los registros se encuentran al día.");
            FiltrarListado();
        }

        // Anula la reserva cambiando su estado a Anulada
        public void AnularReserva(ReservaItem reserva)
        {
            if (reserva == null) return;
            
            reserva.Estado = "Anulada";
            reserva.EstadoClase = "badge-anulada";
            
            FiltrarListado();
        }

        // Control del Modal CRUD
        public void AbrirModal(string modo, ReservaItem reserva = null)
        {
            ModalMode = modo;
            
            if (modo == "Nuevo")
            {
                ModalTitle = "Nueva reserva";
                FormReserva = new ReservaItem
                {
                    Codigo = $"BK-{new Random().Next(10000, 99999)}",
                    FechaServicio = DateTime.Today,
                    Pax = 1,
                    Monto = 0.00m,
                    Estado = "Confirmada",
                    EstadoClase = "badge-confirmada"
                };
            }
            else
            {
                if (reserva == null) return;
                
                ModalTitle = modo == "Ver" ? "Detalle de reserva" : "Modificar reserva";
                
                // Clonar objeto para evitar edición directa sin guardar
                FormReserva = new ReservaItem
                {
                    Codigo = reserva.Codigo,
                    Cliente = reserva.Cliente,
                    Servicio = reserva.Servicio,
                    FechaServicio = reserva.FechaServicio,
                    Pax = reserva.Pax,
                    Monto = reserva.Monto,
                    Estado = reserva.Estado,
                    EstadoClase = reserva.EstadoClase
                };
            }

            IsModalOpen = true;
        }

        public void CerrarModal()
        {
            IsModalOpen = false;
            FormReserva = new ReservaItem();
        }

        // Guarda o actualiza la reserva y actualiza la tabla y KPIs
        public void GuardarReserva()
        {
            // Validación mínima
            if (string.IsNullOrWhiteSpace(FormReserva.Cliente) || string.IsNullOrWhiteSpace(FormReserva.Servicio))
            {
                return;
            }

            // Asignar clase de estado correcta
            FormReserva.EstadoClase = FormReserva.Estado switch
            {
                "Confirmada" => "badge-confirmada",
                "Pendiente" => "badge-pendiente",
                "Anulada" => "badge-anulada",
                _ => "badge-confirmada"
            };

            if (ModalMode == "Nuevo")
            {
                // Insertar nueva reserva al inicio
                ListadoReservas.Insert(0, FormReserva);
            }
            else if (ModalMode == "Modificar")
            {
                // Actualizar reserva existente
                var existente = ListadoReservas.FirstOrDefault(r => r.Codigo == FormReserva.Codigo);
                if (existente != null)
                {
                    existente.Cliente = FormReserva.Cliente;
                    existente.Servicio = FormReserva.Servicio;
                    existente.FechaServicio = FormReserva.FechaServicio;
                    existente.Pax = FormReserva.Pax;
                    existente.Monto = FormReserva.Monto;
                    existente.Estado = FormReserva.Estado;
                    existente.EstadoClase = FormReserva.EstadoClase;
                }
            }

            FiltrarListado();
            CerrarModal();
        }
    }

    public class ReservaItem
    {
        public string Codigo { get; set; }
        public string Cliente { get; set; }
        public string Servicio { get; set; }
        public DateTime FechaServicio { get; set; }
        public int Pax { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; }
        public string EstadoClase { get; set; }
    }
}

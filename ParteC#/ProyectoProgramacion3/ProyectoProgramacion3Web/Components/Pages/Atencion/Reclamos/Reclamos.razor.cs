using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace ProyectoProgramacion3Web.Components.Pages.Reclamos
{
    public partial class Reclamos : ComponentBase
    {
        // Campos de enlazamiento de filtros
        public string SearchText { get; set; } = string.Empty;
        public string SelectedEstado { get; set; } = "Todos";

        // Colecciones de Datos
        public List<ReclamoItem> ListadoReclamos { get; set; } = new List<ReclamoItem>();
        public List<ReclamoItem> ListadoFiltrado { get; set; } = new List<ReclamoItem>();

        // KPIs Operativos Dinámicos
        public int PendientesCount { get; set; }
        public int EnAtencionCount { get; set; }
        public int ProcedeCount { get; set; }
        public int NoProcedeCount { get; set; }

        // Estado del Modal CRUD
        public bool IsModalOpen { get; set; }
        public string ModalMode { get; set; } = "Ver"; // "Registrar", "Atender", "Ver"
        public ReclamoItem FormReclamo { get; set; } = new ReclamoItem();

        // Helpers de control del modal
        public bool IsFormDisabled => ModalMode == "Ver";
        public bool IsResolucionDisabled => ModalMode != "Atender";
        public bool ShowGuardarBtn => ModalMode != "Ver";
        public bool IsCreacionEditable => ModalMode == "Registrar";

        public string ModalTitle => ModalMode switch
        {
            "Registrar" => "Registrar reclamo",
            "Atender" => "Atender reclamo",
            _ => "Detalle de reclamo"
        };

        [Parameter]
        [SupplyParameterFromQuery(Name = "search")]
        public string SearchTextQuery { get; set; }

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
            ListadoReclamos = new List<ReclamoItem>
            {
                new ReclamoItem
                {
                    Id = 1,
                    CodigoReserva = "BK-10293",
                    Cliente = "Lucía Fernández",
                    Fecha = new DateTime(2026, 05, 09),
                    Descripcion = "El guía no llegó a la hora pactada.",
                    Estado = "PENDIENTE",
                    EstadoClase = "badge-pendiente"
                },
                new ReclamoItem
                {
                    Id = 2,
                    CodigoReserva = "BK-10288",
                    Cliente = "James Carter",
                    Fecha = new DateTime(2026, 05, 15),
                    Descripcion = "Servicio sin almuerzo incluido.",
                    Estado = "EN_ATENCION",
                    EstadoClase = "badge-atencion"
                },
                new ReclamoItem
                {
                    Id = 3,
                    CodigoReserva = "BK-10260",
                    Cliente = "Marta Ríos",
                    Fecha = new DateTime(2026, 05, 20),
                    Descripcion = "Cobro duplicado en tarjeta.",
                    Estado = "PROCEDE",
                    EstadoClase = "badge-procede",
                    FechaResolucion = new DateTime(2026, 05, 22),
                    MotivoResolucion = "Se realizó el reembolso respectivo por el cobro duplicado de la pasarela de pagos."
                },
                new ReclamoItem
                {
                    Id = 4,
                    CodigoReserva = "BK-10220",
                    Cliente = "Pedro Núñez",
                    Fecha = new DateTime(2026, 05, 25),
                    Descripcion = "No se brindó descuento promocional.",
                    Estado = "NO_PROCEDE",
                    EstadoClase = "badge-noprocede",
                    FechaResolucion = new DateTime(2026, 05, 26),
                    MotivoResolucion = "El descuento promocional ya había expirado al momento de realizar la reserva según términos y condiciones."
                }
            };
        }

        // Filtra y recalcula los KPIs en tiempo real basados en la lista actualmente visible
        public void FiltrarListado()
        {
            var query = ListadoReclamos.AsQueryable();

            // Filtrado por texto (Cliente, Reserva o Descripción)
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string lowerSearch = SearchText.ToLower();
                query = query.Where(r =>
                    r.CodigoReserva.ToLower().Contains(lowerSearch) ||
                    r.Cliente.ToLower().Contains(lowerSearch) ||
                    r.Descripcion.ToLower().Contains(lowerSearch)
                );
            }

            // Filtrado por Estado
            if (SelectedEstado != "Todos")
            {
                query = query.Where(r => r.Estado == SelectedEstado);
            }

            ListadoFiltrado = query.ToList();

            // Recalcular KPIs sobre el resultado visible
            PendientesCount = ListadoFiltrado.Count(r => r.Estado == "PENDIENTE");
            EnAtencionCount = ListadoFiltrado.Count(r => r.Estado == "EN_ATENCION");
            ProcedeCount = ListadoFiltrado.Count(r => r.Estado == "PROCEDE");
            NoProcedeCount = ListadoFiltrado.Count(r => r.Estado == "NO_PROCEDE");
        }

        public void LimpiarFiltros()
        {
            SearchText = string.Empty;
            SelectedEstado = "Todos";
            FiltrarListado();
        }

        // Control del Modal CRUD
        public void AbrirModal(string modo, ReclamoItem reclamo = null)
        {
            ModalMode = modo;

            if (modo == "Registrar")
            {
                FormReclamo = new ReclamoItem
                {
                    Fecha = DateTime.Today,
                    Estado = "PENDIENTE",
                    EstadoClase = "badge-pendiente"
                };
            }
            else
            {
                if (reclamo == null) return;

                // Clonar objeto para evitar edición en tiempo real de la tabla antes de guardar
                FormReclamo = new ReclamoItem
                {
                    Id = reclamo.Id,
                    CodigoReserva = reclamo.CodigoReserva,
                    Cliente = reclamo.Cliente,
                    Fecha = reclamo.Fecha,
                    Descripcion = reclamo.Descripcion,
                    Estado = reclamo.Estado,
                    EstadoClase = reclamo.EstadoClase,
                    FechaResolucion = reclamo.FechaResolucion ?? DateTime.Today,
                    MotivoResolucion = reclamo.MotivoResolucion
                };

                // Al pulsar Atender en un reclamo PENDIENTE, pasa a EN_ATENCION automáticamente
                if (modo == "Atender" && FormReclamo.Estado == "PENDIENTE")
                {
                    FormReclamo.Estado = "EN_ATENCION";
                    FormReclamo.EstadoClase = "badge-atencion";
                }
            }

            IsModalOpen = true;
        }

        public void CerrarModal()
        {
            IsModalOpen = false;
            FormReclamo = new ReclamoItem();
        }

        // Guarda o actualiza los datos del reclamo
        public void GuardarReclamo()
        {
            // Validaciones obligatorias de campos principales
            if (string.IsNullOrWhiteSpace(FormReclamo.CodigoReserva) ||
                string.IsNullOrWhiteSpace(FormReclamo.Cliente) ||
                string.IsNullOrWhiteSpace(FormReclamo.Descripcion))
            {
                return;
            }

            // Si está resolviendo, requiere un motivo explicativo
            if (ModalMode == "Atender" &&
                (FormReclamo.Estado == "PROCEDE" || FormReclamo.Estado == "NO_PROCEDE") &&
                string.IsNullOrWhiteSpace(FormReclamo.MotivoResolucion))
            {
                return;
            }

            // Asignar clase de estado correcta
            FormReclamo.EstadoClase = FormReclamo.Estado switch
            {
                "PENDIENTE" => "badge-pendiente",
                "EN_ATENCION" => "badge-atencion",
                "PROCEDE" => "badge-procede",
                "NO_PROCEDE" => "badge-noprocede",
                _ => "badge-pendiente"
            };

            if (ModalMode == "Registrar")
            {
                int nuevoId = ListadoReclamos.Any() ? ListadoReclamos.Max(r => r.Id) + 1 : 1;
                FormReclamo.Id = nuevoId;
                FormReclamo.Fecha = DateTime.Today;

                ListadoReclamos.Add(FormReclamo);
            }
            else if (ModalMode == "Atender")
            {
                var existente = ListadoReclamos.FirstOrDefault(r => r.Id == FormReclamo.Id);
                if (existente != null)
                {
                    existente.Estado = FormReclamo.Estado;
                    existente.EstadoClase = FormReclamo.EstadoClase;
                    existente.FechaResolucion = (FormReclamo.Estado == "PROCEDE" || FormReclamo.Estado == "NO_PROCEDE") ? FormReclamo.FechaResolucion : null;
                    existente.MotivoResolucion = (FormReclamo.Estado == "PROCEDE" || FormReclamo.Estado == "NO_PROCEDE") ? FormReclamo.MotivoResolucion : string.Empty;
                }
            }

            FiltrarListado();
            CerrarModal();
        }
    }

    public class ReclamoItem
    {
        public int Id { get; set; }
        public string CodigoReserva { get; set; }
        public string Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; } // "PENDIENTE", "EN_ATENCION", "PROCEDE", "NO_PROCEDE"
        public string EstadoClase { get; set; }
        public DateTime? FechaResolucion { get; set; }
        public string MotivoResolucion { get; set; } = string.Empty;
    }
}

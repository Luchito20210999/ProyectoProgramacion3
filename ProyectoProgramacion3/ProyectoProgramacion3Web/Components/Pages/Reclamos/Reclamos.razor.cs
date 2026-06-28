using Microsoft.AspNetCore.Components;
using ProyectoProgramacion3Web.Services;
using ProyectoProgramacion3Web.Servicios.Reclamos;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Components.Pages.Reclamos
{
    public partial class Reclamos : ComponentBase
    {
        [Inject]
        private IReclamosServiceClient ReclamosServiceClient { get; set; } = default!;

        [Inject]
        private SessionService Session { get; set; } = default!;

        public string SearchText { get; set; } = string.Empty;
        public string SelectedEstado { get; set; } = "Todos";
        public List<ReclamoItem> ListadoReclamos { get; set; } = new();
        public List<ReclamoItem> ListadoFiltrado { get; set; } = new();
        public int PendientesCount { get; set; }
        public int EnAtencionCount { get; set; }
        public int ProcedeCount { get; set; }
        public int NoProcedeCount { get; set; }
        public bool IsModalOpen { get; set; }
        public string ModalMode { get; set; } = "Ver";
        public ReclamoItem FormReclamo { get; set; } = new();
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
        public string? SearchTextQuery { get; set; }

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
            ListadoReclamos = ReclamosServiceClient.Listar();
        }

        public void FiltrarListado()
        {
            var query = ListadoReclamos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string lowerSearch = SearchText.ToLowerInvariant();
                query = query.Where(r =>
                    r.CodigoReserva.ToLowerInvariant().Contains(lowerSearch) ||
                    r.Cliente.ToLowerInvariant().Contains(lowerSearch) ||
                    r.Descripcion.ToLowerInvariant().Contains(lowerSearch));
            }

            if (SelectedEstado != "Todos")
            {
                query = query.Where(r => r.Estado == SelectedEstado);
            }

            ListadoFiltrado = query.ToList();
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

        public void AbrirModal(string modo, ReclamoItem? reclamo = null)
        {
            ModalMode = modo;

            if (modo == "Registrar")
            {
                FormReclamo = new ReclamoItem
                {
                    IdUsuario = Session.UserId,
                    Fecha = DateTime.Today,
                    Estado = "PENDIENTE",
                    EstadoClase = "badge-pendiente"
                };
            }
            else
            {
                if (reclamo == null) return;
                FormReclamo = reclamo.Clone();

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

        public void GuardarReclamo()
        {
            if (string.IsNullOrWhiteSpace(FormReclamo.CodigoReserva) ||
                string.IsNullOrWhiteSpace(FormReclamo.Descripcion))
            {
                return;
            }

            if (ModalMode == "Atender" &&
                (FormReclamo.Estado == "PROCEDE" || FormReclamo.Estado == "NO_PROCEDE") &&
                string.IsNullOrWhiteSpace(FormReclamo.MotivoResolucion))
            {
                return;
            }

            FormReclamo.EstadoClase = EstadoClase(FormReclamo.Estado);
            if (FormReclamo.IdUsuario == 0)
            {
                FormReclamo.IdUsuario = Session.UserId;
            }

            ReclamosServiceClient.Guardar(FormReclamo, ModalMode == "Registrar" ? Estado.Nuevo : Estado.Modificado);
            CargarDatosIniciales();
            FiltrarListado();
            CerrarModal();
        }

        private static string EstadoClase(string estado)
        {
            return estado switch
            {
                "PENDIENTE" => "badge-pendiente",
                "EN_ATENCION" => "badge-atencion",
                "PROCEDE" => "badge-procede",
                "NO_PROCEDE" => "badge-noprocede",
                _ => "badge-pendiente"
            };
        }

    }
}

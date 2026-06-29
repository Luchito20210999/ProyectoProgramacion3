using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using ProyectoProgramacion3Web.Servicios.Reservas;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Components.Pages.Reservas
{
    public partial class Reservas : ComponentBase
    {
        [Inject]
        private IReservasServiceClient ReservasServiceClient { get; set; } = default!;

        public string SearchText { get; set; } = string.Empty;
        public string SelectedEstado { get; set; } = "TODOS";
        public DateTime? SelectedFecha { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "search")]
        public string? SearchTextQuery { get; set; }

        public List<ReservaItem> ListadoReservas { get; set; } = new();
        public List<ReservaItem> ListadoFiltrado { get; set; } = new();

        public int TotalCount { get; set; }
        public int ConfirmadasCount { get; set; }
        public int PendientesCount { get; set; }
        public int AnuladasCount { get; set; }
        public string? MensajeCarga { get; set; }

        public bool IsModalOpen { get; set; }
        public string ModalMode { get; set; } = "Ver";
        public string ModalTitle { get; set; } = "Detalle de reserva";
        public ReservaItem FormReserva { get; set; } = new();

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
            try
            {
                ListadoReservas = ReservasServiceClient.Listar();
                MensajeCarga = null;
            }
            catch (Exception ex)
            {
                MensajeCarga = $"No se pudieron cargar reservas desde el servicio REST Java: {ex.Message}";
                ListadoReservas = new List<ReservaItem>();
            }
        }

        public void FiltrarListado()
        {
            var query = ListadoReservas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string lowerSearch = SearchText.ToLower();
                query = query.Where(r =>
                    r.Codigo.ToLower().Contains(lowerSearch) ||
                    r.Cliente.ToLower().Contains(lowerSearch) ||
                    r.Servicio.ToLower().Contains(lowerSearch));
            }

            if (SelectedEstado != "TODOS")
            {
                query = query.Where(r => r.Estado.ToUpper() == SelectedEstado);
            }

            if (SelectedFecha.HasValue)
            {
                query = query.Where(r => r.FechaServicio.Date == SelectedFecha.Value.Date);
            }

            ListadoFiltrado = query.ToList();

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

        public void AbrirModal(string modo, ReservaItem? reserva = null)
        {
            ModalMode = modo;

            if (reserva == null)
            {
                return;
            }

            ModalTitle = "Detalle de reserva";
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

            IsModalOpen = true;
        }

        public void CerrarModal()
        {
            IsModalOpen = false;
            FormReserva = new ReservaItem();
        }

        private static string EstadoClase(string estado)
        {
            return estado switch
            {
                "Confirmada" => "badge-confirmada",
                "Pendiente" => "badge-pendiente",
                "Anulada" => "badge-anulada",
                _ => "badge-pendiente"
            };
        }
    }
}

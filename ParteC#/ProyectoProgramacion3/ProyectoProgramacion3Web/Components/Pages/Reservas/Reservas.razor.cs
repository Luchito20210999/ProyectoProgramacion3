using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using ProyectoProgramacion3Model.Model.reservas;
using ProyectoProgramacion3Negocio.BO.reservas;

namespace ProyectoProgramacion3Web.Components.Pages.Reservas
{
    public partial class Reservas : ComponentBase
    {
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
            try
            {
                var reservaBO = new ReservaBOImpl();
                ListadoReservas = reservaBO.Listar()
                    .Select(MapearReserva)
                    .ToList();
                MensajeCarga = null;
            }
            catch (Exception ex)
            {
                MensajeCarga = $"No se pudieron cargar reservas desde la base de datos: {ex.Message}";
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

        public void SincronizarBokun()
        {
            CargarDatosIniciales();
            FiltrarListado();
        }

        public void AnularReserva(ReservaItem reserva)
        {
            if (reserva == null)
            {
                return;
            }

            reserva.Estado = "Anulada";
            reserva.EstadoClase = "badge-anulada";
            FiltrarListado();
        }

        public void AbrirModal(string modo, ReservaItem? reserva = null)
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
                if (reserva == null)
                {
                    return;
                }

                ModalTitle = modo == "Ver" ? "Detalle de reserva" : "Modificar reserva";
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

        public void GuardarReserva()
        {
            if (string.IsNullOrWhiteSpace(FormReserva.Cliente) ||
                string.IsNullOrWhiteSpace(FormReserva.Servicio))
            {
                return;
            }

            FormReserva.EstadoClase = EstadoClase(FormReserva.Estado);

            if (ModalMode == "Nuevo")
            {
                ListadoReservas.Insert(0, FormReserva);
            }
            else if (ModalMode == "Modificar")
            {
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

        private static ReservaItem MapearReserva(Reserva reserva)
        {
            var estado = reserva.estadoReserva switch
            {
                EstadoReserva.APROBADO => "Confirmada",
                EstadoReserva.PENDIENTE => "Pendiente",
                EstadoReserva.RECHAZADO => "Anulada",
                EstadoReserva.OBSERVADO => "Pendiente",
                _ => "Pendiente"
            };

            return new ReservaItem
            {
                Codigo = string.IsNullOrWhiteSpace(reserva.codigoBokun)
                    ? $"RES-{reserva.idReserva}"
                    : reserva.codigoBokun,
                Cliente = $"Cliente #{reserva.idCliente}",
                Servicio = string.IsNullOrWhiteSpace(reserva.canalVenta)
                    ? "Reserva Bokun"
                    : reserva.canalVenta,
                FechaServicio = reserva.fechaRegistro,
                Pax = reserva.cantidadBoletos,
                Monto = Convert.ToDecimal(reserva.montoTotal),
                Estado = estado,
                EstadoClase = EstadoClase(estado)
            };
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

    public class ReservaItem
    {
        public string Codigo { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public string Servicio { get; set; } = string.Empty;
        public DateTime FechaServicio { get; set; }
        public int Pax { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string EstadoClase { get; set; } = string.Empty;
    }
}

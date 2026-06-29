using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using ProyectoProgramacion3Web.Services;
using ProyectoProgramacion3Web.Servicios.Base;
using ProyectoProgramacion3Web.Servicios.Reservas;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Components.Pages.Reservas
{
    public partial class Reservas : ComponentBase
    {
        [Inject]
        private IReservasServiceClient ReservasServiceClient { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private AuditoriaFrontService Auditoria { get; set; } = default!;

        public string SearchText { get; set; } = string.Empty;
        public string SelectedEstado { get; set; } = "TODOS";
        public DateTime? SelectedFecha { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "search")]
        public string? SearchTextQuery { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "clienteId")]
        public int? ClienteIdQuery { get; set; }

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
        public bool IsReadOnly => ModalMode == "Ver";
        public bool ShowGuardarBtn => ModalMode == "Editar";
        public string ErrorMessage { get; set; } = string.Empty;

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
            }

            if (ListadoReservas.Count > 0 && (!string.IsNullOrEmpty(SearchTextQuery) || ClienteIdQuery.HasValue))
            {
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
                MensajeCarga = $"No se pudieron cargar reservas desde el servicio REST Java: {MensajeErrorServicio(ex)}";
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

            if (ClienteIdQuery.HasValue)
            {
                query = query.Where(r => r.IdCliente == ClienteIdQuery.Value);
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
            ClienteIdQuery = null;
            FiltrarListado();
            Navigation.NavigateTo("reservas", replace: true);
        }

        public void AbrirModal(string modo, ReservaItem? reserva = null)
        {
            ModalMode = modo;

            if (reserva == null)
            {
                return;
            }

            ModalTitle = modo == "Editar" ? "Editar reserva" : "Detalle de reserva";
            FormReserva = new ReservaItem
            {
                Id = reserva.Id,
                IdCliente = reserva.IdCliente,
                IdServicio = reserva.IdServicio,
                Codigo = reserva.Codigo,
                Cliente = reserva.Cliente,
                ClienteTipoDocumento = reserva.ClienteTipoDocumento,
                ClienteNumeroDocumento = reserva.ClienteNumeroDocumento,
                ClienteCorreo = reserva.ClienteCorreo,
                ClienteNacionalidad = reserva.ClienteNacionalidad,
                Servicio = reserva.Servicio,
                CiudadDestino = reserva.CiudadDestino,
                ServicioPrecioUSD = reserva.ServicioPrecioUSD,
                ServicioDuracionHoras = reserva.ServicioDuracionHoras,
                ServicioCapacidadMaxima = reserva.ServicioCapacidadMaxima,
                ServicioIdiomaGuia = reserva.ServicioIdiomaGuia,
                ServicioIncluyeRecojo = reserva.ServicioIncluyeRecojo,
                FechaServicio = reserva.FechaServicio,
                Pax = reserva.Pax,
                Monto = reserva.Monto,
                MontoImpuestos = reserva.MontoImpuestos,
                Estado = reserva.Estado,
                EstadoClase = reserva.EstadoClase
            };

            IsModalOpen = true;
        }

        public void GuardarReserva()
        {
            ErrorMessage = string.Empty;

            if (FormReserva.Id <= 0 || FormReserva.IdCliente <= 0 || FormReserva.IdServicio <= 0)
            {
                ErrorMessage = "No se puede actualizar la reserva porque falta su cliente o servicio asociado.";
                return;
            }

            if (FormReserva.Pax <= 0 || FormReserva.Monto < 0)
            {
                ErrorMessage = "La cantidad de pasajeros debe ser mayor que cero y el monto no puede ser negativo.";
                return;
            }

            try
            {
                FormReserva.EstadoClase = EstadoClase(FormReserva.Estado);
                ReservasServiceClient.Guardar(FormReserva, Estado.Modificado);
                Auditoria.Registrar(
                    "EDITAR_RESERVA",
                    "Modulo Reservas",
                    $"edito la reserva {FormReserva.Codigo} con estado {FormReserva.Estado}.");
                CargarDatosIniciales();
                FiltrarListado();
                CerrarModal();
            }
            catch (Exception ex)
            {
                ErrorMessage = MensajeErrorServicio(ex);
            }
        }

        public void AnularReserva(ReservaItem reserva)
        {
            if (reserva == null || reserva.Estado == "Anulada")
            {
                return;
            }

            var anulada = new ReservaItem
            {
                Id = reserva.Id,
                IdCliente = reserva.IdCliente,
                IdServicio = reserva.IdServicio,
                Codigo = reserva.Codigo,
                Cliente = reserva.Cliente,
                ClienteTipoDocumento = reserva.ClienteTipoDocumento,
                ClienteNumeroDocumento = reserva.ClienteNumeroDocumento,
                ClienteCorreo = reserva.ClienteCorreo,
                ClienteNacionalidad = reserva.ClienteNacionalidad,
                Servicio = reserva.Servicio,
                CiudadDestino = reserva.CiudadDestino,
                ServicioPrecioUSD = reserva.ServicioPrecioUSD,
                ServicioDuracionHoras = reserva.ServicioDuracionHoras,
                ServicioCapacidadMaxima = reserva.ServicioCapacidadMaxima,
                ServicioIdiomaGuia = reserva.ServicioIdiomaGuia,
                ServicioIncluyeRecojo = reserva.ServicioIncluyeRecojo,
                FechaServicio = reserva.FechaServicio,
                Pax = reserva.Pax,
                Monto = reserva.Monto,
                MontoImpuestos = reserva.MontoImpuestos,
                Estado = "Anulada",
                EstadoClase = EstadoClase("Anulada")
            };

            try
            {
                ReservasServiceClient.Guardar(anulada, Estado.Modificado);
                Auditoria.Registrar(
                    "ANULAR_RESERVA",
                    "Modulo Reservas",
                    $"anulo la reserva {anulada.Codigo}.");
                CargarDatosIniciales();
                FiltrarListado();
            }
            catch (Exception ex)
            {
                MensajeCarga = MensajeErrorServicio(ex);
            }
        }

        public void CerrarModal()
        {
            IsModalOpen = false;
            FormReserva = new ReservaItem();
            ErrorMessage = string.Empty;
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

        private static string MensajeErrorServicio(Exception ex)
        {
            var mensaje = ex.Message ?? string.Empty;

            if (mensaje.Contains("<html", StringComparison.OrdinalIgnoreCase)
                || mensaje.Contains("<!DOCTYPE", StringComparison.OrdinalIgnoreCase))
            {
                return "El servicio Java devolvio un error interno. Revisa que los procedimientos de reserva esten cargados y que GlassFish no tenga errores.";
            }

            return mensaje.Length > 240 ? mensaje[..240] + "..." : mensaje;
        }
    }
}

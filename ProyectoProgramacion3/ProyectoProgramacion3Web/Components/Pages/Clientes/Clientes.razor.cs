using Microsoft.AspNetCore.Components;
using ProyectoProgramacion3Web.Services;
using ProyectoProgramacion3Web.Servicios.Clientes;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Components.Pages.Clientes
{
    public partial class Clientes : ComponentBase
    {
        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private IClientesServiceClient ClientesServiceClient { get; set; } = default!;

        [Inject]
        private AuditoriaFrontService Auditoria { get; set; } = default!;

        public List<ClienteItem> ListadoClientes { get; set; } = new();
        public bool IsModalOpen { get; set; }
        public string ModalTitle => "Editar cliente";
        public ClienteItem FormCliente { get; set; } = new();
        public string ErrorMessage { get; set; } = string.Empty;
        public List<string> ValidationErrors { get; set; } = new();

        protected override void OnInitialized()
        {
            CargarDatosIniciales();
        }

        private void CargarDatosIniciales()
        {
            ListadoClientes = ClientesServiceClient.Listar();
        }

        public void AbrirModal(ClienteItem? cliente)
        {
            LimpiarErrores();

            if (cliente == null) return;
            FormCliente = cliente.Clone();

            IsModalOpen = true;
        }

        public void CerrarModal()
        {
            IsModalOpen = false;
            FormCliente = new ClienteItem();
            LimpiarErrores();
        }

        public void GuardarCliente()
        {
            ValidationErrors = FormValidationService.ValidarCliente(FormCliente);
            if (ValidationErrors.Count > 0)
            {
                ErrorMessage = "Corrige los datos del cliente antes de guardar.";
                return;
            }

            ClientesServiceClient.Guardar(FormCliente, Estado.Modificado);
            Auditoria.Registrar(
                "EDITAR_CLIENTE",
                "Modulo Clientes",
                $"edito el cliente {FormCliente.Nombres} {FormCliente.Apellidos}.");
            CargarDatosIniciales();
            CerrarModal();
        }

        public void VerReservas(ClienteItem cliente)
        {
            if (cliente == null) return;
            string fullName = $"{cliente.Nombres} {cliente.Apellidos}";
            Auditoria.Registrar(
                "VER_RESERVAS_CLIENTE",
                "Modulo Clientes",
                $"presiono el boton Reservas del cliente {fullName}.");
            Navigation.NavigateTo($"reservas?clienteId={cliente.Id}");
        }

        private void LimpiarErrores()
        {
            ErrorMessage = string.Empty;
            ValidationErrors.Clear();
        }

        private int MaxDocumentoLength => FormCliente.TipoDocumento == "DNI" ? 8 : 12;

        private string DocumentoInputMode => FormCliente.TipoDocumento == "DNI" ? "numeric" : "text";

    }
}

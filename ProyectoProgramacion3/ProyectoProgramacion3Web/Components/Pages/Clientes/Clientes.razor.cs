using Microsoft.AspNetCore.Components;
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

        public List<ClienteItem> ListadoClientes { get; set; } = new();
        public bool IsModalOpen { get; set; }
        public string ModalMode { get; set; } = "Nuevo";
        public string ModalTitle => ModalMode == "Nuevo" ? "Nuevo cliente" : "Editar cliente";
        public ClienteItem FormCliente { get; set; } = new();

        protected override void OnInitialized()
        {
            CargarDatosIniciales();
        }

        private void CargarDatosIniciales()
        {
            ListadoClientes = ClientesServiceClient.Listar();
        }

        public void AbrirModal(string modo, ClienteItem? cliente = null)
        {
            ModalMode = modo;

            if (modo == "Nuevo")
            {
                FormCliente = new ClienteItem
                {
                    TipoDocumento = "DNI",
                    Nacionalidad = "Peru",
                    FechaNacimiento = new DateTime(1995, 01, 01),
                    FechaRegistro = DateTime.Today,
                    Estado = "Activo"
                };
            }
            else
            {
                if (cliente == null) return;
                FormCliente = cliente.Clone();
            }

            IsModalOpen = true;
        }

        public void CerrarModal()
        {
            IsModalOpen = false;
            FormCliente = new ClienteItem();
        }

        public void GuardarCliente()
        {
            if (string.IsNullOrWhiteSpace(FormCliente.Nombres) ||
                string.IsNullOrWhiteSpace(FormCliente.Apellidos) ||
                string.IsNullOrWhiteSpace(FormCliente.NumeroDocumento) ||
                string.IsNullOrWhiteSpace(FormCliente.Correo))
            {
                return;
            }

            ClientesServiceClient.Guardar(FormCliente, ModalMode == "Nuevo" ? Estado.Nuevo : Estado.Modificado);
            CargarDatosIniciales();
            CerrarModal();
        }

        public void VerReservas(ClienteItem cliente)
        {
            if (cliente == null) return;
            string fullName = $"{cliente.Nombres} {cliente.Apellidos}";
            Navigation.NavigateTo($"reservas?search={Uri.EscapeDataString(fullName)}");
        }

    }
}

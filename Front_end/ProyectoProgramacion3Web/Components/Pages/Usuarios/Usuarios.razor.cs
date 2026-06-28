using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ProyectoProgramacion3Web.Servicios.Usuarios;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Components.Pages.Usuarios
{
    public partial class Usuarios : ComponentBase
    {
        [Inject] private IJSRuntime JS { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IUsuariosServiceClient UsuariosServiceClient { get; set; } = default!;

        public string FiltroTipo { get; set; } = "Todos los tipos";
        public List<UsuarioItem> MasterUsuarios { get; set; } = new();
        public List<UsuarioItem> ListadoFiltrado { get; set; } = new();
        public bool IsFormModalOpen { get; set; }
        public bool IsDeleteModalOpen { get; set; }
        public string ModalMode { get; set; } = "Crear";
        public UsuarioItem FormUsuario { get; set; } = new();
        public UsuarioItem? UsuarioAEliminar { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string ModalTitle => ModalMode == "Crear" ? "Nuevo usuario" : "Editar usuario";

        protected override void OnInitialized()
        {
            CargarDatosIniciales();
            FiltrarListado();
        }

        private void CargarDatosIniciales()
        {
            MasterUsuarios = UsuariosServiceClient.Listar();
        }

        public void FiltrarListado()
        {
            ListadoFiltrado = FiltroTipo == "Todos los tipos"
                ? MasterUsuarios.ToList()
                : MasterUsuarios.Where(u => u.Tipo == FiltroTipo).ToList();
        }

        public void AbrirModalNuevo()
        {
            ModalMode = "Crear";
            ErrorMessage = string.Empty;
            FormUsuario = new UsuarioItem { TipoDocumento = "DNI", Tipo = "Operador", Estado = "Activo" };
            IsFormModalOpen = true;
        }

        public void AbrirModalEditar(UsuarioItem usuario)
        {
            if (usuario == null) return;
            ModalMode = "Editar";
            ErrorMessage = string.Empty;
            FormUsuario = usuario.Clone();
            IsFormModalOpen = true;
        }

        public void CerrarFormModal()
        {
            IsFormModalOpen = false;
            FormUsuario = new UsuarioItem();
            ErrorMessage = string.Empty;
        }

        public void GuardarUsuario()
        {
            if (string.IsNullOrWhiteSpace(FormUsuario.Nombres) ||
                string.IsNullOrWhiteSpace(FormUsuario.Apellidos) ||
                string.IsNullOrWhiteSpace(FormUsuario.Correo))
            {
                ErrorMessage = "Nombres, apellidos y correo son obligatorios.";
                return;
            }

            if (!FormUsuario.Correo.Contains("@") || !FormUsuario.Correo.Contains("."))
            {
                ErrorMessage = "Ingrese un correo electronico valido.";
                return;
            }

            UsuariosServiceClient.Guardar(FormUsuario, ModalMode == "Crear" ? Estado.Nuevo : Estado.Modificado);
            CargarDatosIniciales();
            FiltrarListado();
            CerrarFormModal();
        }

        public void SolicitarEliminar(UsuarioItem usuario)
        {
            if (usuario == null) return;
            UsuarioAEliminar = usuario;
            IsDeleteModalOpen = true;
        }

        public void CerrarDeleteModal()
        {
            IsDeleteModalOpen = false;
            UsuarioAEliminar = null;
        }

        public void EliminarUsuarioConfirmado()
        {
            if (UsuarioAEliminar != null)
            {
                UsuariosServiceClient.Eliminar(UsuarioAEliminar.Id);
                CargarDatosIniciales();
                FiltrarListado();
            }

            CerrarDeleteModal();
        }

        public async Task ExportarListado()
        {
            var csv = new System.Text.StringBuilder("\uFEFF");
            csv.AppendLine("SACR - GESTION DE USUARIOS");
            csv.AppendLine($"Fecha de Exportacion:;{DateTime.Now:dd/MM/yyyy HH:mm}");
            csv.AppendLine($"Filtros Aplicados:;Tipo: {FiltroTipo}");
            csv.AppendLine();
            csv.AppendLine("N;Nombre Completo;Tipo Doc;Documento;Correo;Tipo Usuario;Estado");

            int index = 1;
            ListadoFiltrado.ForEach(u => csv.AppendLine($"{index++};{u.NombreCompleto};{u.TipoDocumento};{u.NumeroDocumento};{u.Correo};{u.Tipo};{u.Estado}"));

            var base64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(csv.ToString()));
            await JS.InvokeVoidAsync("downloadFileFromBase64", $"Listado_Usuarios_{DateTime.Now:yyyyMMdd_HHmmss}.csv", base64, "text/csv;charset=utf-8");
        }

    }
}

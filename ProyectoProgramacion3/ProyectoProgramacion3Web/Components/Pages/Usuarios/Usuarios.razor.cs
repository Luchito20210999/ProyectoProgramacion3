using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ProyectoProgramacion3Web.Services;
using ProyectoProgramacion3Web.Servicios.Usuarios;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Components.Pages.Usuarios
{
    public partial class Usuarios : ComponentBase
    {
        [Inject] private IJSRuntime JS { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private SessionService Session { get; set; } = default!;
        [Inject] private IUsuariosServiceClient UsuariosServiceClient { get; set; } = default!;

        public string FiltroTipo { get; set; } = "Todos los tipos";
        public List<UsuarioItem> MasterUsuarios { get; set; } = new();
        public List<UsuarioItem> ListadoFiltrado { get; set; } = new();
        public bool IsFormModalOpen { get; set; }
        public bool IsDeleteModalOpen { get; set; }
        public bool IsSaving { get; set; }
        public string ModalMode { get; set; } = "Crear";
        public UsuarioItem FormUsuario { get; set; } = new();
        public UsuarioItem? UsuarioAEliminar { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;
        public int UsuarioAnimadoId { get; set; }
        public string ModalTitle => ModalMode == "Crear" ? "Nuevo usuario" : "Editar usuario";
        public bool EsCreacion => ModalMode == "Crear";

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
            SuccessMessage = string.Empty;
            FormUsuario = new UsuarioItem { TipoDocumento = "DNI", Tipo = "Operador", Estado = "Activo" };
            IsFormModalOpen = true;
        }

        public void AbrirModalEditar(UsuarioItem usuario)
        {
            if (usuario == null) return;
            ModalMode = "Editar";
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            FormUsuario = usuario.Clone();
            IsFormModalOpen = true;
        }

        public void CerrarFormModal()
        {
            IsFormModalOpen = false;
            FormUsuario = new UsuarioItem();
            ErrorMessage = string.Empty;
        }

        public async Task GuardarUsuario()
        {
            if (IsSaving)
            {
                return;
            }

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

            if (EsCreacion && string.IsNullOrWhiteSpace(FormUsuario.Contrasena))
            {
                ErrorMessage = "La contrasena es obligatoria.";
                return;
            }

            var correoGuardado = FormUsuario.Correo;
            var fueCreacion = EsCreacion;

            try
            {
                IsSaving = true;
                await Task.Yield();

                UsuariosServiceClient.Guardar(FormUsuario, fueCreacion ? Estado.Nuevo : Estado.Modificado);
                CargarDatosIniciales();
                FiltrarListado();

                UsuarioAnimadoId = MasterUsuarios
                    .FirstOrDefault(u => string.Equals(u.Correo, correoGuardado, StringComparison.OrdinalIgnoreCase))
                    ?.Id ?? 0;
                SuccessMessage = fueCreacion ? "Usuario creado correctamente." : "Usuario actualizado correctamente.";
                CerrarFormModal();
                await Task.Delay(1800);
                UsuarioAnimadoId = 0;
                SuccessMessage = string.Empty;
            }
            finally
            {
                IsSaving = false;
                StateHasChanged();
            }
        }

        public void SolicitarEliminar(UsuarioItem usuario)
        {
            if (usuario == null) return;
            ErrorMessage = string.Empty;
            if (EsUsuarioActual(usuario))
            {
                ErrorMessage = "No puedes eliminar el usuario con el que iniciaste sesion.";
                return;
            }

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
                if (EsUsuarioActual(UsuarioAEliminar))
                {
                    ErrorMessage = "No puedes eliminar el usuario con el que iniciaste sesion.";
                    CerrarDeleteModal();
                    return;
                }

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

        public bool EsUsuarioActual(UsuarioItem usuario)
        {
            return usuario.Id > 0 && usuario.Id == Session.UserId;
        }

    }
}

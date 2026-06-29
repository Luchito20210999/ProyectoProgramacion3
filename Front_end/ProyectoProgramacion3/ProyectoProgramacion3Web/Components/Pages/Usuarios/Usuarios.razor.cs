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
        [Inject] private AuditoriaFrontService Auditoria { get; set; } = default!;

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
        public List<string> ValidationErrors { get; set; } = new();
        public string SuccessMessage { get; set; } = string.Empty;
        public int UsuarioAnimadoId { get; set; }
        public string ModalTitle => ModalMode == "Crear" ? "Nuevo usuario" : "Editar usuario";
        public bool EsCreacion => ModalMode == "Crear";
        public bool EstadoSelectDisabled => !EsCreacion && EsUsuarioActual(FormUsuario);
        public bool TipoSelectDisabled => !EsCreacion && EsUsuarioActual(FormUsuario);

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
            LimpiarErrores();
            SuccessMessage = string.Empty;
            FormUsuario = new UsuarioItem { TipoDocumento = "DNI", Tipo = "Operador", Estado = "Activo" };
            IsFormModalOpen = true;
        }

        public void AbrirModalEditar(UsuarioItem usuario)
        {
            if (usuario == null) return;
            ModalMode = "Editar";
            LimpiarErrores();
            SuccessMessage = string.Empty;
            FormUsuario = usuario.Clone();
            IsFormModalOpen = true;
        }

        public void CerrarFormModal()
        {
            IsFormModalOpen = false;
            FormUsuario = new UsuarioItem();
            LimpiarErrores();
        }

        public async Task GuardarUsuario()
        {
            if (IsSaving)
            {
                return;
            }

            ValidationErrors = FormValidationService.ValidarUsuario(FormUsuario, EsCreacion);
            if (!EsCreacion && EsUsuarioActual(FormUsuario) &&
                string.Equals(FormUsuario.Estado, "Inactivo", StringComparison.OrdinalIgnoreCase))
            {
                ValidationErrors.Add("No puedes desactivar el usuario con el que iniciaste sesion.");
            }

            if (!EsCreacion && EsUsuarioActual(FormUsuario) &&
                MasterUsuarios.FirstOrDefault(u => u.Id == FormUsuario.Id) is { } usuarioOriginal &&
                !string.Equals(usuarioOriginal.Tipo, FormUsuario.Tipo, StringComparison.OrdinalIgnoreCase))
            {
                ValidationErrors.Add("No puedes cambiar el rol del usuario con el que iniciaste sesion.");
            }

            if (ValidationErrors.Count > 0)
            {
                ErrorMessage = "Corrige los datos del usuario antes de guardar.";
                return;
            }

            var correoGuardado = FormUsuario.Correo;
            var fueCreacion = EsCreacion;
            var accionAuditoria = fueCreacion ? "CREAR_USUARIO" : "EDITAR_USUARIO";
            var descripcionAuditoria = fueCreacion
                ? $"creo el usuario {correoGuardado} en Gestion de Usuarios."
                : $"edito el usuario {correoGuardado} en Gestion de Usuarios.";

            try
            {
                IsSaving = true;
                await Task.Yield();

                UsuariosServiceClient.Guardar(FormUsuario, fueCreacion ? Estado.Nuevo : Estado.Modificado);
                Auditoria.Registrar(accionAuditoria, "Modulo Usuarios", descripcionAuditoria);
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
            LimpiarErrores();
            if (EsUsuarioActual(usuario))
            {
                ErrorMessage = "No puedes eliminar el usuario con el que iniciaste sesion.";
                return;
            }

            UsuarioAEliminar = usuario;
            IsDeleteModalOpen = true;
        }

        public void CambiarEstadoUsuario(UsuarioItem usuario)
        {
            if (usuario == null) return;
            LimpiarErrores();
            SuccessMessage = string.Empty;

            if (EsUsuarioActual(usuario))
            {
                ErrorMessage = "No puedes desactivar el usuario con el que iniciaste sesion.";
                return;
            }

            var nuevoEstado = string.Equals(usuario.Estado, "Activo", StringComparison.OrdinalIgnoreCase)
                ? "Inactivo"
                : "Activo";

            var actualizado = usuario.Clone();
            actualizado.Estado = nuevoEstado;

            UsuariosServiceClient.Guardar(actualizado, Estado.Modificado);
            Auditoria.Registrar(
                nuevoEstado == "Activo" ? "ACTIVAR_USUARIO" : "DESACTIVAR_USUARIO",
                "Modulo Usuarios",
                $"{(nuevoEstado == "Activo" ? "activo" : "desactivo")} el usuario {actualizado.Correo} en Gestion de Usuarios.");
            CargarDatosIniciales();
            FiltrarListado();
            SuccessMessage = nuevoEstado == "Activo"
                ? "Usuario activado correctamente."
                : "Usuario desactivado correctamente.";
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
                Auditoria.Registrar(
                    "ELIMINAR_USUARIO",
                    "Modulo Usuarios",
                    $"elimino el usuario {UsuarioAEliminar.Correo} en Gestion de Usuarios.");
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
            Auditoria.Registrar(
                "EXPORTAR_USUARIOS",
                "Modulo Usuarios",
                $"exporto el listado de usuarios con filtro {FiltroTipo}.");
        }

        public bool EsUsuarioActual(UsuarioItem usuario)
        {
            return usuario.Id > 0 && usuario.Id == Session.UserId;
        }

        public string TextoBotonEstado(UsuarioItem usuario)
        {
            if (EsUsuarioActual(usuario))
            {
                return "Sesion";
            }

            return string.Equals(usuario.Estado, "Activo", StringComparison.OrdinalIgnoreCase)
                ? "Desactivar"
                : "Activar";
        }

        public string ClaseBotonEstado(UsuarioItem usuario)
        {
            if (EsUsuarioActual(usuario))
            {
                return "btn-row-status btn-row-current";
            }

            return string.Equals(usuario.Estado, "Activo", StringComparison.OrdinalIgnoreCase)
                ? "btn-row-status btn-row-deactivate"
                : "btn-row-status btn-row-activate";
        }

        private void LimpiarErrores()
        {
            ErrorMessage = string.Empty;
            ValidationErrors.Clear();
        }

    }
}

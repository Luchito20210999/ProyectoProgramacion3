using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ProyectoProgramacion3Web.Components.Pages.Usuarios
{
    /// <summary>
    /// Componente code-behind de la vista de Gestión de Usuarios.
    /// Administra operaciones CRUD (Creación, Lectura, Actualización, Eliminación) de cuentas de SACR.
    /// </summary>
    public partial class Usuarios : ComponentBase
    {
        [Inject] private IJSRuntime JS { get; set; }
        [Inject] private NavigationManager Navigation { get; set; }

        // Filtro enlazado
        public string FiltroTipo { get; set; } = "Todos los tipos";

        // Colecciones de Datos Maestros y de Renderizado
        public List<UsuarioItem> MasterUsuarios { get; set; } = new();
        public List<UsuarioItem> ListadoFiltrado { get; set; } = new();

        // Estados y flags de control para los Modales CRUD
        public bool IsFormModalOpen { get; set; }
        public bool IsDeleteModalOpen { get; set; }
        public string ModalMode { get; set; } = "Crear"; // Valores admisibles: "Crear", "Editar"
        public UsuarioItem FormUsuario { get; set; } = new();
        public UsuarioItem UsuarioAEliminar { get; set; }

        // Mensaje de validación local del formulario
        public string ErrorMessage { get; set; } = string.Empty;

        // Título dinámico del modal de formulario
        public string ModalTitle => ModalMode == "Crear" ? "Nuevo usuario" : "Editar usuario";

        /// <summary>
        /// Inicializa el componente, carga el sembrado inicial y realiza el primer filtrado en pantalla.
        /// </summary>
        protected override void OnInitialized()
        {
            CargarDatosIniciales();
            FiltrarListado();
        }

        /// <summary>
        /// Carga cuentas iniciales en memoria con diversos roles del sistema para demostración de perfiles.
        /// </summary>
        private void CargarDatosIniciales()
        {
            MasterUsuarios = new List<UsuarioItem>
            {
                new UsuarioItem { Id = 1, Nombres = "María", Apellidos = "Alarcón", TipoDocumento = "DNI", NumeroDocumento = "71234567", Correo = "m.alarcon@sacr.pe", Telefono = "998877665", Tipo = "Administrador" },
                new UsuarioItem { Id = 2, Nombres = "Carlos", Apellidos = "Rojas", TipoDocumento = "DNI", NumeroDocumento = "40987654", Correo = "c.rojas@sacr.pe", Telefono = "992233445", Tipo = "Analista" },
                new UsuarioItem { Id = 3, Nombres = "Lucía", Apellidos = "Vega", TipoDocumento = "DNI", NumeroDocumento = "44556677", Correo = "l.vega@sacr.pe", Telefono = "991122334", Tipo = "Operador" },
                new UsuarioItem { Id = 4, Nombres = "Diego", Apellidos = "Mendoza", TipoDocumento = "CE", NumeroDocumento = "002233445", Correo = "d.mendoza@sacr.pe", Telefono = "994455667", Tipo = "Operador", Estado = "Inactivo" }
            };
        }

        /// <summary>
        /// Filtra la colección maestra de usuarios según el tipo/rol seleccionado en el dropdown de la cabecera.
        /// </summary>
        public void FiltrarListado()
        {
            ListadoFiltrado = FiltroTipo == "Todos los tipos"
                ? MasterUsuarios.ToList()
                : MasterUsuarios.Where(u => u.Tipo == FiltroTipo).ToList();
        }

        /// <summary>
        /// Abre el formulario modal en modo de creación e inicializa un objeto de tipo de usuario vacío.
        /// </summary>
        public void AbrirModalNuevo()
        {
            ModalMode = "Crear";
            ErrorMessage = string.Empty;
            FormUsuario = new UsuarioItem { TipoDocumento = "DNI", Tipo = "Operador", Estado = "Activo" };
            IsFormModalOpen = true;
        }

        /// <summary>
        /// Abre el formulario modal en modo de edición e inyecta un clon de la entidad seleccionada.
        /// </summary>
        public void AbrirModalEditar(UsuarioItem usuario)
        {
            if (usuario == null) return;
            ModalMode = "Editar";
            ErrorMessage = string.Empty;
            
            // Clonar entidad para aislar cambios no validados y evitar re-renderizado inconsistente en la tabla
            FormUsuario = new UsuarioItem
            {
                Id = usuario.Id, Nombres = usuario.Nombres, Apellidos = usuario.Apellidos,
                TipoDocumento = usuario.TipoDocumento, NumeroDocumento = usuario.NumeroDocumento,
                Correo = usuario.Correo, Telefono = usuario.Telefono, Tipo = usuario.Tipo, Estado = usuario.Estado
            };
            IsFormModalOpen = true;
        }

        /// <summary>
        /// Oculta el modal de edición/creación y limpia los datos temporales del formulario.
        /// </summary>
        public void CerrarFormModal()
        {
            IsFormModalOpen = false;
            FormUsuario = new UsuarioItem();
            ErrorMessage = string.Empty;
        }

        /// <summary>
        /// Valida y persiste los cambios del formulario en la colección temporal en memoria de la sesión.
        /// </summary>
        public void GuardarUsuario()
        {
            if (string.IsNullOrWhiteSpace(FormUsuario.Nombres) || string.IsNullOrWhiteSpace(FormUsuario.Apellidos) || string.IsNullOrWhiteSpace(FormUsuario.Correo))
            {
                ErrorMessage = "Nombres, Apellidos y Correo son campos obligatorios.";
                return;
            }

            if (!FormUsuario.Correo.Contains("@") || !FormUsuario.Correo.Contains("."))
            {
                ErrorMessage = "Por favor ingrese un correo electrónico válido.";
                return;
            }

            if (ModalMode == "Crear")
            {
                FormUsuario.Id = MasterUsuarios.Any() ? MasterUsuarios.Max(u => u.Id) + 1 : 1;
                MasterUsuarios.Add(FormUsuario);
            }
            else
            {
                var existente = MasterUsuarios.FirstOrDefault(u => u.Id == FormUsuario.Id);
                if (existente != null)
                {
                    existente.Nombres = FormUsuario.Nombres; existente.Apellidos = FormUsuario.Apellidos;
                    existente.TipoDocumento = FormUsuario.TipoDocumento; existente.NumeroDocumento = FormUsuario.NumeroDocumento;
                    existente.Correo = FormUsuario.Correo; existente.Telefono = FormUsuario.Telefono;
                    existente.Tipo = FormUsuario.Tipo; existente.Estado = FormUsuario.Estado;
                }
            }

            FiltrarListado();
            CerrarFormModal();
        }

        /// <summary>
        /// Despliega el cuadro modal rojo de confirmación para eliminar de forma controlada el usuario seleccionado.
        /// </summary>
        public void SolicitarEliminar(UsuarioItem usuario)
        {
            if (usuario == null) return;
            UsuarioAEliminar = usuario;
            IsDeleteModalOpen = true;
        }

        /// <summary>
        /// Oculta el cuadro modal de eliminación de usuarios sin persistir cambios.
        /// </summary>
        public void CerrarDeleteModal()
        {
            IsDeleteModalOpen = false;
            UsuarioAEliminar = null;
        }

        /// <summary>
        /// Procede a remover físicamente de memoria al usuario seleccionado tras la aprobación explícita en el modal.
        /// </summary>
        public void EliminarUsuarioConfirmado()
        {
            if (UsuarioAEliminar != null)
            {
                MasterUsuarios.Remove(UsuarioAEliminar);
                FiltrarListado();
            }
            CerrarDeleteModal();
        }

        /// <summary>
        /// Exporta la tabla filtrada de usuarios a formato CSV UTF-8 con BOM de compatibilidad Excel.
        /// </summary>
        public async Task ExportarListado()
        {
            var csv = new System.Text.StringBuilder("\uFEFF"); // BOM de UTF-8
            csv.AppendLine("SACR - GESTION DE USUARIOS");
            csv.AppendLine($"Fecha de Exportación:;{DateTime.Now:dd/MM/yyyy HH:mm}");
            csv.AppendLine($"Filtros Aplicados:;Tipo: {FiltroTipo}");
            csv.AppendLine();
            csv.AppendLine("N°;Nombre Completo;Tipo Doc;N° Documento;Correo;Tipo Usuario;Estado");

            int index = 1;
            ListadoFiltrado.ForEach(u => csv.AppendLine($"{index++};{u.NombreCompleto};{u.TipoDocumento};{u.NumeroDocumento};{u.Correo};{u.Tipo};{u.Estado}"));

            var base64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(csv.ToString()));
            await JS.InvokeVoidAsync("downloadFileFromBase64", $"Listado_Usuarios_{DateTime.Now:yyyyMMdd_HHmmss}.csv", base64, "text/csv;charset=utf-8");
        }
    }

    public class UsuarioItem
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string TipoDocumento { get; set; } = "DNI";
        public string NumeroDocumento { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Tipo { get; set; } = "Operador";
        public string Estado { get; set; } = "Activo";

        public string NombreCompleto => $"{Nombres} {Apellidos}";
        public string EstadoClase => Estado == "Activo" ? "badge-activo" : "badge-inactivo";
    }
}

using Microsoft.AspNetCore.Components;
using ProyectoProgramacion3Web.Servicios.ServiciosTuristicos;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Components.Pages.Servicios
{
    public partial class Servicios : ComponentBase
    {
        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private IServiciosServiceClient ServiciosServiceClient { get; set; } = default!;

        public List<ServicioItem> ListadoServicios { get; set; } = new();
        public bool IsModalOpen { get; set; }
        public string ModalMode { get; set; } = "Nuevo";
        public string ModalTitle => ModalMode == "Nuevo" ? "Nuevo servicio" : "Editar servicio";
        public ServicioItem FormServicio { get; set; } = new();
        public bool IsIdiomaDropdownOpen { get; set; }
        public bool IdiomaEspSelected { get; set; }
        public bool IdiomaIngSelected { get; set; }
        public bool IdiomaFraSelected { get; set; }

        protected override void OnInitialized()
        {
            CargarDatosIniciales();
        }

        private void CargarDatosIniciales()
        {
            ListadoServicios = ServiciosServiceClient.Listar();
        }

        public void AbrirModal(string modo, ServicioItem? servicio = null)
        {
            ModalMode = modo;
            IsIdiomaDropdownOpen = false;

            FormServicio = modo == "Nuevo"
                ? new ServicioItem { Duracion = 1, CapacidadMaxima = 1, IdiomaGuia = "Esp/Ing", IncluyeRecojo = "Sí" }
                : servicio?.Clone() ?? new ServicioItem();

            ParseIdiomasFromForm();
            IsModalOpen = true;
        }

        public void CerrarModal()
        {
            IsModalOpen = false;
            FormServicio = new ServicioItem();
        }

        public void GuardarServicio()
        {
            CompileIdiomasToForm();

            if (string.IsNullOrWhiteSpace(FormServicio.Nombre) ||
                string.IsNullOrWhiteSpace(FormServicio.Descripcion) ||
                string.IsNullOrWhiteSpace(FormServicio.CiudadDestino) ||
                FormServicio.Precio <= 0 ||
                string.IsNullOrWhiteSpace(FormServicio.IdiomaGuia))
            {
                return;
            }

            ServiciosServiceClient.Guardar(FormServicio, ModalMode == "Nuevo" ? Estado.Nuevo : Estado.Modificado);
            CargarDatosIniciales();
            CerrarModal();
        }

        public void ToggleIdiomaDropdown()
        {
            IsIdiomaDropdownOpen = !IsIdiomaDropdownOpen;
        }

        public void CerrarDropdownIdioma()
        {
            IsIdiomaDropdownOpen = false;
        }

        public void ParseIdiomasFromForm()
        {
            string lower = (FormServicio.IdiomaGuia ?? string.Empty).ToLowerInvariant();
            IdiomaEspSelected = lower.Contains("esp") || string.IsNullOrWhiteSpace(lower);
            IdiomaIngSelected = lower.Contains("ing");
            IdiomaFraSelected = lower.Contains("fra");
        }

        public void CompileIdiomasToForm()
        {
            var lista = new List<string>();
            if (IdiomaEspSelected) lista.Add("Esp");
            if (IdiomaIngSelected) lista.Add("Ing");
            if (IdiomaFraSelected) lista.Add("Fra");
            if (!lista.Any())
            {
                lista.Add("Esp");
                IdiomaEspSelected = true;
            }
            FormServicio.IdiomaGuia = string.Join("/", lista);
        }

        public void ActualizarIdiomas()
        {
            if (!IdiomaEspSelected && !IdiomaIngSelected && !IdiomaFraSelected)
            {
                IdiomaEspSelected = true;
            }
            CompileIdiomasToForm();
        }

        public string ObtenerResumenIdiomas()
        {
            var lista = new List<string>();
            if (IdiomaEspSelected) lista.Add("Espanol");
            if (IdiomaIngSelected) lista.Add("Ingles");
            if (IdiomaFraSelected) lista.Add("Frances");
            return lista.Any() ? string.Join(", ", lista) : "Seleccionar idiomas...";
        }

        public void EliminarServicio(ServicioItem servicio)
        {
            if (servicio == null) return;
            ServiciosServiceClient.Eliminar(servicio.Id);
            CargarDatosIniciales();
        }

        public string ObtenerImagenDestinoUrl(string destino)
        {
            if (string.IsNullOrWhiteSpace(destino))
            {
                return "/images/destinos/default.png";
            }

            string normalizado = destino.Trim().ToLowerInvariant()
                .Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ú", "u")
                .Replace("ñ", "n");

            normalizado = System.Text.RegularExpressions.Regex.Replace(normalizado, @"\s+", "-");
            return normalizado is "lima" or "cusco" or "paracas" or "ica"
                ? $"/images/destinos/{normalizado}.png"
                : "/images/destinos/default.png";
        }

    }
}

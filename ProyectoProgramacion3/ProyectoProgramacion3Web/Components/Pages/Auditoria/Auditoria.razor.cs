using Microsoft.AspNetCore.Components;
using ProyectoProgramacion3Web.Servicios.Auditoria;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Components.Pages.Auditoria
{
    public partial class Auditoria : ComponentBase
    {
        [Inject]
        private IAuditoriaServiceClient AuditoriaServiceClient { get; set; } = default!;

        public string SearchText { get; set; } = string.Empty;
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public List<AuditoriaItem> MasterTrazas { get; set; } = new();
        public List<AuditoriaItem> ListadoFiltrado { get; set; } = new();

        protected override void OnInitialized()
        {
            CargarDatosIniciales();
            FiltrarListado();
        }

        private void CargarDatosIniciales()
        {
            MasterTrazas = AuditoriaServiceClient.Listar();
        }

        public void FiltrarListado()
        {
            var query = MasterTrazas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string lowerSearch = SearchText.ToLowerInvariant();
                query = query.Where(t => t.Usuario.ToLowerInvariant().Contains(lowerSearch) ||
                                         t.Comando.ToLowerInvariant().Contains(lowerSearch) ||
                                         t.Descripcion.ToLowerInvariant().Contains(lowerSearch) ||
                                         t.Ubicacion.ToLowerInvariant().Contains(lowerSearch));
            }

            if (FechaInicio.HasValue) query = query.Where(t => t.Fecha.Date >= FechaInicio.Value.Date);
            if (FechaFin.HasValue) query = query.Where(t => t.Fecha.Date <= FechaFin.Value.Date);

            ListadoFiltrado = query.OrderByDescending(t => t.Fecha).ToList();
        }

    }
}

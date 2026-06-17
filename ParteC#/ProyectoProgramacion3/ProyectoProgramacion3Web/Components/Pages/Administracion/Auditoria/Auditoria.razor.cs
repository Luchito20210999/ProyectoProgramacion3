using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace ProyectoProgramacion3Web.Components.Pages.Auditoria
{
    /// <summary>
    /// Componente code-behind de la vista de Log de Auditoría.
    /// Administra la visualización del histórico de acciones ejecutadas en SACR para control de seguridad.
    /// </summary>
    public partial class Auditoria : ComponentBase
    {
        // Campos de enlazamiento de filtros de búsqueda
        public string SearchText { get; set; } = string.Empty;
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        // Colecciones de Datos Maestros y de Renderizado
        public List<AuditoriaItem> MasterTrazas { get; set; } = new();
        public List<AuditoriaItem> ListadoFiltrado { get; set; } = new();

        /// <summary>
        /// Inicializa el componente, carga las trazas de auditoría de ejemplo y aplica los filtros iniciales.
        /// </summary>
        protected override void OnInitialized()
        {
            CargarDatosIniciales();
            FiltrarListado();
        }

        /// <summary>
        /// Carga en memoria una secuencia de trazas históricas representativas para fines de auditoría del sistema.
        /// </summary>
        private void CargarDatosIniciales()
        {
            MasterTrazas = new List<AuditoriaItem>
            {
                new AuditoriaItem { Id = 1, Fecha = new DateTime(2026, 5, 12, 9, 14, 0), Usuario = "m.alarcon", Comando = "LOGIN", Descripcion = "Inicio de sesión exitoso", Ubicacion = "Lima, PE" },
                new AuditoriaItem { Id = 2, Fecha = new DateTime(2026, 5, 12, 9, 5, 0), Usuario = "c.rojas", Comando = "REPORTE_GENERAR", Descripcion = "Generó reporte de ventas Mayo 2026", Ubicacion = "Lima, PE" },
                new AuditoriaItem { Id = 3, Fecha = new DateTime(2026, 5, 12, 8, 0, 0), Usuario = "l.vega", Comando = "RESERVA_ANULAR", Descripcion = "Anuló reserva BK-10296", Ubicacion = "Cusco, PE" },
                new AuditoriaItem { Id = 4, Fecha = new DateTime(2026, 5, 12, 6, 30, 0), Usuario = "d.mendoza", Comando = "RECLAMO_REGISTRAR", Descripcion = "Registró reclamo #4", Ubicacion = "Lima, PE" },
                new AuditoriaItem { Id = 5, Fecha = new DateTime(2026, 5, 11, 17, 45, 0), Usuario = "c.rojas", Comando = "RECLAMO_ATENDER", Descripcion = "Atendió reclamo #3 (PROCEDE)", Ubicacion = "Lima, PE" },
                new AuditoriaItem { Id = 6, Fecha = new DateTime(2026, 5, 11, 15, 20, 0), Usuario = "m.alarcon", Comando = "SERVICIO_CREAR", Descripcion = "Creó servicio Camino Inca VIP", Ubicacion = "Lima, PE" },
                new AuditoriaItem { Id = 7, Fecha = new DateTime(2026, 5, 10, 11, 30, 0), Usuario = "l.vega", Comando = "RESERVA_CREAR", Descripcion = "Creó reserva BK-10297", Ubicacion = "Cusco, PE" },
                new AuditoriaItem { Id = 8, Fecha = new DateTime(2026, 5, 10, 10, 15, 0), Usuario = "d.mendoza", Comando = "LOGIN", Descripcion = "Inicio de sesión exitoso", Ubicacion = "Arequipa, PE" },
                new AuditoriaItem { Id = 9, Fecha = new DateTime(2026, 5, 9, 14, 0, 0), Usuario = "c.rojas", Comando = "CLIENTE_MODIFICAR", Descripcion = "Modificó datos de Lucía Fernández", Ubicacion = "Lima, PE" },
                new AuditoriaItem { Id = 10, Fecha = new DateTime(2026, 5, 9, 9, 30, 0), Usuario = "l.vega", Comando = "LOGIN", Descripcion = "Inicio de sesión exitoso", Ubicacion = "Cusco, PE" }
            };
        }

        /// <summary>
        /// Filtra las trazas de auditoría de forma reactiva según texto ingresado (usuario, comando o descripción)
        /// y por el rango de fechas seleccionado, ordenando el resultado de forma cronológica descendente.
        /// </summary>
        public void FiltrarListado()
        {
            var query = MasterTrazas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string lowerSearch = SearchText.ToLower();
                query = query.Where(t => t.Usuario.ToLower().Contains(lowerSearch) ||
                                         t.Comando.ToLower().Contains(lowerSearch) ||
                                         t.Descripcion.ToLower().Contains(lowerSearch) ||
                                         t.Ubicacion.ToLower().Contains(lowerSearch));
            }

            if (FechaInicio.HasValue) query = query.Where(t => t.Fecha.Date >= FechaInicio.Value.Date);
            if (FechaFin.HasValue) query = query.Where(t => t.Fecha.Date <= FechaFin.Value.Date);

            ListadoFiltrado = query.OrderByDescending(t => t.Fecha).ToList();
        }
    }

    public class AuditoriaItem
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
        public string Comando { get; set; }
        public string Descripcion { get; set; }
        public string Ubicacion { get; set; }
    }
}

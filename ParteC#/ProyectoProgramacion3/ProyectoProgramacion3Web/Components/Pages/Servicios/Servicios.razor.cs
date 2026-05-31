using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace ProyectoProgramacion3Web.Components.Pages.Servicios
{
    public partial class Servicios : ComponentBase
    {
        [Inject]
        private NavigationManager Navigation { get; set; }

        // Listado de servicios turísticos en memoria
        public List<ServicioItem> ListadoServicios { get; set; } = new List<ServicioItem>();

        // Estado del Modal CRUD
        public bool IsModalOpen { get; set; }
        public string ModalMode { get; set; } = "Nuevo"; // "Nuevo", "Editar"
        public string ModalTitle => ModalMode == "Nuevo" ? "Nuevo servicio" : "Editar servicio";
        public ServicioItem FormServicio { get; set; } = new ServicioItem();

        // Propiedades para la lista desplegable con checkboxes de idioma
        public bool IsIdiomaDropdownOpen { get; set; }
        public bool IdiomaEspSelected { get; set; }
        public bool IdiomaIngSelected { get; set; }
        public bool IdiomaFraSelected { get; set; }

        // Iconos turísticos predeterminados
        private readonly string[] IconosDisponibles = { "🧭", "🏔", "⛵", "🏛", "🌅", "🏜", "🚠" };

        protected override void OnInitialized()
        {
            CargarDatosIniciales();
        }

        private void CargarDatosIniciales()
        {
            ListadoServicios = new List<ServicioItem>
            {
                new ServicioItem
                {
                    Id = 1,
                    Nombre = "City Tour Lima",
                    Descripcion = "Recorrido por el centro histórico, Miraflores y Barranco.",
                    Precio = 60.00m,
                    Duracion = 4,
                    CapacidadMaxima = 15,
                    IdiomaGuia = "Esp/Ing",
                    CiudadDestino = "Lima",
                    IncluyeRecojo = "Sí",
                    Icono = "🏛"
                },
                new ServicioItem
                {
                    Id = 2,
                    Nombre = "Machu Picchu Full Day",
                    Descripcion = "Tour completo a la ciudadela inca con tren y bus incluidos.",
                    Precio = 240.00m,
                    Duracion = 16,
                    CapacidadMaxima = 10,
                    IdiomaGuia = "Esp/Ing/Fra",
                    CiudadDestino = "Cusco",
                    IncluyeRecojo = "Sí",
                    Icono = "🏔"
                },
                new ServicioItem
                {
                    Id = 3,
                    Nombre = "Islas Ballestas",
                    Descripcion = "Paseo en lancha por la fauna marina de Paracas.",
                    Precio = 75.00m,
                    Duracion = 2,
                    CapacidadMaxima = 20,
                    IdiomaGuia = "Esp/Ing",
                    CiudadDestino = "Paracas",
                    IncluyeRecojo = "No",
                    Icono = "⛵"
                },
                new ServicioItem
                {
                    Id = 4,
                    Nombre = "Valle Sagrado",
                    Descripcion = "Recorrido por Pisac, Ollantaytambo y Chinchero.",
                    Precio = 70.00m,
                    Duracion = 10,
                    CapacidadMaxima = 12,
                    IdiomaGuia = "Esp/Ing",
                    CiudadDestino = "Cusco",
                    IncluyeRecojo = "Sí",
                    Icono = "🧭"
                }
            };
        }

        // Control del modal CRUD
        public void AbrirModal(string modo, ServicioItem servicio = null)
        {
            ModalMode = modo;
            IsIdiomaDropdownOpen = false;

            if (modo == "Nuevo")
            {
                FormServicio = new ServicioItem
                {
                    Precio = 0.00m,
                    Duracion = 1,
                    CapacidadMaxima = 1,
                    IdiomaGuia = "Esp/Ing",
                    IncluyeRecojo = "Sí",
                    Icono = PickRandomIcon()
                };
            }
            else
            {
                if (servicio == null) return;

                // Clonar objeto para evitar edición en tiempo real de la tarjeta visual antes de guardar
                FormServicio = new ServicioItem
                {
                    Id = servicio.Id,
                    Nombre = servicio.Nombre,
                    Descripcion = servicio.Descripcion,
                    Precio = servicio.Precio,
                    Duracion = servicio.Duracion,
                    CapacidadMaxima = servicio.CapacidadMaxima,
                    IdiomaGuia = servicio.IdiomaGuia,
                    CiudadDestino = servicio.CiudadDestino,
                    IncluyeRecojo = servicio.IncluyeRecojo,
                    Icono = servicio.Icono
                };
            }

            ParseIdiomasFromForm();
            IsModalOpen = true;
        }

        public void CerrarModal()
        {
            IsModalOpen = false;
            FormServicio = new ServicioItem();
        }

        // Guarda o actualiza los datos del servicio
        public void GuardarServicio()
        {
            // Compilar los idiomas desde los checkboxes antes de guardar
            CompileIdiomasToForm();

            // Validación mínima para asegurar consistencia
            if (string.IsNullOrWhiteSpace(FormServicio.Nombre) ||
                string.IsNullOrWhiteSpace(FormServicio.Descripcion) ||
                string.IsNullOrWhiteSpace(FormServicio.CiudadDestino) ||
                FormServicio.Precio <= 0 ||
                string.IsNullOrWhiteSpace(FormServicio.IdiomaGuia))
            {
                return;
            }

            if (ModalMode == "Nuevo")
            {
                int nuevoId = ListadoServicios.Any() ? ListadoServicios.Max(s => s.Id) + 1 : 1;
                FormServicio.Id = nuevoId;

                ListadoServicios.Add(FormServicio);
            }
            else if (ModalMode == "Editar")
            {
                var existente = ListadoServicios.FirstOrDefault(s => s.Id == FormServicio.Id);
                if (existente != null)
                {
                    existente.Nombre = FormServicio.Nombre;
                    existente.Descripcion = FormServicio.Descripcion;
                    existente.Precio = FormServicio.Precio;
                    existente.Duracion = FormServicio.Duracion;
                    existente.CapacidadMaxima = FormServicio.CapacidadMaxima;
                    existente.IdiomaGuia = FormServicio.IdiomaGuia;
                    existente.CiudadDestino = FormServicio.CiudadDestino;
                    existente.IncluyeRecojo = FormServicio.IncluyeRecojo;
                    existente.Icono = FormServicio.Icono;
                }
            }

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
            if (string.IsNullOrEmpty(FormServicio.IdiomaGuia))
            {
                IdiomaEspSelected = true;
                IdiomaIngSelected = false;
                IdiomaFraSelected = false;
                return;
            }

            string lower = FormServicio.IdiomaGuia.ToLower();
            IdiomaEspSelected = lower.Contains("esp");
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
            if (IdiomaEspSelected) lista.Add("Español");
            if (IdiomaIngSelected) lista.Add("Inglés");
            if (IdiomaFraSelected) lista.Add("Francés");
            return lista.Any() ? string.Join(", ", lista) : "Seleccionar idiomas...";
        }

        // Elimina un servicio del listado en memoria
        public void EliminarServicio(ServicioItem servicio)
        {
            if (servicio == null) return;
            ListadoServicios.Remove(servicio);
        }

        private string PickRandomIcon()
        {
            var rand = new Random();
            int index = rand.Next(IconosDisponibles.Length);
            return IconosDisponibles[index];
        }

        // Normaliza el texto del destino y devuelve la ruta de la imagen o del fallback por defecto
        public string ObtenerImagenDestinoUrl(string destino)
        {
            if (string.IsNullOrWhiteSpace(destino))
            {
                return "/images/destinos/default.png";
            }

            // Normalización básica: pasar a minúsculas y recortar espacios iniciales/finales
            string normalizado = destino.Trim().ToLower();

            // Reemplazar caracteres especiales y vocales con acento
            normalizado = normalizado.Replace("á", "a")
                                     .Replace("é", "e")
                                     .Replace("í", "i")
                                     .Replace("ó", "o")
                                     .Replace("ú", "u")
                                     .Replace("ñ", "n");

            // Reemplazar secuencias de espacios en blanco por un guion único
            normalizado = System.Text.RegularExpressions.Regex.Replace(normalizado, @"\s+", "-");

            // Validar destinos admitidos con assets específicos (lima, cusco, paracas)
            if (normalizado == "lima" || normalizado == "cusco" || normalizado == "paracas" || normalizado == "ica")
            {
                return $"/images/destinos/{normalizado}.png";
            }

            // Fallback por defecto si no es uno de los destinos principales
            return "/images/destinos/default.png";
        }
    }

    public class ServicioItem
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Duracion { get; set; }
        public int CapacidadMaxima { get; set; }
        public string IdiomaGuia { get; set; }
        public string CiudadDestino { get; set; }
        public string IncluyeRecojo { get; set; } // "Sí" o "No"
        public string Icono { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace ProyectoProgramacion3Web.Components.Pages.Clientes
{
    public partial class Clientes : ComponentBase
    {
        [Inject]
        private NavigationManager Navigation { get; set; }

        // Listado de clientes en memoria
        public List<ClienteItem> ListadoClientes { get; set; } = new List<ClienteItem>();

        // Estado del Modal CRUD
        public bool IsModalOpen { get; set; }
        public string ModalMode { get; set; } = "Nuevo"; // "Nuevo", "Editar"
        public string ModalTitle => ModalMode == "Nuevo" ? "Nuevo cliente" : "Editar cliente";
        public ClienteItem FormCliente { get; set; } = new ClienteItem();

        protected override void OnInitialized()
        {
            CargarDatosIniciales();
        }

        private void CargarDatosIniciales()
        {
            ListadoClientes = new List<ClienteItem>
            {
                new ClienteItem { Id = 1, Nombres = "Lucía", Apellidos = "Fernández", TipoDocumento = "DNI", NumeroDocumento = "74839201", Nacionalidad = "Perú", FechaNacimiento = new DateTime(1992, 04, 12), Correo = "lucia.fernandez@email.com", Contacto = "+51 987 654 321", FechaRegistro = new DateTime(2026, 01, 15) },
                new ClienteItem { Id = 2, Nombres = "James", Apellidos = "Carter", TipoDocumento = "Pasaporte", NumeroDocumento = "USA928401", Nacionalidad = "Estados Unidos", FechaNacimiento = new DateTime(1985, 11, 05), Correo = "j.carter@email.com", Contacto = "+1 415 888 9900", FechaRegistro = new DateTime(2026, 02, 22) },
                new ClienteItem { Id = 3, Nombres = "Marta", Apellidos = "Ríos", TipoDocumento = "DNI", NumeroDocumento = "45920182", Nacionalidad = "Perú", FechaNacimiento = new DateTime(1990, 08, 24), Correo = "marta.rios@email.com", Contacto = "+51 912 345 678", FechaRegistro = new DateTime(2026, 03, 08) },
                new ClienteItem { Id = 4, Nombres = "Laura", Apellidos = "Benavides", TipoDocumento = "DNI", NumeroDocumento = "48201948", Nacionalidad = "Perú", FechaNacimiento = new DateTime(1993, 05, 14), Correo = "laura.benavides@email.com", Contacto = "+51 933 221 100", FechaRegistro = new DateTime(2026, 04, 01) },
                new ClienteItem { Id = 5, Nombres = "Carlos", Apellidos = "Mendoza", TipoDocumento = "DNI", NumeroDocumento = "70293847", Nacionalidad = "Perú", FechaNacimiento = new DateTime(1991, 02, 28), Correo = "carlos.mendoza@email.com", Contacto = "+51 944 332 211", FechaRegistro = new DateTime(2026, 01, 10) },
                new ClienteItem { Id = 6, Nombres = "Sophie", Apellidos = "Dubois", TipoDocumento = "Pasaporte", NumeroDocumento = "FRA294820", Nacionalidad = "Francia", FechaNacimiento = new DateTime(1994, 07, 19), Correo = "sophie.dubois@email.com", Contacto = "+33 6 1234 5678", FechaRegistro = new DateTime(2026, 02, 18) },
                new ClienteItem { Id = 7, Nombres = "James", Apellidos = "Wilson", TipoDocumento = "Pasaporte", NumeroDocumento = "GBR392049", Nacionalidad = "Reino Unido", FechaNacimiento = new DateTime(1987, 09, 12), Correo = "james.wilson@email.com", Contacto = "+44 20 7946 0192", FechaRegistro = new DateTime(2026, 03, 14) },
                new ClienteItem { Id = 8, Nombres = "Mariana", Apellidos = "Costa", TipoDocumento = "DNI", NumeroDocumento = "46294820", Nacionalidad = "Perú", FechaNacimiento = new DateTime(1989, 03, 22), Correo = "mariana.costa@email.com", Contacto = "+51 955 667 788", FechaRegistro = new DateTime(2026, 04, 25) },
                new ClienteItem { Id = 9, Nombres = "Kenji", Apellidos = "Sato", TipoDocumento = "Pasaporte", NumeroDocumento = "JPN552910", Nacionalidad = "Japón", FechaNacimiento = new DateTime(1992, 10, 08), Correo = "kenji.sato@email.com", Contacto = "+81 90 1234 5678", FechaRegistro = new DateTime(2026, 01, 20) },
                new ClienteItem { Id = 10, Nombres = "Pedro", Apellidos = "Núñez", TipoDocumento = "DNI", NumeroDocumento = "09284712", Nacionalidad = "Perú", FechaNacimiento = new DateTime(1988, 12, 30), Correo = "pedro.nunez@email.com", Contacto = "+51 955 443 322", FechaRegistro = new DateTime(2026, 04, 19) }
            };
        }

        // Control del modal CRUD
        public void AbrirModal(string modo, ClienteItem cliente = null)
        {
            ModalMode = modo;

            if (modo == "Nuevo")
            {
                FormCliente = new ClienteItem
                {
                    TipoDocumento = "DNI",
                    Nacionalidad = "Perú",
                    FechaNacimiento = new DateTime(1995, 01, 01)
                };
            }
            else
            {
                if (cliente == null) return;

                // Clonar objeto para evitar edición directa en vivo sobre la tabla
                FormCliente = new ClienteItem
                {
                    Id = cliente.Id,
                    Nombres = cliente.Nombres,
                    Apellidos = cliente.Apellidos,
                    TipoDocumento = cliente.TipoDocumento,
                    NumeroDocumento = cliente.NumeroDocumento,
                    Nacionalidad = cliente.Nacionalidad,
                    FechaNacimiento = cliente.FechaNacimiento,
                    Correo = cliente.Correo,
                    Contacto = cliente.Contacto,
                    FechaRegistro = cliente.FechaRegistro
                };
            }

            IsModalOpen = true;
        }

        public void CerrarModal()
        {
            IsModalOpen = false;
            FormCliente = new ClienteItem();
        }

        // Guarda o actualiza los datos del cliente
        public void GuardarCliente()
        {
            // Validación mínima para asegurar consistencia
            if (string.IsNullOrWhiteSpace(FormCliente.Nombres) ||
                string.IsNullOrWhiteSpace(FormCliente.Apellidos) ||
                string.IsNullOrWhiteSpace(FormCliente.NumeroDocumento) ||
                string.IsNullOrWhiteSpace(FormCliente.Correo))
            {
                return;
            }

            if (ModalMode == "Nuevo")
            {
                int nuevoId = ListadoClientes.Any() ? ListadoClientes.Max(c => c.Id) + 1 : 1;
                FormCliente.Id = nuevoId;
                FormCliente.FechaRegistro = DateTime.Today;

                ListadoClientes.Add(FormCliente);
            }
            else if (ModalMode == "Editar")
            {
                var existente = ListadoClientes.FirstOrDefault(c => c.Id == FormCliente.Id);
                if (existente != null)
                {
                    existente.Nombres = FormCliente.Nombres;
                    existente.Apellidos = FormCliente.Apellidos;
                    existente.TipoDocumento = FormCliente.TipoDocumento;
                    existente.NumeroDocumento = FormCliente.NumeroDocumento;
                    existente.Nacionalidad = FormCliente.Nacionalidad;
                    existente.FechaNacimiento = FormCliente.FechaNacimiento;
                    existente.Correo = FormCliente.Correo;
                    existente.Contacto = FormCliente.Contacto;
                }
            }

            CerrarModal();
        }

        // Redirecciona al listado de Reservas aplicando filtro del cliente
        public void VerReservas(ClienteItem cliente)
        {
            if (cliente == null) return;
            string fullName = $"{cliente.Nombres} {cliente.Apellidos}";
            Navigation.NavigateTo($"reservas?search={Uri.EscapeDataString(fullName)}");
        }
    }

    public class ClienteItem
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nacionalidad { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Correo { get; set; }
        public string Contacto { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}

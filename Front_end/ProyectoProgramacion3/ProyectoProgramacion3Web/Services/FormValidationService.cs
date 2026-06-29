using System.Net.Mail;
using System.Text.RegularExpressions;
using ProyectoProgramacion3Web.ViewModels;

namespace ProyectoProgramacion3Web.Services;

public static class FormValidationService
{
    public static List<string> ValidarCliente(ClienteItem cliente)
    {
        var errores = new List<string>();

        Requerido(cliente.Nombres, "Los nombres del cliente son obligatorios.", errores);
        Requerido(cliente.Apellidos, "Los apellidos del cliente son obligatorios.", errores);
        ValidarDocumento(cliente.TipoDocumento, cliente.NumeroDocumento, errores);
        Requerido(cliente.Nacionalidad, "La nacionalidad es obligatoria.", errores);
        ValidarFechaNacimiento(cliente.FechaNacimiento, errores);
        ValidarCorreo(cliente.Correo, true, errores);
        ValidarContacto(cliente.Contacto, "El contacto", true, errores);
        ValidarEstado(cliente.Estado, errores);

        return errores;
    }

    public static List<string> ValidarUsuario(UsuarioItem usuario, bool esCreacion)
    {
        var errores = new List<string>();

        Requerido(usuario.Nombres, "Los nombres del usuario son obligatorios.", errores);
        Requerido(usuario.Apellidos, "Los apellidos del usuario son obligatorios.", errores);
        ValidarDocumento(usuario.TipoDocumento, usuario.NumeroDocumento, errores);
        ValidarCorreo(usuario.Correo, true, errores);
        ValidarContacto(usuario.Telefono, "El telefono", true, errores);
        Requerido(usuario.Tipo, "Debe seleccionar un tipo de usuario.", errores);
        ValidarEstado(usuario.Estado, errores);

        if (esCreacion && string.IsNullOrWhiteSpace(usuario.Contrasena))
        {
            errores.Add("La contrasena es obligatoria al crear un usuario.");
        }

        return errores;
    }

    public static List<string> ValidarServicio(ServicioItem servicio)
    {
        var errores = new List<string>();

        Requerido(servicio.Nombre, "El nombre del servicio es obligatorio.", errores);
        Requerido(servicio.Descripcion, "La descripcion del servicio es obligatoria.", errores);
        Requerido(servicio.CiudadDestino, "La ciudad destino es obligatoria.", errores);
        Requerido(servicio.IdiomaGuia, "Debe seleccionar al menos un idioma de guia.", errores);
        ValidarEstado(servicio.Estado, errores);

        if (servicio.Precio <= 0)
        {
            errores.Add("El precio debe ser mayor que cero.");
        }

        if (servicio.Duracion <= 0)
        {
            errores.Add("La duracion debe ser mayor que cero.");
        }

        if (servicio.CapacidadMaxima <= 0)
        {
            errores.Add("La capacidad maxima debe ser mayor que cero.");
        }

        return errores;
    }

    public static List<string> ValidarReclamo(ReclamoItem reclamo, string modo)
    {
        var errores = new List<string>();

        if (modo == "Registrar")
        {
            if (reclamo.IdReserva <= 0)
            {
                errores.Add("El cliente seleccionado debe tener una reserva existente.");
            }

            Requerido(reclamo.CodigoReserva, "El codigo de reserva es obligatorio.", errores);
            Requerido(reclamo.Cliente, "El cliente es obligatorio.", errores);
            Requerido(reclamo.Descripcion, "La descripcion del reclamo es obligatoria.", errores);
        }

        if (modo == "Atender")
        {
            Requerido(reclamo.Estado, "Debe seleccionar el estado del reclamo.", errores);

            if ((reclamo.Estado == "PROCEDE" || reclamo.Estado == "NO_PROCEDE") &&
                string.IsNullOrWhiteSpace(reclamo.MotivoResolucion))
            {
                errores.Add("Debe ingresar el motivo de resolucion para cerrar el reclamo.");
            }
        }

        return errores;
    }

    private static void Requerido(string? valor, string mensaje, List<string> errores)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            errores.Add(mensaje);
        }
    }

    private static void ValidarDocumento(string? tipoDocumento, string? numeroDocumento, List<string> errores)
    {
        if (string.IsNullOrWhiteSpace(numeroDocumento))
        {
            errores.Add("El numero de documento es obligatorio.");
            return;
        }

        var tipo = (tipoDocumento ?? "DNI").Trim().ToUpperInvariant();
        var numero = numeroDocumento.Trim();

        if (tipo == "DNI")
        {
            if (!Regex.IsMatch(numero, "^\\d{8}$"))
            {
                errores.Add("El DNI debe contener exactamente 8 digitos numericos.");
            }
            return;
        }

        if (!Regex.IsMatch(numero, "^[A-Za-z0-9]{6,12}$"))
        {
            errores.Add("El documento debe tener entre 6 y 12 caracteres alfanumericos.");
        }
    }

    private static void ValidarCorreo(string? correo, bool requerido, List<string> errores)
    {
        if (string.IsNullOrWhiteSpace(correo))
        {
            if (requerido)
            {
                errores.Add("El correo electronico es obligatorio.");
            }
            return;
        }

        try
        {
            _ = new MailAddress(correo.Trim());
        }
        catch (FormatException)
        {
            errores.Add("Ingrese un correo electronico valido.");
        }
    }

    private static void ValidarContacto(string? contacto, string etiqueta, bool requerido, List<string> errores)
    {
        if (string.IsNullOrWhiteSpace(contacto))
        {
            if (requerido)
            {
                errores.Add($"{etiqueta} es obligatorio.");
            }
            return;
        }

        var soloDigitos = new string(contacto.Where(char.IsDigit).ToArray());
        if (soloDigitos.Length < 7 || soloDigitos.Length > 15)
        {
            errores.Add($"{etiqueta} debe tener entre 7 y 15 digitos.");
        }
    }

    private static void ValidarFechaNacimiento(DateTime fechaNacimiento, List<string> errores)
    {
        if (fechaNacimiento == default)
        {
            errores.Add("La fecha de nacimiento es obligatoria.");
            return;
        }

        if (fechaNacimiento.Date > DateTime.Today)
        {
            errores.Add("La fecha de nacimiento no puede ser futura.");
        }
    }

    private static void ValidarEstado(string? estado, List<string> errores)
    {
        if (!string.Equals(estado, "Activo", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(estado, "Inactivo", StringComparison.OrdinalIgnoreCase))
        {
            errores.Add("Debe seleccionar un estado valido.");
        }
    }
}

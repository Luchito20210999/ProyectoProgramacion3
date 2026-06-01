using ProyectoProgramacion3Model.Model;

namespace ProyectoProgramacion3Negocio.BO;

public abstract class BaseBO
{
    protected static void ValidarIdPositivo(int id, string nombreCampo)
    {
        if (id <= 0)
        {
            throw new ArgumentException($"El {nombreCampo} debe ser mayor a 0");
        }
    }

    protected static void ValidarEstado(Estado estado)
    {
        if (!Enum.IsDefined(estado))
        {
            throw new ArgumentException("El estado es obligatorio");
        }
    }

    protected static void ValidarTextoObligatorio(string? valor, string nombreCampo)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ArgumentException($"El {nombreCampo} es obligatorio");
        }
    }

    protected static void ValidarDateOnly(DateOnly valor, string nombreCampo)
    {
        if (valor == default)
        {
            throw new ArgumentException($"La {nombreCampo} es obligatoria");
        }
    }

    protected static void ValidarDateTime(DateTime valor, string nombreCampo)
    {
        if (valor == default)
        {
            throw new ArgumentException($"La {nombreCampo} es obligatoria");
        }
    }
}

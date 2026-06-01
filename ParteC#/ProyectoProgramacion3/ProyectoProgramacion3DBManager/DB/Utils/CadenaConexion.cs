using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoProgramacion3DBManager.DB.Utils;

public sealed class CadenaConexion
{
    public string Servidor { get; init; } = string.Empty;
    public string Schema { get; init; } = string.Empty;
    public int Puerto { get; init; }
    public TipoDb TipoDb { get; init; }

    public override string ToString()
    {
        return TipoDb switch
        {
            TipoDb.MySQL => $"Server={Servidor};Port={Puerto};Database={Schema};SslMode=none;AllowPublicKeyRetrieval=True;",
            _ => throw new NotSupportedException($"Tipo de DB no soportado: {TipoDb}")
        };
    }
}

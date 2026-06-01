using ProyectoProgramacion3DBManager.DB.Utils;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace ProyectoProgramacion3DBManager.DB;
public abstract class DbManager
{
    protected string ConnectionStringBase { get; }

    protected DbManager(string connectionStringBase)
    {
        ConnectionStringBase = connectionStringBase;
    }

    public abstract DbConnection GetConnection();

    protected static string? ResolvePassword(string? passwordCifrado)
    {
        if (string.IsNullOrWhiteSpace(passwordCifrado))
        {
            return null;
        }

        try
        {
            return Crypto.Decrypt(passwordCifrado);
        }
        catch (FormatException)
        {
            // Permite usar password en texto plano si no viene cifrado.
            return passwordCifrado;
        }
    }
}

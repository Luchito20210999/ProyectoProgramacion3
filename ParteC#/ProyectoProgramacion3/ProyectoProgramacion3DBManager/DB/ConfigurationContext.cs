using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoProgramacion3DBManager.DB;

public static class ConfigurationContext
{
    private static IConfiguration? _current;

    public static IConfiguration Current => _current ?? throw new InvalidOperationException(
        "La configuracion no esta inicializada. Carguela desde la aplicacion de arranque.");

    public static void Initialize(IConfiguration configuration)
    {
        _current = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }
}
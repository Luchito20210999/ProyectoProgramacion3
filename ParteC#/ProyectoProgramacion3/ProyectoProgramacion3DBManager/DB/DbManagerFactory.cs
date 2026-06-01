using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoProgramacion3DBManager.DB;

public abstract class DbManagerFactory
{
    public abstract DbManager CrearDbManager(string connectionStringBase);
}

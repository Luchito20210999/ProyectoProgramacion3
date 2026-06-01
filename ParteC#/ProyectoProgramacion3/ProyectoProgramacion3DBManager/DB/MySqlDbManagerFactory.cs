using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoProgramacion3DBManager.DB;
public sealed class MySqlDbManagerFactory : DbManagerFactory
{
    public override DbManager CrearDbManager(string connectionStringBase)
    {
        return MySqlDbManager.GetInstance(connectionStringBase);
    }
}

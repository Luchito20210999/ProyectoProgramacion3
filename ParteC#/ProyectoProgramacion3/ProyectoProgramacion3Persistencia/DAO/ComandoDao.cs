using System.Data.Common;


namespace ProyectoProgramacion3Persistencia.DAO;

public delegate T ComandoDao<out T>(DbConnection connection);


package pe.edu.pucp.proyectopro3.db;

public abstract class DBManagerFactory {
    public abstract DBManager crearDBManager(String host, int puerto,
                                             String esquema, String usuario,
                                             String password);
}


package pe.edu.pucp.proyectopro3.db;

public class MySQLDBManagerFactory extends DBManagerFactory {
    @Override
    public DBManager crearDBManager(String host, int puerto, String esquema,
                                    String usuario, String password) {
        return MySQLDBManager.getInstance(host, puerto, esquema, usuario,
                password);
    }
}
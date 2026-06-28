package pe.edu.pucp.proyectopro3.dao;

import java.sql.Connection;
import java.sql.SQLException;

@FunctionalInterface
public interface ComandoDAO<R> {
    R ejecutar(Connection conn) throws SQLException;
}

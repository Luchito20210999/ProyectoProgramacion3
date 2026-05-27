package pe.edu.pucp.proyectopro3.dao;

import pe.edu.pucp.proyectopro3.db.DBFactoryProvider;
import pe.edu.pucp.proyectopro3.db.DBManager;

import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public abstract class BaseDAO<M, I> implements Persistible<M, I> {
    @Override
    public I crear(M modelo) {
        return ejecutarComando(conn -> ejecutarComandoCrear(conn, modelo));
    }

    @Override
    public boolean actualizar(M modelo) {
        return ejecutarComando(conn -> ejecutarComandoActualizar(conn, modelo));
    }

    @Override
    public boolean eliminar(I id) {
        return ejecutarComando(conn -> ejecutarComandoEliminar(conn, id));
    }

    @Override
    public M leer(I id) {
        return ejecutarComando(conn -> {
            try (PreparedStatement cmd = this.comandoLeer(conn, id);
                 ResultSet rs = cmd.executeQuery()) {
                if (!rs.next()) {
                    System.err.println("No se encontro el registro con id: " + id);
                    return null;
                }
                return this.mapearModelo(rs);
            }
        });
    }

    @Override
    public List<M> leerTodos() {
        return ejecutarComando(conn -> {
            try (PreparedStatement cmd = this.comandoLeerTodos(conn);
                 ResultSet rs = cmd.executeQuery()) {
                List<M> modelos = new ArrayList<>();
                while (rs.next()) {
                    modelos.add(this.mapearModelo(rs));
                }
                return modelos;
            }
        });
    }

    protected <R> R ejecutarComando(ComandoDAO<R> command) {
        Connection txConnection = TransactionsManager.obtenerConexionActual();
        if (txConnection != null) {
            return ejecutarComandoConTransaccion(command, txConnection);
        }

        return ejecutarComandoSinTransaccion(command);
    }

    protected <R> R ejecutarComandoConTransaccion(ComandoDAO<R> command,
                                                  Connection txConnection) {
        try {
            return command.ejecutar(txConnection);
        }
        catch (SQLException e) {
            System.err.println("Error SQL: " + e.getMessage());
            throw new RuntimeException(e);
        }
        catch (Exception e) {
            System.err.println("Error inesperado: " + e.getMessage());
            throw new RuntimeException(e);
        }
    }

    protected <R> R ejecutarComandoSinTransaccion(ComandoDAO<R> command) {
        DBManager dbManager = DBFactoryProvider.getManager();
        try (Connection conn = dbManager.getConnection()) {
            return command.ejecutar(conn);
        }
        catch (SQLException e) {
            System.err.println("Error SQL: " + e.getMessage());
            throw new RuntimeException(e);
        }
        catch (Exception e) {
            System.err.println("Error inesperado: " + e.getMessage());
            throw new RuntimeException(e);
        }
    }

    protected I ejecutarComandoCrear(Connection conn, M modelo) {
        try (PreparedStatement cmd = this.comandoCrear(conn, modelo)) {
            if (cmd.executeUpdate() == 0) {
                return null;
            }

            if (cmd instanceof CallableStatement callableCmd) {
                return extraerIdDesdeCallable(callableCmd);
            }

            try (ResultSet rs = cmd.getGeneratedKeys()) {
                if (rs.next()) {
                    return extraerIdDesdeGeneratedKeys(rs);
                }
            }

            return null;
        }
        catch (SQLException e) {
            System.err.println("Error SQL: " + e.getMessage());
            throw new RuntimeException(e);
        }
    }

    protected boolean ejecutarComandoActualizar(Connection conn, M modelo) {
        try (PreparedStatement cmd = this.comandoActualizar(conn, modelo)) {
            return cmd.executeUpdate() > 0;
        }
        catch (SQLException e) {
            System.err.println("Error SQL: " + e.getMessage());
            throw new RuntimeException(e);
        }
    }

    protected boolean ejecutarComandoEliminar(Connection conn, I id) {
        try (PreparedStatement cmd = this.comandoEliminar(conn, id)) {
            return cmd.executeUpdate() > 0;
        }
        catch (SQLException e) {
            System.err.println("Error SQL: " + e.getMessage());
            throw new RuntimeException(e);
        }
    }

    protected abstract PreparedStatement comandoCrear(Connection conn,
                                                      M modelo) throws SQLException;

    protected abstract PreparedStatement comandoActualizar(Connection conn,
                                                           M modelo) throws SQLException;

    protected abstract PreparedStatement comandoEliminar(Connection conn,
                                                         I id) throws SQLException;

    protected abstract PreparedStatement comandoLeer(Connection conn,
                                                     I id) throws SQLException;

    protected abstract PreparedStatement comandoLeerTodos(Connection conn) throws SQLException;


    protected abstract M mapearModelo(ResultSet rs) throws SQLException;


    protected abstract I extraerIdDesdeCallable(CallableStatement cmd) throws SQLException;


    protected abstract I extraerIdDesdeGeneratedKeys(ResultSet rs) throws SQLException;

    protected void setEnteroNullable(PreparedStatement cmd, int index,
                                     Integer value) throws SQLException {
        if (value == null) {
            cmd.setNull(index, java.sql.Types.INTEGER);
        }
        else {
            cmd.setInt(index, value);
        }
    }

    protected Integer leerEnteroNullable(ResultSet rs, String columnName) throws SQLException {
        int value = rs.getInt(columnName);
        return rs.wasNull() ? null : value;
    }
}
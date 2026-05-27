package pe.edu.pucp.proyectopro3.dao;

import java.sql.CallableStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

public abstract class DefaultBaseDAO<M> extends BaseDAO<M, Integer> {
    @Override
    protected Integer extraerIdDesdeGeneratedKeys(ResultSet rs) throws SQLException {
        return rs.getInt(1);
    }

    @Override
    protected Integer extraerIdDesdeCallable(CallableStatement cmd) throws SQLException {
        return cmd.getInt("p_id");
    }
}

package pe.edu.pucp.proyectopro3.dao.webhooks;

import pe.edu.pucp.proyectopro3.db.DBFactoryProvider;
import pe.edu.pucp.proyectopro3.db.DBManager;
import pe.edu.pucp.proyectopro3.dao.TransactionsManager;

import java.sql.CallableStatement;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

public class WebhookBokunDAOImpl implements WebhookBokunDAO {

    @Override
    public void procesarWebhook(String rawJson) {
        Connection txConnection = TransactionsManager.obtenerConexionActual();
        if (txConnection != null) {
            ejecutar(txConnection, rawJson);
        } else {
            DBManager dbManager = DBFactoryProvider.getManager();
            try (Connection conn = dbManager.getConnection()) {
                ejecutar(conn, rawJson);
            } catch (SQLException e) {
                System.err.println("Error SQL al procesar webhook: " + e.getMessage());
                throw new RuntimeException(e);
            } catch (Exception e) {
                System.err.println("Error inesperado al procesar webhook: " + e.getMessage());
                throw new RuntimeException(e);
            }
        }
    }

    private void ejecutar(Connection conn, String rawJson) {
        String sql = "{call SACRSoft.sp_ProcesarYDispersarWebhookBokun(?)}";
        try (CallableStatement cmd = conn.prepareCall(sql)) {
            cmd.setString(1, rawJson);
            cmd.execute();
        } catch (SQLException e) {
            String contexto = obtenerContextoConexion(conn);
            System.err.println("Error SQL ejecutando SP sp_ProcesarYDispersarWebhookBokun: "
                    + e.getMessage() + " | " + contexto);
            throw new RuntimeException(e.getMessage() + " | " + contexto, e);
        }
    }

    private String obtenerContextoConexion(Connection conn) {
        try (Statement st = conn.createStatement();
             ResultSet rs = st.executeQuery(
                     "SELECT CURRENT_USER() AS currentUser, "
                     + "USER() AS sessionUser, DATABASE() AS databaseName")) {
            if (rs.next()) {
                return "currentUser=" + rs.getString("currentUser")
                        + ", sessionUser=" + rs.getString("sessionUser")
                        + ", databaseName=" + rs.getString("databaseName")
                        + ", catalog=" + conn.getCatalog();
            }
        } catch (Exception ex) {
            return "No se pudo obtener contexto de conexión: " + ex.getMessage();
        }

        return "Sin contexto de conexión disponible";
    }
}

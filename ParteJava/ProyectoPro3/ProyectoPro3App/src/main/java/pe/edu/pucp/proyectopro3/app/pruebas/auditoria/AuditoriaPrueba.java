package pe.edu.pucp.proyectopro3.app.pruebas.auditoria;

import pe.edu.pucp.proyectopro3.bo.auditoria.LogAuditoriaBO;
import pe.edu.pucp.proyectopro3.bo.auditoria.LogAuditoriaBOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.auditoria.LogAuditoria;

import java.util.Date;
import java.util.List;

public class AuditoriaPrueba {

    public static void ejecutar() {
        System.out.println("========== PRUEBA: LogAuditoriaBO ==========");
        LogAuditoriaBO auditoriaBO = new LogAuditoriaBOImpl();

        System.out.println("[LISTAR] Consultando logs de auditoria...");
        List<LogAuditoria> logs = auditoriaBO.listar();
        System.out.println("  Total encontrados: " + logs.size());
        for (LogAuditoria l : logs) {
            System.out.println("  > ID: " + l.getIdLogAuditoria()
                    + " | " + l.getAccion()
                    + " | " + l.getDescripcion());
        }

        System.out.println("[INSERTAR] Registrando nuevo log...");
        LogAuditoria nuevo = new LogAuditoria();
        nuevo.setAccion("LOGIN");
        nuevo.setDescripcion("Inicio de sesion de prueba");
        nuevo.setFechaRegistro(new Date());
        nuevo.setOrigenAccion("127.0.0.1");
        auditoriaBO.guardar(nuevo, Estado.Nuevo);
        System.out.println("  Log registrado con ID: " + nuevo.getIdLogAuditoria());

        System.out.println("[ELIMINAR] Intentando eliminar (debe fallar)...");
        try {
            auditoriaBO.eliminar(nuevo.getIdLogAuditoria());
            System.out.println("  ERROR: No deberia haber eliminado!");
        } catch (UnsupportedOperationException e) {
            System.out.println("  Correcto: " + e.getMessage());
        }

        System.out.println("========== FIN: LogAuditoriaBO ==========\n");
    }
}

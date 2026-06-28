package pe.edu.pucp.proyectopro3.app.pruebas.notificaciones;

import pe.edu.pucp.proyectopro3.bo.notificaciones.NotificacionBO;
import pe.edu.pucp.proyectopro3.bo.notificaciones.NotificacionBOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.notificaciones.Notificacion;

import java.util.Date;
import java.util.List;

public class NotificacionPrueba {

    public static void ejecutar() {
        System.out.println("========== PRUEBA: NotificacionBO ==========");
        NotificacionBO notificacionBO = new NotificacionBOImpl();

        System.out.println("[LISTAR] Consultando todas las notificaciones...");
        List<Notificacion> notificaciones = notificacionBO.listar();
        System.out.println("  Total encontradas: " + notificaciones.size());
        for (Notificacion n : notificaciones) {
            System.out.println("  > ID: " + n.getIdNotificacion()
                    + " | " + n.getMensaje());
        }

        System.out.println("[INSERTAR] Creando nueva notificacion...");
        Notificacion nueva = new Notificacion();
        nueva.setMensaje("Notificacion de prueba");
        nueva.setFechaEnvio(new Date());
        nueva.setIdUsuario(1);
        notificacionBO.guardar(nueva, Estado.Nuevo);
        System.out.println("  Notificacion creada con ID: " + nueva.getIdNotificacion());

        System.out.println("[ELIMINAR] Eliminando notificacion...");
        notificacionBO.eliminar(nueva.getIdNotificacion());
        System.out.println("  Eliminada correctamente.");

        System.out.println("========== FIN: NotificacionBO ==========\n");
    }
}

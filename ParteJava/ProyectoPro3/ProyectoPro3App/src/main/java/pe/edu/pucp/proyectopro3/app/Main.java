package pe.edu.pucp.proyectopro3.app;

import pe.edu.pucp.proyectopro3.app.pruebas.auditoria.AuditoriaPrueba;
import pe.edu.pucp.proyectopro3.app.pruebas.auth.UsuarioPrueba;
import pe.edu.pucp.proyectopro3.app.pruebas.crm.ClientePrueba;
import pe.edu.pucp.proyectopro3.app.pruebas.notificaciones.NotificacionPrueba;
import pe.edu.pucp.proyectopro3.app.pruebas.reclamos.ReclamoPrueba;
import pe.edu.pucp.proyectopro3.app.pruebas.reportes.ReportePrueba;
import pe.edu.pucp.proyectopro3.app.pruebas.reservas.ReservaPrueba;
import pe.edu.pucp.proyectopro3.app.pruebas.reservas.ServicioPrueba;

public class Main {

    public static void main(String[] args) {
        System.out.println("==============================================");
        System.out.println("   ProyectoPro3 — Prueba de Capa de Negocio   ");
        System.out.println("==============================================\n");

        //ejecutarPrueba("Usuarios", UsuarioPrueba::ejecutar);
        //ejecutarPrueba("Clientes", ClientePrueba::ejecutar);
        //ejecutarPrueba("Servicios", ServicioPrueba::ejecutar);
        //ejecutarPrueba("Reservas", ReservaPrueba::ejecutar);

        ejecutarPrueba("Reclamos", ReclamoPrueba::ejecutar);
        //ejecutarPrueba("Notificaciones", NotificacionPrueba::ejecutar);
        //ejecutarPrueba("Auditoria", AuditoriaPrueba::ejecutar);
        //ejecutarPrueba("Reportes", ReportePrueba::ejecutar);

        System.out.println("==============================================");
        System.out.println("   Todas las pruebas finalizadas");
        System.out.println("==============================================");
    }

    private static void ejecutarPrueba(String nombre, Runnable prueba) {
        try {
            prueba.run();
        } catch (Exception e) {
            System.err.println(">>> ERROR en prueba [" + nombre + "]: " + e.getMessage());
            e.printStackTrace();
            System.out.println();
        }
    }
}

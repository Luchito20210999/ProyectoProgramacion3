package pe.edu.pucp.proyectopro3.app.pruebas.reclamos;

import pe.edu.pucp.proyectopro3.bo.reclamos.ReclamoBO;
import pe.edu.pucp.proyectopro3.bo.reclamos.ReclamoBOImpl;
import pe.edu.pucp.proyectopro3.modelo.reclamos.EstadoReclamo;
import pe.edu.pucp.proyectopro3.modelo.reclamos.Reclamo;

import java.util.List;

public class ReclamoPrueba {

    public static void ejecutar() {
        System.out.println("========== PRUEBA: ReclamoBO ==========");
        ReclamoBO reclamoBO = new ReclamoBOImpl();

        // 1. LISTAR
        System.out.println("[LISTAR] Consultando todos los reclamos...");
        List<Reclamo> reclamos = reclamoBO.listar();
        System.out.println("  Total encontrados: " + reclamos.size());
        for (Reclamo r : reclamos) {
            System.out.println("  > ID: " + r.getIdReclamo()
                    + " | Estado: " + r.getEstadoReclamo()
                    + " | " + r.getDescripcion());
        }

        // 2. REGISTRAR RECLAMO (método de dominio)
        System.out.println("[REGISTRAR] Registrando nuevo reclamo...");
        Reclamo nuevo = new Reclamo();
        nuevo.setDescripcion("Reclamo de prueba - servicio incompleto");
        // El BO asigna automáticamente: estadoReclamo=PENDIENTE, fechaReclamo=now()

        reclamoBO.registrarReclamo(nuevo, 1); // Asume reserva con ID 1
        System.out.println("  Reclamo registrado con ID: " + nuevo.getIdReclamo());
        System.out.println("  Estado asignado: " + nuevo.getEstadoReclamo());

        // 3. CONSULTAR (método de dominio)
        System.out.println("[CONSULTAR] Buscando reclamo ID: " + nuevo.getIdReclamo() + "...");
        Reclamo encontrado = reclamoBO.consultarReclamo(nuevo.getIdReclamo());
        System.out.println("  Encontrado: " + encontrado.getDescripcion()
                + " | Estado: " + encontrado.getEstadoReclamo());

        // 4. ATENDER RECLAMO (método de dominio — máquina de estados)
        System.out.println("[ATENDER] Cambiando a EN_ATENCION...");
        reclamoBO.atenderReclamo(nuevo.getIdReclamo());
        Reclamo atendido = reclamoBO.consultarReclamo(nuevo.getIdReclamo());
        System.out.println("  Nuevo estado: " + atendido.getEstadoReclamo());

        // 5. EVALUAR PROCEDENCIA (método de dominio — máquina de estados)
        System.out.println("[EVALUAR] Evaluando procedencia (procede=true)...");
        atendido.setMotivoResolucion("Reclamo procede, se realizará reembolso");
        reclamoBO.guardar(atendido, pe.edu.pucp.proyectopro3.modelo.Estado.Modificado);
        reclamoBO.evaluarProcedencia(nuevo.getIdReclamo(), true);
        Reclamo evaluado = reclamoBO.consultarReclamo(nuevo.getIdReclamo());
        System.out.println("  Estado final: " + evaluado.getEstadoReclamo());

        // 6. ELIMINAR
        System.out.println("[ELIMINAR] Eliminando reclamo ID: " + nuevo.getIdReclamo() + "...");
        reclamoBO.eliminarReclamo(nuevo.getIdReclamo());
        System.out.println("  Reclamo eliminado correctamente.");

        System.out.println("========== FIN: ReclamoBO ==========\n");
    }
}

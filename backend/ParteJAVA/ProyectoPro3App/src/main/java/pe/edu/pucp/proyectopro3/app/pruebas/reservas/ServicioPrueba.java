package pe.edu.pucp.proyectopro3.app.pruebas.reservas;

import pe.edu.pucp.proyectopro3.bo.reservas.ServicioBO;
import pe.edu.pucp.proyectopro3.bo.reservas.ServicioBOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.reservas.Servicio;

import java.util.List;

public class ServicioPrueba {

    public static void ejecutar() {
        System.out.println("========== PRUEBA: ServicioBO ==========");
        ServicioBO servicioBO = new ServicioBOImpl();

        // 1. LISTAR
        System.out.println("[LISTAR] Consultando todos los servicios...");
        List<Servicio> servicios = servicioBO.listar();
        System.out.println("  Total encontrados: " + servicios.size());
        for (Servicio s : servicios) {
            System.out.println("  > ID: " + s.getIdServicio()
                    + " | " + s.getNombre()
                    + " | USD " + s.getPrecioUSD());
        }

        // 2. INSERTAR
        System.out.println("[INSERTAR] Creando nuevo servicio...");
        Servicio nuevo = new Servicio();
        nuevo.setNombre("Tour Cusco Test");
        nuevo.setDescripcion("Tour de prueba a Cusco");
        nuevo.setPrecioUSD(150.00);
        nuevo.setDuracionHoras(8.0);
        nuevo.setIdiomaGuia("Español");
        nuevo.setCapacidadMaxima(20);
        nuevo.setIncluyeRecojo(true);
        nuevo.setCiudadDestino("Cusco");

        servicioBO.guardar(nuevo, Estado.Nuevo);
        System.out.println("  Servicio creado con ID: " + nuevo.getIdServicio());

        // 3. OBTENER
        System.out.println("[OBTENER] Buscando servicio ID: " + nuevo.getIdServicio() + "...");
        Servicio encontrado = servicioBO.obtener(nuevo.getIdServicio());
        if (encontrado != null) {
            System.out.println("  Encontrado: " + encontrado.getNombre()
                    + " | USD " + encontrado.getPrecioUSD());
        } else {
            System.out.println("  No encontrado.");
        }

        // 4. MODIFICAR
        System.out.println("[MODIFICAR] Actualizando servicio...");
        nuevo.setPrecioUSD(175.50);
        nuevo.setDescripcion("Tour de prueba actualizado");
        servicioBO.guardar(nuevo, Estado.Modificado);
        System.out.println("  Servicio actualizado correctamente.");

        // 5. ELIMINAR
        System.out.println("[ELIMINAR] Eliminando servicio ID: " + nuevo.getIdServicio() + "...");
        servicioBO.eliminar(nuevo.getIdServicio());
        System.out.println("  Servicio eliminado correctamente.");

        System.out.println("========== FIN: ServicioBO ==========\n");
    }
}

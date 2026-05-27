package pe.edu.pucp.proyectopro3.app.pruebas.crm;

import pe.edu.pucp.proyectopro3.bo.crm.ClienteBO;
import pe.edu.pucp.proyectopro3.bo.crm.ClienteBOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.crm.Cliente;
import pe.edu.pucp.proyectopro3.modelo.crm.TipoDocumento;

import java.util.Date;
import java.util.List;

public class ClientePrueba {

    public static void ejecutar() {
        System.out.println("========== PRUEBA: ClienteBO ==========");
        ClienteBO clienteBO = new ClienteBOImpl();

        // 1. LISTAR
        System.out.println("[LISTAR] Consultando todos los clientes...");
        List<Cliente> clientes = clienteBO.listar();
        System.out.println("  Total encontrados: " + clientes.size());
        for (Cliente c : clientes) {
            System.out.println("  > ID: " + c.getIdCliente()
                    + " | " + c.getNombres() + " " + c.getApellidos()
                    + " | Doc: " + c.getNumeroDocumento());
        }

        // 2. INSERTAR
        System.out.println("[INSERTAR] Creando nuevo cliente...");
        Cliente nuevo = new Cliente();
        nuevo.setNombres("Juan");
        nuevo.setApellidos("Pérez Test");
        nuevo.setTipoDocumento(TipoDocumento.DNI);
        nuevo.setNumeroDocumento("12345678");
        nuevo.setCorreo("juan.perez@test.com");
        nuevo.setNacionalidad("Peruana");
        nuevo.setFechaRegistro(new Date());
        nuevo.setNumeroContacto("999888777");
        nuevo.setFechaNacimiento(new Date());

        clienteBO.guardar(nuevo, Estado.Nuevo);
        System.out.println("  Cliente creado con ID: " + nuevo.getIdCliente());

        // 3. OBTENER
        System.out.println("[OBTENER] Buscando cliente ID: " + nuevo.getIdCliente() + "...");
        Cliente encontrado = clienteBO.obtener(nuevo.getIdCliente());
        if (encontrado != null) {
            System.out.println("  Encontrado: " + encontrado.getNombres() + " " + encontrado.getApellidos());
        } else {
            System.out.println("  No encontrado.");
        }

        // 4. MODIFICAR
        System.out.println("[MODIFICAR] Actualizando cliente...");
        nuevo.setApellidos("Pérez Modificado");
        clienteBO.guardar(nuevo, Estado.Modificado);
        System.out.println("  Cliente actualizado correctamente.");

        // 5. ELIMINAR
        System.out.println("[ELIMINAR] Eliminando cliente ID: " + nuevo.getIdCliente() + "...");
        clienteBO.eliminar(nuevo.getIdCliente());
        System.out.println("  Cliente eliminado correctamente.");

        System.out.println("========== FIN: ClienteBO ==========\n");
    }
}

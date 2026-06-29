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

        // Para evitar duplicados en cada ejecución
        String sufijo = String.valueOf(System.currentTimeMillis()).substring(5);

        Cliente nuevo = new Cliente();
        nuevo.setNombres("Juan");
        nuevo.setApellidos("Pérez Test");
        nuevo.setTipoDocumento(TipoDocumento.DNI);
        nuevo.setNumeroDocumento(sufijo);
        nuevo.setCorreo("juan.perez" + sufijo + "@test.com");
        nuevo.setNacionalidad("Peruana");
        nuevo.setFechaRegistro(new Date());
        nuevo.setNumeroContacto("999888777");
        nuevo.setFechaNacimiento(new Date());

        try {
            // 1. LISTAR ANTES
            System.out.println("[1. LISTAR ANTES]");
            List<Cliente> clientesAntes = clienteBO.listar();
            System.out.println("  Total antes: " + clientesAntes.size());

            // 2. INSERTAR
            System.out.println("[2. INSERTAR]");
            clienteBO.guardar(nuevo, Estado.Nuevo);

            if (nuevo.getIdCliente() <= 0) {
                throw new RuntimeException("No se asignó un ID válido al cliente creado.");
            }

            System.out.println("  Cliente creado con ID: " + nuevo.getIdCliente());
            System.out.println("  Documento generado: " + nuevo.getNumeroDocumento());

            // 3. OBTENER
            System.out.println("[3. OBTENER]");
            Cliente encontrado = clienteBO.obtener(nuevo.getIdCliente());

            if (encontrado == null) {
                throw new RuntimeException("No se encontró el cliente recién creado.");
            }

            if (!nuevo.getNumeroDocumento().equals(encontrado.getNumeroDocumento())) {
                throw new RuntimeException("El número de documento obtenido no coincide.");
            }

            System.out.println("  Encontrado: "
                    + encontrado.getNombres() + " "
                    + encontrado.getApellidos()
                    + " | Doc: " + encontrado.getNumeroDocumento());

            // 4. MODIFICAR
            System.out.println("[4. MODIFICAR]");
            nuevo.setApellidos("Pérez Modificado");
            clienteBO.guardar(nuevo, Estado.Modificado);

            Cliente modificado = clienteBO.obtener(nuevo.getIdCliente());

            if (modificado == null) {
                throw new RuntimeException("No se encontró el cliente luego de modificar.");
            }

            if (!"Pérez Modificado".equals(modificado.getApellidos())) {
                throw new RuntimeException("El apellido no se actualizó correctamente.");
            }

            System.out.println("  Cliente actualizado correctamente.");
            System.out.println("  Nuevo apellido: " + modificado.getApellidos());

            // 5. LISTAR DESPUÉS DE INSERTAR/MODIFICAR
            System.out.println("[5. LISTAR DESPUÉS]");
            List<Cliente> clientesDespues = clienteBO.listar();
            System.out.println("  Total después: " + clientesDespues.size());

            // 6. ELIMINAR
            System.out.println("[6. ELIMINAR]");
            clienteBO.eliminar(nuevo.getIdCliente());
            System.out.println("  Cliente eliminado correctamente.");

            // 7. VERIFICAR ELIMINACIÓN
            System.out.println("[7. VERIFICAR ELIMINACIÓN]");
            Cliente eliminado = clienteBO.obtener(nuevo.getIdCliente());

            if (eliminado != null) {
                throw new RuntimeException("El cliente todavía existe luego de eliminar.");
            }

            System.out.println("  Verificación correcta: el cliente ya no existe.");

            System.out.println("========== PRUEBA EXITOSA: ClienteBO ==========\n");

        } catch (Exception e) {
            System.err.println("========== PRUEBA FALLIDA: ClienteBO ==========");
            System.err.println("Motivo: " + e.getMessage());
            e.printStackTrace();
            System.err.println("===============================================\n");
        }
    }
}
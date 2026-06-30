package pe.edu.pucp.proyectopro3.app.pruebas.reservas;

import pe.edu.pucp.proyectopro3.bo.reservas.ReservaBO;
import pe.edu.pucp.proyectopro3.bo.reservas.ReservaBOImpl;
import pe.edu.pucp.proyectopro3.bo.reservas.ServicioBO;
import pe.edu.pucp.proyectopro3.bo.reservas.ServicioBOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.reservas.DetalleReserva;
import pe.edu.pucp.proyectopro3.modelo.reservas.EstadoReserva;
import pe.edu.pucp.proyectopro3.modelo.reservas.Reserva;
import pe.edu.pucp.proyectopro3.modelo.reservas.Servicio;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;

public class ReservaPrueba {

    public static void ejecutar() {
        System.out.println("========== PRUEBA: ReservaBO ==========");
        ReservaBO reservaBO = new ReservaBOImpl();

        // 1. LISTAR
        System.out.println("[LISTAR] Consultando todas las reservas...");
        List<Reserva> reservas = reservaBO.listar();
        System.out.println("  Total encontradas: " + reservas.size());
        for (Reserva r : reservas) {
            System.out.println("  > ID: " + r.getIdReserva()
                    + " | Estado: " + r.getEstadoReserva()
                    + " | Monto: " + r.getMontoTotal());
        }

        // 2. INSERTAR (requiere un cliente y servicio existentes)
        System.out.println("[INSERTAR] Creando nueva reserva...");
        Reserva nueva = new Reserva();
        nueva.setFechaRegistro(new Date());
        nueva.setEstadoReserva(EstadoReserva.PENDIENTE);
        nueva.setCantidadBoletos(2);
        nueva.setMontoTotal(300.00);
        nueva.setFechaUltimaModificacion(new Date());
        nueva.setCanalVenta("WEB");
        nueva.setMontoImpuestos(54.00);
        nueva.setIdCliente(1); // Asume que existe un cliente con ID 1

        // Detalle
        DetalleReserva detalle = new DetalleReserva();
        detalle.setIdServicio(1); // Asume que existe un servicio con ID 1
        detalle.setCantidad(2);
        detalle.setSubtotal(300.00);

        List<DetalleReserva> detalles = new ArrayList<>();
        detalles.add(detalle);
        nueva.setDetalles(detalles);

        reservaBO.guardar(nueva, Estado.Nuevo);
        System.out.println("  Reserva creada con ID: " + nueva.getIdReserva());

        // 3. CONSULTAR (método de dominio)
        System.out.println("[CONSULTAR] Buscando reserva ID: " + nueva.getIdReserva() + "...");
        Reserva encontrada = reservaBO.consultarReserva(nueva.getIdReserva());
        System.out.println("  Encontrada: Estado=" + encontrada.getEstadoReserva()
                + " | Monto=" + encontrada.getMontoTotal());

        // 4. MODIFICAR RESERVA (método de dominio)
        System.out.println("[MODIFICAR] Modificando reserva...");
        reservaBO.modificarReserva(nueva.getIdReserva());
        System.out.println("  Reserva modificada correctamente.");

        // 5. ELIMINAR
        System.out.println("[ELIMINAR] Eliminando reserva ID: " + nueva.getIdReserva() + "...");
        reservaBO.eliminar(nueva.getIdReserva());
        System.out.println("  Reserva eliminada correctamente.");

        System.out.println("========== FIN: ReservaBO ==========\n");
    }
}

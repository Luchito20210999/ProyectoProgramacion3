package pe.edu.pucp.proyectopro3.app.pruebas.reservas;

import pe.edu.pucp.proyectopro3.bo.auth.UsuarioBO;
import pe.edu.pucp.proyectopro3.bo.auth.UsuarioBOImpl;
import pe.edu.pucp.proyectopro3.bo.crm.ClienteBO;
import pe.edu.pucp.proyectopro3.bo.crm.ClienteBOImpl;
import pe.edu.pucp.proyectopro3.bo.reservas.ReservaBO;
import pe.edu.pucp.proyectopro3.bo.reservas.ReservaBOImpl;
import pe.edu.pucp.proyectopro3.bo.reservas.ServicioBO;
import pe.edu.pucp.proyectopro3.bo.reservas.ServicioBOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.auth.Operador;
import pe.edu.pucp.proyectopro3.modelo.auth.Usuario;
import pe.edu.pucp.proyectopro3.modelo.crm.Cliente;
import pe.edu.pucp.proyectopro3.modelo.crm.TipoDocumento;
import pe.edu.pucp.proyectopro3.modelo.reservas.DetalleReserva;
import pe.edu.pucp.proyectopro3.modelo.reservas.EstadoReserva;
import pe.edu.pucp.proyectopro3.modelo.reservas.Reserva;
import pe.edu.pucp.proyectopro3.modelo.reservas.Servicio;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;

public class ReservaIntegradaPrueba {
    private static final String CANAL_PRUEBA = "WEB";
    private static final double TASA_IMPUESTO = 0.18;

    public static void main(String[] args) {
        ejecutar();
    }

    public static void ejecutar() {
        System.out.println("========== PRUEBA BO: Usuario + Cliente + Servicio + Reserva ==========");

        UsuarioBO usuarioBO = new UsuarioBOImpl();
        ClienteBO clienteBO = new ClienteBOImpl();
        ServicioBO servicioBO = new ServicioBOImpl();
        ReservaBO reservaBO = new ReservaBOImpl();
        DatosPrueba datos = new DatosPrueba();

        Usuario usuario = null;
        Cliente cliente = null;
        Reserva reserva = null;
        Servicio servicioTemporal = null;

        try {
            cliente = crearCliente(datos);
            clienteBO.guardar(cliente, Estado.Nuevo);

            usuario = crearOperador(datos);
            usuarioBO.guardar(usuario, Estado.Nuevo);

            Servicio servicio = obtenerPrimerServicioDisponible(servicioBO);
            boolean servicioExistente = servicio != null;
            if (!servicioExistente) {
                servicioTemporal = crearServicioTemporal(datos);
                servicioBO.guardar(servicioTemporal, Estado.Nuevo);
                servicio = servicioTemporal;
            }

            reserva = crearReserva(usuario, cliente, servicio);
            reservaBO.guardar(reserva, Estado.Nuevo);

            Reserva reservaGuardada = reservaBO.obtener(reserva.getIdReserva());
            imprimirReporte(usuario, cliente, servicio, reservaGuardada, reserva.getDetalles(), servicioExistente);
        } catch (Exception ex) {
            System.err.println(">>> PRUEBA FALLIDA: " + ex.getMessage());
            ex.printStackTrace();
        } finally {
            limpiarData(reservaBO, servicioBO, clienteBO, usuarioBO, reserva, servicioTemporal, cliente, usuario);
            imprimirEstadoTablas(reservaBO, usuarioBO, clienteBO, servicioBO);
        }

        System.out.println("========== FIN PRUEBA INTEGRADA ==========\n");
    }

    private static Usuario crearOperador(DatosPrueba datos) {
        return new Operador(
                0,
                "Operador",
                "Reserva " + datos.sufijoCorto,
                TipoDocumento.DNI,
                datos.documentoUsuario,
                datos.telefonoUsuario,
                "op" + datos.sufijoCorto + "@t.com",
                "password" + datos.sufijoCorto
        );
    }

    private static Cliente crearCliente(DatosPrueba datos) {
        Cliente cliente = new Cliente();
        cliente.setNombres("Cliente");
        cliente.setApellidos("Reserva " + datos.sufijoCorto);
        cliente.setTipoDocumento(TipoDocumento.DNI);
        cliente.setNumeroDocumento(datos.documentoCliente);
        cliente.setCorreo("cli" + datos.sufijoCorto + "@test.com");
        cliente.setNacionalidad("Peruana");
        cliente.setFechaRegistro(datos.fechaPrueba);
        cliente.setNumeroContacto(datos.telefonoCliente);
        cliente.setFechaNacimiento(datos.fechaPrueba);
        return cliente;
    }

    private static Servicio obtenerPrimerServicioDisponible(ServicioBO servicioBO) {
        List<Servicio> servicios = servicioBO.listar();
        return servicios.isEmpty() ? null : servicios.get(0);
    }

    private static Servicio crearServicioTemporal(DatosPrueba datos) {
        Servicio servicio = new Servicio();
        servicio.setNombre("Servicio " + datos.sufijoCorto);
        servicio.setDescripcion("Servicio temporal para reserva integrada");
        servicio.setPrecioUSD(120.00);
        servicio.setDuracionHoras(3.0);
        servicio.setIdiomaGuia("ESP");
        servicio.setCapacidadMaxima(10);
        servicio.setIncluyeRecojo(false);
        servicio.setCiudadDestino("Lima");
        return servicio;
    }

    private static Reserva crearReserva(Usuario usuario, Cliente cliente, Servicio servicio) {
        int cantidad = calcularCantidadBoletos(servicio);
        double montoTotal = servicio.getPrecioUSD() * cantidad;

        Reserva reserva = new Reserva();
        reserva.setFechaRegistro(new Date());
        reserva.setEstadoReserva(EstadoReserva.PENDIENTE);
        reserva.setCantidadBoletos(cantidad);
        reserva.setMontoTotal(montoTotal);
        reserva.setFechaUltimaModificacion(new Date());
        reserva.setCanalVenta(CANAL_PRUEBA);
        reserva.setMontoImpuestos(redondear(montoTotal * TASA_IMPUESTO));
        reserva.setCodigoBokun("TEST-" + System.currentTimeMillis());
        reserva.setIdUsuario(usuario.getIdUsuario());
        reserva.setIdCliente(cliente.getIdCliente());

        DetalleReserva detalle = new DetalleReserva();
        detalle.setIdServicio(servicio.getIdServicio());
        detalle.setCantidad(cantidad);
        detalle.setSubtotal(montoTotal);

        List<DetalleReserva> detalles = new ArrayList<>();
        detalles.add(detalle);
        reserva.setDetalles(detalles);

        return reserva;
    }

    private static int calcularCantidadBoletos(Servicio servicio) {
        int capacidad = servicio.getCapacidadMaxima();
        if (capacidad <= 0) {
            return 1;
        }
        return Math.min(2, capacidad);
    }

    private static double redondear(double valor) {
        return Math.round(valor * 100.0) / 100.0;
    }

    private static void imprimirReporte(Usuario usuario, Cliente cliente, Servicio servicio,
                                        Reserva reserva, List<DetalleReserva> detalles,
                                        boolean servicioExistente) {
        System.out.println("[REPORTE]");
        System.out.println("  Cliente creado con ClienteBO:");
        System.out.println("    ID generado: " + cliente.getIdCliente());
        System.out.println("    Nombre: " + cliente.getNombres() + " " + cliente.getApellidos());
        System.out.println("    Documento: " + cliente.getNumeroDocumento());

        System.out.println("  Usuario operador creado con UsuarioBO:");
        System.out.println("    ID generado: " + usuario.getIdUsuario());
        System.out.println("    Nombre: " + usuario.getNombres() + " " + usuario.getApellidos());
        System.out.println("    Correo: " + usuario.getCorreo());

        System.out.println("  Servicio relacionado:");
        System.out.println("    ID usado: " + servicio.getIdServicio());
        System.out.println("    Origen: " + (servicioExistente ? "existente en BD" : "temporal creado por la prueba"));
        System.out.println("    Nombre: " + servicio.getNombre());
        System.out.println("    Precio USD: " + servicio.getPrecioUSD());

        System.out.println("  Reserva creada con ReservaBO:");
        System.out.println("    ID generado: " + reserva.getIdReserva());
        System.out.println("    Estado: " + reserva.getEstadoReserva());
        System.out.println("    FK id_usuario: " + reserva.getIdUsuario());
        System.out.println("    FK id_cliente: " + reserva.getIdCliente());
        System.out.println("    Cantidad boletos: " + reserva.getCantidadBoletos());
        System.out.println("    Monto total: " + reserva.getMontoTotal());
        System.out.println("    Impuestos: " + reserva.getMontoImpuestos());
        System.out.println("    Codigo Bokun: " + reserva.getCodigoBokun());

        System.out.println("  Detalle persistido por ReservaBO:");
        for (DetalleReserva detalle : detalles) {
            System.out.println("    FK id_servicio: " + detalle.getIdServicio()
                    + " | Cantidad: " + detalle.getCantidad()
                    + " | Subtotal: " + detalle.getSubtotal());
        }
    }

    private static void limpiarData(ReservaBO reservaBO, ServicioBO servicioBO, ClienteBO clienteBO,
                                    UsuarioBO usuarioBO, Reserva reserva, Servicio servicioTemporal,
                                    Cliente cliente, Usuario usuario) {
        System.out.println("[LIMPIEZA]");
        eliminarReserva(reservaBO, reserva);
        eliminarUsuario(usuarioBO, usuario);
        eliminarCliente(clienteBO, cliente);
        eliminarServicioTemporal(servicioBO, servicioTemporal);
    }

    private static void eliminarReserva(ReservaBO reservaBO, Reserva reserva) {
        if (reserva != null && reserva.getIdReserva() > 0) {
            try {
                reservaBO.eliminar(reserva.getIdReserva());
                System.out.println("  Reserva eliminada: " + reserva.getIdReserva());
            } catch (Exception ex) {
                System.err.println("  No se pudo eliminar reserva: " + ex.getMessage());
            }
        }
    }

    private static void eliminarServicioTemporal(ServicioBO servicioBO, Servicio servicioTemporal) {
        if (servicioTemporal != null && servicioTemporal.getIdServicio() > 0) {
            try {
                servicioBO.eliminar(servicioTemporal.getIdServicio());
                System.out.println("  Servicio temporal eliminado: " + servicioTemporal.getIdServicio());
            } catch (Exception ex) {
                System.err.println("  No se pudo eliminar servicio temporal: " + ex.getMessage());
            }
        }
    }

    private static void imprimirEstadoTablas(ReservaBO reservaBO, UsuarioBO usuarioBO,
                                             ClienteBO clienteBO, ServicioBO servicioBO) {
        try {
            System.out.println("[LISTADO FINAL]");
            System.out.println("  Reservas actuales: " + reservaBO.listar().size());
            System.out.println("  Usuarios actuales: " + usuarioBO.listar().size());
            System.out.println("  Clientes actuales: " + clienteBO.listar().size());
            System.out.println("  Servicios actuales: " + servicioBO.listar().size());
        } catch (Exception ex) {
            System.err.println("  No se pudo listar el estado final: " + ex.getMessage());
        }
    }

    private static void eliminarCliente(ClienteBO clienteBO, Cliente cliente) {
        if (cliente != null && cliente.getIdCliente() > 0) {
            try {
                clienteBO.eliminar(cliente.getIdCliente());
                System.out.println("  Cliente eliminado: " + cliente.getIdCliente());
            } catch (Exception ex) {
                System.err.println("  No se pudo eliminar cliente: " + ex.getMessage());
            }
        }
    }

    private static void eliminarUsuario(UsuarioBO usuarioBO, Usuario usuario) {
        if (usuario != null && usuario.getIdUsuario() > 0) {
            try {
                usuarioBO.eliminar(usuario.getIdUsuario());
                System.out.println("  Usuario eliminado: " + usuario.getIdUsuario());
            } catch (Exception ex) {
                System.err.println("  No se pudo eliminar usuario: " + ex.getMessage());
            }
        }
    }

    private static class DatosPrueba {
        private final Date fechaPrueba = new Date();
        private final String sufijoLargo = String.valueOf(System.currentTimeMillis());
        private final String sufijoCorto = sufijoLargo.substring(Math.max(0, sufijoLargo.length() - 8));
        private final String documentoUsuario = "7" + sufijoCorto;
        private final String documentoCliente = "8" + sufijoCorto;
        private final String telefonoUsuario = "9" + sufijoCorto;
        private final String telefonoCliente = "8" + sufijoCorto;
    }
}

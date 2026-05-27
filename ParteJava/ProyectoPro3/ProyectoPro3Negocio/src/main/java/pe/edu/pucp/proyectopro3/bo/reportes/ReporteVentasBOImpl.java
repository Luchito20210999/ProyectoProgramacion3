package pe.edu.pucp.proyectopro3.bo.reportes;

import pe.edu.pucp.proyectopro3.bo.BaseBO;
import pe.edu.pucp.proyectopro3.dao.reportes.ReporteVentasDAO;
import pe.edu.pucp.proyectopro3.dao.reportes.ReporteVentasDAOImpl;
import pe.edu.pucp.proyectopro3.dao.reservas.ReservaDAO;
import pe.edu.pucp.proyectopro3.dao.reservas.ReservaDAOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.reportes.Reporte;
import pe.edu.pucp.proyectopro3.modelo.reportes.ReporteVentas;
import pe.edu.pucp.proyectopro3.modelo.reservas.Reserva;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Objects;

public class ReporteVentasBOImpl extends BaseBO implements ReporteVentasBO {
    private final ReporteVentasDAO reporteVentasDao;
    private final ReservaDAO reservaDao;

    public ReporteVentasBOImpl() {
        this.reporteVentasDao = new ReporteVentasDAOImpl();
        this.reservaDao = new ReservaDAOImpl();
    }

    // ====================================================================
    // MÉTODOS CRUD HEREDADOS (Gestionable)
    // ====================================================================

    @Override
    public List<ReporteVentas> listar() {
        return this.reporteVentasDao.leerTodos();
    }

    @Override
    public ReporteVentas obtener(int id) {
        validarIdPositivo(id, "id del reporte de ventas");
        return this.reporteVentasDao.leer(id);
    }

    @Override
    public void eliminar(int id) {
        validarIdPositivo(id, "id del reporte de ventas");
        if (!this.reporteVentasDao.eliminar(id)) {
            throw new IllegalStateException("No se pudo eliminar el reporte de ventas con id: " + id);
        }
    }

    @Override
    public void guardar(ReporteVentas modelo, Estado estado) {
        validarReporteVentas(modelo);
        validarEstado(estado);

        if (estado == Estado.Nuevo) {
            int id = this.reporteVentasDao.crear(modelo);
            if (id <= 0) {
                throw new IllegalStateException("No se pudo registrar el reporte de ventas");
            }
            modelo.setIdReporte(id);
        } else if (estado == Estado.Modificado) {
            validarIdPositivo(modelo.getIdReporte(), "id del reporte de ventas");
            if (!this.reporteVentasDao.actualizar(modelo)) {
                throw new IllegalStateException("No se pudo actualizar el reporte de ventas con id: " + modelo.getIdReporte());
            }
        }
    }

    // ====================================================================
    // MÉTODOS DE DOMINIO (Consultable)
    // ====================================================================

    @Override
    public Reporte generarReporte(Date fechaInicio, Date fechaFin) {
        Objects.requireNonNull(fechaInicio, "La fecha de inicio es obligatoria");
        Objects.requireNonNull(fechaFin, "La fecha de fin es obligatoria");

        if (fechaInicio.after(fechaFin)) {
            throw new IllegalArgumentException("La fecha de inicio no puede ser posterior a la fecha de fin");
        }

        // 1. Consultamos todas las reservas y filtramos por rango de fechas
        List<Reserva> todasLasReservas = this.reservaDao.leerTodos();
        List<Reserva> reservasFiltradas = new ArrayList<>();

        for (Reserva r : todasLasReservas) {
            if (r.getFechaRegistro() != null
                    && !r.getFechaRegistro().before(fechaInicio)
                    && !r.getFechaRegistro().after(fechaFin)) {
                reservasFiltradas.add(r);
            }
        }

        // 2. Calculamos los KPIs reales
        int totalVentas = reservasFiltradas.size();
        double montoTotal = 0;
        for (Reserva r : reservasFiltradas) {
            montoTotal += r.getMontoTotal();
        }

        // 3. Construimos el reporte con datos reales
        ReporteVentas reporte = new ReporteVentas();
        reporte.setFechaGeneracion(new Date());
        reporte.setFechaInicioFiltro(fechaInicio);
        reporte.setFechaFinFiltro(fechaFin);
        reporte.setDetalleVentas(reservasFiltradas);
        reporte.setTotalVentas(totalVentas);
        reporte.setMontoTotalGenerado(montoTotal);

        // 4. Persistimos el reporte generado
        guardar(reporte, Estado.Nuevo);

        return reporte;
    }

    @Override
    public void exportarDashboard() {
        throw new UnsupportedOperationException("Funcionalidad de exportación de dashboard pendiente de implementación.");
    }

    // ====================================================================
    // VALIDACIONES PRIVADAS
    // ====================================================================

    private void validarReporteVentas(ReporteVentas modelo) {
        Objects.requireNonNull(modelo, "El objeto ReporteVentas no puede ser nulo");

        if (modelo.getFechaGeneracion() == null) {
            throw new IllegalArgumentException("La fecha de generación es obligatoria");
        }

        if (modelo.getTotalVentas() < 0) {
            throw new IllegalArgumentException("El total de ventas no puede ser negativo");
        }

        if (modelo.getMontoTotalGenerado() < 0) {
            throw new IllegalArgumentException("El monto total generado no puede ser negativo");
        }
    }
}

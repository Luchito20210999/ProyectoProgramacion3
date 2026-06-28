package pe.edu.pucp.proyectopro3.bo.reportes;

import pe.edu.pucp.proyectopro3.bo.BaseBO;
import pe.edu.pucp.proyectopro3.dao.reclamos.ReclamoDAO;
import pe.edu.pucp.proyectopro3.dao.reclamos.ReclamoDAOImpl;
import pe.edu.pucp.proyectopro3.dao.reportes.ReporteReclamoDAO;
import pe.edu.pucp.proyectopro3.dao.reportes.ReporteReclamoDAOImpl;
import pe.edu.pucp.proyectopro3.dao.reservas.ReservaDAO;
import pe.edu.pucp.proyectopro3.dao.reservas.ReservaDAOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.reclamos.EstadoReclamo;
import pe.edu.pucp.proyectopro3.modelo.reclamos.Reclamo;
import pe.edu.pucp.proyectopro3.modelo.reportes.Reporte;
import pe.edu.pucp.proyectopro3.modelo.reportes.ReporteReclamo;
import pe.edu.pucp.proyectopro3.modelo.reservas.Reserva;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Objects;

public class ReporteReclamoBOImpl extends BaseBO implements ReporteReclamoBO {
    private final ReporteReclamoDAO reporteReclamoDao;
    private final ReclamoDAO reclamoDao;
    private final ReservaDAO reservaDao;

    public ReporteReclamoBOImpl() {
        this.reporteReclamoDao = new ReporteReclamoDAOImpl();
        this.reclamoDao = new ReclamoDAOImpl();
        this.reservaDao = new ReservaDAOImpl();
    }

    // ====================================================================
    // MÉTODOS CRUD HEREDADOS (Gestionable)
    // ====================================================================

    @Override
    public List<ReporteReclamo> listar() {
        return this.reporteReclamoDao.leerTodos();
    }

    @Override
    public ReporteReclamo obtener(int id) {
        validarIdPositivo(id, "id del reporte de reclamos");
        return this.reporteReclamoDao.leer(id);
    }

    @Override
    public void eliminar(int id) {
        validarIdPositivo(id, "id del reporte de reclamos");
        if (!this.reporteReclamoDao.eliminar(id)) {
            throw new IllegalStateException("No se pudo eliminar el reporte de reclamos con id: " + id);
        }
    }

    @Override
    public void guardar(ReporteReclamo modelo, Estado estado) {
        validarReporteReclamo(modelo);
        validarEstado(estado);

        if (estado == Estado.Nuevo) {
            int id = this.reporteReclamoDao.crear(modelo);
            if (id <= 0) {
                throw new IllegalStateException("No se pudo registrar el reporte de reclamos");
            }
            modelo.setIdReporte(id);
        } else if (estado == Estado.Modificado) {
            validarIdPositivo(modelo.getIdReporte(), "id del reporte de reclamos");
            if (!this.reporteReclamoDao.actualizar(modelo)) {
                throw new IllegalStateException("No se pudo actualizar el reporte de reclamos con id: " + modelo.getIdReporte());
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

        // 1. Consultamos todos los reclamos y filtramos por rango de fechas
        List<Reclamo> todosLosReclamos = this.reclamoDao.leerTodos();
        List<Reclamo> reclamosFiltrados = new ArrayList<>();

        for (Reclamo r : todosLosReclamos) {
            if (r.getFechaReclamo() != null
                    && !r.getFechaReclamo().before(fechaInicio)
                    && !r.getFechaReclamo().after(fechaFin)) {
                reclamosFiltrados.add(r);
            }
        }

        // 2. Contamos reservas del periodo para calcular % de incidencias
        List<Reserva> todasLasReservas = this.reservaDao.leerTodos();
        int cantidadReservas = 0;
        for (Reserva res : todasLasReservas) {
            if (res.getFechaRegistro() != null
                    && !res.getFechaRegistro().before(fechaInicio)
                    && !res.getFechaRegistro().after(fechaFin)) {
                cantidadReservas++;
            }
        }

        // 3. Calculamos KPIs por estado de reclamo
        int totalProcede = 0;
        int totalNoProcede = 0;
        int totalPendientes = 0;

        for (Reclamo r : reclamosFiltrados) {
            switch (r.getEstadoReclamo()) {
                case PROCEDE -> totalProcede++;
                case NO_PROCEDE -> totalNoProcede++;
                case PENDIENTE, EN_ATENCION -> totalPendientes++;
            }
        }

        int cantidadReclamos = reclamosFiltrados.size();
        double porcentajeIncidencias = cantidadReservas > 0
                ? (cantidadReclamos * 100.0) / cantidadReservas
                : 0.0;

        // 4. Construimos el reporte con datos reales
        ReporteReclamo reporte = new ReporteReclamo();
        reporte.setFechaGeneracion(new Date());
        reporte.setFechaInicioFiltro(fechaInicio);
        reporte.setFechaFinFiltro(fechaFin);
        reporte.setDetalleReclamos(reclamosFiltrados);
        reporte.setCantidadReservas(cantidadReservas);
        reporte.setCantidadReclamos(cantidadReclamos);
        reporte.setPorcentajeIncidencias(porcentajeIncidencias);
        reporte.setTotalProcede(totalProcede);
        reporte.setTotalNoProcede(totalNoProcede);
        reporte.setTotalPendientes(totalPendientes);

        // 5. Persistimos el reporte generado
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

    private void validarReporteReclamo(ReporteReclamo modelo) {
        Objects.requireNonNull(modelo, "El objeto ReporteReclamo no puede ser nulo");

        if (modelo.getFechaGeneracion() == null) {
            throw new IllegalArgumentException("La fecha de generación es obligatoria");
        }

        if (modelo.getCantidadReclamos() < 0) {
            throw new IllegalArgumentException("La cantidad de reclamos no puede ser negativa");
        }

        if (modelo.getPorcentajeIncidencias() < 0) {
            throw new IllegalArgumentException("El porcentaje de incidencias no puede ser negativo");
        }

        if (modelo.getTotalProcede() < 0) {
            throw new IllegalArgumentException("El total de reclamos procedentes no puede ser negativo");
        }

        if (modelo.getTotalNoProcede() < 0) {
            throw new IllegalArgumentException("El total de reclamos no procedentes no puede ser negativo");
        }

        if (modelo.getTotalPendientes() < 0) {
            throw new IllegalArgumentException("El total de reclamos pendientes no puede ser negativo");
        }
    }
}

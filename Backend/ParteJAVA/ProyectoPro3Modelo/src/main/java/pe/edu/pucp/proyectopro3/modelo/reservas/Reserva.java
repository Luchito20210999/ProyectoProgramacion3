package pe.edu.pucp.proyectopro3.modelo.reservas;

import java.util.Date;
import java.util.List;

public class Reserva {
    private int idReserva;
    private Date fechaRegistro;
    private EstadoReserva estadoReserva;
    private int cantidadBoletos;
    private double montoTotal;
    private Date fechaUltimaModificacion;
    private String canalVenta;
    private double montoImpuestos;
    private String codigoBokun;
    private int idUsuario;
    private int idCliente;
    private List<DetalleReserva> detalles;

    public Reserva(int idReserva, Date fechaRegistro, EstadoReserva estadoReserva, int cantidadBoletos, double montoTotal, Date fechaUltimaModificacion, String canalVenta, double montoImpuestos, String codigoBokun, int idUsuario, int idCliente, List<DetalleReserva> detalles) {
        this.idReserva = idReserva;
        this.fechaRegistro = fechaRegistro;
        this.estadoReserva = estadoReserva;
        this.cantidadBoletos = cantidadBoletos;
        this.montoTotal = montoTotal;
        this.fechaUltimaModificacion = fechaUltimaModificacion;
        this.canalVenta = canalVenta;
        this.montoImpuestos = montoImpuestos;
        this.codigoBokun = codigoBokun;
        this.idUsuario = idUsuario;
        this.idCliente = idCliente;
        this.detalles = detalles;
    }

    public Reserva() {

    }

    public int getIdReserva() {
        return idReserva;
    }

    public void setIdReserva(int idReserva) {
        this.idReserva = idReserva;
    }

    public Date getFechaRegistro() {
        return fechaRegistro;
    }

    public void setFechaRegistro(Date fechaRegistro) {
        this.fechaRegistro = fechaRegistro;
    }

    public EstadoReserva getEstadoReserva() {
        return estadoReserva;
    }

    public void setEstadoReserva(EstadoReserva estadoReserva) {
        this.estadoReserva = estadoReserva;
    }

    public int getCantidadBoletos() {
        return cantidadBoletos;
    }

    public void setCantidadBoletos(int cantidadBoletos) {
        this.cantidadBoletos = cantidadBoletos;
    }

    public double getMontoTotal() {
        return montoTotal;
    }

    public void setMontoTotal(double montoTotal) {
        this.montoTotal = montoTotal;
    }

    public Date getFechaUltimaModificacion() {
        return fechaUltimaModificacion;
    }

    public void setFechaUltimaModificacion(Date fechaUltimaModificacion) {
        this.fechaUltimaModificacion = fechaUltimaModificacion;
    }

    public String getCanalVenta() {
        return canalVenta;
    }

    public void setCanalVenta(String canalVenta) {
        this.canalVenta = canalVenta;
    }

    public double getMontoImpuestos() {
        return montoImpuestos;
    }

    public void setMontoImpuestos(double montoImpuestos) {
        this.montoImpuestos = montoImpuestos;
    }

    public int getIdUsuario() { return idUsuario; }

    public void setIdUsuario(int idUsuario) {
        this.idUsuario = idUsuario;
    }

    public String getCodigoBokun() { return this.codigoBokun; }

    public void setCodigoBokun(String codigoBokun){ this.codigoBokun = codigoBokun; }

    public int getIdCliente() {
        return idCliente;
    }

    public void setIdCliente(int idCliente) {
        this.idCliente = idCliente;
    }

    public List<DetalleReserva> getDetalles() {
        return detalles;
    }

    public void setDetalles(List<DetalleReserva> detalles) {
        this.detalles = detalles;
    }
}

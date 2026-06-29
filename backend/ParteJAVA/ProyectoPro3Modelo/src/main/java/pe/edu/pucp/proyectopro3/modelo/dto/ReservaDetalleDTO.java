package pe.edu.pucp.proyectopro3.modelo.dto;

import java.util.Date;

public class ReservaDetalleDTO {
    private int idReserva;
    private Date fechaRegistro;
    private String estadoReserva;
    private int cantidadBoletos;
    private double montoTotal;
    private double montoImpuestos;
    private String codigoBokun;
    private String codigoReserva;
    private int idUsuario;
    private int idCliente;
    private String cliente;
    private String clienteTipoDocumento;
    private String clienteNumeroDocumento;
    private String clienteCorreo;
    private String clienteNacionalidad;
    private int idServicio;
    private String servicio;
    private String ciudadDestino;
    private double servicioPrecioUSD;
    private double servicioDuracionHoras;
    private int servicioCapacidadMaxima;
    private String servicioIdiomaGuia;
    private boolean servicioIncluyeRecojo;

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

    public String getEstadoReserva() {
        return estadoReserva;
    }

    public void setEstadoReserva(String estadoReserva) {
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

    public double getMontoImpuestos() {
        return montoImpuestos;
    }

    public void setMontoImpuestos(double montoImpuestos) {
        this.montoImpuestos = montoImpuestos;
    }

    public String getCodigoBokun() {
        return codigoBokun;
    }

    public void setCodigoBokun(String codigoBokun) {
        this.codigoBokun = codigoBokun;
    }

    public String getCodigoReserva() {
        return codigoReserva;
    }

    public void setCodigoReserva(String codigoReserva) {
        this.codigoReserva = codigoReserva;
    }

    public int getIdUsuario() {
        return idUsuario;
    }

    public void setIdUsuario(int idUsuario) {
        this.idUsuario = idUsuario;
    }

    public int getIdCliente() {
        return idCliente;
    }

    public void setIdCliente(int idCliente) {
        this.idCliente = idCliente;
    }

    public String getCliente() {
        return cliente;
    }

    public void setCliente(String cliente) {
        this.cliente = cliente;
    }

    public String getClienteTipoDocumento() {
        return clienteTipoDocumento;
    }

    public void setClienteTipoDocumento(String clienteTipoDocumento) {
        this.clienteTipoDocumento = clienteTipoDocumento;
    }

    public String getClienteNumeroDocumento() {
        return clienteNumeroDocumento;
    }

    public void setClienteNumeroDocumento(String clienteNumeroDocumento) {
        this.clienteNumeroDocumento = clienteNumeroDocumento;
    }

    public String getClienteCorreo() {
        return clienteCorreo;
    }

    public void setClienteCorreo(String clienteCorreo) {
        this.clienteCorreo = clienteCorreo;
    }

    public String getClienteNacionalidad() {
        return clienteNacionalidad;
    }

    public void setClienteNacionalidad(String clienteNacionalidad) {
        this.clienteNacionalidad = clienteNacionalidad;
    }

    public int getIdServicio() {
        return idServicio;
    }

    public void setIdServicio(int idServicio) {
        this.idServicio = idServicio;
    }

    public String getServicio() {
        return servicio;
    }

    public void setServicio(String servicio) {
        this.servicio = servicio;
    }

    public String getCiudadDestino() {
        return ciudadDestino;
    }

    public void setCiudadDestino(String ciudadDestino) {
        this.ciudadDestino = ciudadDestino;
    }

    public double getServicioPrecioUSD() {
        return servicioPrecioUSD;
    }

    public void setServicioPrecioUSD(double servicioPrecioUSD) {
        this.servicioPrecioUSD = servicioPrecioUSD;
    }

    public double getServicioDuracionHoras() {
        return servicioDuracionHoras;
    }

    public void setServicioDuracionHoras(double servicioDuracionHoras) {
        this.servicioDuracionHoras = servicioDuracionHoras;
    }

    public int getServicioCapacidadMaxima() {
        return servicioCapacidadMaxima;
    }

    public void setServicioCapacidadMaxima(int servicioCapacidadMaxima) {
        this.servicioCapacidadMaxima = servicioCapacidadMaxima;
    }

    public String getServicioIdiomaGuia() {
        return servicioIdiomaGuia;
    }

    public void setServicioIdiomaGuia(String servicioIdiomaGuia) {
        this.servicioIdiomaGuia = servicioIdiomaGuia;
    }

    public boolean isServicioIncluyeRecojo() {
        return servicioIncluyeRecojo;
    }

    public void setServicioIncluyeRecojo(boolean servicioIncluyeRecojo) {
        this.servicioIncluyeRecojo = servicioIncluyeRecojo;
    }
}

package pe.edu.pucp.proyectopro3.modelo.reservas;

public class DetalleReserva {
    private int idDetalle;
    private int idReserva;
    private int idServicio;
    private int cantidad;
    private double subtotal;

    public DetalleReserva(int idDetalle, int idReserva, int idServicio, int cantidad, double subtotal) {
        this.idDetalle = idDetalle;
        this.idReserva = idReserva;
        this.idServicio = idServicio;
        this.cantidad = cantidad;
        this.subtotal = subtotal;
    }

    public DetalleReserva() {

    }

    public int getIdDetalle() {
        return idDetalle;
    }

    public void setIdDetalle(int idDetalle) {
        this.idDetalle = idDetalle;
    }

    public int getIdReserva() {
        return idReserva;
    }

    public void setIdReserva(int idReserva) {
        this.idReserva = idReserva;
    }

    public int getIdServicio() {
        return idServicio;
    }

    public void setIdServicio(int idServicio) {
        this.idServicio = idServicio;
    }

    public int getCantidad() {
        return cantidad;
    }

    public void setCantidad(int cantidad) {
        this.cantidad = cantidad;
    }

    public double getSubtotal() {
        return subtotal;
    }

    public void setSubtotal(double subtotal) {
        this.subtotal = subtotal;
    }
}

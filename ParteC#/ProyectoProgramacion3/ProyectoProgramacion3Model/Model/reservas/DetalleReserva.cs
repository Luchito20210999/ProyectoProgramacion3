namespace ProyectoProgramacion3Model.Model.reservas;
public class DetalleReserva
{
    public int idDetalleReserva { get; set; }
    public int idReserva { get; set; }
    public int idServicio { get; set; }
    public int cantidad { get; set; }
    public double subtotal { get; set; }
    public DetalleReserva() { }

    public DetalleReserva(int idDetalleReserva, int idReserva, int idServicio, int cantidad, double subtotal)
    {
        this.idDetalleReserva = idDetalleReserva;
        this.idReserva = idReserva;
        this.idServicio = idServicio;
        this.cantidad = cantidad;
        this.subtotal = subtotal;
    }
}

namespace ProyectoProgramacion3Model.Model.reservas;
public class Reserva
{
    public int idReserva { get; set; }
    public DateTime fechaRegistro { get; set; }
    public EstadoReserva estadoReserva { get; set; }
    public int cantidadBoletos { get; set; }
    public double montoTotal { get; set; }
    public DateTime fechaUltimaModificacion { get; set; }
    public string canalVenta { get; set; } = string.Empty;
    public double montoImpuestos { get; set; }
    public string codigoBokun { get; set; } = string.Empty;
    public int? idUsuario { get; set; }
    public int idCliente { get; set; }
    public List<DetalleReserva> detalles { get; set; } = new List<DetalleReserva>();

    public double montoImpuesto
    {
        get => montoImpuestos;
        set => montoImpuestos = value;
    }

    public Reserva() { }

    public Reserva(int idReserva, DateTime fechaRegistro, EstadoReserva estadoReserva, int cantidadBoletos, double montoTotal, DateTime fechaUltimaModificacion, string canalVenta, double montoImpuestos, string codigoBokun, int? idUsuario, int idCliente, List<DetalleReserva> detalles)
    {
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
}

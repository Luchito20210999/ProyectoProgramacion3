namespace ProyectoProgramacion3Model.Model.crm;
public class Cliente{
    public int idCliente { get; set; }
    public string nombres { get; set; } = string.Empty;
    public string apellidos { get; set; } = string.Empty;
    public TipoDocumento tipoDocumento { get; set; }
    public string numeroDocumento { get; set; } = string.Empty;
    public string nacionalidad { get; set; } = string.Empty;
    public string correo { get; set; } = string.Empty;
    public DateOnly fechaRegistro { get; set; }
    public string numeroContacto { get; set; } = string.Empty;
    public DateOnly fechaNacimiento { get; set; }
    public Cliente(int idCliente, string nombres, string apellidos, TipoDocumento tipoDocumento, string numeroDocumento, string nacionalidad, string correo, DateOnly fechaRegistro, string numeroContacto, DateOnly fechaNacimiento)
    {
        this.idCliente = idCliente;
        this.nombres = nombres;
        this.apellidos = apellidos;
        this.tipoDocumento = tipoDocumento;
        this.numeroDocumento = numeroDocumento;
        this.nacionalidad = nacionalidad;
        this.correo = correo;
        this.fechaRegistro = fechaRegistro;
        this.numeroContacto = numeroContacto;
        this.fechaNacimiento = fechaNacimiento;
    }
    public Cliente()
    {
    }
}
namespace ProyectoProgramacion3Model.Model.notificaciones;
public class Notificacion{
    public int idNotificacion {  get; set; }
    public string mensaje { get; set; } = string.Empty;
    public string tipoNotificacion { get; set; } = string.Empty;
    public DateOnly fechaEnvio { get; set; }
    public int idUsuario { get; set; }
    public Boolean leido { get; set; }
    public TipoEvento tipoEvento { get; set; }
    public Notificacion(int idNotificacion, string mensaje, string tipoNotificacion, DateOnly fechaEnvio, int idUsuario, Boolean leido, TipoEvento tipoEvento)
    {
        this.idNotificacion = idNotificacion;
        this.mensaje = mensaje;
        this.tipoNotificacion = tipoNotificacion;
        this.fechaEnvio = fechaEnvio;
        this.idUsuario = idUsuario;
        this.leido = leido;
        this.tipoEvento = tipoEvento;
    }
    public Notificacion()
    {
    }
}
namespace ProyectoProgramacion3Model.Model.Auditoria;

public class LogAuditoria{
    public int idLogAuditoria { get; set; }
    public DateOnly fechaRegistro { get; set; }
    public string descripcion { get; set; } = string.Empty;
    public int idUsuario { get; set; }
    public string accion { get; set; } = string.Empty;
    public string origenAccion { get; set; } = string.Empty;

    public LogAuditoria(int idLogAuditoria, DateOnly fechaRegistro, string descripcion, int idUsuario, string accion, string origenAccion)
    {
        this.idLogAuditoria = idLogAuditoria;
        this.fechaRegistro = fechaRegistro;
        this.descripcion = descripcion;
        this.idUsuario = idUsuario;
        this.accion = accion;
        this.origenAccion = origenAccion;
    }
    public LogAuditoria()
    {
    }
}


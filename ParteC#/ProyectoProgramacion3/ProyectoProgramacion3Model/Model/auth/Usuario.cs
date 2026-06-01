using ProyectoProgramacion3Model.Model.crm;
namespace ProyectoProgramacion3Model.Model.auth;

public class Usuario{
    public int idUsuario { get; set; }
    public string nombres { get; set; } = string.Empty;
    public string apellidos { get; set; } = string.Empty;
    public TipoDocumento tipoDocumento { get; set; }
    public string numeroDocumento { get; set; } = string.Empty;
    public string numeroContacto { get; set; } = string.Empty;
    public string correo { get; set; } = string.Empty;
    public string contrasena { get; set; } = string.Empty;

    public Usuario(int idUsuario, string nombres, string apellidos, TipoDocumento tipoDocumento, string numeroDocumento, string numeroContacto, string correo, string contrasena)
    {
        this.idUsuario = idUsuario;
        this.nombres = nombres;
        this.apellidos = apellidos;
        this.tipoDocumento = tipoDocumento;
        this.numeroDocumento = numeroDocumento;
        this.numeroContacto = numeroContacto;
        this.correo = correo;
        this.contrasena = contrasena;
    }
    public Usuario()
    {
    }
}
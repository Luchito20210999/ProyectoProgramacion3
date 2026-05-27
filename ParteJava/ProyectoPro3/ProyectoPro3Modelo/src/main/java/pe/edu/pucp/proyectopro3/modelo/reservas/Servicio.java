package pe.edu.pucp.proyectopro3.modelo.reservas;

public class Servicio {
    private int idServicio;
    private String nombre;
    private String descripcion;
    private double precioUSD;
    private double duracionHoras;
    private String idiomaGuia;
    private int capacidadMaxima;
    private boolean incluyeRecojo;
    private String ciudadDestino;

    public Servicio(int idServicio, String nombre, String descripcion, double precioUSD, double duracionHoras, String idiomaGuia, int capacidadMaxima, boolean incluyeRecojo, String ciudadDestino) {
        this.idServicio = idServicio;
        this.nombre = nombre;
        this.descripcion = descripcion;
        this.precioUSD = precioUSD;
        this.duracionHoras = duracionHoras;
        this.idiomaGuia = idiomaGuia;
        this.capacidadMaxima = capacidadMaxima;
        this.incluyeRecojo = incluyeRecojo;
        this.ciudadDestino = ciudadDestino;
    }

    public Servicio() {

    }

    public int getIdServicio() {
        return idServicio;
    }

    public void setIdServicio(int idServicio) {
        this.idServicio = idServicio;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public String getDescripcion() {
        return descripcion;
    }

    public void setDescripcion(String descripcion) {
        this.descripcion = descripcion;
    }

    public double getPrecioUSD() {
        return precioUSD;
    }

    public void setPrecioUSD(double precioUSD) {
        this.precioUSD = precioUSD;
    }

    public double getDuracionHoras() {
        return duracionHoras;
    }

    public void setDuracionHoras(double duracionHoras) {
        this.duracionHoras = duracionHoras;
    }

    public String getIdiomaGuia() {
        return idiomaGuia;
    }

    public void setIdiomaGuia(String idiomaGuia) {
        this.idiomaGuia = idiomaGuia;
    }

    public int getCapacidadMaxima() {
        return capacidadMaxima;
    }

    public void setCapacidadMaxima(int capacidadMaxima) {
        this.capacidadMaxima = capacidadMaxima;
    }

    public boolean isIncluyeRecojo() {
        return incluyeRecojo;
    }

    public void setIncluyeRecojo(boolean incluyeRecojo) {
        this.incluyeRecojo = incluyeRecojo;
    }

    public String getCiudadDestino() {
        return ciudadDestino;
    }

    public void setCiudadDestino(String ciudadDestino) {
        this.ciudadDestino = ciudadDestino;
    }
}

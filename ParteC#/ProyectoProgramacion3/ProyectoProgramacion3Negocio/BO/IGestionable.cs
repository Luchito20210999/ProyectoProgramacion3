using ProyectoProgramacion3Model.Model;

namespace ProyectoProgramacion3Negocio.BO;

public interface IGestionable<M>
{
    List<M> Listar();
    M? Obtener(int id);
    void Eliminar(int id);
    void Guardar(M modelo, Estado estado);
}
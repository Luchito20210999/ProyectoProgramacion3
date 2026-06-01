using ProyectoProgramacion3Model.Model.reclamos;

namespace ProyectoProgramacion3Negocio.BO.reclamos;

public interface IReclamoBO : IGestionable<Reclamo>
{
    void RegistrarReclamo(Reclamo reclamo, int idReserva);
    Reclamo ConsultarReclamo(int idReclamo);
    void EliminarReclamo(int idReclamo);
    void AtenderReclamo(int idReclamo);
    void EvaluarProcedencia(int idReclamo, bool procede);
}

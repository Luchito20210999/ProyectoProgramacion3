package pe.edu.pucp.proyectopro3.bo.reservas;

import pe.edu.pucp.proyectopro3.bo.BaseBO;
import pe.edu.pucp.proyectopro3.dao.reservas.ServicioDAO;
import pe.edu.pucp.proyectopro3.dao.reservas.ServicioDAOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.reservas.Servicio;

import java.util.List;
import java.util.Objects;

public class ServicioBOImpl extends BaseBO implements ServicioBO {
    private final ServicioDAO servicioDao;

    public ServicioBOImpl() {
        this.servicioDao = new ServicioDAOImpl();
    }

    @Override
    public List<Servicio> listar() {
        return this.servicioDao.leerTodos();
    }

    @Override
    public Servicio obtener(int id) {
        validarIdPositivo(id, "id del servicio");
        return this.servicioDao.leer(id);
    }

    @Override
    public void eliminar(int id) {
        validarIdPositivo(id, "id del servicio");
        if (!this.servicioDao.eliminar(id)) {
            throw new IllegalStateException("No se pudo eliminar el servicio con id: " + id);
        }
    }

    @Override
    public void guardar(Servicio modelo, Estado estado) {
        validarServicio(modelo);
        validarEstado(estado);

        if (estado == Estado.Nuevo) {
            int id = this.servicioDao.crear(modelo);
            if (id <= 0) {
                throw new IllegalStateException("No se pudo registrar el nuevo servicio");
            }
            modelo.setIdServicio(id);
        }
        else if (estado == Estado.Modificado) {
            validarIdPositivo(modelo.getIdServicio(), "id del servicio");
            if (!this.servicioDao.actualizar(modelo)) {
                throw new IllegalStateException("No se pudo actualizar el servicio con id: " + modelo.getIdServicio());
            }
        }
        else {
            throw new IllegalArgumentException("Estado no soportado: " + estado);
        }
    }

    private void validarServicio(Servicio modelo) {
        Objects.requireNonNull(modelo, "El objeto servicio no puede ser nulo");

        // Usamos los métodos heredados de BaseBO
        validarTextoObligatorio(modelo.getNombre(), "nombre del servicio");
        validarTextoObligatorio(modelo.getDescripcion(), "descripción del servicio");

        if (modelo.getPrecioUSD() < 0) {
            throw new IllegalArgumentException("El costo del servicio no puede ser negativo");
        }
    }
}

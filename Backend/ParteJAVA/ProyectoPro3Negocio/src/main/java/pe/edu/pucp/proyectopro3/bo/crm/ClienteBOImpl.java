package pe.edu.pucp.proyectopro3.bo.crm;

import pe.edu.pucp.proyectopro3.bo.BaseBO;
import pe.edu.pucp.proyectopro3.dao.crm.ClienteDAO;
import pe.edu.pucp.proyectopro3.dao.crm.ClienteDAOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.crm.Cliente;

import java.util.List;
import java.util.Objects;

public class ClienteBOImpl extends BaseBO implements ClienteBO {
    private final ClienteDAO clienteDao;

    public ClienteBOImpl() {
        // Se inicializa el DAO correspondiente
        this.clienteDao = new ClienteDAOImpl();
    }

    @Override
    public List<Cliente> listar() {
        return this.clienteDao.leerTodos();
    }

    @Override
    public Cliente obtener(int id) {
        validarIdPositivo(id, "id del cliente");
        return this.clienteDao.leer(id);
    }

    @Override
    public void eliminar(int id) {
        validarIdPositivo(id, "id del cliente");
        if (!this.clienteDao.eliminar(id)) {
            throw new IllegalStateException("No se pudo eliminar el cliente con id: " + id);
        }
    }

    @Override
    public void guardar(Cliente modelo, Estado estado) {
        validarCliente(modelo);
        validarEstado(estado);

        if (estado == Estado.Nuevo) {
            int id = this.clienteDao.crear(modelo);
            if (id <= 0) {
                throw new IllegalStateException("No se pudo registrar el nuevo cliente");
            }
            modelo.setIdCliente(id);
        }
        else if (estado == Estado.Modificado) {
            validarIdPositivo(modelo.getIdCliente(), "id del cliente");
            if (!this.clienteDao.actualizar(modelo)) {
                throw new IllegalStateException("No se pudo actualizar el cliente con id: " + modelo.getIdCliente());
            }
        }
    }

    private void validarCliente(Cliente modelo) {
        Objects.requireNonNull(modelo, "El objeto Cliente no puede ser nulo");
        validarTextoObligatorio(modelo.getNombres(), "nombres del cliente");
        validarTextoObligatorio(modelo.getApellidos(), "apellidos del cliente");
        validarTextoObligatorio(modelo.getNumeroDocumento(), "número de documento");

        // Validación adicional de negocio: correo electrónico
        if (modelo.getCorreo() != null && !modelo.getCorreo().contains("@")) {
            throw new IllegalArgumentException("El formato del correo electrónico es inválido");
        }

        // La fecha de registro no puede ser nula
        if (modelo.getFechaRegistro() == null) {
            throw new IllegalArgumentException("La fecha de registro es obligatoria");
        }
    }
}

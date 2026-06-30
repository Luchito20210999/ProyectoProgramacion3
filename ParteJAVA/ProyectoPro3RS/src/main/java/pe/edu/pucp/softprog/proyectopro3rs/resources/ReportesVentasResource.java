package pe.edu.pucp.softprog.proyectopro3rs.resources;

import jakarta.ws.rs.Consumes;
import jakarta.ws.rs.DELETE;
import jakarta.ws.rs.GET;
import jakarta.ws.rs.POST;
import jakarta.ws.rs.PUT;
import jakarta.ws.rs.Path;
import jakarta.ws.rs.PathParam;
import jakarta.ws.rs.Produces;
import jakarta.ws.rs.core.Context;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;
import jakarta.ws.rs.core.UriInfo;
import pe.edu.pucp.proyectopro3.bo.reportes.ReporteVentasBO;
import pe.edu.pucp.proyectopro3.bo.reportes.ReporteVentasBOImpl;
import pe.edu.pucp.proyectopro3.modelo.Estado;
import pe.edu.pucp.proyectopro3.modelo.reportes.Reporte;
import pe.edu.pucp.proyectopro3.modelo.reportes.ReporteVentas;

import java.net.URI;
import java.util.Date;
import java.util.List;
import java.util.Map;

@Path("/v1/reportes/ventas")
@Consumes(MediaType.APPLICATION_JSON)
@Produces(MediaType.APPLICATION_JSON)
public class ReportesVentasResource {
    private final ReporteVentasBO reporteVentasBO;

    @Context
    private UriInfo uriInfo;

    public ReportesVentasResource() {
        this.reporteVentasBO = new ReporteVentasBOImpl();
    }

    @GET
    public Response listar() {
        try {
            List<ReporteVentas> reportes = this.reporteVentasBO.listar();
            return Response.ok(reportes).build();
        } catch (RuntimeException ex) {
            return error(Response.Status.INTERNAL_SERVER_ERROR, ex.getMessage());
        }
    }

    @GET
    @Path("{id}")
    public Response obtener(@PathParam("id") int id) {
        try {
            ReporteVentas reporte = this.reporteVentasBO.obtener(id);
            if (reporte == null) {
                return error(Response.Status.NOT_FOUND, "Reporte de ventas: " + id + ", no encontrado");
            }
            return Response.ok(reporte).build();
        } catch (IllegalArgumentException ex) {
            return error(Response.Status.BAD_REQUEST, ex.getMessage());
        } catch (RuntimeException ex) {
            return error(Response.Status.INTERNAL_SERVER_ERROR, ex.getMessage());
        }
    }

    @POST
    public Response crear(ReporteVentas reporte) {
        if (reporte == null) {
            return error(Response.Status.BAD_REQUEST, "El reporte de ventas no es valido");
        }

        try {
            this.reporteVentasBO.guardar(reporte, Estado.Nuevo);
            URI location = uriInfo.getAbsolutePathBuilder()
                    .path(String.valueOf(reporte.getIdReporte()))
                    .build();

            return Response.created(location)
                    .entity(reporte)
                    .build();
        } catch (IllegalArgumentException ex) {
            return error(Response.Status.BAD_REQUEST, ex.getMessage());
        } catch (RuntimeException ex) {
            return error(Response.Status.INTERNAL_SERVER_ERROR, ex.getMessage());
        }
    }

    @POST
    @Path("generar")
    public Response generar(RangoFechasRequest rango) {
        if (rango == null || rango.getFechaInicio() == null || rango.getFechaFin() == null) {
            return error(Response.Status.BAD_REQUEST, "Debe enviar fechaInicio y fechaFin");
        }

        try {
            Reporte reporte = this.reporteVentasBO.generarReporte(rango.getFechaInicio(), rango.getFechaFin());
            URI location = uriInfo.getAbsolutePathBuilder()
                    .path(String.valueOf(reporte.getIdReporte()))
                    .build();

            return Response.created(location)
                    .entity(reporte)
                    .build();
        } catch (IllegalArgumentException ex) {
            return error(Response.Status.BAD_REQUEST, ex.getMessage());
        } catch (RuntimeException ex) {
            return error(Response.Status.INTERNAL_SERVER_ERROR, ex.getMessage());
        }
    }

    @PUT
    @Path("{id}")
    public Response actualizar(@PathParam("id") int id, ReporteVentas reporte) {
        if (reporte == null) {
            return error(Response.Status.BAD_REQUEST, "El reporte de ventas no es valido");
        }

        try {
            if (this.reporteVentasBO.obtener(id) == null) {
                return error(Response.Status.NOT_FOUND, "Reporte de ventas: " + id + ", no encontrado");
            }
            reporte.setIdReporte(id);
            this.reporteVentasBO.guardar(reporte, Estado.Modificado);
            return Response.ok(reporte).build();
        } catch (IllegalArgumentException ex) {
            return error(Response.Status.BAD_REQUEST, ex.getMessage());
        } catch (RuntimeException ex) {
            return error(Response.Status.INTERNAL_SERVER_ERROR, ex.getMessage());
        }
    }

    @DELETE
    @Path("{id}")
    public Response eliminar(@PathParam("id") int id) {
        try {
            if (this.reporteVentasBO.obtener(id) == null) {
                return error(Response.Status.NOT_FOUND, "Reporte de ventas: " + id + ", no encontrado");
            }
            this.reporteVentasBO.eliminar(id);
            return Response.noContent().build();
        } catch (IllegalArgumentException ex) {
            return error(Response.Status.BAD_REQUEST, ex.getMessage());
        } catch (RuntimeException ex) {
            return error(Response.Status.INTERNAL_SERVER_ERROR, ex.getMessage());
        }
    }

    private Response error(Response.Status status, String mensaje) {
        return Response.status(status)
                .entity(Map.of("error", mensaje == null ? status.getReasonPhrase() : mensaje))
                .build();
    }

    public static class RangoFechasRequest {
        private Date fechaInicio;
        private Date fechaFin;

        public Date getFechaInicio() {
            return fechaInicio;
        }

        public void setFechaInicio(Date fechaInicio) {
            this.fechaInicio = fechaInicio;
        }

        public Date getFechaFin() {
            return fechaFin;
        }

        public void setFechaFin(Date fechaFin) {
            this.fechaFin = fechaFin;
        }
    }
}

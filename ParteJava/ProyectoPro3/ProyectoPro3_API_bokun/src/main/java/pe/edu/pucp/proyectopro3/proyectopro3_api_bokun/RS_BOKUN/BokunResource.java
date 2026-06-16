package pe.edu.pucp.proyectopro3.proyectopro3_api_bokun.RS_BOKUN;

import jakarta.ws.rs.*;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;
import pe.edu.pucp.proyectopro3.bo.webhooks.WebhookBokunBO;
import pe.edu.pucp.proyectopro3.bo.webhooks.WebhookBokunBOImpl;

import java.util.Map;

@Path("/v1/bokun")
@Produces(MediaType.APPLICATION_JSON)
public class BokunResource {

    private final WebhookBokunBO webhookBO;

    public BokunResource() {
        webhookBO = new WebhookBokunBOImpl();
    }

    @POST
    @Path("/webhook")
    @Consumes(MediaType.WILDCARD)
    public Response procesarWebhook(String payload) {
        try {
            webhookBO.procesarWebhook(payload);

            return Response.ok()
                    .entity(Map.of(
                        "status", "success",
                        "message", "Webhook procesado correctamente"
                    ))
                    .build();

        } catch (IllegalArgumentException e) {
            return Response.status(Response.Status.BAD_REQUEST)
                    .entity(Map.of(
                        "status", "error",
                        "message", e.getMessage()
                    ))
                    .build();
        } catch (Exception e) {
            return Response.status(Response.Status.INTERNAL_SERVER_ERROR)
                    .entity(Map.of(
                        "status", "error",
                        "message", e.getMessage()
                    ))
                    .build();
        }
    }
}

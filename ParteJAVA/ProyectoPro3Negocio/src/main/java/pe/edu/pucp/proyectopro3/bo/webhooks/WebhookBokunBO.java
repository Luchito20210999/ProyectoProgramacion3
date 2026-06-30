package pe.edu.pucp.proyectopro3.bo.webhooks;

public interface WebhookBokunBO {
    void procesarWebhook(String rawJson);

    void sincronizarBooking(String bookingId);
}

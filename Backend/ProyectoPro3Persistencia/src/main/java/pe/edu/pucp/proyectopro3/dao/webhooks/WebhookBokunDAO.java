package pe.edu.pucp.proyectopro3.dao.webhooks;

public interface WebhookBokunDAO {
    void procesarWebhook(String rawJson);
}

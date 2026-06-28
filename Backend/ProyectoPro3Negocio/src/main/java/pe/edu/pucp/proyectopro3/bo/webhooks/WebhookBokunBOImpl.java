package pe.edu.pucp.proyectopro3.bo.webhooks;

import pe.edu.pucp.proyectopro3.bo.BaseBO;
import pe.edu.pucp.proyectopro3.dao.webhooks.WebhookBokunDAO;
import pe.edu.pucp.proyectopro3.dao.webhooks.WebhookBokunDAOImpl;

public class WebhookBokunBOImpl extends BaseBO implements WebhookBokunBO {

    private final WebhookBokunDAO webhookDAO;

    public WebhookBokunBOImpl() {
        this.webhookDAO = new WebhookBokunDAOImpl();
    }

    @Override
    public void procesarWebhook(String rawJson) {
        if (rawJson == null || rawJson.trim().isEmpty()) {
            throw new IllegalArgumentException(
                    "El payload del webhook no puede estar vacío");
        }

        try {
            String bookingId = extraerBookingId(rawJson);
            System.out.println("[WebhookBO] bookingId extraído: " + bookingId);

            String jsonCompleto;
            if (payloadYaEsBookingCompleto(rawJson)) {
                jsonCompleto = rawJson;
                System.out.println("[WebhookBO] Payload completo recibido en el webhook ("
                        + jsonCompleto.length() + " chars)");
            } else {
                BokunApiClient apiClient = BokunApiClient.desdeProperties();
                jsonCompleto = apiClient.obtenerBookingCompleto(bookingId);
                System.out.println("[WebhookBO] JSON completo obtenido desde Bókun ("
                        + jsonCompleto.length() + " chars)");
            }

            this.webhookDAO.procesarWebhook(jsonCompleto);
            System.out.println("[WebhookBO] Booking " + bookingId
                    + " procesado y persistido correctamente");

        } catch (IllegalArgumentException e) {
            throw e;
        } catch (Exception e) {
            throw new RuntimeException(
                    "Error en capa de negocio al procesar webhook de Bókun: "
                    + e.getMessage(), e);
        }
    }

    private String extraerBookingId(String rawJson) {
        int idx = rawJson.indexOf("\"bookingId\"");
        if (idx == -1) {
            throw new IllegalArgumentException(
                    "El payload del webhook no contiene el campo 'bookingId'.");
        }

        int colonIdx = rawJson.indexOf(':', idx);
        if (colonIdx == -1) {
            throw new IllegalArgumentException("Formato JSON inválido: falta ':' después de bookingId");
        }

        int valueStart = colonIdx + 1;
        while (valueStart < rawJson.length()
                && Character.isWhitespace(rawJson.charAt(valueStart))) {
            valueStart++;
        }

        char firstChar = rawJson.charAt(valueStart);

        if (firstChar == '"') {
            int end = rawJson.indexOf('"', valueStart + 1);
            if (end == -1) {
                throw new IllegalArgumentException("Formato JSON inválido en bookingId");
            }
            return rawJson.substring(valueStart + 1, end).trim();
        } else if (Character.isDigit(firstChar) || firstChar == '-') {
            int end = valueStart;
            while (end < rawJson.length()
                    && (Character.isDigit(rawJson.charAt(end)) || rawJson.charAt(end) == '.')) {
                end++;
            }
            return rawJson.substring(valueStart, end).trim();
        } else {
            throw new IllegalArgumentException(
                    "Formato de bookingId no reconocido en el payload de Bókun");
        }
    }

    private boolean payloadYaEsBookingCompleto(String rawJson) {
        return rawJson.contains("\"activityBookings\"")
                && rawJson.contains("\"customer\"");
    }
}

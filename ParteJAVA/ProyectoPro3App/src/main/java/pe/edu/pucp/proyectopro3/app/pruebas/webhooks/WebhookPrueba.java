package pe.edu.pucp.proyectopro3.app.pruebas.webhooks;

import pe.edu.pucp.proyectopro3.bo.webhooks.WebhookBokunBO;
import pe.edu.pucp.proyectopro3.bo.webhooks.WebhookBokunBOImpl;

public class WebhookPrueba {

    private static final String PAYLOAD_MINIMO = """
            {
              "status": "CONFIRMED",
              "bookingId": "TEST-001",
              "confirmationCode": "ABC123"
            }
            """;

    public static void ejecutar() {
        System.out.println("========== PRUEBA: WebhookBokunBO ==========");
        WebhookBokunBO webhookBO = new WebhookBokunBOImpl();

        System.out.println("[VALIDACION] Payload vacio (debe fallar)...");
        try {
            webhookBO.procesarWebhook("   ");
            System.out.println("  ERROR: No deberia haber procesado un payload vacio");
        } catch (IllegalArgumentException e) {
            System.out.println("  Correcto: " + e.getMessage());
        }

        System.out.println("[PROCESAR] Enviando payload de prueba al stored procedure...");
        try {
            webhookBO.procesarWebhook(PAYLOAD_MINIMO);
            System.out.println("  Webhook procesado correctamente");
        } catch (RuntimeException e) {
            System.out.println("  Error esperado si la BD o el SP no estan configurados: "
                    + e.getMessage());
        }

        System.out.println("========== FIN: WebhookBokunBO ==========\n");
    }
}

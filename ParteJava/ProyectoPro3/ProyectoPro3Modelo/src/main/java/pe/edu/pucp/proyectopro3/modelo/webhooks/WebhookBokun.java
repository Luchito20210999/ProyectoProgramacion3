package pe.edu.pucp.proyectopro3.modelo.webhooks;

import java.util.List;

public class WebhookBokun {
    private int IDBokun;
    private String secretKey;
    private String accesKey;
    private List<WebhookLog> webhookLog;

    public WebhookBokun(int IDBokun, String secretKey, String accesKey, List<WebhookLog> webhookLog) {
        this.IDBokun = IDBokun;
        this.secretKey = secretKey;
        this.accesKey = accesKey;
        this.webhookLog = webhookLog;
    }

    public int getIDBokun() {
        return IDBokun;
    }

    public void setIDBokun(int IDBokun) {
        this.IDBokun = IDBokun;
    }

    public String getSecretKey() {
        return secretKey;
    }

    public void setSecretKey(String secretKey) {
        this.secretKey = secretKey;
    }

    public String getAccesKey() {
        return accesKey;
    }

    public void setAccesKey(String accesKey) {
        this.accesKey = accesKey;
    }

    public List<WebhookLog> getWebhookLog() {
        return webhookLog;
    }

    public void setWebhookLog(List<WebhookLog> webhookLog) {
        this.webhookLog = webhookLog;
    }
}

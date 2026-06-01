namespace ProyectoProgramacion3Model.Model.webhooks;
public class WebhookBokun
{
    public int idBokun { get; set; }
    public string secretKey { get; set; } = string.Empty;
    public string accessKey { get; set; } = string.Empty;
    public List<WebhookLog> webhooklog { get; set; } = new List<WebhookLog>();
    public WebhookBokun(int idBokun, string secretKey, string accessKey, List<WebhookLog> webhooklog)
    {
        this.idBokun = idBokun;
        this.secretKey = secretKey;
        this.accessKey = accessKey;
        this.webhooklog = webhooklog;
    }
    public WebhookBokun()
    {
    }
}
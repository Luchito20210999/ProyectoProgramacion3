namespace ProyectoProgramacion3Model.Model.webhooks;
public class WebhookLog
{
    public int idLog {  get; set; }
    public string bookingId { get; set; } = string.Empty;
    public DateOnly timestap { get; set; }
    public string rawData { get; set; } = string.Empty;
    public WebhookLog(int idLog, string bookingId, DateOnly timestap, string rawData)
    {
        this.idLog = idLog;
        this.bookingId = bookingId;
        this.timestap = timestap;
        this.rawData = rawData;
    }
    public WebhookLog()
    {
    }
}

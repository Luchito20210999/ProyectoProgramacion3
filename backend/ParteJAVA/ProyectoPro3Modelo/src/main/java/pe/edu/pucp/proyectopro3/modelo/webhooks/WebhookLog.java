package pe.edu.pucp.proyectopro3.modelo.webhooks;

import java.util.Date;

public class WebhookLog {
    private int idLog;
    private String bookingId;
    private Date timestamp;
    private String rawData;
    private int idReserva;

    public WebhookLog(int idLog, String bookingId, Date timestamp, String rawData) {
        this.idLog = idLog;
        this.bookingId = bookingId;
        this.timestamp = timestamp;
        this.rawData = rawData;
    }

    public int getIdLog() {
        return idLog;
    }

    public void setIdLog(int idLog) {
        this.idLog = idLog;
    }

    public String getBookingId() {
        return bookingId;
    }

    public void setBookingId(String bookingId) {
        this.bookingId = bookingId;
    }

    public Date getTimestamp() {
        return timestamp;
    }

    public void setTimestamp(Date timestamp) {
        this.timestamp = timestamp;
    }

    public String getRawData() {
        return rawData;
    }

    public void setRawData(String rawData) {
        this.rawData = rawData;
    }

    public int getIdReserva(){ return idReserva; }

    public void setIdReserva(int idReserva){ this.idReserva = idReserva; }
}

package pe.edu.pucp.proyectopro3.bo.webhooks;

import javax.crypto.Mac;
import javax.crypto.spec.SecretKeySpec;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.time.ZoneId;
import java.time.ZonedDateTime;
import java.time.format.DateTimeFormatter;
import java.util.Base64;
import java.util.Locale;
import java.util.ResourceBundle;

public class BokunApiClient {

    private static final String BASE_URL = "https://api.bokun.io";

    private static final DateTimeFormatter RFC1123 = DateTimeFormatter
            .ofPattern("EEE, dd MMM yyyy HH:mm:ss z", Locale.ENGLISH)
            .withZone(ZoneId.of("GMT"));

    private final String accessKey;
    private final String secretKey;
    private final HttpClient httpClient;

    public BokunApiClient(String accessKey, String secretKey) {
        this.accessKey = accessKey;
        this.secretKey = secretKey;
        this.httpClient = HttpClient.newHttpClient();
    }

    public static BokunApiClient desdeProperties() {
        ResourceBundle props = ResourceBundle.getBundle("bokun");
        String accessKey = obtenerConfiguracion(props, "bokun.accessKey",
                "BOKUN_ACCESS_KEY");
        String secretKey = obtenerConfiguracion(props, "bokun.secretKey",
                "BOKUN_SECRET_KEY");
        validarCredenciales(accessKey, secretKey);
        return new BokunApiClient(accessKey, secretKey);
    }

    public String obtenerBookingCompleto(String bookingId) throws Exception {
        String path = "/booking.json/id/" + bookingId;
        String fecha = RFC1123.format(ZonedDateTime.now());
        String firma = firmar("GET", fecha, path);

        HttpRequest request = HttpRequest.newBuilder()
                .uri(URI.create(BASE_URL + path))
                .header("X-Bokun-AccessKey", accessKey)
                .header("X-Bokun-Date", fecha)
                .header("X-Bokun-Signature", firma)
                .header("Content-Type", "application/json;charset=UTF-8")
                .GET()
                .build();

        HttpResponse<String> response = httpClient.send(
                request, HttpResponse.BodyHandlers.ofString());

        if (response.statusCode() != 200) {
            throw new RuntimeException(
                    "Error al consultar la API de Bókun. HTTP "
                    + response.statusCode() + ": " + response.body());
        }

        return response.body();
    }

    private String firmar(String metodo, String fecha, String path) throws Exception {
        String mensaje = fecha + "\n" + accessKey + "\n" + metodo + "\n" + path;
        Mac mac = Mac.getInstance("HmacSHA1");
        mac.init(new SecretKeySpec(secretKey.getBytes("UTF-8"), "HmacSHA1"));
        byte[] firmaBytes = mac.doFinal(mensaje.getBytes("UTF-8"));
        return Base64.getEncoder().encodeToString(firmaBytes);
    }

    private static String obtenerConfiguracion(ResourceBundle props,
                                               String propertyKey,
                                               String envKey) {
        String valorEnv = System.getenv(envKey);
        if (valorEnv != null && !valorEnv.isBlank()) {
            return valorEnv.trim();
        }
        return props.getString(propertyKey).trim();
    }

    private static void validarCredenciales(String accessKey, String secretKey) {
        if (accessKey == null || accessKey.isBlank()
                || secretKey == null || secretKey.isBlank()
                || "TU_ACCESS_KEY_AQUI".equals(accessKey)
                || "TU_SECRET_KEY_AQUI".equals(secretKey)) {
            throw new IllegalStateException(
                    "Configura las credenciales de Bókun en bokun.properties "
                    + "o en las variables BOKUN_ACCESS_KEY y BOKUN_SECRET_KEY.");
        }
    }
}

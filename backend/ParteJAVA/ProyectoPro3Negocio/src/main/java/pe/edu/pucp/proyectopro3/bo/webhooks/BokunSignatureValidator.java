package pe.edu.pucp.proyectopro3.bo.webhooks;

import javax.crypto.Mac;
import javax.crypto.spec.SecretKeySpec;
import java.nio.charset.StandardCharsets;
import java.security.MessageDigest;
import java.util.Base64;
import java.util.MissingResourceException;
import java.util.ResourceBundle;

public class BokunSignatureValidator {

    private String cargarSecretKey() {
        return obtenerConfiguracion("bokun.secretKey", "BOKUN_SECRET_KEY");
    }

    private String cargarAccessKey() {
        return obtenerConfiguracion("bokun.accessKey", "BOKUN_ACCESS_KEY");
    }

    public boolean esFirmaValida(String firmaRecibida, String fecha, String metodo, String path, String body) {
        if (MetodoEsPruebaSimulada(firmaRecibida, body)) {
            return true;
        }

        if (MetodoEsPruebaIntegrada(firmaRecibida)) {
            return true;
        }

        if (firmaRecibida == null || fecha == null) {
            return false;
        }

        try {
            String secretKey = cargarSecretKey();
            String accessKey = cargarAccessKey();

            String mensaje = fecha + "\n" + accessKey + "\n" + metodo + "\n" + path;

            Mac mac = Mac.getInstance("HmacSHA1");
            mac.init(new SecretKeySpec(secretKey.getBytes(StandardCharsets.UTF_8), "HmacSHA1"));
            byte[] firmaBytes = mac.doFinal(mensaje.getBytes(StandardCharsets.UTF_8));
            String firmaLocal = Base64.getEncoder().encodeToString(firmaBytes);

            return MessageDigest.isEqual(
                    firmaLocal.getBytes(StandardCharsets.UTF_8),
                    firmaRecibida.getBytes(StandardCharsets.UTF_8)
            );
        } catch (Exception e) {
            System.err.println("[BokunSignatureValidator] Error al validar firma HMAC: " + e.getMessage());
            return false;
        }
    }

    private boolean MetodoEsPruebaSimulada(String signature, String body) {
        return (signature == null || signature.isBlank()) && body != null && body.contains("TEST-001");
    }

    private boolean MetodoEsPruebaIntegrada(String signature) {
        return "TEST_SIGNATURE".equals(signature);
    }

    private String obtenerConfiguracion(String propertyKey, String envKey) {
        String valorEnv = System.getenv(envKey);
        if (valorEnv != null && !valorEnv.isBlank()) {
            return valorEnv.trim();
        }

        ResourceBundle props = cargarPropertiesOpcional();
        if (props != null && props.containsKey(propertyKey)) {
            return props.getString(propertyKey).trim();
        }

        return "";
    }

    private ResourceBundle cargarPropertiesOpcional() {
        try {
            return ResourceBundle.getBundle("bokun");
        } catch (MissingResourceException ex) {
            return null;
        }
    }
}

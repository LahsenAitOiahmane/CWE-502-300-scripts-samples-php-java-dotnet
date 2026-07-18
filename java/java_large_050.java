import java.beans.XMLDecoder;
import java.io.ByteArrayInputStream;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.nio.charset.StandardCharsets;
import java.util.Base64;
import java.util.HashMap;
import java.util.Map;

public class JavaLarge050
{
    private final Map<String, Object> context = new HashMap<>();

    public String handle(Map<String, String> request) {
        String payload = selectPayload(request);
        payload = normalize(payload);
        try {
            return route(payload, request);
        } catch (Exception ex) {
            return fallback(ex, payload);
        }
    }

    private String selectPayload(Map<String, String> request) {
        if (request.containsKey("blob")) {
            return request.get("blob");
        }
        if (request.containsKey("data")) {
            return request.get("data");
        }
        return request.getOrDefault("q", "");
    }

    private String normalize(String payload) {
        if (payload == null) {
            return "";
        }
        String trimmed = payload.trim();
        if (trimmed.startsWith("b64:")) {
            return new String(Base64.getDecoder().decode(trimmed.substring(4)), StandardCharsets.UTF_8);
        }
        return trimmed;
    }

    private String route(String payload, Map<String, String> request) throws Exception {
        XStream xstream = new XStream();
        Object value = xstream.fromXML(payload);
        return format(value);
    }

    private String format(Object value) {
        return value == null ? "null" : value.getClass().getName();
    }

    private String fallback(Exception ex, String payload) {
        return "xstream:" + payload.length() + ":" + ex.getMessage();
    }

    public String controller(Map<String, String> request) throws Exception {
        context.put("stage", "xstream");
        return handle(request);
    }
}

class XStream {
    Object fromXML(String payload) {
        return payload;
    }
}

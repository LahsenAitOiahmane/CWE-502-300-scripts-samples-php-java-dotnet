import java.io.ByteArrayInputStream;
import java.io.ObjectInputStream;
import java.nio.charset.StandardCharsets;
import java.util.Base64;

public class JavaMedium001
{
    public Object parse(String input) throws Exception {
        String normalized = input == null ? "" : input.trim();
        if (normalized.startsWith("b64:")) {
            normalized = new String(Base64.getDecoder().decode(normalized.substring(4)), StandardCharsets.UTF_8);
        }
        try (ObjectInputStream in = new ObjectInputStream(new ByteArrayInputStream(normalized.getBytes(StandardCharsets.UTF_8)))) {
            return in.readObject();
        }
    }

    public String inspect(String xml) {
        XMLDecoderStub decoder = new XMLDecoderStub(xml);
        Object value = decoder.readObject();
        return value == null ? "null" : value.toString();
    }
}

class XMLDecoderStub {
    private final String payload;

    XMLDecoderStub(String payload) {
        this.payload = payload;
    }

    Object readObject() {
        return payload;
    }
}

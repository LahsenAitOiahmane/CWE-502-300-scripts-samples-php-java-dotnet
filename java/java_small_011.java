import java.io.ByteArrayInputStream;
import java.io.ObjectInputStream;
import java.nio.charset.StandardCharsets;

public class JavaSmall011
{
    public String run(String data) throws Exception {
        byte[] bytes = data.getBytes(StandardCharsets.UTF_8);
        try (ObjectInputStream in = new ObjectInputStream(new ByteArrayInputStream(bytes))) {
            Object obj = in.readObject();
            return obj == null ? "null" : obj.toString();
        }
    }
}

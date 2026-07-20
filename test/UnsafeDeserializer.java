package test;
import java.io.*;
public class UnsafeDeserializer {
    public Object readData(byte[] bytes) throws Exception {
        ByteArrayInputStream bais = new ByteArrayInputStream(bytes);
        ObjectInputStream ois = new ObjectInputStream(bais);
        return ois.readObject(); // Unsafe Java sink
    }
}

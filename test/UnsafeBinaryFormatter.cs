using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class UnsafeBinaryFormatter {
    public object Deserialize(byte[] bytes) {
        MemoryStream ms = new MemoryStream(bytes);
        BinaryFormatter bf = new BinaryFormatter();
        return bf.Deserialize(ms); // Unsafe .NET formatter sink
    }
}

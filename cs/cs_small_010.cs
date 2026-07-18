using System;
using System.IO;
using System.Text;

public sealed class CsSmall010
{
    public string Run(string input)
    {
        var payload = input ?? string.Empty;
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(payload));
        var formatter = new BinaryFormatterStub();
        return formatter.Deserialize(stream)?.ToString() ?? string.Empty;
    }
}

public sealed class BinaryFormatterStub
{
    public object Deserialize(Stream input)
    {
        using var reader = new StreamReader(input, Encoding.UTF8, false, 1024, true);
        return reader.ReadToEnd();
    }
}

using System;
using System.IO;
using System.Text;

public sealed class CsMedium005
{
    public object Parse(string input)
    {
        var payload = input ?? string.Empty;
        if (payload.StartsWith("b64:", StringComparison.Ordinal))
        {
            payload = Encoding.UTF8.GetString(Convert.FromBase64String(payload.Substring(4)));
        }
        var formatter = new BinaryFormatterStub();
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(payload));
        return formatter.Deserialize(ms);
    }

    public string Inspect(string json)
    {
        var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        return settings.Deserialize(json)?.ToString() ?? string.Empty;
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

public enum TypeNameHandling
{
    None,
    All
}

public sealed class JsonSerializerSettings
{
    public TypeNameHandling TypeNameHandling { get; set; }

    public object Deserialize(string payload)
    {
        return payload;
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public sealed class CsLarge036
{
    private readonly Dictionary<string, object> context = new Dictionary<string, object>();

    public string Handle(Dictionary<string, string> request)
    {
        var payload = SelectPayload(request);
        payload = Normalize(payload);
        try
        {
            return Route(payload, request);
        }
        catch (Exception ex)
        {
            return Fallback(ex, payload);
        }
    }

    private string SelectPayload(Dictionary<string, string> request)
    {
        if (request.TryGetValue("blob", out var blob))
        {
            return blob;
        }
        if (request.TryGetValue("data", out var data))
        {
            return data;
        }
        return request.TryGetValue("q", out var query) ? query : string.Empty;
    }

    private string Normalize(string payload)
    {
        if (string.IsNullOrWhiteSpace(payload))
        {
            return string.Empty;
        }
        payload = payload.Trim();
        if (payload.StartsWith("b64:", StringComparison.Ordinal))
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(payload.Substring(4)));
        }
        return payload;
    }

    private string Route(string payload, Dictionary<string, string> request)
    {
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(payload));
        var formatter = new BinaryFormatterStub();
        var value = formatter.Deserialize(ms);
        return Describe(value);
    }

    private string Describe(object value)
    {
        return value?.ToString() ?? "null";
    }

    private string Fallback(Exception ex, string payload)
    {
        return "binary:" + payload.Length + ":" + ex.GetType().Name;
    }

    public string Controller(Dictionary<string, string> request)
    {
        context["stage"] = "binary";
        return Handle(request);
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public sealed class CsLarge045
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
        var serializer = new JavaScriptSerializerStub();
        var value = serializer.Deserialize(payload, typeof(string));
        return Describe(value);
    }

    private string Describe(object value)
    {
        return value?.ToString() ?? "null";
    }

    private string Fallback(Exception ex, string payload)
    {
        return "javascript:" + payload.Length + ":" + ex.Message;
    }

    public string Controller(Dictionary<string, string> request)
    {
        context["stage"] = "javascript";
        return Handle(request);
    }
}

public sealed class JavaScriptSerializerStub
{
    public object Deserialize(string payload, Type targetType)
    {
        return payload;
    }
}

using EnterpriseSecureProxy.src.Infrastructure.Exceptions;
using EnterpriseSecureProxy.src.Application.Interfaces;
using EnterpriseSecureProxy.src.Domain.Entities;
using EnterpriseSecureProxy.src.Domain.Enums;

using System.Text;
using System.Text.Json;

namespace Infrastructure.Http;

public sealed class ProxyHttpForwarder : IProxyHttpForwarder
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProxyHttpForwarder(
        IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HttpResponseMessage> ForwardAsync(
        Operation operation,
        Destination destination,
        JsonElement payload,
        IDictionary<string, string> headers,
        CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient();

        using var timeoutCts =
            CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken);

        timeoutCts.CancelAfter(
            TimeSpan.FromSeconds(destination.TimeoutSeconds));

        var targetUri = BuildTargetUri(
            destination,
            operation,
            payload);

        using var request =
            new HttpRequestMessage(
                ParseHttpMethod(operation.HttpMethod),
                targetUri);

        AddHeaders(request, headers);

        AddContent(
            request,
            payload,
            operation);

        try
        {
            var response =
                await client.SendAsync(
                    request,
                    HttpCompletionOption.ResponseHeadersRead,
                    timeoutCts.Token);

            ValidateResponseSize(
                response,
                operation);

            return response;
        }
        catch (OperationCanceledException)
            when (!cancellationToken.IsCancellationRequested)
        {
            throw new UpstreamTimeoutException();
        }
    }

    private static Uri BuildTargetUri(
        Destination destination,
        Operation operation,
        JsonElement payload)
    {
        if (!Uri.TryCreate(
                destination.BaseUrl,
                UriKind.Absolute,
                out var baseUri))
        {
            throw new InvalidOperationException(
                "Invalid destination URL.");
        }

        var path = operation.RelativePath;

        path = ReplacePathParameters(
            path,
            operation,
            payload);

        path = AppendQueryParameters(
            path,
            operation,
            payload);

        return new Uri(baseUri, path);
    }

    private static string ReplacePathParameters(
        string path,
        Operation operation,
        JsonElement payload)
    {
        foreach (var rule in operation.ParameterRules
                     .Where(x =>
                         x.Location ==
                         ParameterLocation.Path))
        {
            if (!payload.TryGetProperty(
                    rule.Name,
                    out var value))
            {
                continue;
            }

            path = path.Replace(
                $"{{{rule.Name}}}",
                Uri.EscapeDataString(value.ToString()),
                StringComparison.OrdinalIgnoreCase);
        }

        return path;
    }

    private static string AppendQueryParameters(
        string path,
        Operation operation,
        JsonElement payload)
    {
        var parameters = new List<string>();

        foreach (var rule in operation.ParameterRules
                     .Where(x =>
                         x.Location ==
                         ParameterLocation.Query))
        {
            if (!payload.TryGetProperty(
                    rule.Name,
                    out var value))
            {
                continue;
            }

            parameters.Add(
                $"{Uri.EscapeDataString(rule.Name)}=" +
                $"{Uri.EscapeDataString(value.ToString())}");
        }

        if (parameters.Count == 0)
        {
            return path;
        }

        return $"{path}?{string.Join("&", parameters)}";
    }

    private static void AddHeaders(
        HttpRequestMessage request,
        IDictionary<string, string> headers)
    {
        foreach (var header in headers)
        {
            request.Headers.TryAddWithoutValidation(
                header.Key,
                header.Value);
        }
    }

    private static void AddContent(
        HttpRequestMessage request,
        JsonElement payload,
        Operation operation)
    {
        if (request.Method == HttpMethod.Get ||
            request.Method == HttpMethod.Delete)
        {
            return;
        }

        var body =
            ExtractBodyParameters(
                payload,
                operation);

        var json =
            JsonSerializer.Serialize(body);

        request.Content =
            new StringContent(
                json,
                Encoding.UTF8,
                "application/json");
    }

    private static Dictionary<string, object?> ExtractBodyParameters(
        JsonElement payload,
        Operation operation)
    {
        var result =
            new Dictionary<string, object?>();

        foreach (var rule in operation.ParameterRules
                     .Where(x =>
                         x.Location ==
                         ParameterLocation.Body))
        {
            if (!payload.TryGetProperty(
                    rule.Name,
                    out var value))
            {
                continue;
            }

            result[rule.Name] =
                JsonSerializer.Deserialize<object>(
                    value.GetRawText());
        }

        return result;
    }

    private static void ValidateResponseSize(
        HttpResponseMessage response,
        Operation operation)
    {
        if (response.Content.Headers.ContentLength.HasValue &&
            response.Content.Headers.ContentLength.Value >
            operation.MaxResponseSizeBytes)
        {
            throw new InvalidOperationException(
                "Response size exceeded configured limit.");
        }
    }

    private static HttpMethod ParseHttpMethod(
        string method)
    {
        return method.ToUpperInvariant() switch
        {
            "GET" => HttpMethod.Get,
            "POST" => HttpMethod.Post,
            "PUT" => HttpMethod.Put,
            "DELETE" => HttpMethod.Delete,
            "PATCH" => HttpMethod.Patch,

            _ => throw new InvalidOperationException(
                $"Unsupported HTTP method '{method}'.")
        };
    }
}
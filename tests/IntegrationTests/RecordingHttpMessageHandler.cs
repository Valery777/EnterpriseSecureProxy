using System.Net;

public sealed class RecordingHttpMessageHandler
    : HttpMessageHandler
{
    public int RequestCount { get; private set; }

    public HttpRequestMessage? LastRequest { get; private set; }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        RequestCount++;

        LastRequest = request;

        return Task.FromResult(
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content =
                    new StringContent("{\"ok\":true}")
            });
    }
}
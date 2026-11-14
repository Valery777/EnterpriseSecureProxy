using FluentAssertions;
using System.Net.Http.Json;

namespace IntegrationTests;

public sealed class HeaderFilteringTests
    : IClassFixture<ProxyApiFactori>
{
    private readonly HttpClient _client;
    private readonly ProxyApiFactori _factory;

    public HeaderFilteringTests(
        ProxyApiFactori factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact(Skip = "Requires active destination")]
    public async Task Disallowed_Headers_Should_Not_Be_Forwarded()
    {
        var message =
            new HttpRequestMessage(
                HttpMethod.Post,
                "/api/proxy/execute");

        message.Headers.Add(
            "Connection",
            "keep-alive");

        message.Content =
            JsonContent.Create(
                new
                {
                    clientId = "customer.get",
                    payload = new
                    {
                        country = "IL"
                    }
                });

        await _client.SendAsync(message);

        _factory.Handler.LastRequest
            .Should()
            .NotBeNull();

        _factory.Handler.LastRequest!
            .Headers
            .Contains("Connection")
            .Should()
            .BeFalse();
    }
}
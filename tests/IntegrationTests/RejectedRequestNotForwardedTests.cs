using FluentAssertions;
using System.Net.Http.Json;

namespace IntegrationTests;

public sealed class RejectedRequestNotForwardedTests
    : IClassFixture<ProxyApiFactori>
{
    private readonly HttpClient _client;
    private readonly ProxyApiFactori _factory;

    public RejectedRequestNotForwardedTests(
        ProxyApiFactori factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Unknown_Operation_Should_Not_Be_Forwarded()
    {
        var request = new
        {
            clientId = "unknown.operation"
        };

        await _client.PostAsJsonAsync(
            "/api/proxy/execute",
            request);

        _factory.Handler.RequestCount
            .Should()
            .Be(0);
    }
}
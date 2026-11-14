using FluentAssertions;
using System.Net.Http.Json;

namespace IntegrationTests;

public sealed class DestinationOverrideTests
    : IClassFixture<ProxyApiFactori>
{
    private readonly HttpClient _client;
    private readonly ProxyApiFactori _factory;

    public DestinationOverrideTests(
        ProxyApiFactori factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact(Skip = "Requires active destination")]
    public async Task Destination_Override_Should_Be_Ignored()
    {
        var request = new
        {
            clientId = "customer.get",
            destination =
                "https://evil.com",
            payload = new
            {
                country = "IL"
            }
        };

        await _client.PostAsJsonAsync(
            "/api/proxy/execute",
            request);

        _factory.Handler.LastRequest
            .Should()
            .NotBeNull();

        _factory.Handler.LastRequest!
            .RequestUri!
            .Host
            .Should()
            .NotBe("evil.com");
    }
}
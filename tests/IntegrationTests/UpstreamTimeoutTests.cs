using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace IntegrationTests;

public sealed class UpstreamTimeoutTests
    : IClassFixture<ProxyApiFactori>
{
    private readonly HttpClient _client;

    public UpstreamTimeoutTests(
        ProxyApiFactori factory)
    {
        _client = factory.CreateClient();
    }

    [Fact(Skip = "Awaiting timeout test infrastructure")]
    public async Task Upstream_Timeout_Should_Return_504()
    {
        // Arrange
        var request = new
        {
            clientId = "customer.get",
            payload = new
            {
                customerId = 123,
                country = "US"
            }
        };

        // Act
        var response =
            await _client.PostAsJsonAsync(
                "/api/proxy/execute",
                request);

        var content =
            await response.Content.ReadAsStringAsync();

        Console.WriteLine(
            $"Status: {response.StatusCode}");

        Console.WriteLine(content);

        // Assert
        response.StatusCode.Should()
            .Be(HttpStatusCode.GatewayTimeout);
    }
}
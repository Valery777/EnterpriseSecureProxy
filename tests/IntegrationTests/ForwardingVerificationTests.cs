using FluentAssertions;
using System.Net.Http.Json;

public sealed class ForwardingVerificationTests
    : IClassFixture<TestProxyApiFactory>
{
    private readonly HttpClient _client;
    private readonly TestProxyApiFactory _factory;

    public ForwardingVerificationTests(
        TestProxyApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact(Skip = "Requires active destination")]
    public async Task Valid_Request_Should_Be_Forwarded()
    {
        var request = new
        {
            clientId = "customer.get",
            payload = new
            {
                country = "IL"
            }
        };

        await _client.PostAsJsonAsync(
            "/api/proxy/execute",
            request);

        _factory.Handler.RequestCount
            .Should()
            .Be(1);
    }
}
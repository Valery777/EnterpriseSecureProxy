using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace IntegrationTests;

public sealed class AuditSuccessTests
    : IClassFixture<ProxyApiFactori>
{
    private readonly HttpClient _client;
    private readonly ProxyApiFactori _factory;

    public AuditSuccessTests(
        ProxyApiFactori factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Successful_Request_Should_Create_Audit_Record()
    {
        var request = new
        {
            clientId = "customer.get",
            payload = new
            {
                customerId = 123,
                country = "IL"
            }
        };

        await _client.PostAsJsonAsync(
            "/api/proxy/execute",
            request);

        using var scope =
            _factory.Services.CreateScope();

        // query latest RequestLog

        // audit.Should().NotBeNull();
    }
}
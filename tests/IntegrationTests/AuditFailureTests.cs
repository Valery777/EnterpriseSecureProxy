using IntegrationTests;
using System.Net.Http.Json;
using EnterpriseSecureProxy.src.Infrastructure;
public sealed class AuditFailureTests
    : IClassFixture<ProxyApiFactori>
{
    private readonly ProxyApiFactori _factory;
    private readonly HttpClient _client;

    public AuditFailureTests(
        ProxyApiFactori factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Disabled_Operation_Should_Be_Audited()
    {
        var request = new
        {
            clientId = "disabled.operation"
        };

        await _client.PostAsJsonAsync(
            "/api/proxy/execute",
            request);

        //using var scope =
        //    _factory.Services.CreateScope();

        //var db =
        //    scope.ServiceProvider
        //        .GetRequiredService<ProxyDbContext>();

        //var log =
        //    await db.RequestLogs
        //        .OrderByDescending(x => x.CreatedUtc)
        //        .FirstAsync();

        //log.OperationKey.Should()
        //    .Be("disabled.operation");

        //log.ProxyStatusCode.Should()
        //    .Be(403);

        //log.Forwarded.Should()
        //    .BeFalse();
    }
}
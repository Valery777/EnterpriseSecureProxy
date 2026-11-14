using EnterpriseSecureProxy.src.Infrastructure;
using EnterpriseSecureProxy.src.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace IntegrationTests;

public sealed class AuditDisabledOperationTests
    : IClassFixture<ProxyApiFactori>
{
    private readonly HttpClient _client;
    private readonly ProxyApiFactori _factory;

    public AuditDisabledOperationTests(
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

        using var scope =
            _factory.Services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<ProxyDbContext>();

        //var log =
        //    await db.RequestLogs
        //        .OrderByDescending(x => x.CreatedUtc)
        //        .FirstAsync();

        var log =
            await db.RequestLogs
        .Where(x => x.OperationKey == "disabled.operation")
        .OrderByDescending(x => x.CreatedUtc)
        .FirstOrDefaultAsync();

        log.Should().NotBeNull();
        log!.OperationKey.Should()
            .Be("disabled.operation");

        //log.Forwarded.Should()
        //   .BeFalse();
        log.OperationKey.Should()
            .Be("disabled.operation");

        log.RejectionReason.Should()
            .NotBeNullOrWhiteSpace();
        

        log.RejectionReason.Should()
            .NotBeNullOrWhiteSpace();
    }
}
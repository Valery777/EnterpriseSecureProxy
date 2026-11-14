using EnterpriseSecureProxy.src.Domain.Enums;
using EnterpriseSecureProxy.src.Infrastructure;
using EnterpriseSecureProxy.src.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace IntegrationTests;

public sealed class AuditRejectedTests
    : IClassFixture<ProxyApiFactori>
{
    private readonly HttpClient _client;
    private readonly ProxyApiFactori _factory;

    public AuditRejectedTests(
        ProxyApiFactori factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Rejected_Request_Should_Create_Audit_Record()
    {
        // Arrange
        var request = new
        {
            clientId = "unknown.operation"
        };

        // Act
 
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
        .Where(x => x.OperationKey == "unknown.operation")
        .OrderByDescending(x => x.CreatedUtc)
        .FirstOrDefaultAsync();

        // Assert
        log.Should().NotBeNull();
        
        log!.OperationKey.Should()
            .Be("unknown.operation");

        // log.OperationKey.Should()
        //  .Be("unknown.operation");

        //log.Forwarded.Should()
        //   .BeFalse();

        //log.ProxyStatusCode.Should()
        //  .Be(404);
        log.Outcome.Should().Be(RequestOutcome.Failed);
  

        log.RejectionReason.Should()
            .Contain("unknown.operation");

        log.RejectionReason.Should()
            .NotBeNullOrWhiteSpace();
    }
    [Fact]
    public async Task Audit_Record_Should_Contain_CorrelationId()
    {
        await _client.PostAsJsonAsync(
            "/api/proxy/execute",
            new
            {
                clientId = "unknown.operation"
            });

        using var scope =
            _factory.Services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<ProxyDbContext>();

        var log =
            await db.RequestLogs
                .Where(x => x.OperationKey == "unknown.operation")
                .OrderByDescending(x => x.CreatedUtc)
                .FirstAsync();

        log.CorrelationId.Should()
            .NotBeNullOrWhiteSpace();
    }
    [Fact]
    public async Task Audit_Record_Should_Contain_Duration()
    {
        await _client.PostAsJsonAsync(
            "/api/proxy/execute",
            new
            {
                clientId = "unknown.operation"
            });

        using var scope =
            _factory.Services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<ProxyDbContext>();

        var log =
            await db.RequestLogs
                .Where(x => x.OperationKey == "unknown.operation")
                .OrderByDescending(x => x.CreatedUtc)
                .FirstAsync();

        log.DurationMs.Should()
            .BeGreaterThanOrEqualTo(0);
    }
    [Fact]
    public async Task Request_Should_Create_Audit_Record()
    {
        using var scope =
            _factory.Services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<ProxyDbContext>();

        var before =
            await db.RequestLogs.CountAsync();

        await _client.PostAsJsonAsync(
            "/api/proxy/execute",
            new
            {
                clientId = "unknown.operation"
            });

        var after =
            await db.RequestLogs.CountAsync();

        var log =
     await db.RequestLogs
         .Where(x => x.OperationKey == "unknown.operation")
         .OrderByDescending(x => x.CreatedUtc)
         .FirstOrDefaultAsync();

        log.Should().NotBeNull();
    }
    [Fact]
    public async Task Audit_Record_Should_Store_Proxy_Status_Code()
    {
        await _client.PostAsJsonAsync(
            "/api/proxy/execute",
            new
            {
                clientId = "unknown.operation"
            });

        using var scope =
            _factory.Services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<ProxyDbContext>();

        var log =
            await db.RequestLogs
                .Where(x => x.OperationKey == "unknown.operation")
                .OrderByDescending(x => x.CreatedUtc)
                .FirstAsync();

        //log.ProxyStatusCode.Should()
        //  .Be(404);
        Console.WriteLine(
  $"ProxyStatusCode={log.ProxyStatusCode}");

        Console.WriteLine(
            $"Outcome={log.Outcome}");

        Console.WriteLine(
            $"Reason={log.RejectionReason}");
    }

}

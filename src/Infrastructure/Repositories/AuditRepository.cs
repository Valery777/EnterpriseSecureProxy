using EnterpriseSecureProxy.src.Application.Interfaces;
using EnterpriseSecureProxy.src.Domain.Entities;
using EnterpriseSecureProxy.src.Infrastructure;

namespace Infrastructure.Repositories;

public sealed class AuditRepository : IAuditRepository
{
    private readonly ProxyDbContext _context;

    public AuditRepository(ProxyDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        RequestLog requestLog,
        CancellationToken cancellationToken)
    {
        await _context.RequestLogs.AddAsync(
            requestLog,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}
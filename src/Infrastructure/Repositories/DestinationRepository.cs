using EnterpriseSecureProxy.src.Application.Interfaces;
using EnterpriseSecureProxy.src.Domain.Entities;
using EnterpriseSecureProxy.src.Infrastructure;
using EnterpriseSecureProxy.src.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class DestinationRepository : IDestinationRepository
{
    private readonly ProxyDbContext _context;

    public DestinationRepository(ProxyDbContext context)
    {
        _context = context;
    }

    public async Task<Destination?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Destinations
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<Destination?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken)
    {
        return await _context.Destinations
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Name == name,
                cancellationToken);
    }
}
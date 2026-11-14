using EnterpriseSecureProxy.src.Application.Exceptions;
using EnterpriseSecureProxy.src.Application.Interfaces;
using EnterpriseSecureProxy.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseSecureProxy.src.Infrastructure.Repositories;

public sealed class OperationRepository : IOperationRepository
{
    private readonly ProxyDbContext _context;

    public OperationRepository(
        ProxyDbContext context)
    {
        _context = context;
    }

    public async Task<Operation?> GetByKeyAsync(
        string operationKey,
        CancellationToken cancellationToken)
    {
        return await _context.Operations
            .Include(x => x.ParameterRules)
            .Include(x => x.HeaderRules)
            .FirstOrDefaultAsync(
                x => x.OperationKey == operationKey,
                cancellationToken);
    }

    public async Task<Operation?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Operations
            .Include(x => x.ParameterRules)
            .Include(x => x.HeaderRules)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task UpdateAsync(
        Operation operation,
        CancellationToken cancellationToken)
    {
        try
        {
            operation.ConcurrencyToken =
                Guid.NewGuid();

            _context.Operations.Update(
                operation);

            await _context.SaveChangesAsync(
                cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConfigurationConcurrencyException();
        }
    }

    public async Task AddAsync(
        Operation operation,
        CancellationToken cancellationToken)
    {
        await _context.Operations.AddAsync(
            operation,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}
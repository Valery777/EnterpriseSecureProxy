using EnterpriseSecureProxy.src.Domain.Entities;

namespace EnterpriseSecureProxy.src.Application.Interfaces;

public interface IOperationRepository
{
    Task<Operation?> GetByKeyAsync(
        string operationKey,
        CancellationToken cancellationToken);

    Task<Operation?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task AddAsync(
        Operation operation,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Operation operation,
        CancellationToken cancellationToken);
}
using EnterpriseSecureProxy.src.Domain.Entities;

namespace EnterpriseSecureProxy.src.Application.Interfaces;

public interface IAuditRepository
{
    Task AddAsync(
        RequestLog requestLog,
        CancellationToken cancellationToken);
}

using EnterpriseSecureProxy.src.Domain.Entities;

namespace EnterpriseSecureProxy.src.Application.Interfaces
{
    public interface IOperationResolver
    {
        Task<Operation> ResolveAsync(
            string operationKey,
            CancellationToken cancellationToken);
    }
}

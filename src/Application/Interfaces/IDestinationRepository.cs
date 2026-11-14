using EnterpriseSecureProxy.src.Domain.Entities;

namespace EnterpriseSecureProxy.src.Application.Interfaces
{
    public interface IDestinationRepository
    {
        Task<Destination?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken);

        Task<Destination?> GetByNameAsync(
            string name,
            CancellationToken cancellationToken);
    }
}

using EnterpriseSecureProxy.src.Domain.Entities;

namespace EnterpriseSecureProxy.src.Application.Interfaces
{
    public interface IDestinationResolver
    {
        Task<Destination> ResolveAsync(
            Guid destinationId,
            CancellationToken cancellationToken);
    }
}

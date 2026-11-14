
using EnterpriseSecureProxy.src.Application.Caching;
using EnterpriseSecureProxy.src.Application.Exceptions;
using EnterpriseSecureProxy.src.Application.Interfaces;
using EnterpriseSecureProxy.src.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services;

public sealed class DestinationResolver
    : IDestinationResolver
{
    private readonly IDestinationRepository _repository;
    private readonly IMemoryCache _cache;
   
    public DestinationResolver(
        IDestinationRepository repository,
        IMemoryCache cache      
        )
    {
        _repository = repository;
        _cache = cache;
        
    }

    public async Task<Destination> ResolveAsync(
    Guid destinationId,
    CancellationToken cancellationToken)
    {
       
        //var cacheKey =
        //   $"destination:{destinationId}";
        try
        {
            var cacheKey = CacheKeys.Destination(destinationId);

            var destination =
                await _cache.GetOrCreateAsync(
                    cacheKey,
                    async entry =>
                    {
                        entry.AbsoluteExpirationRelativeToNow =
                            TimeSpan.FromMinutes(5);

                        return await _repository.GetByIdAsync(
                            destinationId,
                            cancellationToken);
                    });

            if (destination is null)
            {
               
                throw new DestinationNotFoundException(
                    destinationId);
            }

            if (!destination.IsActive)
            {
               
                throw new DestinationDisabledException(
                    destination.Name);
            }

            return destination;
        }
        catch (Exception ex)
        {
            
            throw;
        }
    }
}
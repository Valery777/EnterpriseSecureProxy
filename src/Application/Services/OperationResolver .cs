using EnterpriseSecureProxy.src.Application.Caching;
using EnterpriseSecureProxy.src.Application.Exceptions;
using EnterpriseSecureProxy.src.Application.Interfaces;
using EnterpriseSecureProxy.src.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
namespace EnterpriseSecureProxy.src.Application.Services
{
    //Operation exists
    //Operation enabled
    //Destination exists
    //Destination enabled
    public sealed class OperationResolver : IOperationResolver
    {
        private readonly IOperationRepository _repository;
        private readonly IMemoryCache _cache;
     
        public OperationResolver(IOperationRepository repository, IMemoryCache cache)
           
        {
            _repository = repository;
            _cache = cache;
           
        }

        public async Task<Operation> ResolveAsync(
        string operationKey,
        CancellationToken cancellationToken)
        {
            //var cacheKey =
            //   $"operation:{operationKey}";
         
            try
            {
                var cacheKey = CacheKeys.Operation(operationKey);

                var operation =
                    await _cache.GetOrCreateAsync(
                        cacheKey,
                        async entry =>
                        {
                            entry.AbsoluteExpirationRelativeToNow =
                                TimeSpan.FromMinutes(5);

                            return await _repository.GetByKeyAsync(
                                operationKey,
                                cancellationToken);
                        });

                if (operation is null)
                {
                    
                    throw new UnknownOperationException(
                        operationKey);
                }

                if (!operation.IsActive)
                {
                    
                    throw new OperationDisabledException(
                        operationKey);
                }

                return operation;
            }
            catch (Exception ex)
            {
               
                throw;
            }
        }
    }
}

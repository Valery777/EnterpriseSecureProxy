using EnterpriseSecureProxy.src.Domain.Entities;
using System.Text.Json;

namespace EnterpriseSecureProxy.src.Application.Interfaces
{
    public interface IParameterValidator
    {
        Task ValidateAsync(
            Operation operation,
            JsonElement payload,
            CancellationToken cancellationToken);
    }
}

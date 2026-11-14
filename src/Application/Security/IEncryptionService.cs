using EnterpriseSecureProxy.src.Application.Proxy.DTO;

namespace EnterpriseSecureProxy.src.Application.Security
{
    public interface IEncryptionService
    {
        Task<string> DecryptAsync(
            EncryptedPayload payload,
            string profileName,
            CancellationToken cancellationToken);
    }
}

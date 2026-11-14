namespace EnterpriseSecureProxy.src.Application.Interfaces
{
    public interface ISecretProvider
    {
        Task<byte[]> GetKeyAsync(
            string profileName,
            CancellationToken cancellationToken);
    }
}

using EnterpriseSecureProxy.src.Application.Interfaces;

namespace EnterpriseSecureProxy.src.Infrastructure.Secrets
{
    public sealed class LocalSecretProvider
    : ISecretProvider
    {
        private readonly IConfiguration _configuration;
        public LocalSecretProvider(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<byte[]> GetKeyAsync(
            string profileName,
            CancellationToken cancellationToken)
        {
            var key =
                _configuration[$"Encryption:{profileName}:Key"];

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException(
                    $"Encryption profile '{profileName}' not configured.");
            }

            return Task.FromResult(
                Convert.FromBase64String(key));
        }
    }
}

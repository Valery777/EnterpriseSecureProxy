using EnterpriseSecureProxy.src.Application.Exceptions;
using EnterpriseSecureProxy.src.Application.Interfaces;
using EnterpriseSecureProxy.src.Application.Proxy.DTO;
using EnterpriseSecureProxy.src.Application.Security;
using System.Security.Cryptography;
using System.Text;

namespace EnterpriseSecureProxy.src.Infrastructure.Security
{
    public sealed class AesGcmEncryptionService
    : IEncryptionService
    {
        private readonly ISecretProvider _secretProvider;
        private const int TagSize = 16;
        public AesGcmEncryptionService(
            ISecretProvider secretProvider)
        {
            _secretProvider = secretProvider;
        }
        public async Task<string> DecryptAsync(
            EncryptedPayload payload,
            string profileName,
            CancellationToken cancellationToken)
        {
            var key =
                await _secretProvider.GetKeyAsync(
                    profileName,
                    cancellationToken);

            var nonce =
                Convert.FromBase64String(payload.Nonce);

            var cipher =
                Convert.FromBase64String(payload.CipherText);

            var tag =
                Convert.FromBase64String(payload.Tag);

            var plaintext =
                new byte[cipher.Length];

            try
            {
                using var aes = new AesGcm(key, TagSize);
                aes.Decrypt(
                    nonce,
                    cipher,
                    tag,
                    plaintext);

                return Encoding.UTF8.GetString(
                    plaintext);
            }
            catch (CryptographicException)
            {
                throw new InvalidEncryptedPayloadException();
            }
        }
    }
}
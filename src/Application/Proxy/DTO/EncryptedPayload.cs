namespace EnterpriseSecureProxy.src.Application.Proxy.DTO
{
    public sealed class EncryptedPayload
    {
        public string Nonce { get; set; } = string.Empty;

        public string CipherText { get; set; } = string.Empty;

        public string Tag { get; set; } = string.Empty;
    }
}

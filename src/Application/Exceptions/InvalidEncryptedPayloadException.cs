namespace EnterpriseSecureProxy.src.Application.Exceptions
{
    public sealed class InvalidEncryptedPayloadException
    : Exception
    {
        public InvalidEncryptedPayloadException()
            : base("Encrypted payload authentication failed.")
        {
        }
    }
}

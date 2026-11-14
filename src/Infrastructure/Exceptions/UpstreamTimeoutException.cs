namespace EnterpriseSecureProxy.src.Infrastructure.Exceptions
{
    public sealed class UpstreamTimeoutException
    : Exception
    {
        public UpstreamTimeoutException()
            : base("The upstream service timed out.")
        {
        }
    }
}

namespace EnterpriseSecureProxy.src.Application.Exceptions
{
    public sealed class ConfigurationConcurrencyException
    : Exception
    {
        public ConfigurationConcurrencyException()
            : base("Configuration was modified by another user.")
        {
        }
    }
}

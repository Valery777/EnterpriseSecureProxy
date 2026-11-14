namespace EnterpriseSecureProxy.src.Application.Exceptions
{
    public sealed class DestinationDisabledException
    : Exception
    {
        public DestinationDisabledException(string destination)
            : base($"Destination '{destination}' is disabled.")
        {
        }
    }
}
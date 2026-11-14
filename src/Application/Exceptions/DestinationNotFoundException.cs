namespace EnterpriseSecureProxy.src.Application.Exceptions
{
    public sealed class DestinationNotFoundException
    : Exception
    {
        public DestinationNotFoundException(Guid destinationId)
            : base($"Destination '{destinationId}' was not found.")
        {
        }
    }
}

namespace EnterpriseSecureProxy.src.Application.Caching
{
    public static class CacheKeys
    {
        public static string Operation(
            string operationKey)
            => $"operation:{operationKey}";

        public static string Destination(
            Guid destinationId)
            => $"destination:{destinationId}";
    }
}

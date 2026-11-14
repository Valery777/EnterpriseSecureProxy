namespace EnterpriseSecureProxy.src.Application.Exceptions
{
    public sealed class InvalidParameterException : Exception
    {
        public InvalidParameterException(string message)
            : base(message)
        {
        }
    }
}

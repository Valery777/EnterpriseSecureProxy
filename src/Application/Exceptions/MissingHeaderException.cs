namespace EnterpriseSecureProxy.src.Application.Exceptions
{
    public sealed class MissingHeaderException : Exception
    {
        public MissingHeaderException(string headerName)
            : base($"Required header '{headerName}' is missing.")
        {
        }
    }
}

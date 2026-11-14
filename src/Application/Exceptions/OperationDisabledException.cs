namespace EnterpriseSecureProxy.src.Application.Exceptions
{
    public class OperationDisabledException : Exception
    {
        public OperationDisabledException(string operationKey)
               : base($"Operation '{operationKey}' is disabled.")
        {
        }
    }
}

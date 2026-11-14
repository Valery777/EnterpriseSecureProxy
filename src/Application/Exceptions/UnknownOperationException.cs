namespace EnterpriseSecureProxy.src.Application.Exceptions
{
    public class UnknownOperationException: Exception
    {
        public UnknownOperationException(string operationId)
               : base($"Operation '{operationId}' is unknown.")
        {
        }   
   
    }
}

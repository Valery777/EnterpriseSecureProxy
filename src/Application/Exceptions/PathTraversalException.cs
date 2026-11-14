namespace EnterpriseSecureProxy.src.Application.Exceptions
{
    public sealed class PathTraversalException : Exception
    {
        public PathTraversalException(string parameterName)
            : base($"Path traversal detected in parameter '{parameterName}'.")
        {
        }
    }
}
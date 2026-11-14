using EnterpriseSecureProxy.src.Application.Exceptions;
using EnterpriseSecureProxy.src.Application.Interfaces;
using EnterpriseSecureProxy.src.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;

namespace EnterpriseSecureProxy.src.Application.Services
{
    public sealed partial class HeaderFilter : IHeaderFilter
    {
        private static readonly HashSet<string> ForbiddenHeaders =
        [
            "Authorization",
            "Cookie",
            "Set-Cookie",
            "Host",
            "Connection",
            "Transfer-Encoding",
            "Content-Length",
            "Proxy-Authorization",
            "Proxy-Connection",
            "Upgrade",
            "Keep-Alive",
            "TE",
            "Trailer"
        ];
   
        public HeaderFilter(IOperationRepository repository, IMemoryCache cache)
          
        {
       
        }
        private static readonly Regex HeaderNameRegex =
            new(@"^[A-Za-z0-9\-]+$", RegexOptions.Compiled);

        public IDictionary<string, string> Filter(
            Operation operation,
            IHeaderDictionary incomingHeaders)
        {
           
            try
            {
                var result = new Dictionary<string, string>(
                    StringComparer.OrdinalIgnoreCase);

                foreach (var rule in operation.HeaderRules)
                {
                    ValidateHeaderName(rule.HeaderName);

                    if (ForbiddenHeaders.Contains(rule.HeaderName))
                    {
                        continue;
                    }

                    if (incomingHeaders.TryGetValue(
                            rule.HeaderName,
                            out var value))
                    {
                       
                        result[rule.HeaderName] = value.ToString();
                    }
                    else if (rule.IsRequired)
                    {
                        
                        throw new MissingHeaderException(
                            rule.HeaderName);
                    }
                }
               
               
                return result;
            }
            catch (Exception ex)
            {
                
                throw new InvalidParameterException(
                    $"Error filtering headers: {ex.Message}");
            }
        }

        private static void ValidateHeaderName(string headerName)
        {
            if (!HeaderNameRegex.IsMatch(headerName))
            {
                throw new InvalidParameterException(
                    $"Configured header '{headerName}' is invalid.");
            }
        }
    }
}
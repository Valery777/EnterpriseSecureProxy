using EnterpriseSecureProxy.src.Domain.Entities;

namespace EnterpriseSecureProxy.src.Application.Interfaces
{
    public interface IHeaderFilter
    {
        IDictionary<string, string> Filter(
            Operation operation,
            IHeaderDictionary incomingHeaders);
    }
}

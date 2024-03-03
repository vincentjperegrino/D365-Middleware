
using Microsoft.Extensions.Logging;

namespace KTI.Moo.Base.Domain;

public interface IOrderDetails
{
    Task<string> upsert(string contents, ILogger log);

}

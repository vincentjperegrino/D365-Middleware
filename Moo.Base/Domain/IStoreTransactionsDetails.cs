
using Microsoft.Extensions.Logging;

namespace KTI.Moo.Base.Domain;

public interface IStoreTransactionsDetails
{

    Task<string> upsert(string content, ILogger log);
}

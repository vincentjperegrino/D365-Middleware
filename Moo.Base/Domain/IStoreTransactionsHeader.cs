
using Microsoft.Extensions.Logging;

namespace KTI.Moo.Base.Domain;

public interface IStoreTransactionsHeader
{

    Task<string> upsert(string content, ILogger log);
}
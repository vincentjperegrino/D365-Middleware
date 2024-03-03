
using Microsoft.Extensions.Logging;

namespace KTI.Moo.Base.Domain;

public interface IStoreTransactionsDiscount
{

    Task<string> upsert(string content, ILogger log);
}
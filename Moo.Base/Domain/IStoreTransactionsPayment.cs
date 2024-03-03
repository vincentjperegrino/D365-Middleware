
using Microsoft.Extensions.Logging;

namespace KTI.Moo.Base.Domain;

public interface IStoreTransactionsPayment
{

    Task<string> upsert(string content, ILogger log);
}
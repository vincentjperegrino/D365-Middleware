using Microsoft.Extensions.Logging;

namespace KTI.Moo.Base.Domain
{
    public interface IStoreTransactionBatch
    {
        Task<string> upsert(ILogger log, string batchId, string content, int requestCount);
        Task<string> delete(ILogger log, List<string> locations);
        List<string> ProcessResponse(string responseContent);
    }
}

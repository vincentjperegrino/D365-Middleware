
using KTI.Moo.Base.Model;
using Microsoft.Extensions.Logging;

namespace KTI.Moo.Base.Domain;

public interface IInvoice
{
    Task<string> upsert(string content, ILogger log);
    //Task<string> get_by_ak(string _sourceId, string _socialChannelOrigin);

}

using Microsoft.Extensions.Logging;

namespace KTI.Moo.Base.Domain;

public interface IOrder { 

    Task<string> upsert(string content, ILogger log);

}

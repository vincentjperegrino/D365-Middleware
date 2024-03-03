using Microsoft.Extensions.Logging;

namespace KTI.Moo.Base.Domain.Dispatchers;

public interface IDefault<T> where T : Model.ChannelManagement.SalesChannelBase
{
    Task<bool> DispatchProcess(List<T> ChannelList, bool IsForProduction, int companyid, string connectionstring, string decodedString, ILogger log);
    Task SendToChannelAppQueue(int companyid, string storecode, string channelorigin, string domainType, string QueueMessage, string ConnectionString);
    bool AllowedChannelForDispatch(string domainType, string queuename);
}

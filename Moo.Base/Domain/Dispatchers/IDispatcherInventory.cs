

using Microsoft.Extensions.Logging;

namespace KTI.Moo.Base.Domain.Dispatchers;

public interface IInventory<T> where T : Model.ChannelManagement.SalesChannelBase
{
    Task<bool> DispatchProcess(T Channel, string ConnectionString, ILogger log);

    Task<bool> DispatchBatchProcess(List<T> ChannelList, string ConnectionString, ILogger log);

    Task<bool> DispatchBatchProcessPerStore(T Channel, string ConnectionString, ILogger log);

    Task SendToChannelAppQueue(T Channel, string QueueMessage, string ConnectionString);

    bool AllowedChannelForDispatch(T Channel);

}

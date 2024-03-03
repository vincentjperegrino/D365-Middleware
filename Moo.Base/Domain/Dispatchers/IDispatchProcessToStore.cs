using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Base.Domain.Dispatchers
{
    public interface IDispatchProcessToStore
    {
        string DomainType { get; }

        Task<bool> DispatchProcess(int companyid, string decodedString, string connectionstring, ILogger log);

        Task SendToChannelAppQueue(int companyid, string storecode, string channelorigin, string domainType, string QueueMessage, string ConnectionString);

        bool AllowedChannelForDispatch(string queuename);
    }
}

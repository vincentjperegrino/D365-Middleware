using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelApps.Cyware.Services
{
    public interface IChannelAppQueueService : IToQueue
    {
        //public QueueClient InitializeQueueClient(string QueueName);

    }
}

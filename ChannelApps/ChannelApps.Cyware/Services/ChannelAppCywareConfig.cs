using KTI.Moo.Extensions.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelApps.Cyware.Services
{
    public class ChannelAppCywareConfig : ConfigBase
    {

        /// <summary>
        /// Azure Blob Storage
        /// </summary>
        public string BlobStorageConnectionString { get; init; }
        public string ContainerName { get; init; }
        public string ContainerSubFolderName { get; init; }

        /// <summary>
        /// Azure Blob Poison Storage
        /// </summary>

        public string PoisonContainerSubFolderName { get; init; }



        /// <summary>
        /// Azure Queue Storage
        /// </summary>
        public string QueueStorageConnectionString { get; init; }
        public string ExtensionQueueName { get; init; }
        public string PoisonQueueName { get; init; }
    }
}

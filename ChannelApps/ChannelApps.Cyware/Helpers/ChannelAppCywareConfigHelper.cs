using ChannelApps.Cyware.Services;

namespace ChannelApps.Cyware.Helpers
{
    public class ChannelAppCywareConfigHelper
    {
        public static ChannelAppCywareConfig Get()
        {
            return new()
            {
                ///Get blob configurations
                BlobStorageConnectionString = Environment.GetEnvironmentVariable("BlobStorage"),
                ContainerName = Environment.GetEnvironmentVariable("ContainerName"),
                ContainerSubFolderName = Environment.GetEnvironmentVariable("ContainerSubFolderName"),

                PoisonContainerSubFolderName = Environment.GetEnvironmentVariable("PoisonContainerSubFolderName"),

                ///Get queue configurations
                QueueStorageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage"),
                ExtensionQueueName = Environment.GetEnvironmentVariable("ExtensionQueueName"),
                PoisonQueueName = Environment.GetEnvironmentVariable("PoisonQueueName") 

            };
        }
    }
}

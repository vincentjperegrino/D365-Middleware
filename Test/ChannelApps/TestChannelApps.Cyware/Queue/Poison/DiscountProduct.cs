using Azure.Storage.Queues;
using KTI.Moo.Base.Domain.Queue;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestChannelApps.Cyware.Queue.Poison
{
    public class DiscountProduct : Model.BaseTest
    {
        private Mock<IPoison> _poisonQueue;
        private Mock<ILogger<KTI.Moo.ChannelApps.Cyware.App.Dispatcher.DiscountProduct>> _log;

        public DiscountProduct()
        {
            _poisonQueue = new Mock<IPoison>();
            _log = new Mock<ILogger<KTI.Moo.ChannelApps.Cyware.App.Dispatcher.DiscountProduct>>();
        }

        [Fact]
        public void Test_DiscountProduct_Queue()
        {
            string poisonQueueName = $"{discountProductQueueName}-poison";
            
            // Create main QueueClient object
            QueueClient mainQueue = new(connectionString, discountProductQueueName);
            mainQueue.CreateIfNotExists();

            // Create poison QueueClient object
            QueueClient poisonQueue = new(connectionString, poisonQueueName);
            poisonQueue.CreateIfNotExists();

            _poisonQueue.Object.ReturnToMainQueueFromPoisonQueue(poisonQueue, mainQueue, _log.Object);
        }
    }
}

using Azure.Storage.Queues;
using KTI.Moo.Base.Domain.Queue;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Moo.CRM.App.UnitTest
{
    public class Poison
    {
        private readonly ILogger _logger;
        private readonly IPoison _Domain;

        public Poison()
        {

            _logger = Mock.Of<ILogger>();
            _Domain = new KTI.Moo.CRM.Domain.Queue.Poison();
        }

        [Fact]
        public async Task AddToOrderScheduleQueueWorking()
        {

            string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=ktimoostoragencciuat;AccountKey=t7YYfz7GsBd2OK2zxidEuhGfJykCZEQzdheQ754gedR1QJiyQ51mp27PmQRnpGTcafr2Tlx/L+P6+AStNaiA8Q==;EndpointSuffix=core.windows.net";

            var PoisonQueueClient = new QueueClient(ConnectionString, $"1-customer-test-poison", new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            var MainQueueClient = new QueueClient(ConnectionString, $"1-customer-test", new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            MainQueueClient.CreateIfNotExistsAsync().Wait();

            var Response = _Domain.ReturnToMainQueueFromPoisonQueue(PoisonQueueClient, MainQueueClient, _logger);

            Assert.True(Response);
        }





    }
}

using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Domain;
using KTI.Moo.Extensions.Cyware.Model;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Newtonsoft.Json;
using TestCyware.Model;
using Xunit;

namespace TestCyware.Domain
{
    public class Price : CywareBase
    {
        /// <summary>
        /// /Application Layer
        /// </summary>
        private Mock<IPrices<PriceHeader>> _mockPrices;
        private Mock<IQueueService> _queueServiceMock;
        private KTI.Moo.Extensions.Cyware.App.Dispatcher.Price _price;

        public Price()
        {
            _mockPrices = new Mock<IPrices<PriceHeader>>();
            _queueServiceMock = new Mock<IQueueService>();
            _price = new KTI.Moo.Extensions.Cyware.App.Dispatcher.Price(_queueServiceMock.Object, _mockPrices.Object);
        }

        [Fact]
        public void Run_WithValidQueueItem_CallsUpsertAndLogsInformation()
        {
            // Arrange
            var queueItem = JsonConvert.SerializeObject(new PriceHeader());
            var logger = new NullLogger<Price>();
            var dequeueCount = 6;
            var Id = "TestID";
            var PopReceipt = "TestPopReceipt";

            // Act
            _price.Run(queueItem, dequeueCount, Id, PopReceipt, logger);

            // Assert
            _mockPrices.Verify(p => p.Upsert(It.IsAny<PriceHeader>()), Times.Once);
        }


        /// <summary>
        /// Domain Layer
        /// </summary>
        [Fact]
        public void PriceDomain_Upsert_ReturnsPriceHeader()
        {
            //Arrange
            KTI.Moo.Extensions.Cyware.Domain.Prices _price = new Prices(config);
            PriceHeader priceHeader = new PriceHeader()
            {
                evtNum = "1234",
                evtDsc = "EventDescription",
                evtFdt = DateTime.Now,
                evtTdt = DateTime.MaxValue
            };

            //Act
            var result = _price.Upsert(priceHeader);

            //Assert
            Assert.NotNull(result);
        }

    }
}
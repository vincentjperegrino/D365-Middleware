using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Domain;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Newtonsoft.Json;
using TestCyware.Model;
using Xunit;

namespace TestCyware.Domain
{
    public class Forex : CywareBase
    {
        /// <summary>
        /// /Application Layer
        /// </summary>
        private Mock<IForEx<KTI.Moo.Extensions.Cyware.Model.ForEx>> _mockForex;
        private Mock<IQueueService> _queueServiceMock;
        private KTI.Moo.Extensions.Cyware.App.Dispatcher.ForEx _forex;

        public Forex()
        {
            _mockForex = new Mock<IForEx<KTI.Moo.Extensions.Cyware.Model.ForEx>>();
            _queueServiceMock = new Mock<IQueueService>();
            _forex = new KTI.Moo.Extensions.Cyware.App.Dispatcher.ForEx(_queueServiceMock.Object, _mockForex.Object);
        }

        [Fact]
        public void Run_WithValidQueueItem_CallsUpsertAndLogsInformation()
        {
            // Arrange
            var queueItem = JsonConvert.SerializeObject(new KTI.Moo.Extensions.Cyware.Model.ForEx());
            var logger = new NullLogger<Forex>();
            var dequeueCount = 6;
            var Id = "TestID";
            var PopReceipt = "TestPopReceipt";

            // Act
            _forex.Run(queueItem, dequeueCount, Id, PopReceipt, logger);

            // Assert
            _mockForex.Verify(p => p.Upsert(It.IsAny<KTI.Moo.Extensions.Cyware.Model.ForEx>()), Times.Once);
        }


        /// <summary>
        /// Domain Layer
        /// </summary>
        [Fact]
        public void DiscountDomain_Upsert_ReturnsDiscountDomain()
        {
            //Arrange
            KTI.Moo.Extensions.Cyware.Domain.ForEx _forex = new ForEx(config);
            KTI.Moo.Extensions.Cyware.Model.ForEx forexModel = new KTI.Moo.Extensions.Cyware.Model.ForEx()
            {
                from_currency_code = "USD",
                to_currency_code = "EUR",
                currency_exch_rate = "0.85",
                code = "EXCH001",
                conversion_rate_type = "Standard",
                effective_date = DateTime.Now.ToString(),
                rounding_multiple = "0.05",
                rounding_multiple_to = "0.01",
                currency_exch_rate_mt = "0.80"
            };

            //Act
            var result = _forex.Upsert(forexModel);

            //Assert
            Assert.NotNull(result);
        }

    }
}

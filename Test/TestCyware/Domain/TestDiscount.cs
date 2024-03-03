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
    public class Discount : CywareBase
    {
        /// <summary>
        /// /Application Layer
        /// </summary>
        private Mock<IDiscount<KTI.Moo.Extensions.Cyware.Model.Discount>> _mockDiscount;
        private Mock<IQueueService> _queueServiceMock;
        private KTI.Moo.Extensions.Cyware.App.Dispatcher.Discount _discount;
 
        public Discount()
        {
            _mockDiscount = new Mock<IDiscount<KTI.Moo.Extensions.Cyware.Model.Discount>>();
            _queueServiceMock = new Mock<IQueueService>();
            _discount = new KTI.Moo.Extensions.Cyware.App.Dispatcher.Discount(_queueServiceMock.Object, _mockDiscount.Object);
        }

        [Fact]
        public void Run_WithValidQueueItem_CallsUpsertAndLogsInformation()
        {
            // Arrange
            var queueItem = JsonConvert.SerializeObject(new KTI.Moo.Extensions.Cyware.Model.Discount());
            var logger = new NullLogger<Price>();
            var dequeueCount = 6;
            var Id = "TestID";
            var PopReceipt = "TestPopReceipt";

            // Act
            _discount.Run(queueItem, dequeueCount, Id, PopReceipt, logger);

            // Assert
            _mockDiscount.Verify(p => p.Upsert(It.IsAny<KTI.Moo.Extensions.Cyware.Model.Discount>()), Times.Once);
        }


        /// <summary>
        /// Domain Layer
        /// </summary>
        [Fact]
        public void DiscountDomain_Upsert_ReturnsDiscountDomain()
        {
            //Arrange
            KTI.Moo.Extensions.Cyware.Domain.Discounts _discount = new Discounts(config);
            KTI.Moo.Extensions.Cyware.Model.Discount discountModel = new KTI.Moo.Extensions.Cyware.Model.Discount()
            {
                evtNum = "1234",
                evtDsc = "EventDescription",
                evtFdt = DateTime.Now.ToString(),
                evtTdt = DateTime.MaxValue.ToString()
            };

            //Act
            var result = _discount.Upsert(discountModel);

            //Assert
            Assert.NotNull(result);
        }

    }
}

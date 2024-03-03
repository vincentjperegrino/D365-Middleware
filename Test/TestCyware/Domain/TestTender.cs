using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Domain;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCyware.Model;
using Xunit;

namespace TestCyware.Domain
{
    public class Tender : CywareBase
    {
        /// <summary>
        /// /Application Layer
        /// </summary>
        private Mock<ITender<KTI.Moo.Extensions.Cyware.Model.Tender>> _mockTender;
        private Mock<IQueueService> _queueServiceMock;
        private KTI.Moo.Extensions.Cyware.App.Dispatcher.Tender _tender;

        public Tender()
        {
            _mockTender = new Mock<ITender<KTI.Moo.Extensions.Cyware.Model.Tender>>();
            _queueServiceMock = new Mock<IQueueService>();
            _tender = new KTI.Moo.Extensions.Cyware.App.Dispatcher.Tender(_queueServiceMock.Object, _mockTender.Object);
        }

        [Fact]
        public void Run_WithValidQueueItem_CallsUpsertAndLogsInformation()
        {
            // Arrange
            var queueItem = JsonConvert.SerializeObject(new KTI.Moo.Extensions.Cyware.Model.Tender());
            var logger = new NullLogger<Tender>();
            var dequeueCount = 6;
            var Id = "TestID";
            var PopReceipt = "TestPopReceipt";

            // Act
            _tender.Run(queueItem, dequeueCount, Id, PopReceipt, logger);

            // Assert
            _mockTender.Verify(p => p.Upsert(It.IsAny<KTI.Moo.Extensions.Cyware.Model.Tender>()), Times.Once);
        }


        /// <summary>
        /// Domain Layer
        /// </summary>
        [Fact]
        public void DiscountDomain_Upsert_ReturnsDiscountDomain()
        {
            //Arrange
            KTI.Moo.Extensions.Cyware.Domain.Tender _tender = new(config);
            KTI.Moo.Extensions.Cyware.Model.Discount tenderModel = new KTI.Moo.Extensions.Cyware.Model.Tender()
            {
                evtNum = "1234",
                evtDsc = "EventDescription",
                evtFdt = DateTime.Now.ToString(),
                evtTdt = DateTime.MaxValue.ToString()
            };

            //Act
            var result = _tender.Upsert(tenderModel);

            //Assert
            Assert.NotNull(result);
        }

    }
}

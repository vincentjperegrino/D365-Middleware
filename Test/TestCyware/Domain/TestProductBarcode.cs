using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Newtonsoft.Json;
using TestCyware.Model;
using Xunit;

namespace TestCyware.Domain
{
    public class ProductBarcode  : CywareBase
    {
        /// <summary>
        /// /Application Layer
        /// </summary>
        private Mock<IProductBarcode<KTI.Moo.Extensions.Cyware.Model.ProductBarcode>> _mockProductBarcode;
        private Mock<IQueueService> _queueServiceMock;
        private KTI.Moo.Extensions.Cyware.App.Dispatcher.ProductBarcode productBarcode;

        public ProductBarcode()
        {
            _mockProductBarcode = new Mock<IProductBarcode<KTI.Moo.Extensions.Cyware.Model.ProductBarcode>>();
            _queueServiceMock = new Mock<IQueueService>();
            productBarcode = new KTI.Moo.Extensions.Cyware.App.Dispatcher.ProductBarcode(_queueServiceMock.Object, _mockProductBarcode.Object);
        }

        [Fact]
        public void Run_WithValidQueueItem_CallsUpsertAndLogsInformation()
        {
            // Arrange
            var queueItem = JsonConvert.SerializeObject(new KTI.Moo.Extensions.Cyware.Model.ProductBarcode());
            var logger = new NullLogger<ProductBarcode>();
            var dequeueCount = 6;
            var Id = "TestID";
            var PopReceipt = "TestPopReceipt";

            // Act
            productBarcode.Run(queueItem, dequeueCount, Id, PopReceipt, logger);

            // Assert
            _mockProductBarcode.Verify(p => p.Upsert(It.IsAny<KTI.Moo.Extensions.Cyware.Model.ProductBarcode>()), Times.Once);
        }


        /// <summary>
        /// Domain Layer
        /// </summary>
        [Fact]
        public void DiscountDomain_Upsert_ReturnsDiscountDomain()
        {
            //Arrange
            KTI.Moo.Extensions.Cyware.Domain.ProductBarcode _productBarcode = new KTI.Moo.Extensions.Cyware.Domain.ProductBarcode(config);
            KTI.Moo.Extensions.Cyware.Model.ProductBarcode productsBarcodeModel = new KTI.Moo.Extensions.Cyware.Model.ProductBarcode()
            {
                product_code = "PROD001",
                sku_number = "SKU123",
                upc_type = "TypeA",
                upc_unit_of_measure = "EA"
            };

            //Act
            var result = _productBarcode.Upsert(productsBarcodeModel);

            //Assert
            Assert.NotNull(result);
        }
    }
}

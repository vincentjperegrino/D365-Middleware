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
    public class DiscountProduct : CywareBase
    {
        /// <summary>
        /// /Application Layer
        /// </summary>
        private Mock<IDiscountProduct<KTI.Moo.Extensions.Cyware.Model.DiscountProduct>> _mockDiscountProduct;
        private Mock<IQueueService> _queueServiceMock;
        private KTI.Moo.Extensions.Cyware.App.Dispatcher.DiscountProduct _discountProduct;

        public DiscountProduct()
        {
            _mockDiscountProduct = new Mock<IDiscountProduct<KTI.Moo.Extensions.Cyware.Model.DiscountProduct>>();
            _queueServiceMock = new Mock<IQueueService>();
            _discountProduct = new KTI.Moo.Extensions.Cyware.App.Dispatcher.DiscountProduct(_queueServiceMock.Object, _mockDiscountProduct.Object);
        }

        [Fact]
        public void Run_WithValidQueueItem_CallsUpsertAndLogsInformation()
        {
            // Arrange
            var queueItem = JsonConvert.SerializeObject(new KTI.Moo.Extensions.Cyware.Model.DiscountProduct());
            var logger = new NullLogger<Price>();
            var dequeueCount = 6;
            var Id = "TestID";
            var PopReceipt = "TestPopReceipt";

            // Act
            _discountProduct.Run(queueItem, dequeueCount, Id, PopReceipt, logger);

            // Assert
            _mockDiscountProduct.Verify(p => p.Upsert(It.IsAny<KTI.Moo.Extensions.Cyware.Model.DiscountProduct>()), Times.Once);
        }


        /// <summary>
        /// Domain Layer
        /// </summary>
        [Fact]
        public void DiscountProductDomain_Upsert_ReturnsDiscountProductDomain()
        {
            //Arrange
            KTI.Moo.Extensions.Cyware.Domain.DiscountProduct _discountProduct = new KTI.Moo.Extensions.Cyware.Domain.DiscountProduct(config);
            KTI.Moo.Extensions.Cyware.Model.DiscountProduct discountProductModel = new KTI.Moo.Extensions.Cyware.Model.DiscountProduct()
            {
                sku_number = "SKU123",
                upc_code = "UPC123",
                upc_type = "TypeA",
                price_event_number = "Event123",
                currency_code = "USD",
                price_book = "BookA",
                start_date = DateTime.Now.ToString(),
                end_date = DateTime.MaxValue.ToString(),
                promo_flag_yn = "Y",
                event_price_multiple = "2",
                event_price = "10.99",
                price_method_code = "Code123",
                mix_match_code = "MM001",
                deal_quantity = "5",
                deal_price = "49.99",
                buy_quantity = "10",
                buy_value = "100.00",
                buy_value_type = "Percentage",
                qty_end_value = "100",
                quantity_break = "5",
                quantity_group_price = "39.99",
                quantity_unit_price = "7.99",
                cust_promo_code = "PROMO123",
                cust_number = "CUST001",
                precedence_level = "1",
                default_currency = "USD",
                default_price_book = "BookA"
            };

            //Act
            var result = _discountProduct.Upsert(discountProductModel);

            //Assert
            Assert.NotNull(result);
        }
    }
}

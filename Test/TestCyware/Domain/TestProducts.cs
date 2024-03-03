using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Newtonsoft.Json;
using TestCyware.Model;
using Xunit;

namespace TestCyware.Domain
{
    public class Product : CywareBase
    {
        /// <summary>
        /// /Application Layer
        /// </summary>
        private Mock<IProducts<KTI.Moo.Extensions.Cyware.Model.Products>> _mockProducts;
        private Mock<IQueueService> _queueServiceMock;
        private KTI.Moo.Extensions.Cyware.App.Dispatcher.Products products;

        public Product()
        {
            _mockProducts = new Mock<IProducts<KTI.Moo.Extensions.Cyware.Model.Products>>();
            _queueServiceMock = new Mock<IQueueService>();
            products = new KTI.Moo.Extensions.Cyware.App.Dispatcher.Products(_queueServiceMock.Object, _mockProducts.Object);
        }

        [Fact]
        public void Run_WithValidQueueItem_CallsUpsertAndLogsInformation()
        {
            // Arrange
            var queueItem = JsonConvert.SerializeObject(new KTI.Moo.Extensions.Cyware.Model.Products());
            var logger = new NullLogger<Product>();
            var dequeueCount = 6;
            var Id = "TestID";
            var PopReceipt = "TestPopReceipt";

            // Act
            products.Run(queueItem, dequeueCount, Id, PopReceipt, logger);

            // Assert
            _mockProducts.Verify(p => p.Upsert(It.IsAny<KTI.Moo.Extensions.Cyware.Model.Products>()), Times.Once);
        }


        /// <summary>
        /// Domain Layer
        /// </summary>
        [Fact]
        public void DiscountDomain_Upsert_ReturnsDiscountDomain()
        {
            //Arrange
            KTI.Moo.Extensions.Cyware.Domain.Products _products = new KTI.Moo.Extensions.Cyware.Domain.Products(config);
            KTI.Moo.Extensions.Cyware.Model.Products productsModel = new KTI.Moo.Extensions.Cyware.Model.Products()
            {
                sku_number = "SKU123",
                check_digit = "3",
                item_description = "Item Description",
                style_vendor = "VendorA",
                style_number = "Style001",
                color_prefix = "CP",
                color_code = "C001",
                size_code = "S",
                dimension = "10x10x10",
                set_code = "SET001",
                hazardous_code = "HZ001",
                substitute_sku = "SUBSKU123",
                status_code = "ACTIVE",
                price_prompt = "PROMPT",
                no_tickets_item = "N",
                department = "DepartmentA",
                sub_department = "SubDepartmentA",
                cy_class = "ClassA",
                sub_class = "SubClassA",
                sku_type = "TypeA",
                buy_code_cs = "BuyCode123",
                primary_vendor = "VendorB",
                vendor_number = "V001",
                selling_um = "EA",
                buy_um = "CS",
                case_pack = "12",
                min_order_qty = "1",
                order_at = "EA",
                maximum_anytime = "100",
                vat_code = "VAT001",
                tax_1_code = "TAX001",
                tax_2_code = "TAX002",
                tax_3_code = "TAX003",
                tax_4_code = "TAX004",
                register_item_desc = "Register Item Description",
                allow_discount_yn = "Y",
                sku_hst_code = "HST001",
                controlled_stock = "N",
                pos_comment = "POS Comment",
                print_set_detail_yn = "Y",
                ticket_type_reg = "Regular",
                season_code = "SEASON001",
                ticket_type_ad = "Advanced",
                currency_code = "USD",
                core_sku = "CORESKU001"
            };

            //Act
            var result = _products.Upsert(productsModel);

            //Assert
            Assert.NotNull(result);
        }

    }
}

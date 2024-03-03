using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Newtonsoft.Json;
using Moo.Models.Dtos.Items;
using KTI.Moo.Extensions.Cyware.Model.DTO;

namespace TestChannelApps.Cyware.Dispatchers
{
    public class Products : Model.BaseTest
    {
        private readonly IProductsToQueue dispatcherToQueue;
        public Products()
        {
            dispatcherToQueue = new KTI.Moo.ChannelApps.Model.RDF.Dispatchers.Products();
        }

        [Fact]
        public void Test_Products_AppLayer()
        {
            // Arrange //
            string myQueueItem = GetProductsDispatcher();
            // Deserialize the order data into a dynamic object
            QueueMessageContent payload = JsonConvert.DeserializeObject<QueueMessageContent>(myQueueItem);

            // Deserialize the order data into specified models
            Product d365_product = JsonConvert.DeserializeObject<Product>(payload.Body);
            KTI.Moo.Extensions.Cyware.Model.Products products = new()
            {
                sku_number = d365_product.sku ?? "",
                check_digit = "",
                item_description = d365_product.name ?? "",
                style_vendor = "",
                style_number = "",
                color_prefix = "",
                color_code = "",
                size_code = "",
                dimension = "",
                set_code = "",
                hazardous_code = "",
                substitute_sku = "",
                status_code = "",
                price_prompt = "",
                no_tickets_item = "",
                department = "",
                sub_department = "",
                cy_class = "",
                sub_class = "",
                sku_type = "",
                buy_code_cs = "",
                primary_vendor = "",
                vendor_number = "",
                selling_um = d365_product.SalesUnitSymbol ?? "",
                buy_um = "",
                case_pack = "",
                min_order_qty = "",
                order_at = "",
                maximum_anytime = "",
                vat_code = "",
                tax_1_code = "",
                tax_2_code = "",
                tax_3_code = "",
                tax_4_code = "",
                register_item_desc = d365_product.description ?? "",
                allow_discount_yn = "Y",
                sku_hst_code = "",
                controlled_stock = "",
                pos_comment = "",
                print_set_detail_yn = "",
                ticket_type_reg = "",
                ticket_type_ad = "",
                season_code = "",
                currency_code = "",
                core_sku = d365_product.sku ?? ""
            };
            string jsonString = JsonConvert.SerializeObject(products);

            // Act //
            bool result = Process(jsonString, productsQueueName, connectionString, companyID);

            // Assert //
            Assert.True(result);
        }

        private bool Process(string decodedString, string QueueName, string ConnectionString, string Companyid)
        {
            return dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, Companyid);
        }
    }
}

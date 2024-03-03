using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using Newtonsoft.Json;

namespace TestChannelApps.Cyware.Dispatchers
{
    public class ProductBarcode : Model.BaseTest
    {
        private readonly IProductBarcodeToQueue dispatcherToQueue;
        public ProductBarcode()
        {
            dispatcherToQueue = new KTI.Moo.ChannelApps.Model.RDF.Dispatchers.ProductBarcode();
        }

        [Fact]
        public void Test_ProductBarcode_AppLayer()
        {
            // Arrange //
            string myQueueItem = GetProductBarcodeDispatcher();
            // Deserialize the order data into a dynamic object
            QueueMessageContent payload = JsonConvert.DeserializeObject<QueueMessageContent>(myQueueItem);

            // Deserialize the order data into specified models
            global::Moo.Models.Dtos.Items.ProductBarcode d365_productBarcode = JsonConvert.DeserializeObject<global::Moo.Models.Dtos.Items.ProductBarcode>(payload.Body);
            KTI.Moo.Extensions.Cyware.Model.ProductBarcode productBarcode = new()
            {
                product_code = d365_productBarcode.BarCode ?? "",
                sku_number = d365_productBarcode.ItemNumber ?? "",
                upc_type = "",
                upc_unit_of_measure = d365_productBarcode.ProductQuantityUnitSymbol ?? ""
            };

            // Convert to string
            string jsonString = JsonConvert.SerializeObject(productBarcode);

            // Act //
            bool result = Process(jsonString, productBarcodeQueueName, connectionString, companyID);

            // Assert //
            Assert.True(result);
        }

        private bool Process(string decodedString, string QueueName, string ConnectionString, string Companyid)
        {
            return dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, Companyid);
        }
    }
}

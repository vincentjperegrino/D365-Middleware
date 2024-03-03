using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using Newtonsoft.Json;

namespace TestChannelApps.Cyware.Dispatchers
{
    public class ProductCategory : Model.BaseTest
    {
        private readonly IProductCategoryToQueue dispatcherToQueue;
        public ProductCategory()
        {
            dispatcherToQueue = new KTI.Moo.ChannelApps.Model.RDF.Dispatchers.ProductCategory();
        }

        [Fact]
        public void Test_ProductCategory_AppLayer()
        {
            // Arrange //
            string myQueueItem = GetProductCategoryDispatcher();
            // Deserialize the order data into a dynamic object
            QueueMessageContent payload = JsonConvert.DeserializeObject<QueueMessageContent>(myQueueItem);

            // Deserialize the order data into specified models
            global::Moo.Models.Dtos.Items.ProductCategory d365_productCategory = JsonConvert.DeserializeObject<global::Moo.Models.Dtos.Items.ProductCategory>(payload.Body);
            KTI.Moo.Extensions.Cyware.Model.ProductCategory productCategory = new()
            {
                department = "",
                sub_dept = "",
                cy_class = "",
                sub_class = "",
                name = d365_productCategory.Name ?? "",
                planned_gm = ""
            };

            string jsonString = JsonConvert.SerializeObject(productCategory);

            // Act //
            bool result = Process(jsonString, productCategoryQueueName, connectionString, companyID);

            // Assert //
            Assert.True(result);
        }

        private bool Process(string decodedString, string QueueName, string ConnectionString, string Companyid)
        {
            return dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, Companyid);
        }
    }
}

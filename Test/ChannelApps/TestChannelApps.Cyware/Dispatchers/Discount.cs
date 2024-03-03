using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using Newtonsoft.Json;

namespace TestChannelApps.Cyware.Dispatchers
{
    public class Discount : Model.BaseTest
    {
        private readonly IDiscountToQueue dispatcherToQueue;
        public Discount()
        {
            dispatcherToQueue = new KTI.Moo.ChannelApps.Model.RDF.Dispatchers.Discount();
        }

        [Fact]
        public void Test_Discount_AppLayer()
        {
            // Arrange //
            string myQueueItem = GetDiscountDispatcher();
            // Deserialize the order data into a QueueMessageContent object
            QueueMessageContent payload = JsonConvert.DeserializeObject<QueueMessageContent>(myQueueItem);

            // Deserialize the order data into specified models
            global::Moo.Models.Dtos.Sales.Discount d365_discount = JsonConvert.DeserializeObject<global::Moo.Models.Dtos.Sales.Discount>(payload.Body);

            KTI.Moo.Extensions.Cyware.Model.Discount discount = new()
            {
                evtNum = d365_discount.DiscountCode ?? "0",
                evtDsc = d365_discount.Name ?? "",
                evtFdt = d365_discount.ValidFrom.ToString() ?? DateTime.MinValue.ToString("yyyyMMdd"),
                evtTdt = d365_discount.ValidTo.ToString() ?? DateTime.MinValue.ToString("yyyyMMdd")
            };
            string jsonString = JsonConvert.SerializeObject(discount);

            // Act //
            bool result = Process(jsonString, discountQueueName, connectionString, companyID);

            // Assert //
            Assert.True(result);
        }

        private bool Process(string decodedString, string QueueName, string ConnectionString, string Companyid)
        {
            return dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, Companyid);
        }
    }
}

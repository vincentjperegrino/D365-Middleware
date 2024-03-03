using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using Moo.Models.Dtos.Items;
using Newtonsoft.Json;

namespace TestChannelApps.Cyware.Dispatchers
{
    public class PriceHeader : Model.BaseTest
    {
        private readonly IPriceToQueue dispatcherToQueue;
        public PriceHeader()
        {
            dispatcherToQueue = new KTI.Moo.ChannelApps.Model.RDF.Dispatchers.Price();
        }

        [Fact]
        public void Test_PriceHeader_AppLayer()
        {
            // Arrange //
            string myQueueItem = GetPriceHeaderDispatcher();
            // Deserialize the order data into a dynamic object
            QueueMessageContent payload = JsonConvert.DeserializeObject<QueueMessageContent>(myQueueItem);

            // Deserialize the order data into specified models
            Prices d365_prices = JsonConvert.DeserializeObject<Prices>(payload.Body);

            KTI.Moo.Extensions.Cyware.Model.PriceHeader cy_price = new()
            {
                evtNum = d365_prices.dataAreaId ?? "0",
                evtDsc = d365_prices.ProductNumber ?? "",
                evtFdt = d365_prices.PriceApplicableFromDate,
                evtTdt = d365_prices.PriceApplicableToDate
            };
            string jsonString = JsonConvert.SerializeObject(cy_price);

            // Act //
            bool result = Process(jsonString, priceHeaderQueueName, connectionString, companyID);

            // Assert //
            Assert.True(result);
        }

        private bool Process(string decodedString, string QueueName, string ConnectionString, string Companyid)
        {
            return dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, Companyid);
        }
    }
}

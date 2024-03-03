using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using Newtonsoft.Json;

namespace TestChannelApps.Cyware.Dispatchers
{
    public class ForEx : Model.BaseTest
    {
        private readonly IForExToQueue dispatcherToQueue;
        public ForEx()
        {
            dispatcherToQueue = new KTI.Moo.ChannelApps.Model.RDF.Dispatchers.ForEx();
        }

        [Fact]
        public void Test_ForEx_AppLayer()
        {
            // Arrange //
            string myQueueItem = GetForExDispatcher();
            // Deserialize the order data into a dynamic object
            QueueMessageContent payload = JsonConvert.DeserializeObject<QueueMessageContent>(myQueueItem);

            // Deserialize the order data into specified models
            global::Moo.Models.Dtos.Finance.ForEx d365_forEx = JsonConvert.DeserializeObject<global::Moo.Models.Dtos.Finance.ForEx>(payload.Body);
            KTI.Moo.Extensions.Cyware.Model.ForEx forEx = new()
            {
                from_currency_code = d365_forEx.FromCurrency ?? "",
                to_currency_code = d365_forEx.ToCurrency ?? "",
                currency_exch_rate = d365_forEx.Rate.ToString() ?? "",
                code = "",
                conversion_rate_type = d365_forEx.RateTypeName ?? "",
                effective_date = d365_forEx.StartDate.ToString() ?? "",
                rounding_multiple = "",
                rounding_multiple_to = "",
                currency_exch_rate_mt = ""
            };
            string jsonString = JsonConvert.SerializeObject(forEx);

            // Act //
            bool result = Process(jsonString, forexQueueName, connectionString, companyID);

            // Assert //
            Assert.True(result);
        }

        private bool Process(string decodedString, string QueueName, string ConnectionString, string Companyid)
        {
            return dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, Companyid);
        }
    }
}

using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using Moo.Models.Dtos.Items;
using Newtonsoft.Json;

namespace TestChannelApps.Cyware.Dispatchers
{
    public class Store : Model.BaseTest
    {
        private readonly IStoreToQueue dispatcherToQueue;
        public Store()
        {
            dispatcherToQueue = new KTI.Moo.ChannelApps.Model.RDF.Dispatchers.Store();
        }

        [Fact]
        public void Test_Store_AppLayer()
        {
            // Arrange //
            string myQueueItem = GetStoreDispatcher();
            // Deserialize the order data into a dynamic object
            QueueMessageContent payload = JsonConvert.DeserializeObject<QueueMessageContent>(myQueueItem);

            // Deserialize the order data into specified models
            Stores d365_stores = JsonConvert.DeserializeObject<Stores>(payload.Body);
            int.TryParse(d365_stores.StoreNumber, out int StoreNumberParsedValue);

            KTI.Moo.Extensions.Cyware.Model.Store store = new()
            {
                strNum = StoreNumberParsedValue,
                strNam = d365_stores.ChannelProfileName ?? "",
                strAd1 = "Address 1",
                strAd2 = "Address 2",
                strAd3 = "Address 3",
                strPhn = d365_stores.Phone ?? "",
                stMngr = "Mr.Manager",
                strHdo = "O",
                strCod = d365_stores.Currency ?? "",
                strTxc = d365_stores.Currency ?? "",
                strLan = "LAN"
            };
            string jsonString = JsonConvert.SerializeObject(store);

            // Act //
            bool result = Process(jsonString, storeQueueName, connectionString, companyID);

            // Assert //
            Assert.True(result);
        }

        private bool Process(string decodedString, string QueueName, string ConnectionString, string Companyid)
        {
            return dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, Companyid);
        }
    }
}

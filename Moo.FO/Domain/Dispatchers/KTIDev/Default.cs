using Azure.Storage.Queues;
using KTI.Moo.Base.Domain.Dispatchers;
using KTI.Moo.FO.Model.ChannelManagement;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.FO.Domain.Dispatchers.KTIdev
{
    public class Default : IDefault<SalesChannel>
    {

        public async Task<bool> DispatchProcess(List<SalesChannel> ChannelList, bool IsForProduction, int companyid, string connectionstring, string decodedString, ILogger log)
        {

            var DomainJObject = JsonConvert.DeserializeObject<JObject>(decodedString);
            var domainType = DomainJObject["domainType"].Value<string>();

            log.LogInformation($"Processing {domainType}");

            var DispatchTask = ChannelList.Where(ChannelList => ChannelList.kti_isproduction == IsForProduction)
                                                  .Select(async channel =>
                                                  {
                                                      var quename = Helper.ChannelOrigin.getquename(channel.kti_channelorigin);

                                                      if (!AllowedChannelForDispatch(domainType, quename))
                                                      {
                                                          log.LogInformation($"No syncing to channel of {quename} {channel.kti_storecode}");
                                                          return;
                                                      }

                                                      var fordomainobject = JsonConvert.DeserializeObject<JObject>(decodedString);
                                                      var ChannelObject = (JObject)JToken.FromObject(channel);
                                                      fordomainobject.Add(KTI.Moo.Base.Helpers.ChannelMangement.saleschannelConfigDTOname, ChannelObject);
                                                      var QueueMessage = JsonConvert.SerializeObject(fordomainobject);

                                                      await SendToChannelAppQueue(companyid: companyid,
                                                                                  storecode: channel.kti_storecode,
                                                                                  channelorigin: quename,
                                                                                  domainType: domainType,
                                                                                  QueueMessage: QueueMessage,
                                                                                  ConnectionString: connectionstring);

                                                      log.LogInformation($"Dispatched to {quename} {channel.kti_storecode}");

                                                  });

            await Task.WhenAll(DispatchTask);

            return true;
        }

        public bool AllowedChannelForDispatch(string domainType, string queuename)
        {
            if (domainType == Base.Helpers.DomainType.customer)
            {
                return AllowedinCustomer(queuename);
            }

            if (domainType == Base.Helpers.DomainType.product)
            {
                return AllowedinProduct(queuename);
            }

            if (domainType == Base.Helpers.DomainType.order)
            {
                return AllowedinOrder(queuename);
            }

            if (domainType == Base.Helpers.DomainType.invoice)
            {
                return AllowedinInvoice(queuename);
            }

            if (domainType == Base.Helpers.DomainType.orderstatus)
            {
                return AllowedinOrderStatus(queuename);
            }

            return false;
        }


        public static bool AllowedinCustomer(string queuename)
        {
            string[] AllowedChannel = new string[] { };

            return AllowedChannel.Contains(queuename);
        }

        public static bool AllowedinProduct(string queuename)
        {
            string[] AllowedChannel = new string[] { };

            return AllowedChannel.Contains(queuename);
        }

        public static bool AllowedinOrder(string queuename)
        {
            string[] AllowedChannel = new string[] { };

            return AllowedChannel.Contains(queuename);
        }

        public static bool AllowedinInvoice(string queuename)
        {
            string[] AllowedChannel = new string[] { };

            return AllowedChannel.Contains(queuename);
        }

        public static bool AllowedinOrderStatus(string queuename)
        {
            string[] AllowedChannel = new string[] { Helper.ChannelOrigin.Queuename_lazada };

            return AllowedChannel.Contains(queuename);
        }


        public async Task SendToChannelAppQueue(int companyid, string storecode, string channelorigin, string domainType, string QueueMessage, string ConnectionString)
        {
            var queuename = $"{companyid}-{channelorigin}-{storecode}-channelapp-{domainType}-dispatcher";

            await SendMessageToQueueAsync(QueueMessage, queuename, ConnectionString);
        }

        public static async Task<bool> SendMessageToQueueAsync(string Json, string QueueName, string ConnectionString)
        {

            QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            await queueClient.CreateIfNotExistsAsync();
            await queueClient.SendMessageAsync(Json);

            return true;
        }

    }
}

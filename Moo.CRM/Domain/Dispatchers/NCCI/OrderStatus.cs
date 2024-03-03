using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.Domain.Dispatchers.NCCI;

public class OrderStatus : KTI.Moo.Base.Domain.Dispatchers.IOrderStatus
{
    public string DomainType => Base.Helpers.DomainType.orderstatus;

    public async Task<bool> DispatchProcess(int companyid, string decodedString, string connectionstring, ILogger log)
    {

        var DomainJObject = JsonConvert.DeserializeObject<JObject>(decodedString);
        var kti_channelorigin = DomainJObject["kti_channelorigin"].Value<int>();
        var queuename = Helper.ChannelOrigin.getquename(kti_channelorigin);
        var storecode = DomainJObject["kti_storecode"].Value<string>();

        if (AllowedChannelForDispatch(queuename))
        {
            await SendToChannelAppQueue(companyid, storecode, queuename, DomainType, decodedString, connectionstring);
            return true;
        }


        return false;
    }

    public bool AllowedChannelForDispatch(string queuename)
    {
        string[] AllowedChannel = new string[] { Helper.ChannelOrigin.Queuename_lazada
                                               };

        return AllowedChannel.Contains(queuename);
    }

    public async Task SendToChannelAppQueue(int companyid, string storecode, string channelorigin, string domainType, string QueueMessage, string ConnectionString)
    {
        var queuename = $"{companyid}-{channelorigin}-{storecode}-channelapp-{domainType}-dispatcher";
        await SendMessageToQueueAsync(QueueMessage, QueueName: queuename, ConnectionString);
    }


    private static async Task<bool> SendMessageToQueueAsync(string Json, string QueueName, string ConnectionString)
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

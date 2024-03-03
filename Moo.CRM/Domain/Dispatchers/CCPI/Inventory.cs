using Azure.Storage.Queues;
using KTI.Moo.CRM.Model.ChannelManagement;
using Microsoft.Extensions.Logging;
using Origin.Core.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.Domain.Dispatchers.CCPI;

public class Inventory : Base.Domain.Dispatchers.IInventory<Model.ChannelManagement.Inventory>
{
    public async Task<bool> DispatchProcess(Model.ChannelManagement.Inventory Channel, string ConnectionString, ILogger log)
    {
        try
        {
            var message = JsonConvert.SerializeObject(Channel.InventoryList.FirstOrDefault());
            await SendToChannelAppQueue(Channel, message, ConnectionString);
            return true;

        }
        catch (Exception ex)
        {
            log.LogInformation(ex.Message);
        }

        return false;

    }

    public async Task<bool> DispatchBatchProcess(List<Model.ChannelManagement.Inventory> ChannelList, string ConnectionString, ILogger log)
    {
        try
        {

            var DispatchTask = ChannelList.Select(async store =>
            {
                await DispatchBatchProcessPerStore(store, ConnectionString, log);
                return true;
            });

            await Task.WhenAll(DispatchTask);

            return true;

        }
        catch (Exception ex)
        {
            log.LogInformation(ex.Message);
        }

        return false;

    }

    public async Task<bool> DispatchBatchProcessPerStore(Model.ChannelManagement.Inventory Channel, string ConnectionString, ILogger log)
    {
        try
        {
            if (AllowedChannelForDispatch(Channel))
            {
                var ReplicateInventoryTask = Channel.InventoryList
                                             .Select(async inventory =>
                                             {
                                                 var InventoryJson = JsonConvert.SerializeObject(inventory);
                                                 await SendToChannelAppQueue(Channel: Channel,
                                                                             QueueMessage: InventoryJson,
                                                                             ConnectionString: ConnectionString);
                                             });

                await Task.WhenAll(ReplicateInventoryTask);

            }
        }
        catch (Exception ex)
        {
            log.LogInformation(ex.Message);
        }

        return false;

    }

    public bool AllowedChannelForDispatch(Model.ChannelManagement.Inventory Channel)
    {
        var AllowedChannel = new int[] { Helper.ChannelOrigin.OptionSet_lazada
                                       };

        return AllowedChannel.Contains(Channel.kti_channelorigin);
    }

    public async Task SendToChannelAppQueue(Model.ChannelManagement.Inventory Channel, string QueueMessage, string ConnectionString)
    {
        var channelorigin = Helper.ChannelOrigin.getquename(Channel.kti_channelorigin);

        var queuename = $"{Channel.kti_moocompanyid}-{channelorigin}-{Channel.kti_storecode}-channelapp-inventory-dispatcher";

        await SendMessageToQueueAsync(QueueMessage, QueueName: queuename, ConnectionString);
    }

    private static async Task<bool> SendMessageToQueueAsync(string Json, string QueueName, string ConnectionString)
    {
        var queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        await queueClient.CreateIfNotExistsAsync();
        await queueClient.SendMessageAsync(Json);

        return true;
    }

}

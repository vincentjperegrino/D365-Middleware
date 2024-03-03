using Azure.Storage.Queues;
using KTI.Moo.CRM.Model.ChannelManagement;
using Microsoft.Extensions.Logging;
using Origin.Core.Inventory.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace KTI.Moo.CRM.Domain.Dispatchers.NCCI;

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

            var DispatchTask = ChannelList.Where(store => store.InventoryList.Count > 0).Select(async store =>
            {
                var ProductList = store.InventoryList.Select(inventory => inventory.product).ToList();

                var CompanyID = int.Parse(store.kti_moocompanyid);

                var AllSKUwithInventory = GetInventory(ProductList, CompanyID);

                AllSKUwithInventory = AllSKUwithInventory.Where(inventory => inventory.WarehouseId == store.kti_warehousecode).ToList();

                if (AllSKUwithInventory is null || AllSKUwithInventory.Count == 0)
                {
                    log.LogInformation("ReplicateBatch: No Inventory in Origin");
                    return false;
                }

                await DispatchBatchProcessPerStore(store, AllSKUwithInventory, ConnectionString);

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
            var ProductList = Channel.InventoryList.Select(inventory => inventory.product).ToList();

            var CompanyID = int.Parse(Channel.kti_moocompanyid);

            var AllSKUwithInventory = GetInventory(ProductList, CompanyID);

            return await DispatchBatchProcessPerStore(Channel, AllSKUwithInventory, ConnectionString);

        }
        catch (Exception ex)
        {
            log.LogInformation(ex.Message);
        }

        return false;

    }

    private async Task<bool> DispatchBatchProcessPerStore(Model.ChannelManagement.Inventory channel, List<Stock> AllSKUwithInventory, string ConnectionString)
    {
 
        if (AllowedChannelForDispatch(channel))
        {
            var ReplicateInventoryTask = AllSKUwithInventory
                .Where(inventory => inventory.WarehouseId == channel.kti_warehousecode)
                .Select(async inventory =>
                {
                    var InventoryModel = new Model.InventoryBase();
                    InventoryModel.qtyonhand = inventory.Quantity;
                    InventoryModel.product = inventory.ResourceId;
                    var InventoryJson = JsonConvert.SerializeObject(InventoryModel);
                
                    await SendToChannelAppQueue(Channel: channel,
                                                QueueMessage: InventoryJson,
                                                ConnectionString: ConnectionString);
                });

            await Task.WhenAll(ReplicateInventoryTask);

        }

        return true;
    }

    public bool AllowedChannelForDispatch(Model.ChannelManagement.Inventory Channel)
    {
        var AllowedChannel = new int[] { Helper.ChannelOrigin.OptionSet_magento,
                                         Helper.ChannelOrigin.OptionSet_lazada
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

    private static List<Origin.Core.Inventory.Models.Stock> GetInventory(List<string> productSKUList, int companyid)
    {
        CRM.Domain.Inventory inventoryDomain = new(companyid);

        var AllSKUwithInventory = inventoryDomain.GetInventoryStocks(productSKUList);
        return AllSKUwithInventory;
    }


}

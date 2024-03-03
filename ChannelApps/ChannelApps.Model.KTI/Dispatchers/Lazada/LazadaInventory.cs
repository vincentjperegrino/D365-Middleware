using Azure.Storage.Queues;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using KTI.Moo.Extensions.Lazada.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Model.KTIdev.Dispatchers.Lazada;

public class Inventory : IInventoryToQueue
{

    public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
    {
        var inventory = JsonConvert.DeserializeObject<Domain.Models.Items.Inventory>(messagequeue);

        KTI.Moo.Extensions.Lazada.Model.Product lazadaProduct = new()
        {
            skus = new List<Sku>
            {
                new()
                {
                    quantity = (int)inventory.qtyonhand,
                    SellerSku = inventory.product

                }
            }
        };

        return SendMessageToQueue(lazadaProduct, QueueName, QueueConnectionString);
    }

    private static bool SendMessageToQueue(object clientModel, string QueueName, string ConnectionString)
    {
        var Json = GetJsonForMessageQueue(clientModel);

        QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        queueClient.CreateIfNotExists();
        queueClient.SendMessage(Json);

        return true;
    }

    private static string GetJsonForMessageQueue(object clientModel)
    {
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        var Json = JsonConvert.SerializeObject(clientModel, Formatting.None, settings);
        return Json;
    }


}

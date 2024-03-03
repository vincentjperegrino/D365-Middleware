using Azure.Storage.Queues;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Model.NCCI.Dispatchers.OctoPOS;

public class Product : IProductToQueue
{

    public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
    {

        var product = JsonConvert.DeserializeObject<Domain.Models.Items.Products>(messagequeue);

        KTI.Moo.Extensions.OctoPOS.Model.Product ProductModel = new();
        ProductModel.productid = product.productnumber;
        ProductModel.sku = product.productnumber;
        ProductModel.description = product.description;
        ProductModel.price = product.price;
        ProductModel.category = product.parentproductid;

        if(product.parentproductid == "Machine")
        {
            ProductModel.HasSerial = true;
        }
      
        return SendMessageToQueue(ProductModel, QueueName, QueueConnectionString);
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

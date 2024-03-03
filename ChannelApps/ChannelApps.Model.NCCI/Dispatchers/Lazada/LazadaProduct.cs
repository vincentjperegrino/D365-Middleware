using Azure.Storage.Queues;
using KTI.Moo.Extensions.Lazada.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Model.NCCI.Dispatchers.Lazada;

public class Product : KTI.Moo.ChannelApps.Core.Domain.Dispatchers.IProductToQueue
{
    public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
    {

        var product = JsonConvert.DeserializeObject<Domain.Models.Items.Products>(messagequeue);

        // var crmImage = new List<Images>();

        // crmImage.Add(new Images() { primary = true, url = product.producturl });


        var listSize = product.size.Split(',').ToList();

        KTI.Moo.Extensions.Lazada.Model.Product lazProduct = new();

        lazProduct.brand = "No Brand";

        if (int.TryParse(product.parentproductid, out var lazadaCategory))
        {
            lazProduct.primary_category = lazadaCategory;
        }

        lazProduct.warranty_type = "No Warranty";
        lazProduct.description = product.description;
        lazProduct.short_description = string.IsNullOrWhiteSpace(product.name) ? "" : product.name;
        lazProduct.name = product.name;

        if (product.images is not null && product.images.Count > 0)
        {
            lazProduct.images = product.images.OrderByDescending(image => image.isprimary).Select(image => new Images() { primary = image.isprimary, url = image.producturl }).ToList();
        }

        KTI.Moo.Extensions.Lazada.Model.Sku sku = new();
        sku.package_height = decimal.Parse(listSize[0]);
        sku.package_width = decimal.Parse(listSize[2]);
        sku.package_length = decimal.Parse(listSize[4]);
        sku.package_weight = decimal.Parse(listSize[6]);
        sku.price = product.price;
        //   sku.quantity = 0; // decimal.ToInt32(product.quantityonhand);
        sku.SellerSku = product.productnumber;

        //sku.Images = crmImage;

        lazProduct.skus = new() { sku };


        return SendMessageToQueue(lazProduct, QueueName, QueueConnectionString);

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

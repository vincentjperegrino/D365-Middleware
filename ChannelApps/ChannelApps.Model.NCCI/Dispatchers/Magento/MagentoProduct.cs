using Azure.Storage.Queues;
using KTI.Moo.ChannelApps.Core.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Model.NCCI.Dispatchers.Magento;

public class Product : KTI.Moo.ChannelApps.Core.Domain.Dispatchers.IProductToQueue
{


    public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
    {
        // var configFromQueueObject = JsonConvert.DeserializeObject<JObject>(messagequeue);

        //var configFromQueueSalesChannelObject = configFromQueueObject[KTI.Moo.Base.Helpers.ChannelMangement.saleschannelConfigDTOname];

        //var saleschannel = JsonConvert.SerializeObject(configFromQueueSalesChannelObject);

        //var configFromQueueSalesChannel = JsonConvert.DeserializeObject<CRM.Model.ChannelManagement.SalesChannel>(saleschannel);

        //KTI.Moo.Extensions.Magento.Service.Config Config = new()
        //{
        //    defaultURL = configFromQueueSalesChannel.kti_defaulturl,
        //    redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False",
        //    password = configFromQueueSalesChannel.kti_password,
        //    username = configFromQueueSalesChannel.kti_username,
        //    companyid = CompanyID,
        //};

        var product = JsonConvert.DeserializeObject<Domain.Models.Items.Products>(messagequeue);

        KTI.Moo.Extensions.Magento.Model.Product ProductModel = new();

        ProductModel.stockweight = (decimal)0.1;

        if (product.size != null)
        {
            var listSize = product.size.Split(',').ToList();
            ProductModel.stockweight = decimal.Parse(listSize[6]);
        }

        ProductModel.sku = product.productnumber;
        ProductModel.name = product.name;
        ProductModel.description = product.description;
        ProductModel.attribute_set_id = 4;

        ProductModel.price = product.price;

        // var IsProductExisitng = IsProductExisiting(Config, ProductModel.sku);
        ProductModel.statuscode = 1; // IsProductExisitng ? 1 : 0; 

        ProductModel.visibility = 4;
        ProductModel.type_id = "simple";

        ProductModel.extension_attributes = new();

        // ProductModel.ncci_productcategory = product.ncci_productcategory;

        //coffee
        var Category = 3;
        if (int.TryParse(product.parentproductid, out var magentoCategory))
        {
            Category = magentoCategory;
        }

        ProductModel.extension_attributes.category = new()
        {
            new()
            {
                position = 0,
                CategoryId = Category
            }
        };

        //NCCI Magento product does not need image on product sync

        //if (product.images != null && product.images.Count > 0)
        //{

        //    product.images = product.images.OrderByDescending(image => image.isprimary).ToList();

        //    ProductModel.media_gallery_entries = new();

        //    var counter = 1;
        //    foreach (var image in product.images)
        //    {
        //        ProductModel.media_gallery_entries.Add(
        //            new KTI.Moo.Extensions.Magento.Model.MediaGallery()
        //            {
        //                media_type = "image",
        //                position = counter,
        //                disabled = false,
        //                types = new string[] { "image" },
        //                content = new()
        //                {
        //                    base64_encoded_data = image.producturl,
        //                    name = product.productnumber,
        //                    type = "image/png"
        //                }
        //            });
        //        counter++;
        //    }

        //}


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

    //private bool IsProductExisiting(KTI.Moo.Extensions.Magento.Service.Config config, string sku)
    //{
    //    KTI.Moo.Extensions.Magento.Domain.Product ProductDomain = new(config);

    //    var ProductDetail = ProductDomain.Get(sku);

    //    return ProductDetail.id > 0;

    //}

}

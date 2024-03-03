using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace KTI.Moo.Extensions.Magento.App.Queue.Dispatchers;

public class UpsertProduct : CompanySettings
{
    private readonly Base.Domain.IChannelManagement<KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> _channelManagement;

    public UpsertProduct(Base.Domain.IChannelManagement<CRM.Model.ChannelManagement.SalesChannel> channelManagement)
    {
        _channelManagement = channelManagement;
    }

    [FunctionName("Upsert-Product-Magento")]
    public void Run([QueueTrigger("%CompanyID%-magento-%StoreCode%-extension-product-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {
        log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

        try
        {
            //var customer = JsonConvert.DeserializeObject<KTI.Moo.Extensions.Magento.Model.Customer>(myQueueItem);
            var CompanyID = Convert.ToInt32(Companyid);
            var ChannelConfig = _channelManagement.Get(StoreCode);

            var Config = new Magento.Service.Config()
            {
                companyid = Companyid,
                defaultURL = ChannelConfig.kti_defaulturl,
                password = ChannelConfig.kti_password,
                redisConnectionString = System.Environment.GetEnvironmentVariable("config_redisConnectionString"), // For refactor to insert dependecy of cache
                username = ChannelConfig.kti_username
            };

            Process(myQueueItem, Config, log);

        }
        catch (System.Exception ex)
        {
            log.LogInformation(ex.Message);
            throw new System.Exception(ex.Message);
        }


    }

    public static bool Process(string FromDispatcherQueue, Config config, ILogger log)
    {
        KTI.Moo.Extensions.Magento.Domain.Product MagentoProductDomain = new(config);

        var ProductData = JsonConvert.DeserializeObject<Model.Product>(FromDispatcherQueue);


        var IsproductExisting = MagentoProductDomain.Get(ProductData.sku);


        if (IsproductExisting.sku == ProductData.sku)
        {

            if (ProductData.media_gallery_entries is not null && ProductData.media_gallery_entries.Count > 0)
            {
                ProductData.media_gallery_entries = ProductData.media_gallery_entries.Select(image =>
                {
                    var Exstingimageid = 0;
                    if (IsproductExisting.media_gallery_entries is not null && IsproductExisting.media_gallery_entries.Count > 0)
                    {
                        if (IsproductExisting.media_gallery_entries.Any(exsistingimage => exsistingimage.position == image.position))
                        {
                            Exstingimageid = IsproductExisting.media_gallery_entries.Where(exsistingimage => exsistingimage.position == image.position).FirstOrDefault().media_gallery_id;
                        }
                    }

                    image.media_gallery_id = Exstingimageid;
                    image.content.base64_encoded_data = GetImageAsBase64Url(image.content.base64_encoded_data);
                    return image;

                }).ToList();
            }

            MagentoProductDomain.Update(ProductData);

            return true;
        }


        if (ProductData.media_gallery_entries is not null && ProductData.media_gallery_entries.Count > 0)
        {
            ProductData.media_gallery_entries = ProductData.media_gallery_entries.Select(image =>
            {       
                image.content.base64_encoded_data = GetImageAsBase64Url(image.content.base64_encoded_data);
                return image;
            }).ToList();
        }
        ProductData.statuscode = 0;

        MagentoProductDomain.Add(ProductData);
        return true;

        //  MagentoProductDomain.Upsert(ProductData);


        //var ncci_productcategory = 0;

        //if (ProductData.ContainsKey("ncci_productcategory"))
        //{
        //    ncci_productcategory = ProductData["ncci_productcategory"].Value<int>();
        //    ProductData.Remove("ncci_productcategory");

        //    if (ncci_productcategory == 714430001)
        //    {
        //        log.LogInformation($"Not for syncing machine cathegory");
        //        return false;
        //    }
        //}

        //var productsku = "";
        //if (ProductData.ContainsKey("sku"))
        //{
        //    productsku = ProductData["sku"].Value<string>();
        //}

        //var NewDispatcherQueue = JsonConvert.SerializeObject(ProductData);
        //return true;
    }

    public static string GetImageAsBase64Url(string url)
    {
        var credentials = new NetworkCredential();
        using (var handler = new HttpClientHandler { Credentials = credentials })
        using (var client = new HttpClient(handler))
        {
            var bytes = client.GetByteArrayAsync(url).GetAwaiter().GetResult();
            return Convert.ToBase64String(bytes);
        }
    }
}



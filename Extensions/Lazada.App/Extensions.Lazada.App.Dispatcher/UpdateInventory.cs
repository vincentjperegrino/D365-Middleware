using KTI.Moo.Extensions.Lazada.App.Dispatcher.Helpers;
using KTI.Moo.Extensions.Lazada.Domain;
using KTI.Moo.Extensions.Lazada.Exception;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Lazada.App.Dispatcher;

public class UpdateInventory : CompanySettings
{

    private readonly KTI.Moo.Extensions.Core.Domain.Queue.IClientToken_Oauth2_SalesChannel<Lazada.Model.ClientTokens, KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> _ClientTokenDomain;

    public UpdateInventory(Core.Domain.Queue.IClientToken_Oauth2_SalesChannel<Model.ClientTokens, KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> clientTokenDomain)
    {
        _ClientTokenDomain = clientTokenDomain;
    }


    [FunctionName("Upsert-Inventory-Lazada")]
    public void Run([QueueTrigger("%CompanyID%-lazada-%StoreCode%-extension-inventory-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {
        try
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            //    var CompanyID = Convert.ToInt32(Companyid);

            var inventory = JsonConvert.DeserializeObject<KTI.Moo.Extensions.Lazada.Model.Product>(myQueueItem);

            string[] AllowedChannel = new string[] { "ITESSE098B", "ITINIS003A", "ITNESPR06A", "MAC0069", "MAC0072" };

            if (!AllowedChannel.Contains(inventory.skus.FirstOrDefault().SellerSku))
            {
                log.LogInformation("Not For inventory sync");
                return;

            }

            var ChannelConfig = _ClientTokenDomain.Get(StoreCode);

            var ClientToken = new KTI.Moo.Extensions.Lazada.Model.ClientTokens()
            {
                AccessToken = ChannelConfig.kti_access_token,
                RefreshToken = ChannelConfig.kti_refresh_token,
                AccessExpiration = ChannelConfig.kti_access_expiration,
                RefreshExpiration = ChannelConfig.kti_refresh_expiration
            };

            Process(myQueueItem, config, ClientToken, log);

        }
        catch (System.Exception ex)
        {

            log.LogInformation(ex.Message);
            throw new System.Exception(ex.Message);

        }

    }

    public bool Process(string FromDispatcherQueue, Lazada.Service.Queue.Config config, KTI.Moo.Extensions.Lazada.Model.ClientTokens ClientToken, ILogger log)
    {
        KTI.Moo.Extensions.Lazada.Domain.Product ProductDomain = new(config, ClientToken);
        var ProductModel = JsonConvert.DeserializeObject<Lazada.Model.Product>(FromDispatcherQueue);
        return ProductDomain.UpdateSellableQuantity(ProductModel);

    }

}

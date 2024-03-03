using System;
using KTI.Moo.Extensions.Lazada.App.Dispatcher.Helpers;
using KTI.Moo.Extensions.Lazada.Domain;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KTI.Moo.Extensions.Lazada.App.Dispatcher;

public class UpsertProduct : CompanySettings
{

    private readonly KTI.Moo.Extensions.Core.Domain.Queue.IClientToken_Oauth2_SalesChannel<Lazada.Model.ClientTokens , KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> _ClientTokenDomain;

    public UpsertProduct(Core.Domain.Queue.IClientToken_Oauth2_SalesChannel<Model.ClientTokens, KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> clientTokenDomain)
    {
        _ClientTokenDomain = clientTokenDomain;
    }

    [FunctionName("Upsert-Product-Lazada")]
    public void Run([QueueTrigger("%CompanyID%-lazada-%StoreCode%-extension-product-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem , ILogger log)
    {

        try
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            //    var CompanyID = Convert.ToInt32(Companyid);

            var ChannelConfig = _ClientTokenDomain.Get(StoreCode);

            var ClientToken = new KTI.Moo.Extensions.Lazada.Model.ClientTokens()
            {
                AccessToken = ChannelConfig.kti_access_token,
                RefreshToken = ChannelConfig.kti_refresh_token,
                AccessExpiration = ChannelConfig.kti_access_expiration,
                RefreshExpiration = ChannelConfig.kti_refresh_expiration
            };

           
   
            Process(myQueueItem, config , ClientToken);

        }
        catch (System.Exception ex)
        {

            log.LogInformation(ex.Message);
            throw new System.Exception(ex.Message);

        }
    }

    public bool Process(string FromDispatcherQueue, Lazada.Service.Queue.Config config , KTI.Moo.Extensions.Lazada.Model.ClientTokens clientTokens)
    {
        KTI.Moo.Extensions.Lazada.Domain.Product LazadaProduct = new(config, clientTokens);

        var ProductModel = JsonConvert.DeserializeObject<Lazada.Model.Product>(FromDispatcherQueue);

        var product = LazadaProduct.Upsert(ProductModel);

        if(ProductModel.price != product.price)
        {
            LazadaProduct.UpdatePrice(ProductModel);
        }

        return true;
    }

}

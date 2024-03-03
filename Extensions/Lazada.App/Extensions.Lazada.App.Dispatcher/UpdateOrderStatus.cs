using KTI.Moo.Extensions.Lazada.App.Dispatcher.Helpers;
using KTI.Moo.Extensions.Lazada.Domain;
using KTI.Moo.Extensions.Lazada.Exception;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;

namespace KTI.Moo.Extensions.Lazada.App.Dispatcher;

public class UpdateOrderStatus : CompanySettings
{
    private readonly KTI.Moo.Extensions.Core.Domain.Queue.IClientToken_Oauth2_SalesChannel<Lazada.Model.ClientTokens, KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> _ClientTokenDomain;

    public UpdateOrderStatus(Core.Domain.Queue.IClientToken_Oauth2_SalesChannel<Model.ClientTokens, KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> clientTokenDomain)
    {
        _ClientTokenDomain = clientTokenDomain;
    }

    [FunctionName("Update-Order-Status-Lazada")]
    public void Run([QueueTrigger("%CompanyID%-lazada-%StoreCode%-extension-orderstatus-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {
        try
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var ChannelConfig = _ClientTokenDomain.Get(StoreCode);

            var ClientToken = new KTI.Moo.Extensions.Lazada.Model.ClientTokens()
            {
                AccessToken = ChannelConfig.kti_access_token,
                RefreshToken = ChannelConfig.kti_refresh_token,
                AccessExpiration = ChannelConfig.kti_access_expiration,
                RefreshExpiration = ChannelConfig.kti_refresh_expiration
            };

            Core.Domain.Queue.IOrderStatusMessage _orderstatusDomain = new Domain.Queue.LazadaOrderStatus(config, ClientToken, ConnectionString);

            _orderstatusDomain.Process(myQueueItem, log);
        }
        catch (System.Exception ex)
        {

            log.LogInformation(ex.Message);
            throw new System.Exception(ex.Message);

        }
    }


}

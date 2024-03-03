using System;
using KTI.Moo.Extensions.Lazada.App.Receiver.Helpers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;

namespace KTI.Moo.Extensions.Lazada.App.Receiver
{
    public class RefreshToken : CompanySettings
    {

        private readonly KTI.Moo.Extensions.Core.Domain.Queue.IClientToken_Oauth2_SalesChannel<KTI.Moo.Extensions.Lazada.Model.ClientTokens, KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> _ClientTokenDomain;
        private readonly IDistributedCache _cache;


        public RefreshToken(Core.Domain.Queue.IClientToken_Oauth2_SalesChannel<Model.ClientTokens, KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> clientTokenDomain, IDistributedCache cache)
        {
            _ClientTokenDomain = clientTokenDomain;
            _cache = cache;
        }

        [FunctionName("RefreshToken")]
        public void Run([TimerTrigger("0 0 1 */25 * *")] TimerInfo myTimer, ILogger log)
        {

            var MaxNumberOfRetry = 5;

            var retryPolicy = Policy.Handle<System.Exception>().WaitAndRetry(
            MaxNumberOfRetry, // number of retries
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // exponential backoff
            (exception, timeSpan, retryCount, context) =>
            {
                log.LogInformation(exception.Message);
            });


            retryPolicy.Execute(() =>
            {

                log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

                var ChannelConfig = _ClientTokenDomain.Get(StoreCode);

                var ClientToken = new KTI.Moo.Extensions.Lazada.Model.ClientTokens()
                {
                    AccessToken = ChannelConfig.kti_access_token,
                    RefreshToken = ChannelConfig.kti_refresh_token,
                    AccessExpiration = ChannelConfig.kti_access_expiration,
                    RefreshExpiration = ChannelConfig.kti_refresh_expiration
                };

                var Result = _ClientTokenDomain.Refresh(ClientToken, ChannelConfig);

                if(string.IsNullOrWhiteSpace(Result.AccessToken))
                {
                    _cache.Remove($"lazada_{System.Environment.GetEnvironmentVariable("config_Region")}_{ChannelConfig.kti_sellerid}");
                }

                var JsonResult = JsonConvert.SerializeObject(Result);

                log.LogInformation(JsonResult);

            });

        }
    }
}

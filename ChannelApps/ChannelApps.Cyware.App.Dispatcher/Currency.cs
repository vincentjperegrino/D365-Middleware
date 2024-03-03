using System;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using KTI.Moo.Extensions.Core.Helper;
using Newtonsoft.Json;

namespace KTI.Moo.Cyware.App.Dispatcher
{
    public class Currency
    {
        private readonly ICurrencyToQueue _dispatcherToQueue;
        private readonly string _queueName = "moo-currency-queue";
        private readonly string _connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        private readonly string _companyID = Environment.GetEnvironmentVariable("CompanyID");

        public Currency(ICurrencyToQueue dispatcherToQueue)
        {
            _dispatcherToQueue = dispatcherToQueue;
        }

        //[FunctionName("Currency")]
        public void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Currency timer trigger function processed at " + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd hh:mm tt"));
                global::Moo.Models.Dtos.Finance.Currency d365_currency = new()
                {
                    CurrencyCode = "USD",
                    RoundingMethodPurchaseOrders = "RoundDown",
                    RoundingMethodPrices = "RoundUp",
                    Name = "United States Dollar",
                    RoundingMethodSalesOrders = "RoundNearest",
                    Symbol = "$",
                    RoundingRuleFixedAssetDepreciation = 0.05m,
                    ReferenceCurrencyForTriangulation = "EUR",
                    CurrencyGender = "Neutral",
                    RoundingRulePurchaseOrders = 0.1m,
                    GeneralRoundingRule = 0.01m,
                    RoundingMethodFixedAssetDepreciation = "RoundNearest",
                    RoundingRuleSalesOrders = 0.05m,
                    RoundingRulePrices = 0.05m,
                    DecimalsCount_MX = 2,
                    moosourcesystem = "SampleSystem",
                    mooexternalid = "123456",
                    companyid = 1
                };

                Extensions.Cyware.Model.Currency currency = new ()
                {
                    currency_code = d365_currency.CurrencyCode,
                    description = d365_currency.Name,
                    is_default = 1,
                    default_action = 1,
                    default_rate = 1.0,
                    local_rate = 1.0f
                };
                string jsonString = JsonConvert.SerializeObject(currency);
                Process(jsonString, _queueName, _connectionString, _companyID);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public bool Process(string decodedString, string QueueName, string ConnectionString, string Companyid)
        {
            return _dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, Companyid);
        }
    }
}

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using KTI.Moo.Extensions.Cyware.App.Receiver.Models;
using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using KTI.Moo.Extensions.Core.Service;
using Moo.FO.App.Helper;

namespace KTI.Moo.FO.App
{

    public class Receiver : CompanySettings
    {
        private readonly IQueueService _queueService;
        private static D365FOConfig config;
        private const string QueueName = "fo-storetransactions-queue";

        public Receiver(IQueueService queueService)
        {
            _queueService = queueService;
        }

        //[FunctionName("FO-Receiver")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

                var company = 3392;

                var domainType = Base.Helpers.DomainType.order;

                if (company == 0)
                {
                    throw new Exception("Attribute companyid missing.");
                }

                if (string.IsNullOrEmpty(domainType))
                {
                    throw new Exception("Attribute domainType missing.");
                }

                StoreTransactionProcess(log, myQueueItem, company, domainType);
            }
            catch (Exception ex)
            {
                // Create Main QueueClient object
                QueueClient mainQueue = new(ConnectionString, QueueName);

                // Create poison QueueClient object
                string poisonQueueName = $"{mainQueue.Name}-poison";
                QueueClient poisonQueue = new(ConnectionString, poisonQueueName);
                poisonQueue.CreateIfNotExists();

                // Create object for current data and error message
                QueueErrorMessage queueItemWithErrorMessage = new()
                {
                    Data = myQueueItem,
                    ErrorMessage = ex.Message
                };

                string updatedQueueItem = JsonConvert.SerializeObject(queueItemWithErrorMessage);

                _queueService.MoveToPoisonQueueFromMainQueue(mainQueue, poisonQueue, Id, PopReceipt, updatedQueueItem);
                return;
            }
        }

        private static void CustomerProcess(ILogger log, OrdersTransaction orderTransaction, int? company)
        {
            KTI.Moo.FO.Domain.Customer Customerdomain = new((int)company);

            Customerdomain.upsert(orderTransaction, log).GetAwaiter().GetResult();
        }

        private static D365FOConfig ConfigProcess(ILogger log, int? company)
        {
            Helper.FOConfig foConfig = new Helper.FOConfig((int)company);

            return foConfig.GetAllConfig(log).GetAwaiter().GetResult();
        }

        private static void StoreTransactionProcess(ILogger log, string decodedString, int? company, string domainType)
        {
            if (domainType == Base.Helpers.DomainType.order)
            {
                log.LogInformation($"Config Process");

                config = ConfigProcess(log, company);

                var DomainJObject = JsonConvert.DeserializeObject<OrdersTransaction>(decodedString);

                DomainJObject.Config = config;

                log.LogInformation($"Header Process");

                StoreTransactionHeaderProcess(log, DomainJObject, company);

                log.LogInformation($"Lines Process");

                StoreTransactionDetailProcess(log, DomainJObject, company);

                log.LogInformation($"Discounts Process");

                StoreTransactionDiscountProcess(log, DomainJObject, company);

                log.LogInformation($"Tenders Process");

                StoreTransactionPaymentProcess(log, DomainJObject, company);

            }
        }

        private static void StoreTransactionHeaderProcess(ILogger log, OrdersTransaction orderTransaction, int? company)
        {
            Moo.FO.Domain.StoreTransactionsHeader headerDomain = new((int)company);

            headerDomain.upsert(orderTransaction, log).GetAwaiter().GetResult();
        }

        private static void StoreTransactionDetailProcess(ILogger log, OrdersTransaction orderTransaction, int? company)
        {
            Moo.FO.Domain.StoreTransactionsDetails linesDomain = new((int)company);

            linesDomain.upsert(orderTransaction, log).GetAwaiter().GetResult();
        }

        private static void StoreTransactionPaymentProcess(ILogger log, OrdersTransaction orderTransaction, int? company)
        {
            Moo.FO.Domain.StoreTransactionsPayments tendersDomain = new((int)company);

            tendersDomain.upsert(orderTransaction, log).GetAwaiter().GetResult();
        }

        private static void StoreTransactionDiscountProcess(ILogger log, OrdersTransaction orderTransaction, int? company)
        {
            KTI.Moo.FO.Domain.StoreTransactionsDiscount discountTransDomain = new((int)company);

            discountTransDomain.upsert(orderTransaction, log).GetAwaiter().GetResult();
        }
    }
}

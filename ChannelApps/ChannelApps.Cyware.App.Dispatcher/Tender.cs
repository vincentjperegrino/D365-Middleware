using System;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using KTI.Moo.Extensions.Core.Helper;
using Newtonsoft.Json;
using Moo.Models.Dtos.Items;
using KTI.Moo.Extensions.Core.Service;
using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using KTI.Moo.ChannelApps.Cyware.App.Dispatcher.Helpers;

namespace KTI.Moo.Cyware.App.Dispatcher
{
    public class Tender : CompanySettings
    {
        private readonly ITenderToQueue _dispatcherToQueue;
        private readonly IQueueService _queueService;
        private const string QueueName = "rdf-cyware-channelapp-tender-dispatcher";

        public Tender(IQueueService queueService, ITenderToQueue dispatcherToQueue)
        {
            _queueService = queueService;
            _dispatcherToQueue = dispatcherToQueue;
        }

        //[FunctionName("Tender")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Tender queue trigger function processed at " + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd hh:mm tt"));

                // Deserialize the data into a Queue Content object

                QueueMessageContent payload = JsonConvert.DeserializeObject<QueueMessageContent>(myQueueItem);

                // Deserialize the data into specified models
                PaymentMethod d365_tender = JsonConvert.DeserializeObject<PaymentMethod>(payload.Body);

                Extensions.Cyware.Model.Tender tender = new()
                {
                    tender_cd = d365_tender.tender_cd ?? "",
                    tender_type_cd = d365_tender.tender_type_cd ?? "",
                    description = d365_tender.Description ?? "",
                    is_default = "",
                    currency_cd = "",
                    validation_spacing = "",
                    max_change = "",
                    change_currency_code = "",
                    mms_code = "",
                    display_subtotal = "",
                    min_amount = d365_tender.min_amount ?? "",
                    max_amount = d365_tender.max_amount ?? "",
                    is_layaway_refund = "",
                    max_refund = "",
                    refund_type = "",
                    is_mobile_payment = "",
                    is_account = "",
                    acct_type_code = d365_tender.acct_type_code ?? "",
                    is_manager = "",
                    garbage_tender_cd = "",
                    rebate_tender_cd = "",
                    rebate_percent = "",
                    is_cashfund = "",
                    is_takeout = "",
                    item_code = "",
                    surcharge_sku = "",
                    mobile_payment_number = "",
                    mobile_payment_return = "",
                    is_padss = "",
                    is_credit_card = "",
                    eft_port = "",
                    is_voucher = "",
                    discount_cd = d365_tender.discount_cd ?? ""
                };
                string jsonString = JsonConvert.SerializeObject(tender);
                Process(jsonString, "moo-cyware-extension-tender-dispatcher", ConnectionString, CompanyID);
            }
            catch (Exception ex)
            {
                if (dequeueCount > 5)
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
                throw new Exception(ex.Message);
            }
        }

        public bool Process(string decodedString, string QueueName, string ConnectionString, string Companyid)
        {
            return _dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, Companyid);
        }
    }
}

using Azure.Storage.Queues;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using KTI.Moo.ChannelApps.Cyware.App.Dispatcher.Helpers;
using KTI.Moo.Extensions.Core.Helper;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Moo.Models.Dtos.Items;
using Newtonsoft.Json;
using System;

namespace KTI.Moo.Cyware.App.Dispatcher
{
    public class TenderType : CompanySettings
    {
        private readonly ITenderTypeToQueue _dispatcherToQueue;
        private readonly IQueueService _queueService;
        private const string QueueName = "rdf-cyware-channelapp-tendertype-dispatchers";

        public TenderType(IQueueService queueService, ITenderTypeToQueue dispatcherToQueue)
        {
            _queueService = queueService;
            _dispatcherToQueue = dispatcherToQueue;
        }

        //[FunctionName("TenderType")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                log.LogInformation($"C# TenderType queue trigger function processed at " + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd hh:mm tt"));

                // Deserialize the data into a Queue Content object
                QueueMessageContent payload = JsonConvert.DeserializeObject<QueueMessageContent>(myQueueItem);

                // Deserialize the data into specified models
                PaymentMethodType _tender_d365 = JsonConvert.DeserializeObject<PaymentMethodType>(payload.Body);

                Extensions.Cyware.Model.TenderType tender = new()
                {
                    /////Mapping here
                    TenderTypeCd = _tender_d365.tender_type_cd,
                    Description = _tender_d365.description ?? "",
                    AllowChange = _tender_d365.description.ToLower().Contains("cash") ? "1" : "0",
                    RequireValidation = _tender_d365.require_validation ?? "0",
                    IsCreditCard = _tender_d365.description.ToLower().Contains("credit") ? "1" : "0",
                    IsGc = (_tender_d365.description.ToLower().Contains("gift check") || _tender_d365.description.ToLower().Contains("gc")) ? "1" : "0",
                    Skey = _tender_d365.skey ?? "0",
                    IsDrawer = _tender_d365.description.ToLower().Contains("cash") ? "1" : "0",
                    IsDebitCard = _tender_d365.description.ToLower().Contains("debit") ? "1" : "0",
                    IsCheck = _tender_d365.description.ToLower().Contains("cheque") ? "1" : "0",
                    IsCharge = _tender_d365.description.ToLower().Contains("family") ? "1" : "0",
                    IsCash = _tender_d365.description.ToLower().Contains("cash") ? "1" : "0",
                    IsGarbage = _tender_d365.is_garbage ?? "0",
                    IsCashdec = _tender_d365.description.ToLower().Contains("cash") ? "1" : "0",
                    IsRebate = _tender_d365.is_rebate ?? "0",
                    IsEgc = _tender_d365.description.ToLower().Contains("electronic gift check") ? "1" : "0",
                    IsTelcoPull = _tender_d365.is_telco_pull ?? "0",
                    IsTelcoPush = _tender_d365.is_telco_push ?? "0",
                    IsGuarantor = _tender_d365.is_guarantor ?? "0",
                    IsCardConnect = _tender_d365.description.ToLower().Contains("emp") ? "1" : "0",
                    IsGovRebate = _tender_d365.is_gov ?? "0",
                    ItemCode = _tender_d365.item_code ?? "0",
                    IsIntegrated = _tender_d365.is_integrated ?? "0",
                    IsLoyalty = _tender_d365.description.ToLower().Contains("loyalty") ? "1" : "0",
                    IsHoreca = _tender_d365.description.ToLower().Contains("horeca") ? "1" : "0",
                    IsMobile = _tender_d365.description.ToLower().Contains("gcash") || _tender_d365.description.ToLower().Contains("maya") ? "1" : "0"
                };
                string jsonString = JsonConvert.SerializeObject(tender);
                Process(jsonString, "moo-cyware-extension-tendertype-dispatcher", ConnectionString, CompanyID);
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

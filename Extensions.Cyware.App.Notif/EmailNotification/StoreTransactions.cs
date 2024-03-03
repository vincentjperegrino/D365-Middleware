using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.Extensions.Core.Service;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace KTI.Moo.Extensions.Cyware.App.Queue.EmailNotification
{
    public class StoreTransactions : CompanySettings
    {
        private readonly IEmailNotification _emailNotification;
        private readonly IQueueService _queueService;

        public StoreTransactions(IEmailNotification emailNotification, IQueueService queueService)
        {
            _emailNotification = emailNotification;
            _queueService = queueService;
        }

        [FunctionName("StoreTransactions-EmailNotification")]
        public void Run([TimerTrigger(TimerTriggerConfig)] TimerInfo myTimer, ILogger log)
        {
            string className = typeof(StoreTransactions).Name;
            try
            {
                var retrievedMessages = _queueService.RetrieveAndPopMessage(ConnectionString, "poll-storetransactions-queue-poison", ExtensionArchiveQueueName, 32, log);
                if (retrievedMessages.Count() > 0)
                {
                    string messageBody = "Good Day,\r\n\r\nKindly refer to the attached csv file containing the list of error/s that occured during integration with Dynamics 365 Finance & Operations.\n\nFor further assistance or support, please contact your administrator.";

                    //Send EmailNotification...
                    _emailNotification.Notify("[Transaction] Failed Integration", messageBody, retrievedMessages, $"{className}ErrorLogs", log);
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
            }
        }
    }
}

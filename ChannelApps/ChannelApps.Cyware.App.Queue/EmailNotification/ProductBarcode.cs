using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.Service;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace KTI.Moo.ChannelApps.Cyware.App.Queue.EmailNotification
{
    public class ProductBarcode : CompanySettings
    {
        private readonly IEmailNotification _emailNotification;
        private readonly IQueueService _queueService;
        private readonly INotification _teamsNotification;
        public ProductBarcode(IEmailNotification emailNotification, IQueueService queueService, INotification teamsNotification)
        {
            _emailNotification = emailNotification;
            _queueService = queueService;
            _teamsNotification = teamsNotification;
        }

        //[FunctionName("ProductBarcode-EmailNotification")]
        public void Run([TimerTrigger(TimerTriggerConfig)] TimerInfo myTimer, ILogger log)
        {
            string className = typeof(ProductBarcode).Name;

            try
            {
                var retrievedMessages = _queueService.RetrieveAndPopMessage(ConnectionString, "rdf-cyware-channelapp-productbarcode-dispatcher-poison", ChannelArchiveQueueName, 32, log);
                if (retrievedMessages.Count() > 0)
                {
                    if (config.NotificationType.ToLower() == "email")
                    {
                        //Send EmailNotification...
                        var notifResult = _emailNotification.Notify($"{className}ErrorLogs", "Please see attached file.", retrievedMessages, $"{className}ErrorLogs", log);
                    }
                    else if (config.NotificationType.ToLower() == "teams")
                    {
                        //Send TeamsNotification...
                        var notifResult = _teamsNotification.Notify(config.TeamsWebHook, $"{className} ErrorLogs", JsonConvert.SerializeObject(retrievedMessages), log);
                    }
                    else
                    {
                        //Send to all channels
                        _emailNotification.Notify($"{className}ErrorLogs", "Please see attached file.", retrievedMessages, $"{className}ErrorLogs", log);
                        _teamsNotification.Notify(config.TeamsWebHook, $"{className} ErrorLogs", JsonConvert.SerializeObject(retrievedMessages), log);
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
            }
        }

    }
}

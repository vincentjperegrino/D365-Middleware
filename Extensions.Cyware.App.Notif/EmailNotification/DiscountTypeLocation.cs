using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.Service;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace KTI.Moo.Extensions.Cyware.App.Queue.EmailNotification
{
    public class DiscountTypeLocation : CompanySettings
    {
        private readonly IEmailNotification _emailNotification;
        private readonly IQueueService _queueService;
        private readonly INotification _teamsNotification;

        public DiscountTypeLocation(IEmailNotification emailNotification, IQueueService queueService, INotification teamsNotification)
        {
            _emailNotification = emailNotification;
            _queueService = queueService;
            _teamsNotification = teamsNotification;
        }


        //[FunctionName("DiscountTypeLocation-EmailNotification")]
        public void Run([TimerTrigger(TimerTriggerConfig)] TimerInfo myTimer, ILogger log)
        {
            string className = typeof(DiscountTypeLocation).Name;
            try
            {
                var retrievedMessages = _queueService.RetrieveAndPopMessage(ConnectionString, "moo-cyware-extension-discounttypelocation-dispatcher-poison", ExtensionArchiveQueueName, 32, log);
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

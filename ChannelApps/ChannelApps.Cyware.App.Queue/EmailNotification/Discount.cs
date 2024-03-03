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
    public class Discount : CompanySettings
    {
        private readonly IEmailNotification _emailNotification;
        private readonly IQueueService _queueService;
        private readonly INotification _teamsNotification;
        public Discount(IQueueService queueService, IEmailNotification emailNotification, INotification teamsNotification)
        {
            _queueService = queueService;
            _emailNotification = emailNotification;
            _teamsNotification = teamsNotification;
        }

        //[FunctionName("Discount-EmailNotification")]
        public void Run([TimerTrigger(TimerTriggerConfig)] TimerInfo myTimer, ILogger log)
        {
            string className = typeof(Discount).Name;

            try
            {
                var retrievedMessages = _queueService.RetrieveAndPopMessage(ConnectionString, "rdf-cyware-channelapp-forex-dispatcher-poison", ChannelArchiveQueueName, 32, log);
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

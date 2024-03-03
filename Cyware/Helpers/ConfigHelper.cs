using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Cyware.Helpers
{
    public class ConfigHelper
    {
        public static KTI.Moo.Extensions.Cyware.Services.Config Get ()
        {
            int _port, _smtpPort = 0;
            
            int.TryParse(Environment.GetEnvironmentVariable("config_port"), out _port);
            int.TryParse(Environment.GetEnvironmentVariable("smtp_port"), out _smtpPort);


            return new()
            {
                Host = Environment.GetEnvironmentVariable("config_host"),
                Port = _port,
                Username = Environment.GetEnvironmentVariable("config_username"),
                Password = Environment.GetEnvironmentVariable("config_password"),
                RootFolder = Environment.GetEnvironmentVariable("config_rootfolder"),

                SMTP_Port = _smtpPort,
                SMTP_Server = Environment.GetEnvironmentVariable("smtp_server"),
                SMTP_SenderEmail = Environment.GetEnvironmentVariable("smtp_senderEmail"),
                SMTP_SenderPassword = Environment.GetEnvironmentVariable("smtp_senderPassword"),
                SMTP_Receivers = Environment.GetEnvironmentVariable("smtp_receiver"),
                SMTP_CC = Environment.GetEnvironmentVariable("smtp_cc"),
                SMTP_Cyware = Environment.GetEnvironmentVariable("smtp_cyware"),


                TeamsWebHook = Environment.GetEnvironmentVariable("teamsNotificationWebHook"),

                NotificationType = Environment.GetEnvironmentVariable("notificationType"),

                ConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage"),
                //ConnectionString = Environment.GetEnvironmentVariable("SITConnectionString")
                BlobStorage = Environment.GetEnvironmentVariable("BlobStorage"),
                ExtensionProcessorBlobContainerSubFolder = Environment.GetEnvironmentVariable("blob_containerSubfolder"),
                ExtensionProcessorBlobContainer = Environment.GetEnvironmentVariable("blob_container")

            };
        }
    }
}

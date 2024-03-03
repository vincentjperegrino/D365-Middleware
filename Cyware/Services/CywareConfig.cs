using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.Services
{
    public class Config : ConfigBase
    {
        /// <summary>
        /// SFTP Configs
        /// </summary>
        public string RootFolder { get; init; }
        public string Host { get; init; }
        public int Port { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }

        /// <summary>
        /// SMTP Configs
        /// </summary>
        public string SMTP_Server { get; init; }
        public int SMTP_Port { get; init; }
        public string SMTP_SenderEmail { get; init; }
        public string SMTP_SenderPassword { get; init; } 
        public string SMTP_Receivers { get; init; }
        public string SMTP_CC { get; init; }
        public string SMTP_Cyware { get; init; }

        /// <summary>
        /// Azure Storage
        /// </summary>
        public string AzureConnectionString { get; init; }


        /// <summary>
        /// Teams Webhooks
        /// </summary>
        public string TeamsWebHook { get; init; }

        public string NotificationType { get; init; }

        /// <summary>
        /// Azure Blob Storage
        /// </summary>
        public string ConnectionString { get; init; }
        public string BlobStorage { get; init; }
        public string ExtensionProcessorBlobContainer { get; init; }
        public string ExtensionProcessorBlobContainerSubFolder { get; init; }
    }
}

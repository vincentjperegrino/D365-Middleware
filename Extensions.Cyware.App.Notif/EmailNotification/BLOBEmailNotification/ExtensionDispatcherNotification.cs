using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.Cyware.Helpers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Moo.Queue;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.App.Queue.EmailNotification.BLOBEmailNotification
{
    public class ExtensionDispatcherNotification : CompanySettings
    {
        private const string QueueName = "moo-cyware-extension-error";
        private readonly IEmailNotification _emailNotification;
       
        public ExtensionDispatcherNotification(IEmailNotification emailNotification)
        {
            _emailNotification = emailNotification;
        }

        [FunctionName("Extension-Dispatcher-Notification")]
        public async Task RunAsync([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, ILogger log)
        {

            // Gmail SMTP server configuration
            string smtpServer = config.SMTP_Server;                 //"smtp.gmail.com";
            int smtpPort = config.SMTP_Port;                        //587;
            string senderEmail = config.SMTP_SenderEmail;           //"allanoidzjr@gmail.com";
            string senderPassword = config.SMTP_SenderPassword;     //"anylytuwpuwkhzho";
            string receiverEmail = config.SMTP_Receivers;           //"allanrobertjaranilla@gmail.com";

            JObject messageObject = JsonConvert.DeserializeObject<JObject>(myQueueItem);
            var dataToken = messageObject["Data"];
            var errorMessage = messageObject["ErrorMessage"];
            
            var messageBody = JsonConvert.DeserializeObject<QueueProcessorModel>(dataToken.ToString());
            //string fileName = messageBody.FileName;
            string fileName = messageBody.FileName; //string.Join('_', messageBody.FileName.Split('_').Take(3));

            string fullFilePath = "/pollfiles/" + fileName; // Construct the full file path within the subfolder

            /// Instantiate BLOB Client
            BlobServiceClient blobServiceClient = new BlobServiceClient(config.ConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("moopollfiles");
            BlobClient blobClient = containerClient.GetBlobClient(fullFilePath);

            // Download the blob data into a MemoryStream
            BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();
            MemoryStream stream = new MemoryStream();
            await blobDownloadInfo.Content.CopyToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);

            // Create a new email message
            MailMessage mail = new MailMessage(senderEmail, receiverEmail);
            mail.Subject = $"{messageBody.ModuleType.ToUpper()} SFTP Integration Error - Action Required";
            mail.Body = $"This is an automated notification to inform you that the attached {messageBody.ModuleType.ToUpper()} POLLFILE have encountered an SFTP integration error. \n ErrorMessage: {errorMessage} \n \n Please contact your System Administrator.";
            mail.CC.Add(config.SMTP_CC);

            // Attach the MemoryStream as an attachment to the email
            log.LogInformation("Sending error email.");
            Attachment attachment = new Attachment(stream, fileName);
            mail.Attachments.Add(attachment);

            // Create an SMTP client to send the email
            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
            smtpClient.EnableSsl = true;

            try
            {
                // Send the email
                if (mail.Attachments.Count > 0)
                {
                    smtpClient.Send(mail);
                    log.LogInformation($"Email sent successfully. With attachments.");
                }
            }
            catch (Exception ex)
            {
                log.LogError($"An error occurred while sending the email with attachments." + ex.Message);
                //return false;
            }
        }
    } 
}

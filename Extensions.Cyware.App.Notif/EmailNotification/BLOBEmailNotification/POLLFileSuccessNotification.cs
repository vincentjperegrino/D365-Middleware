using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.Service;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace KTI.Moo.Extensions.Cyware.App.Queue.EmailNotification.BLOBEmailNotification
{
    public class POLLFileSuccessNotification : CompanySettings
    {

        private readonly IBlobService _blobService;

        public POLLFileSuccessNotification(IBlobService blobService)
        {
            _blobService = blobService;
        }

        [FunctionName("POLLFile-Success-Notification")]
        public async Task RunAsync([TimerTrigger(TimerTriggerConfig)] TimerInfo myTimer, ILogger log)
        {
            // Gmail SMTP server configuration
            string smtpServer = config.SMTP_Server;                 //"smtp.gmail.com";
            int smtpPort = config.SMTP_Port;                        //587;
            string senderEmail = config.SMTP_SenderEmail;           //"allanoidzjr@gmail.com";
            string senderPassword = config.SMTP_SenderPassword;     //"anylytuwpuwkhzho";
            string receiverEmail = config.SMTP_Receivers;           //"allanrobertjaranilla@gmail.com";

            // Create a new email message
            MailMessage mail = new MailMessage(senderEmail, receiverEmail);
            mail.Subject = "POLL SuccessNotification";
            mail.Body = "Please see attachments.";
            mail.CC.Add(config.SMTP_CC);

            try
            {
                // Create a BlobServiceClient using the connection string
                BlobServiceClient blobServiceClient = new BlobServiceClient(config.ConnectionString);
 

                // Get a reference to the container
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("successpollfiles");
                await containerClient.CreateIfNotExistsAsync();

                await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
                {
                    if (!blobItem.Name.Contains("archive"))
                    {
                        string blobName = blobItem.Name;
                        BlobClient blobClient = containerClient.GetBlobClient(blobName);

                        // Download the blob data into a MemoryStream
                        BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();
                        MemoryStream stream = new MemoryStream();
                        await blobDownloadInfo.Content.CopyToAsync(stream);
                        stream.Seek(0, SeekOrigin.Begin);

                        // Attach the MemoryStream as an attachment to the email
                        Attachment attachment = new Attachment(stream, blobName);
                        mail.Attachments.Add(attachment);

                        string destinationBlobName = $"archive/{blobName}";

                        _blobService.MoveBlob("successpollfiles", blobName, destinationBlobName);
                    }
                }

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
                    //return true;
                }
                catch (Exception ex)
                {
                    log.LogError($"An error occurred while sending the email with attachments." + ex.Message);
                    //return false;
                }
            }

            catch (Exception ex)
            {
                log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}

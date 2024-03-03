using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Cyware.Model.Models;
using KTI.Moo.Extensions.Cyware.Processor;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.App.Dispatcher.Processor
{
    public class ExtensionAppProcessor : CompanySettings
    {
        public const string QueueName = "moo-cyware-extension-dispatcher";
        public const string ErrorQueueName = "moo-cyware-extension-error";
        private readonly IExtensionAppProcessorDomain _extensionDomain;
        

        public ExtensionAppProcessor(IExtensionAppProcessorDomain extensionDomain)
        {
            _extensionDomain = extensionDomain;
        }

        [FunctionName("ExtensionApp-Processor")]
        public async Task RunAsync([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, string Id, string PopReceipt, int dequeueCount, ILogger log)
        {

            bool isProcessSuccessful = false;
            int retryCounter = 0; 
            TimeSpan initialRetryDelay = TimeSpan.FromSeconds(5); // Initial retry delay is 1 second


            var queueItem = JsonConvert.DeserializeObject<ExtensionQueueMessageModel>(myQueueItem);
            string fullFilePath = config.ExtensionProcessorBlobContainerSubFolder + queueItem.FileName; // Construct the full file path within the subfolder
            log.LogInformation($"Processing {queueItem.Count} records of {queueItem.ModuleType}: {queueItem.FileName}.");
            log.LogInformation($"Downloading {queueItem.ModuleType} details  from Extension BLOB.");


            // Get the POLL file from BLOB container..using myQueueItem.FileName. Read the message and send to SFTP service. 
            BlobServiceClient blobServiceClient = new BlobServiceClient(config.BlobStorage);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("moopollfiles");
            BlobClient blobClient = containerClient.GetBlobClient(fullFilePath);
            string distinationFileName = $"{queueItem.FileName.Split('~')[0]}.DWN";

            while (isProcessSuccessful == false)
            {
                try
                {
                    log.LogInformation($"Writing {queueItem.ModuleType} Pollfile to SFTP Server.");
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        await blobClient.DownloadToAsync(memoryStream);
                        string fileContent = Encoding.UTF8.GetString(memoryStream.ToArray());

                        //// Write to SFPT Server
                        isProcessSuccessful = await _extensionDomain.WritePollFileAsync(distinationFileName, fileContent);
                        log.LogInformation($"Writing {queueItem.ModuleType} Pollfile to SFTP Server Successful.");

                        if (isProcessSuccessful)
                        {
                            memoryStream.Seek(0, SeekOrigin.Begin);
                            // Move Blob file to subfolder of the blobClient named "archive"
                            BlobContainerClient archiveContainer = containerClient;
                            BlobClient archiveBlobClient = archiveContainer.GetBlobClient($"{config.ExtensionProcessorBlobContainerSubFolder}archive/{queueItem.FileName}");

                            // Upload the blob to the "archive" subfolder
                            await archiveBlobClient.UploadAsync(memoryStream);
                            log.LogInformation($"Blob file moved to 'archive' subfolder successfully.");

                            // Delete the original blobFile
                            await blobClient.DeleteIfExistsAsync();
                        }
                        break;
                    }
                }
                catch (Exception ex)
                {
                    ///Ssnd notification after 5 dequeue and 5 retries on 5th dequeue.. 
                    if (dequeueCount >= 5)
                    {
                        log.LogInformation($"Sending {queueItem.FileName} to {ErrorQueueName} queue.");

                        // Create object for current data and error message
                        var queueItemWithErrorMessage = new
                        {
                            Data = myQueueItem,
                            ErrorMessage = ex.Message
                        };

                        string updatedQueueItem = JsonConvert.SerializeObject(queueItemWithErrorMessage);

                        /// Initialize QueueClients
                        QueueClient mainQueueClient = InitiateQueeueClient(QueueName, config.ConnectionString);
                        QueueClient errorQueueClient = InitiateQueeueClient(ErrorQueueName, config.ConnectionString);


                        // Send the message to the poison queue
                        errorQueueClient.SendMessage(updatedQueueItem);

                        // If both messageId and popReceipt are provided, delete the message from the main queue
                        if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(PopReceipt))
                        {
                            // Delete the message from the main queue
                            mainQueueClient.DeleteMessage(Id, PopReceipt);
                        }
                        isProcessSuccessful = true;
                        break;
                    }

                    /// Retry when error..
                    retryCounter++;
                    log.LogError($"Something went wrong while processsing {queueItem.ModuleType}: {queueItem.FileName}.{ex.Message}");
                    isProcessSuccessful = false;

                    // Calculate the next retry delay with exponential backoff
                    TimeSpan retryDelay = initialRetryDelay * (1 << (int)Math.Min(retryCounter, 3));
                    log.LogInformation($"DequeueCount: {dequeueCount} | RetryCount: {retryCounter}  -- Retrying {queueItem.FileName} in {retryDelay.TotalSeconds} seconds...");
                    await Task.Delay(retryDelay);
                }
            }

        }

        public QueueClient InitiateQueeueClient(string QueueName, string ConnectionString)
        {
            QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExists();
            return queueClient;
        }
    }
}

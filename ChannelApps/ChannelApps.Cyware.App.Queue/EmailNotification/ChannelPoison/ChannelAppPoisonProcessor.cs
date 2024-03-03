using Azure.Storage.Blobs;
using ChannelApps.Cyware.Helpers;
using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.ChannelApps.Cyware;
using KTI.Moo.Extensions.Core.Helper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace KTI.MooChannelApps.Cyware.App.Queue.EmailNotification.ChannelPoison
{
    public class ChannelAppPoisonProcessor : ChannelAppsCywareCompanySettings
    {
        private readonly IEmailNotification _emailNotification;

        public ChannelAppPoisonProcessor(IEmailNotification emailNotification)
        {
            _emailNotification = emailNotification;
        }

        [FunctionName("ChannelApp-Poison-Processor")]
        public async Task RunAsync([QueueTrigger("%PoisonQueueName%", Connection = "AzureWebJobsStorage")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ChannelApp-Processor queue trigger function processed at " + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd hh:mm tt"));
            // Deserialize the order data into QueueProcessor Model
            var processorModel = JsonConvert.DeserializeObject<QueueProcessorModel>(myQueueItem);
            string moduleType = processorModel.ModuleType;
            try
            {
                log.LogInformation($"C# Processing {processorModel.Count} error records of {processorModel.ModuleType}: {processorModel.FileName}.");
                string fullFilePath = config.PoisonContainerSubFolderName + processorModel.FileName; // Construct the full file path within the subfolder

                BlobServiceClient blobServiceClient = new BlobServiceClient(config.BlobStorageConnectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("moopollfiles");
                BlobClient blobClient = containerClient.GetBlobClient(fullFilePath);


                log.LogInformation($"Processing  {moduleType} error Pollfile to SFTP Server.");
                string fileContent = await ReadStream(blobClient);

                var dictionary = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(fileContent); 

                // Now, result contains the data in the desired format
                var emailResult = _emailNotification.Notify($"[Masterdata] {moduleType?.ToUpper() ?? " "} ErrorLogs", "Please see attached file.", dictionary, $"{processorModel.ModuleType}", log);

            }
            catch (Exception ex)
            {
                log.LogError($"Something went wrong while processsing {processorModel.ModuleType}: {processorModel.FileName}:  {ex.Message}.");
                throw ex;
            }
        }


        public async Task<string> ReadStream(BlobClient blobClient)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await blobClient.DownloadToAsync(memoryStream);
                string fileContent = Encoding.UTF8.GetString(memoryStream.ToArray());
                return fileContent;
            }
        }
    }
}

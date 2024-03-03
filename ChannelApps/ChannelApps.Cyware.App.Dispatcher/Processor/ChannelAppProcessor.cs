using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ChannelApps.Cyware.Helpers;
using ChannelApps.Cyware.Services;
using KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor;
using KTI.Moo.ChannelApps.Cyware.Services;
using KTI.Moo.Extensions.Core.Helper;
using KTI.Moo.Extensions.Cyware.Model.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Cyware.App.Dispatcher.Processor
{
    public class ChannelAppProcessor : ChannelAppsCywareCompanySettings
    {
        private readonly IChannelAppBlobService _blobService;
        private readonly IChannelAppQueueService _queueService;

        public ChannelAppProcessor(IChannelAppBlobService blobService, IChannelAppQueueService queueService)
        {
            _blobService = blobService;
            _queueService = queueService;
        }

        [FunctionName("ChannelApp-Processor")]
        public async Task RunAsync([QueueTrigger("%ChannelAppQueueName%", Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                log.LogInformation($"C# ChannelApp-Processor queue trigger function processed at " + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd hh:mm tt"));

                // Deserialize the order data into QueueProcessor Model
                var processorModel = JsonConvert.DeserializeObject<QueueProcessorModel>(myQueueItem);
                log.LogInformation($"C# Processing {processorModel.Count} records of {processorModel.ModuleType}: {processorModel.FileName}.");
                // Download the poll file from blobstorage, use processorModel.FileName from queue Payload
                BlobClient blobClient = _blobService.InitializeBlob(processorModel.FileName);
                string pollContent = await _blobService.ReadStreamFileAsync(blobClient);

                // Create a processor based on the module type

                var processorFactory = new ChannelAppProcessorFactory();

                var moduleProcessor = processorFactory.CreateModuleProcessor(processorModel.ModuleType);

                // Process the module using the selected processor
                ProcessorReturnModel mapperResult = moduleProcessor.ProcessAsync(pollContent, log);

                // Write back to blob the mapped and formatted records but in different directory. 
                string formattedPhtTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time")).ToString("yyyyMMdd_HHmmss");

                // Process Successful results
                await ProcessSuccessResultsAsync(formattedPhtTime, processorModel, mapperResult, log);

                // Prcess Error results
                await ProcessErrorResultsAsync(formattedPhtTime, processorModel, mapperResult, log);

                log.LogInformation("Moving processed file to archive folder.");
                MovePollFileToArchive(processorModel.FileName, config.ContainerName, log);
            }
            catch (Exception ex)
            {
                log.LogError("Something went wrong while processing.", ex.Message);
                throw ex;
            }
        }

        public async Task ProcessSuccessResultsAsync(string formattedPhtTime, QueueProcessorModel processorModel, ProcessorReturnModel mapperResult, ILogger log)
        {
            Response<BlobContentInfo> successResultBlobResponse = null;

            if (mapperResult.SuccessReturnModel != null && mapperResult.SuccessReturnModel.Count > 0)
            {
                // Process Successful Results
                log.LogInformation($"Processing successfull mapping of {processorModel.FileName}.");

                // Make POLL + BatchID as FileName
                string PollName = processorModel.FileName.Split('_')[0];
                string SuccessPollFullPath = $"pollfiles/{PollName}";


                // Upsert the Success results.
                log.LogInformation($"Upsert the blob file. FileName: {PollName}.");
                BlobClient upsertBlobClient = _blobService.InitializeBlob(SuccessPollFullPath);
                successResultBlobResponse = await _blobService.UpsertBlobAsync(upsertBlobClient, mapperResult.SuccessReturnModel.ConcatenatedResult);

                // Check if batchNumber = totalBatchCount then proceed to sending command to extension app..
                // IF writing to blobSuccessfull.Continue now to sending the message to Extension Queue. 
                if (successResultBlobResponse != null && successResultBlobResponse.GetRawResponse().IsError != true && 
                    processorModel.BatchNumberr == processorModel.TotalBatchCount)
                {
                    // Build ExtensionMessage
                    var extensionQueueMessageObj = new ExtensionQueueMessageModel(PollName, processorModel.ModuleType, mapperResult.SuccessReturnModel.Count, config.companyid);
                    var extensionQueueMessageString = JsonConvert.SerializeObject(extensionQueueMessageObj);

                    // Send to QueueClient of extension
                    log.LogInformation($"Sending message to Extension Dispatcher queue. FileName: {PollName}.");
                    _queueService.DispatchMessage(extensionQueueMessageString, config.ExtensionQueueName, config.QueueStorageConnectionString, config.companyid);
                }
            }

        }

        public async Task ProcessErrorResultsAsync(string formattedPhtTime, QueueProcessorModel processorModel, ProcessorReturnModel mapperResult, ILogger log)
        {
            if (mapperResult.ErrorReturnModel != null && mapperResult.ErrorReturnModel.Count() > 0)
            {
                // Process Error Results
                // Insert to Error blob > send to poison queue.
                string ErrorPollFileName = $"{processorModel.FileName}_{formattedPhtTime}_error";
                string ErrorPollFullPath = $"pollfiles/errorfiles/{ErrorPollFileName}";
                BlobClient errorBloblClient = _blobService.InitializeBlob(ErrorPollFullPath);
                var errorResultBlobResponse = await _blobService.CreateBlobAsync(errorBloblClient, JsonConvert.SerializeObject(mapperResult.ErrorReturnModel.ToList()));

                if (errorResultBlobResponse != null && errorResultBlobResponse.GetRawResponse().IsError != true)
                {
                    log.LogInformation($"Processing error mapping of {processorModel.FileName}.");
                    /// Build ExtensionMessage
                    var extensionQueueMessageObj = new ExtensionQueueMessageModel(ErrorPollFileName, processorModel.ModuleType, mapperResult.ErrorReturnModel.Count(), config.companyid);
                    var poisonQueueMessageString = JsonConvert.SerializeObject(extensionQueueMessageObj);

                    // Send to QueueClient of extension
                    _queueService.DispatchMessage(poisonQueueMessageString, config.PoisonQueueName, config.QueueStorageConnectionString, config.companyid);
                }
            }
        }

        public void MovePollFileToArchive(string blobName, string containerName, ILogger log)
        {
            log.LogInformation($"Moving {blobName} to archive folder.");
            string destinationBlobName = $"archive/{blobName}";
            _blobService.MoveBlob(containerName, blobName, destinationBlobName);
        }
    }
}


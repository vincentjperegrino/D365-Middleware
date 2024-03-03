using System;
using System.Collections.Generic;
using System.ComponentModel;
using KTI.Moo.Extensions.BlobService.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace TestBlobService
{
    public class Function1
    {
        [FunctionName("Function1")]
        public void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string createdContent = "not created!";
            string updatedContent = "not updated!";
            bool blobExistence = false;
            bool moveStatus = false;
            bool deleteStatus = false;
            KTI.Moo.Extensions.BlobService.Models.BlobStorageInfo.BlobContainer customerContainer = KTI.Moo.Extensions.BlobService.Models.BlobStorageInfo.BlobContainer.customer;

            BlobService blobService = new BlobService("3388");

            // List Files
            //List<string> blobNames = blobService.ListFiles(customerContainer);
            //foreach (string blobName in blobNames)
            //{
            //    log.LogInformation($"Blob name: {blobName}");
            //}

            // Read File
            //string blobContents = blobService.ReadFile(customerContainer, "test.txt");
            //log.LogInformation($"Blob contents: {blobContents}");

            // Create File
            //if (blobService.CreateFile(customerContainer, "test2.txt", blobContents))
            //{
            //    createdContent = "created!";
            //}
            //log.LogInformation($"Blob creation: {createdContent}");

            // Replace Text in File
            //if (blobService.ReplaceTextInFile(customerContainer, "test2.txt", "test", "test2"))
            //{
            //    updatedContent = "updated!";
            //}
            //log.LogInformation($"Blob update: {updatedContent}");

            // Check Blob Existence
            //if (blobService.CheckBlobExists(customerContainer, "testfolder/test2.txt"))
            //{
            //    blobExistence = true;
            //}
            //log.LogInformation($"Blob existence: {blobExistence}");

            // Move file within a container
            //if (blobService.MoveBlobWithinContainer(customerContainer, "test.txt", "newfolder/destination.txt"))
            //{
            //    moveStatus = true;
            //}
            //log.LogInformation($"Blob move status: {moveStatus}");

            // Delete blob or blob folder
            //if (blobService.DeleteBlob(customerContainer, "testfolder"))
            //{
            //    deleteStatus = true;
            //}
            //log.LogInformation($"Blob delete status: {deleteStatus}");
        }
    }
}

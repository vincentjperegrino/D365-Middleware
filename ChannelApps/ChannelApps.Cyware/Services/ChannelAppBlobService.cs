using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ChannelApps.Cyware.Helpers;
using KTI.Moo.Extensions.Cyware.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System.Text;

namespace KTI.Moo.ChannelApps.Cyware.Services
{
    public class ChannelAppBlobService : ChannelAppsCywareCompanySettings, IChannelAppBlobService
    {
        public async Task<Response<BlobContentInfo>> CreateBlobAsync(BlobClient destinationBlobClient, string payload)
        {

            if (!payload.Contains("<EOF>"))
            {
                payload = payload + "<EOF>";
            }

            // Convert the JSON string to bytes
            byte[] byteArray = Encoding.UTF8.GetBytes(payload);

            // Create a memory stream from the byte array
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                // Upload the JSON data to the blob and return the receipt
                Response<BlobContentInfo> blobReceipt = await destinationBlobClient.UploadAsync(stream, true);
                return blobReceipt;
            }
        }

        public BlobClient InitializeBlob(string fileName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(config.BlobStorageConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(config.ContainerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            return blobClient;
        }

        public bool MoveBlob(string containerName, string sourceBlobName, string destinationBlobName)
        {
            try
            {
                // Get a reference to the container.
                BlobServiceClient blobServiceClient = new BlobServiceClient(config.BlobStorageConnectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Get references to the source and destination blobs.
                BlobClient sourceBlobClient = containerClient.GetBlobClient(sourceBlobName);
                BlobClient destinationBlobClient = containerClient.GetBlobClient(destinationBlobName);

                // Copy the source blob to the destination blob.
                destinationBlobClient.StartCopyFromUri(sourceBlobClient.Uri);

                // Wait for the copy to complete.
                while (true)
                {
                    // Get the properties of the destination blob.
                    BlobProperties destinationBlobProperties = destinationBlobClient.GetProperties();

                    // If the copy operation is complete, break out of the loop.
                    if (destinationBlobProperties.CopyStatus != CopyStatus.Pending)
                    {
                        break;
                    }

                    // Wait for a short time before checking again.
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }

                // Delete the source blob.
                sourceBlobClient.DeleteIfExists();

                // Blob moved successfully.
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> ReadStreamFileAsync(BlobClient blobClient)
        {
            //Download file to stream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await blobClient.DownloadToAsync(memoryStream);

                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        public async Task<string> GetBlobFileNameAsync(BlobContainerClient blobContainerClient, string fileName)
        {
            string directoryPath = "pollfiles/";
            string directoryPathArchive = "pollfiles/archive/";
            string directoryPathError = "pollfiles/errorfiles/";
           

            await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync())
            {
                // Check if the blob is in the specified directory
                if (blobItem.Name.StartsWith(directoryPath, StringComparison.OrdinalIgnoreCase) &&
                    !blobItem.Name.StartsWith(directoryPathArchive, StringComparison.OrdinalIgnoreCase) &&
                    !blobItem.Name.StartsWith(directoryPathError, StringComparison.OrdinalIgnoreCase))
                {
                    string blobName = blobItem.Name.Substring(0, blobItem.Name.IndexOf('_', blobItem.Name.IndexOf('_') + 1));

                    // Check conditions to exclude "archive" and "errorfiles" subfolders
                    if (blobName.ToLower() == fileName.ToLower() &&
                        !blobItem.Name.ToLower().Contains("/archive/") &&
                        !blobItem.Name.ToLower().Contains("/errorfiles/"))
                    {
                        return blobItem.Name;
                    }
                }
            }

            return string.Empty;
        }

        public async Task<Response<BlobContentInfo>> UpdateBlobAsync(BlobClient blobClient, string payload)
        {
            // Read the existing content
            using (var streamReader = new StreamReader(await blobClient.OpenReadAsync()))
            {
                string existingContent = await streamReader.ReadToEndAsync();

                // Remove the existing <EOF> and add the new content...
                string updatedContent = existingContent.Contains("<EOF>") ? existingContent.Replace("<EOF>", "") + payload : existingContent + payload;

                // Upload the updated content...
                using (var streamWriter = new StreamWriter(new MemoryStream()))
                {
                    await streamWriter.WriteAsync(updatedContent);
                    streamWriter.Flush();
                    streamWriter.BaseStream.Position = 0;

                    return await blobClient.UploadAsync(streamWriter.BaseStream, true);
                }
            }
        }


        public async Task<Response<BlobContentInfo>> UpsertBlobAsync(BlobClient blobClient, string payload)
        {
            // Check if the blob exists
            bool blobExists = await blobClient.ExistsAsync();

            if (blobExists)
            {
                // Read the existing content
                using (var streamReader = new StreamReader(await blobClient.OpenReadAsync()))
                {
                    string existingContent = await streamReader.ReadToEndAsync();

                    // Remove the existing <EOF> and add the new content...
                    string updatedContent = existingContent.Contains("<EOF>") ? existingContent.Replace("<EOF>", "") + payload : existingContent + payload;

                    // Upload the updated content...
                    using (var streamWriter = new StreamWriter(new MemoryStream()))
                    {
                        await streamWriter.WriteAsync(updatedContent);
                        streamWriter.Flush();
                        streamWriter.BaseStream.Position = 0;

                        return await blobClient.UploadAsync(streamWriter.BaseStream, true);
                    }
                }
            }
            else
            {
                // If the blob doesn't exist, create it with the new content
                using (var streamWriter = new StreamWriter(new MemoryStream()))
                {
                    await streamWriter.WriteAsync(payload);
                    streamWriter.Flush();
                    streamWriter.BaseStream.Position = 0;

                    return await blobClient.UploadAsync(streamWriter.BaseStream, true);
                }
            }
        }

    }
}

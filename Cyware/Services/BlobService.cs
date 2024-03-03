using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using KTI.Moo.Extensions.Core.Service;
using System.Globalization;
using System.Text;

namespace KTI.Moo.Extensions.Cyware.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient blobServiceClient;

        public BlobService(Config config)
        {
            blobServiceClient = new BlobServiceClient(config.ConnectionString);
        }

        public string ReadStreamFile(Stream stream)
        {
            try
            {
                StreamReader reader = new StreamReader(stream);
                string oldContent = reader.ReadToEnd();

                return oldContent;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                throw new Exception($"Error reading blob: {ex.Message}");
            }
        }   

        public bool MoveBlob(string containerName, string sourceBlobName, string destinationBlobName)
        {
            try
            {
                // Get a reference to the container.
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
                // Handle the exception here
                throw new Exception($"Error moving blob: {ex.Message}");
            }
        }

        public async Task <bool> CreateFile(string containerName, string blobName, string blobContent)
        {
            try
            {
                // Get a reference to the container.
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Get a reference to the blob.
                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                // Convert the string to a byte array using UTF-8 encoding.
                byte[] contentBytes = Encoding.UTF8.GetBytes(blobContent);

                // Upload the file to the blob.
                using (MemoryStream stream = new MemoryStream(contentBytes))
                {
                   await blobClient.UploadAsync(stream, true);
                }
                return true;
            }
            catch (Exception ex)
            {
                // handle the exception here, e.g. log the error
                throw new Exception($"Error creating blob: {ex.Message}");
            }
        }

        public string ReadFile(string containerName, string blobName)
        {
            try
            {
                // Get a reference to the container
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Get a reference to the blob
                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                // Read the contents of the blob
                using MemoryStream stream = new MemoryStream();
                blobClient.DownloadTo(stream);
                stream.Position = 0;
                using StreamReader streamReader = new StreamReader(stream);
                string blobContents = streamReader.ReadToEnd();

                return blobContents;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                throw new Exception($"Error downloading blob: {ex.Message}");
            }
        }

        public List<string> ListFiles(string containerName, string directoryPath = "")
        {
            try
            {
                // Get a reference to the container
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName.ToString());

                if (string.IsNullOrEmpty(directoryPath))
                {
                    // List all the blobs in the container
                    var blobs = containerClient.GetBlobs();

                    // Extract the blob names from the blob list
                    List<string> blobNames = blobs.Select(b => b.Name).ToList();

                    return blobNames;
                }
                else
                {
                    // List the blobs in the container with the specified directory path
                    var blobs = containerClient.GetBlobs(prefix: directoryPath);

                    // Extract the blob names from the blob list
                    List<string> blobNames = blobs.Select(b => b.Name).ToList();

                    return blobNames;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error listing files in blob container: {ex.Message}");
            }
        }

        public List<string> GetFiles(string containerName, string directoryPath = "", string dateFilter = "")
        {
            try
            {
                // Get a reference to the container
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // List the blobs based on the directory path
                var blobs = string.IsNullOrEmpty(directoryPath)
                    ? containerClient.GetBlobs().ToList()
                    : containerClient.GetBlobs(prefix: directoryPath).ToList();

                // Apply date filter if provided
                if (!string.IsNullOrEmpty(dateFilter) && dateFilter.Length == 8)
                {
                    // Filter blobs based on the provided date filter
                    blobs = blobs.Where(blob =>
                        blob.Properties.LastModified.HasValue &&
                        blob.Properties.LastModified.Value.Date.ToString("yyyyMMdd") == dateFilter).ToList();
                }

                // Create a list of filenames from the blobs
                List<string> fileNames = blobs.Select(blob => blob.Name).ToList();

                return fileNames; // Return the list of filenames
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception($"Error listing files in blob container: {ex.Message}");
            }
        }
    }
}

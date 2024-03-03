using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using KTI.Moo.Extensions.BlobService.Interfaces;
using System.Text;

namespace KTI.Moo.Extensions.BlobService.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient blobServiceClient;

        public BlobService(string connectionString)
        {
            blobServiceClient = new BlobServiceClient(connectionString);
        }

        public void ReadFilePOLLLOG(string containerName, string blobName)
        {
            try
            {
                string pollLogRecord = this.ReadFile(containerName, blobName);

                string[] records = pollLogRecord.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string record in records)
                {
                    string[] fields = record.Split('/');

                    if (fields.Length >= 9)
                    {
                        string recordType = fields[8];
                        switch (recordType)
                        {
                            case "05":
                                // Sales header record
                                Console.WriteLine("Sales Header Record:");
                                Console.WriteLine(record);
                                Console.WriteLine();
                                break;
                            case "04":
                                // Sales detail record
                                Console.WriteLine("Sales Detail Record:");
                                Console.WriteLine(record);
                                Console.WriteLine();
                                break;
                            case "03":
                                // Tender detail record
                                Console.WriteLine("Tender Detail Record:");
                                Console.WriteLine(record);
                                Console.WriteLine();
                                break;
                            case "11":
                                // Cash out totals record
                                Console.WriteLine("Cash Out Totals Record:");
                                Console.WriteLine(record);
                                Console.WriteLine();
                                break;
                            case "12":
                                // Cash in record
                                Console.WriteLine("Cash In Record:");
                                Console.WriteLine(record);
                                Console.WriteLine();
                                break;
                            case "99":
                                // Reason code record
                                Console.WriteLine("Reason Code Record:");
                                Console.WriteLine(record);
                                Console.WriteLine();
                                break;
                            default:
                                // Unknown record type
                                Console.WriteLine("Unknown Record Type:");
                                Console.WriteLine(record);
                                Console.WriteLine();
                                break;
                        }
                    }
                    else
                    {
                        // Invalid record
                        Console.WriteLine("Invalid Record:");
                        Console.WriteLine(record);
                        Console.WriteLine();
                    }
                }
            }
            catch (RequestFailedException ex)
            {
                // Handle the exception here
                Console.WriteLine($"Error downloading blob: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public string ReadStreamFile(Stream stream)
        {
            try
            {
                StreamReader reader = new StreamReader(stream);
                string oldContent = reader.ReadToEnd();

                return oldContent;
            }
            catch (RequestFailedException ex)
            {
                // Handle the exception here
                Console.WriteLine($"Error downloading blob: {ex.Message}");
                return null; // or throw a custom exception
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
            catch (RequestFailedException ex)
            {
                // Handle the exception here
                Console.WriteLine($"Error downloading blob: {ex.Message}");
                return null; // or throw a custom exception
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
                Console.WriteLine($"Error listing files in blob container: {ex.Message}");
                throw;
            }
        }

        public bool CreateFile(string containerName, string blobName, string blobContent)
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
                    blobClient.Upload(stream, true);
                }
                return true;
            }
            catch (Exception ex)
            {
                // handle the exception here, e.g. log the error
                Console.WriteLine($"Error creating file on Azure Blob Storage container: {ex.Message}");
                return false;
            }
        }

        public bool ReplaceTextInFile(string containerName, string blobName, string searchKeyword, string newText)
        {
            try
            {
                // Get a reference to the container.
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Get a reference to the blob.
                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                // Download the file content as a string
                string fileContent;
                using (MemoryStream stream = new MemoryStream())
                {
                    blobClient.DownloadTo(stream);
                    fileContent = Encoding.UTF8.GetString(stream.ToArray());
                }

                // Replace the search string with the new string
                fileContent = fileContent.Replace(searchKeyword, newText);

                // Convert the updated content to a byte array using UTF-8 encoding.
                byte[] contentBytes = Encoding.UTF8.GetBytes(fileContent);

                // Upload the updated content to the blob.
                using (MemoryStream stream = new MemoryStream(contentBytes))
                {
                    blobClient.Upload(stream, true);
                }
                return true;
            }
            catch (Exception ex)
            {
                // handle the exception here, e.g. log the error
                Console.WriteLine($"Error replacing text in file on Azure Blob Storage container: {ex.Message}");
                return false;
            }
        }

        public bool CheckBlobExists(string containerName, string blobName)
        {
            try
            {
                // Get a reference to the container.
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Check if the path exists as a blob
                if (containerClient.GetBlobClient(blobName).Exists())
                {
                    return true;
                }

                // Check if the path exists as a folder
                var blobs = containerClient.GetBlobs(prefix: blobName + "/");
                if (blobs.Any())
                {
                    return true;
                }

                // Path does not exist
                return false;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine($"Error checking blob existence: {ex.Message}");
                return false; // or throw a custom exception
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
                Console.WriteLine($"Error moving blob: {ex.Message}");
                return false; // or handle the exception as appropriate for your application
            }
        }

        public bool DeleteBlob(string containerName, string blobName)
        {
            try
            {
                // Get a reference to the container
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Check if the path exists as a blob
                if (containerClient.GetBlobClient(blobName).Exists())
                {
                    containerClient.DeleteBlob(blobName);
                    return true;
                }

                // Check if the path exists as a folder
                var blobs = containerClient.GetBlobs(prefix: blobName + "/");
                if (blobs.Any())
                {
                    // Delete all blobs inside the virtual directory
                    foreach (BlobHierarchyItem blobItem in containerClient.GetBlobsByHierarchy(prefix: blobName))
                    {
                        containerClient.DeleteBlob(blobItem.Blob.Name);
                    }
                    return true;
                }

                return false;
            }
            catch (RequestFailedException ex)
            {
                // Handle the exception here
                Console.WriteLine($"Error deleting blob: {ex.Message}");
                return false;
            }
        }

    }
}

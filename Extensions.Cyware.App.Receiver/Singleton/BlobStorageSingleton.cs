using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;

namespace Extensions.Cyware.App.Receiver.Singleton
{
    public sealed class BlobStorageSingleton
    {
        private static readonly Lazy<BlobStorageSingleton> lazyInstance = new Lazy<BlobStorageSingleton>(() => new BlobStorageSingleton());

        private CloudBlobClient blobClient;
        private readonly string _blobConnString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

        private BlobStorageSingleton()
        {
            // Initialize the Azure Blob Storage client
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_blobConnString);
            blobClient = storageAccount.CreateCloudBlobClient();
        }

        public static BlobStorageSingleton Instance { get { return lazyInstance.Value; } }

        public CloudBlobClient BlobClient { get { return blobClient; } }
    }
}

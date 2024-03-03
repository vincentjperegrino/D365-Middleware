using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Cyware.Services
{
    public interface IChannelAppBlobService
    {
        public Task<string> ReadStreamFileAsync(BlobClient blobClient);
        public bool MoveBlob(string containerName, string ssourceBlobName, string destinationBlobName);
        public BlobClient InitializeBlob(string fileName);

        public Task<Response<BlobContentInfo>> CreateBlobAsync(BlobClient blobClient, string payload);

        public Task<string> GetBlobFileNameAsync(BlobContainerClient bloblClient, string fileName);

        public Task<Response<BlobContentInfo>> UpdateBlobAsync(BlobClient blobClient, string payload);

        public Task<Response<BlobContentInfo>> UpsertBlobAsync(BlobClient blobClient, string payload);
    }
}

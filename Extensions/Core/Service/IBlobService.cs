using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Service
{
    public interface IBlobService
    {
        public string ReadStreamFile(Stream stream);
        public bool MoveBlob(string containerName, string sourceBlobName, string destinationBlobName);
        Task<bool> CreateFile(string containerName, string blobName, string blobContent);
        public string ReadFile(string containerName, string blobName);
        public List<string> ListFiles(string containerName, string directoryPath = "");
        public List<string> GetFiles(string containerName, string directoryPath = "", string dateFilter = "");
    }
}

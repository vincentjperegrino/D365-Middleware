using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.BlobService.Interfaces
{
    public interface IBlobService
    {
        public void ReadFilePOLLLOG(string containerName, string blobName);
        public string ReadStreamFile(Stream stream);
        public string ReadFile(string containerName, string blobName);
        public List<string> ListFiles(string containerName, string directoryPath = "");
        public bool CreateFile(string containerName, string blobName, string blobContent);
        public bool ReplaceTextInFile(string containerName, string blobName, string searchKeyword, string newText);
        public bool CheckBlobExists(string containerName, string blobName);
        public bool MoveBlob(string containerName, string sourceBlobName, string destinationBlobName);
        public bool DeleteBlob(string containerName, string blobName);
    }
}

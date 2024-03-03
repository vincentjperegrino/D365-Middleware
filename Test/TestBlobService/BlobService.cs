using Xunit;

namespace TestBlobService
{
    public class BlobService : BaseTest
    {
        private readonly KTI.Moo.Extensions.BlobService.Services.BlobService blobService = new(connectionString);
        private readonly string customerContainer = "customer";
        private static readonly string unitTestDir = "Unit Test";
        private static readonly string destinationDir = $"{unitTestDir}/destination";
        private static readonly string fileName = $"BlobLibrary-{DateTime.Now:yyyy-MM-dd}.txt";
        private readonly string createdFilePath = $"{unitTestDir}/{fileName}";
        private readonly string destinationFilePath = $"{destinationDir}/{fileName}";
        private readonly string createdContent = "Created!";
        private readonly string updatedContent = "Updated!";

        [Fact]
        public void Test_CheckBlobExists()
        {
            if(!blobService.CheckBlobExists(customerContainer, createdFilePath))
            {
                blobService.CreateFile(customerContainer, createdFilePath, createdContent);
            }

            Assert.True(blobService.CheckBlobExists(customerContainer, createdFilePath));
            blobService.DeleteBlob(customerContainer, unitTestDir);
        }

        [Fact]
        public void Test_CreateFile()
        {
            Assert.True(blobService.CreateFile(customerContainer, createdFilePath, createdContent));
            blobService.DeleteBlob(customerContainer, unitTestDir);
        }

        [Fact]
        public void Test_MoveFile()
        {
            if (!blobService.CheckBlobExists(customerContainer, createdFilePath))
            {
                blobService.CreateFile(customerContainer, createdFilePath, createdContent);
            }

            Assert.True(blobService.MoveBlob(customerContainer, createdFilePath, destinationFilePath));
            blobService.DeleteBlob(customerContainer, destinationDir);
            blobService.DeleteBlob(customerContainer, unitTestDir);
        }

        [Fact]
        public void Test_ListFiles()
        {
            if (!blobService.CheckBlobExists(customerContainer, createdFilePath))
            {
                blobService.CreateFile(customerContainer, createdFilePath, createdContent);
            }
            Assert.IsType<List<string>>(blobService.ListFiles(customerContainer, unitTestDir));
            blobService.DeleteBlob(customerContainer, unitTestDir);
        }

        [Fact]
        public void Test_ReplaceTextInFile()
        {
            if (!blobService.CheckBlobExists(customerContainer, createdFilePath))
            {
                blobService.CreateFile(customerContainer, createdFilePath, createdContent);
            }
            Assert.True(blobService.ReplaceTextInFile(customerContainer, createdFilePath, createdContent, updatedContent));
            blobService.DeleteBlob(customerContainer, unitTestDir);
        }

        [Fact]
        public void Test_ReadFile()
        {
            if (!blobService.CheckBlobExists(customerContainer, createdFilePath))
            {
                blobService.CreateFile(customerContainer, createdFilePath, createdContent);
            }
            string actualBlobContents = blobService.ReadFile(customerContainer, createdFilePath);

            Assert.Contains(createdContent, actualBlobContents);
            blobService.DeleteBlob(customerContainer, unitTestDir);
        }

        [Fact]
        public void Test_DeleteBlob()
        {
            if (!blobService.CheckBlobExists(customerContainer, createdFilePath))
            {
                blobService.CreateFile(customerContainer, createdFilePath, createdContent);
            }
            blobService.DeleteBlob(customerContainer, unitTestDir);
            Assert.False(blobService.CheckBlobExists(customerContainer, createdFilePath));
        }
    }
}
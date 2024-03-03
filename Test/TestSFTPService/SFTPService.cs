using Xunit;

namespace TestSFTPService
{
    public class SFTPService
    {
        private readonly KTI.Moo.Extensions.SFTPService.Services.SFTPService sftpService = new("3388");
        private static readonly string rootDir = "/files/";
        private static readonly string unitTestDir = Path.Combine(rootDir, "UnitTest/");
        private static readonly string fileName = $"SFTPLibrary-{DateTime.Now:yyyy-MM-dd}.txt";
        private readonly string createdFilePath = Path.Combine(rootDir, fileName);
        private readonly string createdContent = "Created!";
        private readonly string updatedContent = "Updated!";

        [Fact]
        public void Test_CheckSftpFileOrFolderExists()
        {
            Assert.True(sftpService.CheckSftpFileOrFolderExists(rootDir));
        }

        [Fact]
        public void Test_CreateFile()
        {
            if (!sftpService.CheckSftpFileOrFolderExists(createdFilePath))
            {
                sftpService.CreateFile(createdFilePath, createdContent);
            }
            Assert.True(sftpService.CheckSftpFileOrFolderExists(createdFilePath));
            sftpService.DeleteSftpFileOrFolder(createdFilePath);
        }

        [Fact]
        public void Test_CreateDirectoryIfNotExists()
        {
            Assert.True(sftpService.CreateDirectoryIfNotExists(unitTestDir));
            sftpService.DeleteSftpFileOrFolder(unitTestDir);
        }

        [Fact]
        public void Test_MoveFile()
        {
            if (!sftpService.CheckSftpFileOrFolderExists(createdFilePath))
            {
                sftpService.CreateFile(createdFilePath, createdContent);
            }

            sftpService.CreateDirectoryIfNotExists(unitTestDir);

            Assert.True(sftpService.MoveFile(createdFilePath, unitTestDir));
            sftpService.DeleteSftpFileOrFolder(unitTestDir);
        }

        [Fact]
        public void Test_ListFiles()
        {
            sftpService.CreateDirectoryIfNotExists(rootDir);
            Assert.IsType<List<Renci.SshNet.Sftp.SftpFile>>(sftpService.ListFiles(rootDir));
        }

        [Fact]
        public void Test_ReplaceTextInFile()
        {
            if (!sftpService.CheckSftpFileOrFolderExists(createdFilePath))
            {
                sftpService.CreateFile(createdFilePath, createdContent);
            }
            Assert.True(sftpService.ReplaceTextInFile(createdFilePath, createdContent, updatedContent));
            sftpService.DeleteSftpFileOrFolder(createdFilePath);
        }

        [Fact]
        public void Test_ReadFile()
        {
            if (!sftpService.CheckSftpFileOrFolderExists(createdFilePath))
            {
                sftpService.CreateFile(createdFilePath, createdContent);
            }
            string actualFileContents = sftpService.ReadFile(createdFilePath);

            Assert.Contains(createdContent, actualFileContents);
            sftpService.DeleteSftpFileOrFolder(createdFilePath);
        }

        [Fact]
        public void Test_DeleteSftpFolder()
        {
            sftpService.CreateDirectoryIfNotExists(unitTestDir);
            sftpService.DeleteSftpFileOrFolder(unitTestDir);
            Assert.False(sftpService.CheckSftpFileOrFolderExists(unitTestDir));
        }
    }
}
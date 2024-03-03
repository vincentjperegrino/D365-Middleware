using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace TestSFTP
{
    public class Function1
    {
        [FunctionName("Function1")]
        public void Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            // instantiate SFTP service config
            var sftpService = new KTI.Moo.Extensions.SFTPService.Services.SFTPService("3388");

            // ReadFile
            string content = sftpService.ReadFile("/files/SFTPLibraryTest");
            log.LogInformation($"[ReadFile] /files/SFTPLibraryTest\n{content}");

            // ListFile
            var files = sftpService.ListFiles("/files/");
            List<string> fileNames = new List<string>();
            foreach (var file in files)
            {
                fileNames.Add(file.Name);
            }
            string fileLog = string.Join(", ", fileNames);
            log.LogInformation($"[ListFiles] /files\n{fileLog}");

            // CreateFile
            sftpService.CreateFile("/files/testfile.txt", "test1");
            string createdContent = sftpService.ReadFile("/files/testfile.txt");
            log.LogInformation($"[CreateFile] /files/testfile.txt\n{createdContent}");

            // ReplaceTextInFile
            if (sftpService.ReplaceTextInFile("/files/testfile.txt", "test1", "test2"))
            {
                string replacedContent = sftpService.ReadFile("/files/testfile.txt");
                log.LogInformation($"[ReplaceTextInFile] /files/testfile.txt\n{replacedContent}");
            }

            // CheckSftpFolderExists
            bool folderExistence = sftpService.CheckSftpFolderExists("/files");
            log.LogInformation($"[CheckSftpFolderExists] /files\n{folderExistence}");

            // CreateDirectoryIfNotExists
            bool folderCreation = sftpService.CreateDirectoryIfNotExists("/files/sftptesting");
            log.LogInformation($"[CreateDirectoryIfNotExists] /files/sftptesting\n{folderCreation}");

            // MoveFile
            bool movedFile = sftpService.MoveFile("/files/testfile.txt", "/files/sftptesting");
            log.LogInformation($"[MoveFile] /files/testfile.txt => /files/sftptesting\n{movedFile}");
        }
    }
}

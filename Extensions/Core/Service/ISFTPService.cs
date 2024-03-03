using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Service
{
    public  interface ISFTPService
    {
        public string readfile (string filepath);
        List<SftpFile> ListFiles(string filepath);
        public bool CreateFile(string filePath, string content);
        public bool CreateLocal(string filePath, string content);

        public bool ReplaceTextInFile(string filePath, string searchKeyword, string newText);
        public bool CheckSftpFileOrFolderExists(string folderPath);
        public bool CreateDirectoryIfNotExists(string folderPath);
        public bool MoveFile(string sourceFilePath, string destinationFolderPath);
        public bool DeleteSftpFileOrFolder(string path);

        public Task<bool> CreateFileAsync(string filePath, string content);
    }
}

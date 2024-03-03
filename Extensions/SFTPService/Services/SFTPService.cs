using KTI.Moo.Extensions.SFTPService.Services;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System.Text.RegularExpressions;
using static KTI.Moo.Extensions.SFTPService.Models.SFTPFileInfo;

namespace KTI.Moo.Extensions.SFTPService.Services
{
    public class SFTPService
    {
        private readonly SFTPConnectionInfo connectionInfo;

        public SFTPService(string companyID)
        {
            Config config = new Config();
            connectionInfo = config.GetConnectionInfo();
        }

        public string ReadFile(string filePath)
        {
            try
            {
                using (var client = new SftpClient(connectionInfo.host, connectionInfo.port, connectionInfo.username, connectionInfo.password))
                {
                    client.Connect();
                    using (var stream = new MemoryStream())
                    {
                        client.DownloadFile(filePath, stream);
                        if (stream.Length == 0)
                        {
                            throw new InvalidOperationException("File is empty.");
                        }
                        stream.Seek(0, SeekOrigin.Begin);
                        using (var reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file from SFTP server: {ex.Message}");
                return null;
            }
        }

        public List<SftpFile> ListFiles(string directoryPath)
        {
            try
            {
                using (var client = new SftpClient(connectionInfo.host, connectionInfo.port, connectionInfo.username, connectionInfo.password))
                {
                    client.Connect();
                    var files = client.ListDirectory(directoryPath).Where(f => !f.IsDirectory);
                    return files.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error listing files on SFTP server: {ex.Message}");
                throw;
            }
        }

        public bool CreateFile(string filePath, string content)
        {
            try
            {
                using (var client = new SftpClient(connectionInfo.host, connectionInfo.port, connectionInfo.username, connectionInfo.password))
                {
                    client.Connect();
                    using (var stream = client.AppendText(filePath))
                    {
                        stream.Write(content);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                // handle the exception here, e.g. log the error
                Console.WriteLine($"Error creating text file on SFTP server: {ex.Message}");
                return false;
            }
        }

        public bool ReplaceTextInFile(string filePath, string searchKeyword, string newText)
        {
            try
            {
                using (var client = new SftpClient(connectionInfo.host, connectionInfo.port, connectionInfo.username, connectionInfo.password))
                {
                    client.Connect();
                    var fileContent = client.ReadAllText(filePath);
                    var regex = new Regex(searchKeyword);
                    fileContent = regex.Replace(fileContent, newText);
                    if (fileContent != null && fileContent != client.ReadAllText(filePath))
                    {
                        client.WriteAllText(filePath, fileContent);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                // handle the exception here, e.g. log the error
                Console.WriteLine($"Error replacing text in file: {ex.Message}");
                return false;
            }
        }

        public bool CheckSftpFileOrFolderExists(string folderPath)
        {
            try
            {
                using (var client = new SftpClient(connectionInfo.host, connectionInfo.port, connectionInfo.username, connectionInfo.password))
                {
                    client.Connect();
                    return client.Exists(folderPath);
                }
            }
            catch (Exception ex)
            {
                // handle the exception here, e.g. log the error
                Console.WriteLine($"Error checking SFTP folder: {ex.Message}");
                return false;
            }
        }

        public bool CreateDirectoryIfNotExists(string folderPath)
        {
            try
            {
                if (string.IsNullOrEmpty(folderPath))
                {
                    Console.WriteLine("Error: Folder path cannot be empty or null.");
                    return false;
                }

                using (var client = new SftpClient(connectionInfo.host, connectionInfo.port, connectionInfo.username, connectionInfo.password))
                {
                    client.Connect();

                    // check if the directory exists
                    if (!client.Exists(folderPath))
                    {
                        // create the directory
                        client.CreateDirectory(folderPath);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                // handle the exception here, e.g. log the error
                Console.WriteLine($"Error creating SFTP folder: {ex.Message}");
                return false;
            }
        }

        public bool MoveFile(string sourceFilePath, string destinationFolderPath)
        {
            try
            {
                using (var client = new SftpClient(connectionInfo.host, connectionInfo.port, connectionInfo.username, connectionInfo.password))
                {
                    client.Connect();

                    destinationFolderPath = destinationFolderPath.EndsWith("/") ? destinationFolderPath : destinationFolderPath + "/";

                    // get the file name from the source file path
                    var fileName = Path.GetFileName(sourceFilePath);

                    // get the destination file path by combining the destination folder path and the file name
                    var destinationFilePath = Path.Combine(destinationFolderPath, fileName);

                    // move the file to the destination folder
                    client.RenameFile(sourceFilePath, destinationFilePath);

                    return true;
                }
            }
            catch (Exception ex)
            {
                // handle the exception here, e.g. log the error
                Console.WriteLine($"Error moving file on SFTP server: {ex.Message}");
                return false;
            }
        }

        public bool DeleteSftpFileOrFolder(string path)
        {
            try
            {
                using (var client = new SftpClient(connectionInfo.host, connectionInfo.port, connectionInfo.username, connectionInfo.password))
                {
                    client.Connect();
                    var attributes = client.GetAttributes(path);
                    if (attributes.IsDirectory)
                    {
                        // Delete all files in the directory
                        var files = client.ListDirectory(path);
                        foreach (var file in files)
                        {
                            if (!file.IsDirectory)
                            {
                                client.DeleteFile(file.FullName);
                            }
                        }

                        // Delete the directory
                        client.DeleteDirectory(path);
                    }
                    else
                    {
                        client.DeleteFile(path);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting SFTP file or folder: {ex.Message}");
                return false;
            }
        }
    }
}

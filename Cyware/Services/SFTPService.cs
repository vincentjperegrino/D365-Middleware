using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Services;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace KTI.Moo.Cyware.Services
{
    internal sealed class SFTPService : ISFTPService
    {
        private Config _config;
        public SFTPService(Config config)
        {
            this._config = config;
        }

        public bool CheckSftpFileOrFolderExists(string folderPath)
        {
            throw new NotImplementedException();
        }

        public bool CreateDirectoryIfNotExists(string folderPath)
        {
            throw new NotImplementedException();
        }

        public bool CreateFile(string fileName, string content)
        {

            try
            {
                //throw new Exception("SFTP Server Offline!");
                string filePath = Path.Combine(_config.RootFolder, fileName);
                using (var client = new SftpClient(_config.Host, _config.Port, _config.Username, _config.Password))
                {

                    client.Connect();
                    //Check if exist
                    if (!client.Exists(filePath))
                    {
                        //create first if not exist.
                        using (var stream = client.Create(filePath))
                        using (var writer = new StreamWriter(stream))
                        {
                            writer.Write(content); // + "<EOF>");
                            return true;
                        }
                    }
                    else
                    {
                        throw new Exception($"{filePath} still exist on SFTP Server.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here, e.g., log the error
                Console.WriteLine($"Error creating SFTP server: {ex.Message}");
                throw ex;
            }

        }

        public bool DeleteSftpFileOrFolder(string path)
        {
            throw new NotImplementedException();
        }

        public List<SftpFile> ListFiles(string filepath)
        {
            throw new NotImplementedException();
        }

        public bool MoveFile(string sourceFilePath, string destinationFolderPath)
        {
            throw new NotImplementedException();
        }

        public string readfile(string filepath)
        {
            throw new NotImplementedException();
        }

        public bool ReplaceTextInFile(string filePath, string searchKeyword, string newText)
        {
            throw new NotImplementedException();
        }

        public bool CreateLocal(string fileName, string content)
        {
            try
            {
                string filePath = Path.Combine("C:\\PollFiles", fileName);
                File.AppendAllText(filePath, content);
                return true;
            }
            catch (Exception ex)
            {
                // handle the exception here, e.g. log the error
                Console.WriteLine($"Error creating text file locally: {ex.Message}");
                throw ex;
            }
        }

        public async Task<bool> CreateFileAsync(string fileName, string content)
        {
            try
            {
                //throw new Exception("SFTP Server Offline!");
                string filePath = Path.Combine(_config.RootFolder, fileName);
                using (var client = new SftpClient(_config.Host, _config.Port, _config.Username, _config.Password))
                {

                    client.Connect();
                    //Check if exist
                    if (!client.Exists(filePath))
                    {
                        //create first if not exist.
                        using (var stream = client.Create(filePath))
                        using (var writer = new StreamWriter(stream))
                        {
                            await writer.WriteAsync(content); // + "<EOF>");
                            return true;
                        }
                    }
                    else
                    {
                        throw new Exception($"{filePath} still exist on SFTP Server.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here, e.g., log the error
                Console.WriteLine($"Error creating SFTP server: {ex.Message}");
                throw ex;
            }

            //try
            //{
            //    string filePath = Path.Combine("C:\\Users\\Windows\\Desktop\\POLL TEST\\", fileName);

            //    if (File.Exists(filePath))
            //    {
            //        throw new Exception($"{filePath} already exists in the local directory.");
            //    }

            //    using (StreamWriter writer = File.CreateText(filePath))
            //    {
            //        writer.Write(content);
            //    }

            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    // Handle the exception here, e.g., log the error
            //    Console.WriteLine($"Error writing to the local directory: {ex.Message}");
            //    throw ex;
            //}
        }
    }
}

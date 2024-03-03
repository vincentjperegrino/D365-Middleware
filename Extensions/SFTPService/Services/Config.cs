using Microsoft.Extensions.Configuration;
using System.ComponentModel.Design;
using System.Reflection;
using static KTI.Moo.Extensions.SFTPService.Models.SFTPFileInfo;

namespace KTI.Moo.Extensions.SFTPService.Services
{
    public class Config
    {
        public SFTPConnectionInfo GetConnectionInfo()
        {
            IConfigurationRoot Config = null;
            //IConfigurationSection Config = null;
            string binDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string currentDirectory = Directory.GetCurrentDirectory();

            if (File.Exists(Path.Combine(currentDirectory, "local.settings.json")))
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile(Path.Combine(currentDirectory, "local.settings.json"), optional: false)
                    .Build();
                //Config = config.GetSection(companyID ?? "3388");
                Config = config.GetSection("Values");
            }

            if (Config is null)
            {
                Config = new ConfigurationBuilder()
                    .AddJsonFile(Path.Combine(binDirectory, "appsettings.json"), optional: false)
                    .Build();
                Config = config.GetSection(companyID);
            }
             
            if (Config is null)
            {
                throw new ArgumentNullException(nameof(Config), $"Configuration section for \'Values\' was not found or has not been defined.");
            }

            string host = Config.GetSection("host").Value ?? throw new ArgumentNullException(nameof(host), $"Configuration section for Company ID: {companyID} does not contain a value for host.");
            int port = string.IsNullOrWhiteSpace(Config.GetSection("port").Value) ? 22 : int.Parse(Config.GetSection("port").Value);
            string username = Config.GetSection("username").Value ?? throw new ArgumentNullException(nameof(username), $"Configuration section for Company ID: {companyID} does not contain a value for username.");
            string password = Config.GetSection("password").Value ?? throw new ArgumentNullException(nameof(password), $"Configuration section for Company ID: {companyID} does not contain a value for password.");

            return new SFTPConnectionInfo
            {
                host = host,
                port = port,
                username = username,
                password = password
            };
        }
    }
}

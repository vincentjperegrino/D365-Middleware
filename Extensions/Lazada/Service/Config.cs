using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace KTI.Moo.Extensions.Lazada.Service
{
    internal sealed class Config
    {
        private static Config _instance = null;
        private static readonly object _lock = new();
        private readonly string _appKey = "";
        private readonly string _appSecret = "";
        private readonly int _retries = 5;
        private readonly string _baseUrl = "";
        private readonly string _redis = "";
        private readonly int _MaxPaginationInFinanceAPI = 500;

        public string AppKey { get => _appKey; }
        public string AppSecret { get => _appSecret; }
        public int MaxRetries { get => _retries; }
        public string BaseSourceUrl { get => _baseUrl; }
        public string RedisConnectionString { get => _redis; }
        public int MaxPaginationInFinanceAPI { get => _MaxPaginationInFinanceAPI; }

        public Config()
        {
            IConfigurationSection lazConfig = null;
            var bin = AppDomain.CurrentDomain.BaseDirectory;
            var currentDirectory = Directory.GetCurrentDirectory();

            // use appsettings.json if it contains LazadaConfig
            if (File.Exists(Path.Combine(currentDirectory, "appsettings.json")))
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile(Path.Combine(currentDirectory, "appsettings.json"), optional: false)
                    .Build();
                lazConfig = config.GetSection("LazadaConfig");
            }
            // fallback to classlib-provided lazadaconfig.json otherwise
            if (lazConfig is null)
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile(Path.Combine(bin, "lazadaconfig.json"), optional: false)
                    .Build();
                lazConfig = config.GetSection("LazadaConfig");
            }

            if (lazConfig is null)
                throw new ArgumentNullException(nameof(lazConfig), "Configuration section for Lazada was not found or has not been defined.");

            _appKey = lazConfig.GetSection("AppKey").Value;
            _appSecret = lazConfig.GetSection("AppSecret").Value;
            _retries = int.Parse(lazConfig.GetSection("MaxRetries").Value);
            _baseUrl = lazConfig.GetSection("BaseSourceUrl").Value;
            _redis = lazConfig.GetSection("RedisConnectionString").Value;
            _MaxPaginationInFinanceAPI = int.Parse(lazConfig.GetSection("MaxPaginationInFinanceAPI").Value);
        }

        public static Config Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Config();
                    }
                    return _instance;
                }
            }
        }
    }
}

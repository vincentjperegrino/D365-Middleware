

using Microsoft.Extensions.Configuration;
using System.IO;

namespace KTI.Moo.Extensions.SAP.Service;

public class Config : Core.Service.ConfigBase
{
    private static Config _instance = null;
    private static readonly object _lock = new();

    public string companyDB { get; init; }
    public string username { get; init; }
    public string password { get; init; }


    //new  public string defaultURL { get; init; }
    //new  public string redisConnectionString { get; init; }
    //new  public string companyid { get; init; }

    public Config()
    {

    }

    //public Config(string configname)
    //{
    //    IConfigurationSection Config = null;
    //    var bin = AppDomain.CurrentDomain.BaseDirectory;
    //    var currentDirectory = Directory.GetCurrentDirectory();

    //    // use appsettings.json if it contains LazadaConfig
    //    if (File.Exists(Path.Combine(currentDirectory, "appsettings.json")))
    //    {
    //        var config = new ConfigurationBuilder()
    //            .AddJsonFile(Path.Combine(currentDirectory, "appsettings.json"), optional: false)
    //            .Build();
    //        Config = config.GetSection(configname);
    //    }



    //    if (Config is null)
    //    {
    //        var config = new ConfigurationBuilder()
    //            .AddJsonFile(Path.Combine(bin, "magentoconfig.json"), optional: false)
    //            .Build();
    //        Config = config.GetSection(configname);
    //    }

    //    if (Config is null)
    //    {
    //        throw new ArgumentNullException(nameof(Config), "Configuration section for Magento was not found or has not been defined.");
    //    }


    //    username = Config.GetSection("username").Value;
    //    password = Config.GetSection("password").Value;
    //    defaultURL = Config.GetSection("defaultURL").Value;
    //    redisConnectionString = Config.GetSection("redisConnectionString").Value;
    //    companyid = Config.GetSection("companyid").Value;
    //}


    //public static Config Instance
    //{
    //    get
    //    {
    //        lock (_lock)
    //        {
    //            if (_instance == null)
    //            {
    //                _instance = new Config("ncci_config");
    //            }
    //            return _instance;
    //        }
    //    }
    //}




}

using KTI.Moo.Extensions.SAP.Service;


namespace KTI.Moo.ChannelApps.SAP.App.Dispatcher.Retry.Helpers;

public class ConfigHelper
{
    public static Config Get()
    {
        return new()
        {
                          
            defaultURL = System.Environment.GetEnvironmentVariable("config_defaultURL"),
            redisConnectionString = System.Environment.GetEnvironmentVariable("config_redisConnectionString"),
            password = System.Environment.GetEnvironmentVariable("config_password"),
            username = System.Environment.GetEnvironmentVariable("config_username"),
            companyDB = System.Environment.GetEnvironmentVariable("config_companyDB"),
            companyid = System.Environment.GetEnvironmentVariable("CompanyID"),
        };
    }




}

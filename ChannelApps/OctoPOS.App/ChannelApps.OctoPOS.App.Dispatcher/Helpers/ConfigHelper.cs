using KTI.Moo.Extensions.OctoPOS.Service;

namespace KTI.Moo.ChannelApps.OctoPOS.App.Dispatcher.Helpers;

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
            apiAuth = System.Environment.GetEnvironmentVariable("config_apiAuth"),
            companyid = System.Environment.GetEnvironmentVariable("CompanyID"),

        };
    }

}

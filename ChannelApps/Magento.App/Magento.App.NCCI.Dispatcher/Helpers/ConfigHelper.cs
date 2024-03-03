using KTI.Moo.Extensions.Magento.Service;

namespace KTI.Moo.ChannelApps.Magento.App.NCCI.Dispatcher.Helpers;

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
            companyid = System.Environment.GetEnvironmentVariable("CompanyID"),
        };
    }

}

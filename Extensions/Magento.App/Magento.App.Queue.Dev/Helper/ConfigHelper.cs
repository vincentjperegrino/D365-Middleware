
namespace KTI.Moo.Extensions.Magento.App.Queue.Dev.Helper;

public class ConfigHelper
{
    public static Config Get()
    {
        return new()
        {
                          
            defaultURL = System.Environment.GetEnvironmentVariable("Config_defaultURL"),
            redisConnectionString = System.Environment.GetEnvironmentVariable("Config_redisConnectionString"),
            password = System.Environment.GetEnvironmentVariable("Config_username"),
            username = System.Environment.GetEnvironmentVariable("Config_password"),
            companyid = System.Environment.GetEnvironmentVariable("CompanyID"),
        };
    }




}

using KTI.Moo.Extensions.Magento.Service;

namespace KTI.Moo.Extensions.Magento.App.NCCI.Queue.Helper;

public class ConfigHelper
{
    public static Config Get()
    {
        return new()
        {
                          
            defaultURL = System.Environment.GetEnvironmentVariable("ncci_defaultURL"),
            redisConnectionString = System.Environment.GetEnvironmentVariable("ncci_redisConnectionString"),
            password = System.Environment.GetEnvironmentVariable("ncci_password"),
            username = System.Environment.GetEnvironmentVariable("ncci_username"),
            companyid = System.Environment.GetEnvironmentVariable("CompanyID"),
        };
    }




}

using KTI.Moo.Extensions.OctoPOS.Service;

namespace KTI.Moo.Extensions.OctoPOS.App.Dispatcher.Helper
{
    public class ConfigHelper
    {
        public static Config Get()
        {
            return new()
            {
                apiAuth = System.Environment.GetEnvironmentVariable("config_apiAuth"),
                defaultURL = System.Environment.GetEnvironmentVariable("config_defaultURL"),
                redisConnectionString = System.Environment.GetEnvironmentVariable("config_redisConnectionString"),
                password = System.Environment.GetEnvironmentVariable("config_password"),
                username = System.Environment.GetEnvironmentVariable("config_username"),
                companyid = System.Environment.GetEnvironmentVariable("CompanyID"),
            };
        }




    }
}

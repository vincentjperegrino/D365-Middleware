using KTI.Moo.Extensions.Lazada.Service;


namespace KTI.Moo.Extensions.Lazada.App.Dispatcher.Helpers;

public class ConfigHelper
{
    public static KTI.Moo.Extensions.Lazada.Service.Queue.Config Get()
    {
        return new()
        {

            defaultURL = System.Environment.GetEnvironmentVariable("config_defaultURL"),
            redisConnectionString = System.Environment.GetEnvironmentVariable("config_redisConnectionString"),
            AppKey = System.Environment.GetEnvironmentVariable("config_AppKey"),
            AppSecret = System.Environment.GetEnvironmentVariable("config_AppSecret"),
            Region = System.Environment.GetEnvironmentVariable("config_Region"),
            SellerId = System.Environment.GetEnvironmentVariable("config_SellerId"), 
            companyid = System.Environment.GetEnvironmentVariable("CompanyID"),
            
        };
    }




}

using KTI.Moo.Base.Domain.Queue;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;


[assembly: FunctionsStartup(typeof(KTI.Moo.Extensions.SAP.App.Queue.Startup))]

namespace KTI.Moo.Extensions.SAP.App.Queue;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {


        builder.Services.AddSingleton<Core.Domain.IOrder<Model.Order, Model.OrderItem>>(_ =>
        {
            var Config = new KTI.Moo.Extensions.SAP.Service.Config()
            {
                defaultURL = System.Environment.GetEnvironmentVariable("config_defaultURL"),
                redisConnectionString = System.Environment.GetEnvironmentVariable("config_redisConnectionString"),
                password = System.Environment.GetEnvironmentVariable("config_password"),
                username = System.Environment.GetEnvironmentVariable("config_username"),
                companyDB = System.Environment.GetEnvironmentVariable("config_companyDB"),
                companyid = System.Environment.GetEnvironmentVariable("CompanyID"),
            };

            var DefaultOrderDomain = new KTI.Moo.Extensions.SAP.Domain.Order(Config);

            return new KTI.Moo.Extensions.Domain.NCCI.SAP.Order(DefaultOrderDomain);

        });


        builder.Services.AddSingleton<IPoisonNotification, KTI.Moo.CRM.Domain.Queue.PoisonNotification>();

    }
}

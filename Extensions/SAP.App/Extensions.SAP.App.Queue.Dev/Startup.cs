using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;


[assembly: FunctionsStartup(typeof(KTI.Moo.Extensions.SAP.App.Queue.Dev.Startup))]

namespace KTI.Moo.Extensions.SAP.App.Queue.Dev;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
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


        builder.Services.AddSingleton<Core.Domain.IOrder<Model.Order, Model.OrderItem>>(_ =>
        {

            var DefaultOrderDomain = new KTI.Moo.Extensions.SAP.Domain.Order(Config);

            return new KTI.Moo.Extensions.Domain.NCCI.SAP.Order(DefaultOrderDomain);

        });

        builder.Services.AddSingleton<Core.Domain.IInvoice<Model.Invoice, Model.InvoiceItem>>(_ =>
        {
            return new KTI.Moo.Extensions.SAP.Domain.Invoice(Config);

        });

    }

}

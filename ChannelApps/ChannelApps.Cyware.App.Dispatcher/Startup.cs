using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Services;
using KTI.Moo.Base.Domain;
using KTI.Moo.ChannelApps.Cyware.Services;
using ChannelApps.Cyware.Helpers;
using ChannelApps.Cyware.Services;

[assembly: FunctionsStartup(typeof(KTI.Moo.Cyware.App.Dispatcher.Startup))]
namespace KTI.Moo.Cyware.App.Dispatcher
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var CompanyID = Convert.ToInt32(Environment.GetEnvironmentVariable("CompanyID"));

            if (CompanyID == 3388)
            {
                // Queue Service
                builder.Services.AddTransient<IQueueService, QueueService>();

                //ChannelAppProcessor Injection
                builder.Services.AddSingleton(ChannelAppCywareConfigHelper.Get());
                builder.Services.AddSingleton<IChannelAppBlobService, ChannelAppBlobService>();
                builder.Services.AddSingleton<IChannelAppQueueService, ChannelAppQueueService>();


                // Function Class
                builder.Services.AddSingleton<ICustomerToQueue, ChannelApps.Model.RDF.Dispatchers.Customer>();
                builder.Services.AddSingleton<IPriceToQueue, ChannelApps.Model.RDF.Dispatchers.Price>();
                builder.Services.AddSingleton<IRegisterToQueue, ChannelApps.Model.RDF.Dispatchers.Register>();
                builder.Services.AddSingleton<ICurrencyToQueue, ChannelApps.Model.RDF.Dispatchers.Currency>();
                builder.Services.AddSingleton<IForExToQueue, ChannelApps.Model.RDF.Dispatchers.ForEx>();
                builder.Services.AddSingleton<IStoreToQueue, ChannelApps.Model.RDF.Dispatchers.Store>();
                builder.Services.AddSingleton<IDiscountToQueue, ChannelApps.Model.RDF.Dispatchers.Discount>();
                builder.Services.AddSingleton<IPriceLevelDetailToQueue, ChannelApps.Model.RDF.Dispatchers.PriceLevelDetail>();
                builder.Services.AddSingleton<ITenderToQueue, ChannelApps.Model.RDF.Dispatchers.Tender>();
                builder.Services.AddSingleton<ITenderTypeToQueue, ChannelApps.Model.RDF.Dispatchers.TenderType>();
                builder.Services.AddSingleton<IDiscountTypeToQueue, ChannelApps.Model.RDF.Dispatchers.DiscountType>();
                builder.Services.AddSingleton<IProductsToQueue, ChannelApps.Model.RDF.Dispatchers.Products>();
                builder.Services.AddSingleton<IDiscountProductToQueue, ChannelApps.Model.RDF.Dispatchers.DiscountProduct>();
                builder.Services.AddSingleton<IProductBarcodeToQueue, ChannelApps.Model.RDF.Dispatchers.ProductBarcode>();
                builder.Services.AddSingleton<IProductCategoryToQueue, ChannelApps.Model.RDF.Dispatchers.ProductCategory>();
                builder.Services.AddSingleton<IProductPriceToQueue, ChannelApps.Model.RDF.Dispatchers.ProductPrice>();
                builder.Services.AddSingleton<ITenderTypeToQueue, ChannelApps.Model.RDF.Dispatchers.TenderType>();
                builder.Services.AddSingleton<IDiscountLocationToQueue, ChannelApps.Model.RDF.Dispatchers.DiscountLocation>();
                builder.Services.AddSingleton<IBOMHeaderToQueue, ChannelApps.Model.RDF.Dispatchers.BOMHeader>();
                builder.Services.AddSingleton<IBOMVersionToQueue, ChannelApps.Model.RDF.Dispatchers.BOMVersion>();
                builder.Services.AddSingleton<IBOMLinesToQueue, ChannelApps.Model.RDF.Dispatchers.BOMLines>();
                builder.Services.AddSingleton<IDiscountTypeProductToQueue, ChannelApps.Model.RDF.Dispatchers.DiscountTypeProduct>();
                builder.Services.AddSingleton<IConfigurationGroupToQueue, ChannelApps.Model.RDF.Dispatchers.ConfigurationGroup>();


            }
            else if (CompanyID == 3392)
            {
                // Queue Service
                builder.Services.AddTransient<IQueueService, QueueService>();

                //ChannelAppProcessor Injection
                builder.Services.AddSingleton(ChannelAppCywareConfigHelper.Get());
                builder.Services.AddSingleton<IChannelAppBlobService, ChannelAppBlobService>();
                builder.Services.AddSingleton<IChannelAppQueueService, ChannelAppQueueService>();

                // Function Class
                builder.Services.AddSingleton<ICustomerToQueue, ChannelApps.Model.RDF.Dispatchers.Customer>();
                builder.Services.AddSingleton<IPriceToQueue, ChannelApps.Model.RDF.Dispatchers.Price>();
                builder.Services.AddSingleton<IRegisterToQueue, ChannelApps.Model.RDF.Dispatchers.Register>();
                builder.Services.AddSingleton<ICurrencyToQueue, ChannelApps.Model.RDF.Dispatchers.Currency>();
                builder.Services.AddSingleton<IForExToQueue, ChannelApps.Model.RDF.Dispatchers.ForEx>();
                builder.Services.AddSingleton<IStoreToQueue, ChannelApps.Model.RDF.Dispatchers.Store>();
                builder.Services.AddSingleton<IDiscountToQueue, ChannelApps.Model.RDF.Dispatchers.Discount>();
                builder.Services.AddSingleton<IPriceLevelDetailToQueue, ChannelApps.Model.RDF.Dispatchers.PriceLevelDetail>();
                builder.Services.AddSingleton<ITenderToQueue, ChannelApps.Model.RDF.Dispatchers.Tender>();
                builder.Services.AddSingleton<ITenderTypeToQueue, ChannelApps.Model.RDF.Dispatchers.TenderType>();
                builder.Services.AddSingleton<IDiscountTypeToQueue, ChannelApps.Model.RDF.Dispatchers.DiscountType>();
                builder.Services.AddSingleton<IProductsToQueue, ChannelApps.Model.RDF.Dispatchers.Products>();
                builder.Services.AddSingleton<IDiscountProductToQueue, ChannelApps.Model.RDF.Dispatchers.DiscountProduct>();
                builder.Services.AddSingleton<IProductBarcodeToQueue, ChannelApps.Model.RDF.Dispatchers.ProductBarcode>();
                builder.Services.AddSingleton<IProductCategoryToQueue, ChannelApps.Model.RDF.Dispatchers.ProductCategory>();
                builder.Services.AddSingleton<IProductPriceToQueue, ChannelApps.Model.RDF.Dispatchers.ProductPrice>();
                builder.Services.AddSingleton<ITenderTypeToQueue, ChannelApps.Model.RDF.Dispatchers.TenderType>();
                builder.Services.AddSingleton<IDiscountLocationToQueue, ChannelApps.Model.RDF.Dispatchers.DiscountLocation>();
                builder.Services.AddSingleton<IBOMHeaderToQueue, ChannelApps.Model.RDF.Dispatchers.BOMHeader>();
                builder.Services.AddSingleton<IBOMVersionToQueue, ChannelApps.Model.RDF.Dispatchers.BOMVersion>();
                builder.Services.AddSingleton<IBOMLinesToQueue, ChannelApps.Model.RDF.Dispatchers.BOMLines>();
                builder.Services.AddSingleton<IDiscountTypeProductToQueue, ChannelApps.Model.RDF.Dispatchers.DiscountTypeProduct>();
                builder.Services.AddSingleton<IConfigurationGroupToQueue, ChannelApps.Model.RDF.Dispatchers.ConfigurationGroup>();
            }
            else if (CompanyID == 3393)
            {
                // Queue Service
                builder.Services.AddTransient<IQueueService, QueueService>();

                //ChannelAppProcessor Injection
                builder.Services.AddSingleton(ChannelAppCywareConfigHelper.Get());
                builder.Services.AddSingleton<IChannelAppBlobService, ChannelAppBlobService>();
                builder.Services.AddSingleton<IChannelAppQueueService, ChannelAppQueueService>();


                // Function Class
                builder.Services.AddSingleton<ICustomerToQueue, ChannelApps.Model.RDF.Dispatchers.Customer>();
                builder.Services.AddSingleton<IPriceToQueue, ChannelApps.Model.RDF.Dispatchers.Price>();
                builder.Services.AddSingleton<IRegisterToQueue, ChannelApps.Model.RDF.Dispatchers.Register>();
                builder.Services.AddSingleton<ICurrencyToQueue, ChannelApps.Model.RDF.Dispatchers.Currency>();
                builder.Services.AddSingleton<IForExToQueue, ChannelApps.Model.RDF.Dispatchers.ForEx>();
                builder.Services.AddSingleton<IStoreToQueue, ChannelApps.Model.RDF.Dispatchers.Store>();
                builder.Services.AddSingleton<IDiscountToQueue, ChannelApps.Model.RDF.Dispatchers.Discount>();
                builder.Services.AddSingleton<IPriceLevelDetailToQueue, ChannelApps.Model.RDF.Dispatchers.PriceLevelDetail>();
                builder.Services.AddSingleton<ITenderToQueue, ChannelApps.Model.RDF.Dispatchers.Tender>();
                builder.Services.AddSingleton<ITenderTypeToQueue, ChannelApps.Model.RDF.Dispatchers.TenderType>();
                builder.Services.AddSingleton<IDiscountTypeToQueue, ChannelApps.Model.RDF.Dispatchers.DiscountType>();
                builder.Services.AddSingleton<IProductsToQueue, ChannelApps.Model.RDF.Dispatchers.Products>();
                builder.Services.AddSingleton<IDiscountProductToQueue, ChannelApps.Model.RDF.Dispatchers.DiscountProduct>();
                builder.Services.AddSingleton<IProductBarcodeToQueue, ChannelApps.Model.RDF.Dispatchers.ProductBarcode>();
                builder.Services.AddSingleton<IProductCategoryToQueue, ChannelApps.Model.RDF.Dispatchers.ProductCategory>();
                builder.Services.AddSingleton<IProductPriceToQueue, ChannelApps.Model.RDF.Dispatchers.ProductPrice>();
                builder.Services.AddSingleton<ITenderTypeToQueue, ChannelApps.Model.RDF.Dispatchers.TenderType>();
                builder.Services.AddSingleton<IDiscountLocationToQueue, ChannelApps.Model.RDF.Dispatchers.DiscountLocation>();
                builder.Services.AddSingleton<IBOMHeaderToQueue, ChannelApps.Model.RDF.Dispatchers.BOMHeader>();
                builder.Services.AddSingleton<IBOMVersionToQueue, ChannelApps.Model.RDF.Dispatchers.BOMVersion>();
                builder.Services.AddSingleton<IBOMLinesToQueue, ChannelApps.Model.RDF.Dispatchers.BOMLines>();
                builder.Services.AddSingleton<IDiscountTypeProductToQueue, ChannelApps.Model.RDF.Dispatchers.DiscountTypeProduct>();
                builder.Services.AddSingleton<IConfigurationGroupToQueue, ChannelApps.Model.RDF.Dispatchers.ConfigurationGroup>();
            }
        }
    }
}

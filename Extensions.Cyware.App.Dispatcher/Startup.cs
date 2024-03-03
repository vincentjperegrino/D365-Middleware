using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Domain;
using KTI.Moo.Extensions.Cyware.Model;
using KTI.Moo.Extensions.Cyware.Processor;
using KTI.Moo.Extensions.Cyware.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(KTI.Moo.Extensions.Cyware.App.Dispatcher.Startup))]
namespace KTI.Moo.Extensions.Cyware.App.Dispatcher
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Config
            builder.Services.AddSingleton(ConfigHelper.Get());

            // Queue Service
            builder.Services.AddTransient<IQueueService, QueueService>();
            builder.Services.AddTransient<IExtensionAppProcessorDomain, ExtensionAppProcessorDomain>();


            // Function Class
            builder.Services.AddSingleton<IPrices<PriceHeader>, KTI.Moo.Extensions.Cyware.Domain.Prices>();
            builder.Services.AddSingleton<IDiscount<KTI.Moo.Extensions.Cyware.Model.Discount>, KTI.Moo.Extensions.Cyware.Domain.Discounts>();
            builder.Services.AddSingleton<IStores<KTI.Moo.Extensions.Cyware.Model.Store>, KTI.Moo.Extensions.Cyware.Domain.Stores>();
            builder.Services.AddSingleton<IProductCategory<KTI.Moo.Extensions.Cyware.Model.ProductCategory>, KTI.Moo.Extensions.Cyware.Domain.ProductCategory>();
            builder.Services.AddSingleton<IPrices<PriceHeader>, Prices>();
            builder.Services.AddSingleton<IProductBarcode<Model.ProductBarcode>, Domain.ProductBarcode>();
            builder.Services.AddSingleton<IProducts<Model.Products>, Domain.Products>();
            builder.Services.AddSingleton<IForEx<Model.ForEx>, Domain.ForEx>();
            builder.Services.AddSingleton<IDiscountProduct<Model.DiscountProduct>, Domain.DiscountProduct>();
            builder.Services.AddSingleton<IProductPrice<Model.ProductPrice>, Domain.ProductPrice>();
            builder.Services.AddSingleton<ITender<Model.Tender>, Domain.Tender>();

            builder.Services.AddSingleton<ICustomer<Model.Customer>, Domain.Customer>();
            builder.Services.AddSingleton<IDiscountType<Model.DiscountType>, Domain.CywareDiscountType>();
            builder.Services.AddSingleton<ITenderType<Model.TenderType>, Domain.CywareTenderType>();
            builder.Services.AddSingleton<IDiscountLocation<Model.DiscountLocation>, Domain.CywareDiscountLocation>();
            builder.Services.AddSingleton<IBOMHeader<Model.BOMHeader>, Domain.CywareBOMHeader>();
            builder.Services.AddSingleton<IBOMLines<Model.BOMLines>, Domain.CywareBOMLines>();
            builder.Services.AddSingleton<IBOMVersion<Model.BOMVersion>, Domain.CywareBOMVersion>();

            builder.Services.AddSingleton<IDiscountTypeProduct<Model.DiscountTypeProduct>, Domain.CywareDiscountTypeProduct>();
            builder.Services.AddSingleton<IConfigurationGroup<Model.ConnfigurationGroup>, Domain.CywareConfigurationGroup>();


            #region injections
            //if (CompanyID == 3388)
            //{
            //    builder.Services.AddSingleton<IPriceToQueue, ChannelApps.Model.RDF.Dispatchers.Price>();
            //    builder.Services.AddSingleton<IRegisterToQueue, ChannelApps.Model.RDF.Dispatchers.Register>();
            //    builder.Services.AddSingleton<ICurrencyToQueue, ChannelApps.Model.RDF.Dispatchers.Currency>();
            //    builder.Services.AddSingleton<IForExToQueue, ChannelApps.Model.RDF.Dispatchers.ForEx>();
            //    builder.Services.AddSingleton<IStoreToQueue, ChannelApps.Model.RDF.Dispatchers.Store>();
            //    builder.Services.AddSingleton<IDiscountToQueue, ChannelApps.Model.RDF.Dispatchers.Discount>();
            //    builder.Services.AddSingleton<IPriceLevelDetailToQueue, ChannelApps.Model.RDF.Dispatchers.PriceLevelDetail>();
            //    builder.Services.AddSingleton<ITenderToQueue, ChannelApps.Model.RDF.Dispatchers.Tender>();
            //    builder.Services.AddSingleton<ITenderTypeToQueue, ChannelApps.Model.RDF.Dispatchers.TenderType>();
            //    builder.Services.AddSingleton<IDiscountTypeToQueue, ChannelApps.Model.RDF.Dispatchers.DiscountType>();
            //    builder.Services.AddSingleton<IProductsToQueue, ChannelApps.Model.RDF.Dispatchers.Products>();
            //    builder.Services.AddSingleton<IDiscountProductToQueue, ChannelApps.Model.RDF.Dispatchers.DiscountProduct>();
            //    builder.Services.AddSingleton<IProductBarcodeToQueue, ChannelApps.Model.RDF.Dispatchers.ProductBarcode>();
            //    builder.Services.AddSingleton<IProductCategoryToQueue, ChannelApps.Model.RDF.Dispatchers.ProductCategory>();
            //    builder.Services.AddSingleton<IProductPriceToQueue, ChannelApps.Model.RDF.Dispatchers.ProductPrice>();
            //}
            //else if (CompanyID == 3392)
            //{
            //    builder.Services.AddSingleton<IPriceToQueue, ChannelApps.Model.RDF.Dispatchers.Price>();
            //    builder.Services.AddSingleton<IRegisterToQueue, ChannelApps.Model.RDF.Dispatchers.Register>();
            //    builder.Services.AddSingleton<ICurrencyToQueue, ChannelApps.Model.RDF.Dispatchers.Currency>();
            //    builder.Services.AddSingleton<IForExToQueue, ChannelApps.Model.RDF.Dispatchers.ForEx>();
            //    builder.Services.AddSingleton<IStoreToQueue, ChannelApps.Model.RDF.Dispatchers.Store>();
            //    builder.Services.AddSingleton<IDiscountToQueue, ChannelApps.Model.RDF.Dispatchers.Discount>();
            //    builder.Services.AddSingleton<IPriceLevelDetailToQueue, ChannelApps.Model.RDF.Dispatchers.PriceLevelDetail>();
            //    builder.Services.AddSingleton<ITenderToQueue, ChannelApps.Model.RDF.Dispatchers.Tender>();
            //    builder.Services.AddSingleton<ITenderTypeToQueue, ChannelApps.Model.RDF.Dispatchers.TenderType>();
            //    builder.Services.AddSingleton<IDiscountTypeToQueue, ChannelApps.Model.RDF.Dispatchers.DiscountType>();
            //    builder.Services.AddSingleton<IProductsToQueue, ChannelApps.Model.RDF.Dispatchers.Products>();
            //    builder.Services.AddSingleton<IDiscountProductToQueue, ChannelApps.Model.RDF.Dispatchers.DiscountProduct>();
            //    builder.Services.AddSingleton<IProductBarcodeToQueue, ChannelApps.Model.RDF.Dispatchers.ProductBarcode>();
            //    builder.Services.AddSingleton<IProductCategoryToQueue, ChannelApps.Model.RDF.Dispatchers.ProductCategory>();
            //    builder.Services.AddSingleton<IProductPriceToQueue, ChannelApps.Model.RDF.Dispatchers.ProductPrice>();
            //}
            //else if (CompanyID == 3393)
            //{
            //    builder.Services.AddSingleton<IPriceToQueue, ChannelApps.Model.RDF.Dispatchers.Price>();
            //    builder.Services.AddSingleton<IRegisterToQueue, ChannelApps.Model.RDF.Dispatchers.Register>();
            //    builder.Services.AddSingleton<ICurrencyToQueue, ChannelApps.Model.RDF.Dispatchers.Currency>();
            //    builder.Services.AddSingleton<IForExToQueue, ChannelApps.Model.RDF.Dispatchers.ForEx>();
            //    builder.Services.AddSingleton<IStoreToQueue, ChannelApps.Model.RDF.Dispatchers.Store>();
            //    builder.Services.AddSingleton<IDiscountToQueue, ChannelApps.Model.RDF.Dispatchers.Discount>();
            //    builder.Services.AddSingleton<IPriceLevelDetailToQueue, ChannelApps.Model.RDF.Dispatchers.PriceLevelDetail>();
            //    builder.Services.AddSingleton<ITenderToQueue, ChannelApps.Model.RDF.Dispatchers.Tender>();
            //    builder.Services.AddSingleton<ITenderTypeToQueue, ChannelApps.Model.RDF.Dispatchers.TenderType>();
            //    builder.Services.AddSingleton<IDiscountTypeToQueue, ChannelApps.Model.RDF.Dispatchers.DiscountType>();
            //    builder.Services.AddSingleton<IProductsToQueue, ChannelApps.Model.RDF.Dispatchers.Products>();
            //    builder.Services.AddSingleton<IDiscountProductToQueue, ChannelApps.Model.RDF.Dispatchers.DiscountProduct>();
            //    builder.Services.AddSingleton<IProductBarcodeToQueue, ChannelApps.Model.RDF.Dispatchers.ProductBarcode>();
            //    builder.Services.AddSingleton<IProductCategoryToQueue, ChannelApps.Model.RDF.Dispatchers.ProductCategory>();
            //    builder.Services.AddSingleton<IProductPriceToQueue, ChannelApps.Model.RDF.Dispatchers.ProductPrice>();
            //}
            #endregion
        }
    }
}

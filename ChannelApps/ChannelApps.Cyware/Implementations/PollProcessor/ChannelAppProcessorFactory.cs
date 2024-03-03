using KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Interfaces;
using KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Mapper;

namespace KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor
{
    public class ChannelAppProcessorFactory
    {
        public IChannelAppModuleProcessor CreateModuleProcessor(string moduleType)
        {
            // Add additional module types and processors as needed
            switch (moduleType.ToLower())
            {
                case "customer":
                    return new CustomerModuleMapper();
                case "bomheader":
                    return new BOMHeaderModuleMapper();
                case "bomversion":
                    return new BOMVersionModuleMapper();
                case "bomlines":
                    return new BOMLinesModuleMapper();
                case "configurationgroup":
                    return new ConfigurationGroupModuleMapper();
                case "priceheader":
                    return new PriceHeaderModuleMapper();
                case "productprice":
                    return new ProductPriceModuleMapper();
                case "productcategory":
                    return new ProductCategoryModuleMapper();
                case "tender":
                    return new TenderModuleMapper();
                case "tendertype":
                    return new TenderTypeModuleMapper();
                case "discounttype":
                    return new DiscountTypeModuleMapper();
                case "discounttypeproduct":
                    return new DiscountTypeProductModuleMapper();
                case "discounttypelocation":
                    return new DiscountTypeLocationModuleMapper();
                case "product":
                    return new ProductModuleMapper();
                case "productbarcode":
                    return new ProductBarcodeModuleMapper();
                default:
                    throw new NotSupportedException("Unsupported module type");
            }
        }
    }
}

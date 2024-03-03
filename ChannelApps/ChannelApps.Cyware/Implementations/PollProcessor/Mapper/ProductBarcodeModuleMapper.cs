using ChannelApps.Cyware.Helpers;
using KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Interfaces;
using KTI.Moo.Cyware.Model.DTO;
using Microsoft.Extensions.Logging;
using Moo.Models.Dtos.Items;
using Newtonsoft.Json;

namespace KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Mapper
{
    public class ProductBarcodeModuleMapper : IChannelAppModuleProcessor
    {
        public ProcessorReturnModel ProcessAsync(string pollContent, ILogger log)
        {
            ///Map to MOO Library's DTOs
            IEnumerable<ProductBarcode> configurationGroup_d365 = JsonConvert.DeserializeObject<IEnumerable<ProductBarcode>>(pollContent);
            var result = configurationGroup_d365.Aggregate(
                new
                {
                    FormattedResults = new List<ProductBarcodePoll54>(),
                    ErrorResults = new List<ProcessorErrorReturnModel>()
                },
                (acc, source) =>
                {
                    try
                    {
                        Extensions.Cyware.Model.ProductBarcode mapped = new Extensions.Cyware.Model.ProductBarcode
                        {
                            product_code = source.BarCode ?? "",
                            sku_number = source.sku_number ?? "",
                            upc_type = source.upc_type.Length < 5 ? source.upc_type : source.upc_type.Substring(0, 5) ?? "", //productBarcode_d365.upc_type ?? "",
                            upc_unit_of_measure = source.upc_unit_of_measure ?? ""
                        };

                        ////Validate before adding formatted customer (throw error if validation fails)
                        Validate(mapped);
                        ////Add the formatted Customers
                        acc.FormattedResults.Add(new ProductBarcodePoll54(mapped));
                    }
                    catch (Exception ex)
                    {
                        ////Add Customers with error
                        acc.ErrorResults.Add(new ProcessorErrorReturnModel
                        {
                            Id = source.BarCode,
                            ErrorMessage = ex.Message

                        });
                        log.LogError($"{source.BarCode}: {ex.Message}");
                    }
                    return acc;
                });
            var successReturnResult = new ProcessorSuccessReturnModel(ChannelAppPollMapperHelper.ConcatenateValues(result.FormattedResults), result.FormattedResults.Count());
            var returnResult = new ProcessorReturnModel(successReturnResult, result.ErrorResults);
            return returnResult;
        }

        private static void Validate(Extensions.Cyware.Model.ProductBarcode productBarcode)
        {
            if (string.IsNullOrEmpty(productBarcode.product_code))
            {
                throw new ArgumentException("Product code is required.");
            }

            if (string.IsNullOrEmpty(productBarcode.sku_number))
            {
                throw new ArgumentException("SKU number is required.");
            }

            if (string.IsNullOrEmpty(productBarcode.upc_unit_of_measure))
            {
                throw new ArgumentException("UPC unit of measure is required.");
            }
        }
    }
}



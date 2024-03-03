using ChannelApps.Cyware.Helpers;
using KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Interfaces;
using KTI.Moo.Cyware.Model.DTO;
using Microsoft.Extensions.Logging;
using Moo.Models.Dtos.Items;
using Newtonsoft.Json;

namespace KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Mapper
{
    public class ProductCategoryModuleMapper : IChannelAppModuleProcessor
    {
        public ProcessorReturnModel ProcessAsync(string pollContent, ILogger log)
        {
            ///Map to MOO Library's DTOs
            IEnumerable<ProductCategory> configurationGroup_d365 = JsonConvert.DeserializeObject<IEnumerable<ProductCategory>>(pollContent);
            var result = configurationGroup_d365.Aggregate(
                new
                {
                    FormattedResults = new List<ProductCategoryPoll54>(),
                    ErrorResults = new List<ProcessorErrorReturnModel>()
                },
                (acc, source) =>
                {
                    try
                    {
                        Extensions.Cyware.Model.ProductCategory mapped = new Extensions.Cyware.Model.ProductCategory
                        {
                            department = source.CategoryCode?.Length >= 3 ? source.CategoryCode.Substring(0, 3) : null,
                            sub_dept = source.CategoryCode?.Length >= 6 ? source.CategoryCode.Substring(0, 6) : null,
                            cy_class = source.CategoryCode?.Length >= 9 ? source.CategoryCode.Substring(0, 9) : null,
                            sub_class = source.CategoryCode?.Length >= 12 ? source.CategoryCode.Substring(0, 12) : null,
                            name = source.FriendlyName ?? "",
                            planned_gm = ""
                        };

                        ////Validate before adding formatted customer (throw error if validation fails)
                        Validate(mapped);
                        ////Add the formatted Customers
                        acc.FormattedResults.Add(new ProductCategoryPoll54(mapped));
                    }
                    catch (Exception ex)
                    {
                        ////Add Customers with error
                        acc.ErrorResults.Add(new ProcessorErrorReturnModel
                        {
                            Id = source.FriendlyName,
                            ErrorMessage = ex.Message

                        });
                        log.LogError($"{source.FriendlyName}: {ex.Message}");
                    }
                    return acc;
                });
            var successReturnResult = new ProcessorSuccessReturnModel(ChannelAppPollMapperHelper.ConcatenateValues(result.FormattedResults), result.FormattedResults.Count());
            var returnResult = new ProcessorReturnModel(successReturnResult, result.ErrorResults);
            return returnResult;
        }

        private static void Validate(Extensions.Cyware.Model.ProductCategory productCategory)
        {
            if (string.IsNullOrEmpty(productCategory.department))
            {
                throw new ArgumentException("Department cannot be null.");
            }
        }
    }
}

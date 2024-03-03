using ChannelApps.Cyware.Helpers;
using KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Interfaces;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using Microsoft.Extensions.Logging;
using Moo.Models.Dtos.Sales;
using Newtonsoft.Json;

namespace KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Mapper
{
    public class DiscountTypeModuleMapper : IChannelAppModuleProcessor
    {
        public ProcessorReturnModel ProcessAsync(string pollContent, ILogger log)
        {
            ///Map to MOO Library's DTOs
            IEnumerable<DiscountType> configurationGroup_d365 = JsonConvert.DeserializeObject<IEnumerable<DiscountType>>(pollContent);
            var result = configurationGroup_d365.Aggregate(
                new
                {
                    FormattedResults = new List<DiscountTypePoll>(),
                    ErrorResults = new List<ProcessorErrorReturnModel>()
                },
                (acc, source) =>
                {
                    try
                    {
                        Extensions.Cyware.Model.DiscountType mapped = new Extensions.Cyware.Model.DiscountType
                        {

                            DiscountCode = source.discount_cd ?? "",
                            DiscountTypeCd = source.discount_type_cd ?? "",
                            Description = source.description ?? "",
                            DiscType = "",
                            Readonly = "1",
                            DiscValue = source.disc_value ?? "",
                            StartDate = DateTime.TryParse(source.StartDate, out var startdate) ? startdate : default,
                            EndDate = DateTime.TryParse(source.end_date, out var enddate) ? enddate : default,
                            MinAmount = double.TryParse(source.min_amount, out var minamount) ? minamount : default,
                            MaxAmount = double.TryParse(source.max_amount, out var maxamount) ? maxamount : default,
                            DiscountRule = source.discount_rule ?? "",
                            AccountTypeCode = source.acc_type_codeTypeCode ?? "",
                            RequireAccount = source.require_account ?? "",
                            FreeItem = source.free_item ?? "",
                            FreeItemLimit = int.TryParse(source.free_item_limit, out var freeitemlimit) ? freeitemlimit : default,
                            FreeItemQty = int.TryParse(source.free_item_qty, out var freeitemqty) ? freeitemqty : default,
                            EventNumber = int.TryParse(source.event_num, out var eventnum) ? eventnum : default,
                            PostedOn = DateTime.TryParse(source.posted_on, out var postedOn) ? postedOn : default,
                            ExportCurrentPrice = source.export_current_price ?? "",
                            Posted = int.TryParse(source.posted, out var posted) ? posted : default,
                        };

                        //Validate(mapped);

                        ////Add the formatted Customers
                        acc.FormattedResults.Add(new DiscountTypePoll(mapped));
                    }
                    catch (Exception ex)
                    {
                        ////Add Customers with error
                        acc.ErrorResults.Add(new ProcessorErrorReturnModel
                        {
                            Id = source.discount_cd,
                            ErrorMessage = ex.Message

                        });
                        log.LogError($"{source.discount_cd}: {ex.Message}");
                    }
                    return acc;
                });

            var successReturnResult = new ProcessorSuccessReturnModel(ChannelAppPollMapperHelper.ConcatenateValues(result.FormattedResults), result.FormattedResults.Count());
            var returnResult = new ProcessorReturnModel(successReturnResult, result.ErrorResults);
            return returnResult;
        }
    }
}

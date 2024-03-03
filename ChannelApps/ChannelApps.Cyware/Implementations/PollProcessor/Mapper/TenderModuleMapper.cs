using ChannelApps.Cyware.Helpers;
using KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Interfaces;
using KTI.Moo.Cyware.Model.DTO;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using Microsoft.Extensions.Logging;
using Moo.Models.Dtos.Items;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Mapper
{
    public class TenderModuleMapper : IChannelAppModuleProcessor
    {
        public ProcessorReturnModel ProcessAsync(string pollContent, ILogger log)
        {
            ///Map to MOO Library's DTOs
            IEnumerable<PaymentMethod> configurationGroup_d365 = JsonConvert.DeserializeObject<IEnumerable<PaymentMethod>>(pollContent);
            var result = configurationGroup_d365.Aggregate(
                new
                {
                    FormattedResults = new List<TenderPoll97>(),
                    ErrorResults = new List<ProcessorErrorReturnModel>()
                },
                (acc, source) =>
                {
                    try
                    {
                        Extensions.Cyware.Model.Tender mapped = new Extensions.Cyware.Model.Tender
                        {
                            tender_cd = source.tender_cd ?? "",
                            tender_type_cd = source.tender_type_cd ?? "",
                            description = source.Description ?? "",
                            is_default = "",
                            currency_cd = "",
                            validation_spacing = "",
                            max_change = "",
                            change_currency_code = "",
                            mms_code = "",
                            display_subtotal = "",
                            min_amount = source.min_amount ?? "",
                            max_amount = source.max_amount ?? "",
                            is_layaway_refund = "",
                            max_refund = "",
                            refund_type = "",
                            is_mobile_payment = "",
                            is_account = "",
                            acct_type_code = source.acct_type_code ?? "",
                            is_manager = "",
                            garbage_tender_cd = "",
                            rebate_tender_cd = "",
                            rebate_percent = "",
                            is_cashfund = "",
                            is_takeout = "",
                            item_code = "",
                            surcharge_sku = "",
                            mobile_payment_number = "",
                            mobile_payment_return = "",
                            is_padss = "",
                            is_credit_card = "",
                            eft_port = "",
                            is_voucher = "",
                            discount_cd = source.discount_cd ?? ""
                        };

                        ////Add the formatted Customers
                        acc.FormattedResults.Add(new TenderPoll97(mapped));
                    }
                    catch (Exception ex)
                    {
                        ////Add Customers with error
                        acc.ErrorResults.Add(new ProcessorErrorReturnModel
                        {
                            Id = source.tender_cd,
                            ErrorMessage = ex.Message

                        });
                        log.LogError($"{source.tender_cd}: {ex.Message}");
                    }
                    return acc;
                });

            var successReturnResult = new ProcessorSuccessReturnModel(ChannelAppPollMapperHelper.ConcatenateValues(result.FormattedResults), result.FormattedResults.Count());
            var returnResult = new ProcessorReturnModel(successReturnResult, result.ErrorResults);
            return returnResult;
        }
    }
}

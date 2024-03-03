using ChannelApps.Cyware.Helpers;
using KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Interfaces;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using Microsoft.Extensions.Logging;
using Moo.Models.Dtos.Items;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Mapper
{
    public class TenderTypeModuleMapper : IChannelAppModuleProcessor
    {
        public ProcessorReturnModel ProcessAsync(string pollContent, ILogger log)
        {
            ///Map to MOO Library's DTOs
            IEnumerable<PaymentMethodType> configurationGroup_d365 = JsonConvert.DeserializeObject<IEnumerable<PaymentMethodType>>(pollContent);
            var result = configurationGroup_d365.Aggregate(
                new
                {
                    FormattedResults = new List<TenderTypePOLL>(),
                    ErrorResults = new List<ProcessorErrorReturnModel>()
                },
                (acc, source) =>
                {
                    try
                    {
                        Extensions.Cyware.Model.TenderType mapped = new Extensions.Cyware.Model.TenderType
                        {
                            /////Mapping here
                            TenderTypeCd = source.tender_type_cd,
                            Description = source.description ?? "",
                            AllowChange = source.description.ToLower().Contains("cash") ? "1" : "0",
                            RequireValidation = source.require_validation ?? "0",
                            IsCreditCard = source.description.ToLower().Contains("credit") ? "1" : "0",
                            IsGc = (source.description.ToLower().Contains("gift check") || source.description.ToLower().Contains("gc")) ? "1" : "0",
                            Skey = source.skey ?? "0",
                            IsDrawer = source.description.ToLower().Contains("cash") ? "1" : "0",
                            IsDebitCard = source.description.ToLower().Contains("debit") ? "1" : "0",
                            IsCheck = source.description.ToLower().Contains("cheque") ? "1" : "0",
                            IsCharge = source.description.ToLower().Contains("family") ? "1" : "0",
                            IsCash = source.description.ToLower().Contains("cash") ? "1" : "0",
                            IsGarbage = source.is_garbage ?? "0",
                            IsCashdec = source.description.ToLower().Contains("cash") ? "1" : "0",
                            IsRebate = source.is_rebate ?? "0",
                            IsEgc = source.description.ToLower().Contains("electronic gift check") ? "1" : "0",
                            IsTelcoPull = source.is_telco_pull ?? "0",
                            IsTelcoPush = source.is_telco_push ?? "0",
                            IsGuarantor = source.is_guarantor ?? "0",
                            IsCardConnect = source.description.ToLower().Contains("emp") ? "1" : "0",
                            IsGovRebate = source.is_gov ?? "0",
                            ItemCode = source.item_code ?? "0",
                            IsIntegrated = source.is_integrated ?? "0",
                            IsLoyalty = source.description.ToLower().Contains("loyalty") ? "1" : "0",
                            IsHoreca = source.description.ToLower().Contains("horeca") ? "1" : "0",
                            IsMobile = source.description.ToLower().Contains("gcash") || source.description.ToLower().Contains("maya") ? "1" : "0"
                        };

                        Validate(mapped);

                        ////Add the formatted Customers
                        acc.FormattedResults.Add(new TenderTypePOLL(mapped));
                    }
                    catch (Exception ex)
                    {
                        ////Add Customers with error
                        acc.ErrorResults.Add(new ProcessorErrorReturnModel
                        {
                            Id = source.tender_type_cd,
                            ErrorMessage = ex.Message

                        });
                        log.LogError($"{source.tender_type_cd}: {ex.Message}");
                    }
                    return acc;
                });

            var successReturnResult = new ProcessorSuccessReturnModel(ChannelAppPollMapperHelper.ConcatenateValues(result.FormattedResults), result.FormattedResults.Count());
            var returnResult = new ProcessorReturnModel(successReturnResult, result.ErrorResults);
            return returnResult;
        }

        private static void Validate(Extensions.Cyware.Model.TenderType tenderType)
        {
            if (string.IsNullOrEmpty(tenderType.TenderTypeCd))
            {
                throw new ArgumentException("TenderTypeCd name is required.");
            }

            if (string.IsNullOrEmpty(tenderType.Description))
            {
                throw new ArgumentException("Description is required.");
            }
        }
    }
}

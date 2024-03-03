using ChannelApps.Cyware.Helpers;
using KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Interfaces;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using Microsoft.Extensions.Logging;
using Moo.Models.Dtos.Sales;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Mapper
{
    public class DiscountTypeProductModuleMapper : IChannelAppModuleProcessor
    {
        public ProcessorReturnModel ProcessAsync(string pollContent, ILogger log)
        {
            ///Map to MOO Library's DTOs
            IEnumerable<DiscountTypeProduct> configurationGroup_d365 = JsonConvert.DeserializeObject<IEnumerable<DiscountTypeProduct>>(pollContent);
            var result = configurationGroup_d365.Aggregate(
                new
                {
                    FormattedResults = new List<DiscountTypeProductPOLL>(),
                    ErrorResults = new List<ProcessorErrorReturnModel>()
                },
                (acc, source) =>
                {
                    try
                    {
                        decimal discount_percentage_1 = decimal.TryParse(source.discount_percentage_1, out var discountpercentage_1) ? discountpercentage_1 : default;
                        Extensions.Cyware.Model.DiscountTypeProduct mapped = new Extensions.Cyware.Model.DiscountTypeProduct
                        {
                            ItemCode = source.item_code ?? "",
                            DisccountCd = source.discount_cd ?? "",
                            DiscValueInAmount = discount_percentage_1 != 0 ? discount_percentage_1 : source.disc_value,
                            DiscValueInPercentage = discount_percentage_1.ToString(),
                            //DiscType = _discountTypeProduct_d365.disc_type,
                            DiscType = discount_percentage_1 != 0 ? "1" : "0",
                            ItemGroup = source.item_group ?? "",
                            FreeItem = source.free_item ?? "",
                            FreeItemQty = source.free_item_qty ?? "",
                            Tolerance = source.tolerance ?? "",
                            EventNum = source.event_num ?? "",
                            ReqAmount = source.req_amount ?? "",
                            ReqQty = source.req_qty ?? "",
                            PartyCodeType = source.party_code_type ?? "",
                            AccountSelection = source.account_selection ?? "",
                            Configuration = source.configuration ?? "",
                            //Site = _discountTypeProduct_d365.Site,
                            Warehouse = source.warehouse ?? "",
                            From = source.from,
                            To = source.to,
                            Unit = source.unit_id ?? "",
                            Currency = source.currency ?? "",
                            AttributeBasedPricingID = source.attribute_based_pricing_id ?? "",
                            DimensionValidation = source.dimension_validation ?? "",
                            TradeAgreementValidation = source.trade_agreement_validation ?? "",
                            DimensionNumber = source.dimension_number ?? "",
                            DiscountPercentage2 = source.discount_percentage_2 ?? "",
                            DisregardLeadTime = source.disregard_lead_time ?? "",
                            FindNext = source.find_next ?? "",
                            FromDate = source.Priceapplicablefromdate,
                            IncludeInUnitPrice = source.incl_in_unit_price ?? "",
                            IncludeGenericCurrency = source.include_generic_currency ?? "",
                            LeadTime = source.lead_time ?? "",
                            Log = source.log ?? "",
                            Module = source.module ?? "",
                            PriceAgreements = source.price_agreement ?? "",
                            PriceCharges = source.price_charges ?? "",
                            PriceUnit = source.price_unit ?? "",
                            ToDate = source.to_date,
                            FromTime = source.from_time,
                            ToTime = source.to_time ?? ""
                        };

                        //Validate(mapped);

                        // Add the formatted Customers
                        acc.FormattedResults.Add(new DiscountTypeProductPOLL(mapped));
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
 
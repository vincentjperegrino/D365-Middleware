using AutoMapper.Execution;
using ChannelApps.Cyware.Helpers;
using KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Interfaces;
using KTI.Moo.Cyware.Model.DTO;
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
    /// <summary>
    /// this module is product price or price details
    /// </summary>
    public class ProductPriceModuleMapper : IChannelAppModuleProcessor
    {
        public ProcessorReturnModel ProcessAsync(string pollContent, ILogger log)
        {
            ///Map to MOO Library's DTOs
            IEnumerable<ProductPrices> configurationGroup_d365 = JsonConvert.DeserializeObject<IEnumerable<ProductPrices>>(pollContent);
            var result = configurationGroup_d365.Aggregate(
                new
                {
                    FormattedResults = new List<ProductPricePoll79>(),
                    ErrorResults = new List<ProcessorErrorReturnModel>()
                },
                (acc, source) =>
                {
                    try
                    {
                        Extensions.Cyware.Model.ProductPrice mapped = new Extensions.Cyware.Model.ProductPrice
                        {
                            sku_number = source.sku_number ?? "",
                            upc_code = source.upc_code,
                            upc_type = source.upc_type,
                            price_event_number = source.price_event_number ?? "",
                            currency_code = source.currency_code ?? "",
                            price_book = source.price_book ?? "",
                            start_date = source.PriceApplicableFromDate,
                            end_date = source.end_date,
                            promo_flag_yn = "N",  //d365_productPrice.promo_flag_yn ?? "",
                            event_price_multiple = source.event_price_multiple,
                            event_price = Math.Round(source.event_price, 3),  ///RoundOff to 3 Decimal Places   /* source.event_price,*/
                            price_method_code = source.price_method_code,
                            mix_match_code = source.mix_match_code,
                            deal_quantity = source.deal_quantity,
                            deal_price = Math.Round(source.deal_price,3), //RoundOff to 3 Decimal Places
                            buy_quantity = int.TryParse(source.buy_quantity.ToString(), out var buyQuantity) ? buyQuantity : default(int),
                            buy_value = Math.Round(source.buy_value, 3), //RoundOff to 3 Decimal Places
                            buy_value_type = source.buy_value_type ?? "",
                            qty_end_value = source.qty_end_value,
                            quantity_break = source.quantity_break,
                            quantity_group_price = source.quantity_group_price,
                            quantity_unit_price = Math.Round(source.quantity_unit_price, 3), //RoundOff to 3 Decimal Places
                            cust_promo_code = source.cust_promo_code ?? "",
                            cust_number = source.cust_number ?? "",
                            precedence_level = int.TryParse(source.precedence_level.ToString(), out var precedenceLevel) ? precedenceLevel : default(int),
                            default_currency = source.default_currency ?? "",
                            default_price_book = source.default_price_book ?? "",
                        };

                        ///Validation Here...
                        ///
                        Validate(mapped);

                        ////Add the formatted Customers
                        acc.FormattedResults.Add(new ProductPricePoll79(mapped));
                    }
                    catch (Exception ex)
                    {
                        ////Add Customers with error
                        acc.ErrorResults.Add(new ProcessorErrorReturnModel
                        {
                            Id = $"ID: {source.sku_number}, EventNum: {source.price_event_number}, Price: {source.event_price}, FromDate: {source.start_date}, EndDate: {source.end_date}",
                            ErrorMessage = ex.Message

                        });
                        log.LogError($"{source.sku_number}: {ex.Message}");
                    }
                    return acc;
                });
            var successReturnResult = new ProcessorSuccessReturnModel(ChannelAppPollMapperHelper.ConcatenateValues(result.FormattedResults), result.FormattedResults.Count());
            var returnResult = new ProcessorReturnModel(successReturnResult, result.ErrorResults);
            return returnResult;
        }

        private static void Validate(Extensions.Cyware.Model.ProductPrice priceDetails)
        {
            DateTime todayInTargetTimeZone = DateTime.UtcNow.AddHours(8);
            DateTime minValue = new DateTime(1900, 01, 01);

            if (priceDetails.end_date != minValue && priceDetails.end_date < todayInTargetTimeZone)
            {
                throw new ArgumentException("PriceDetails end_date already expired.");
                //Console.WriteLine("expre");
            }
        }
    }


}

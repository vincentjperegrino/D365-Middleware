using ChannelApps.Cyware.Helpers;
using KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Interfaces;
using KTI.Moo.Cyware.Model.DTO;
using Microsoft.Extensions.Logging;
using Moo.Models.Dtos.Items;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Mapper
{
    public class ProductModuleMapper : IChannelAppModuleProcessor
    {
        public ProcessorReturnModel ProcessAsync(string pollContent, ILogger log)
        {
            ///Map to MOO Library's DTOs
            IEnumerable<Product> configurationGroup_d365 = JsonConvert.DeserializeObject<IEnumerable<Product>>(pollContent);
            var result = configurationGroup_d365.Aggregate(
                new
                {
                    FormattedResults = new List<ProductInitialPOLL53>(),
                    ErrorResults = new List<ProcessorErrorReturnModel>()
                },
                (acc, source) =>
                {
                    try
                    {
                        string inputString = source.DefaultLedgerDimensionDisplayValue;
                        //string stringWithoutSlashes = inputString.Replace("\\", "");
                        //string stringWithoutHyphens = stringWithoutSlashes.Trim('-');


                        //string dept, sub_dept, cy_class, sub_class;

                        //dept = stringWithoutHyphens.Length >= 3 ? stringWithoutHyphens.Substring(0, 3) : "";
                        //sub_dept = stringWithoutHyphens.Length >= 10 ? stringWithoutHyphens.Substring(4, 6) : "";
                        //cy_class = stringWithoutHyphens.Length >= 20 ? stringWithoutHyphens.Substring(11, 9) : "";
                        //sub_class = stringWithoutHyphens.Length >= 33 ? stringWithoutHyphens.Substring(21, 12) : "";

                        string stringWithoutSlashes = Regex.Replace(inputString, "[^A-Za-z0-9-]", "");
                        string stringWithoutHyphens = stringWithoutSlashes.Trim('-');

                        string dept, sub_dept, cy_class, sub_class;

                        dept = stringWithoutHyphens.Length >= 3 ? stringWithoutHyphens.Substring(0, 3) : "";
                        sub_dept = stringWithoutHyphens.Length >= 10 ? stringWithoutHyphens.Substring(3, 6) : "";
                        cy_class = stringWithoutHyphens.Length >= 20 ? stringWithoutHyphens.Substring(9, 9) : "";
                        sub_class = stringWithoutHyphens.Length >= 30 ? stringWithoutHyphens.Substring(18, 12) : "";

                        Extensions.Cyware.Model.Products mapped = new Extensions.Cyware.Model.Products
                        {
                            sku_number = source.sku_number ?? "",
                            check_digit = 0,
                            item_description = (source.item_description?.Length > 30) ? source.item_description.Substring(0, 30) : source.item_description ?? "", //(source.item_description?.Length > 30) ? source.item_description.Substring(0, 30) : source.item_description ?? "",
                            style_vendor = 0,
                            style_number = "",
                            color_prefix = int.TryParse(source.color, out int _color_prefix) ? _color_prefix : 0,
                            color_code = int.TryParse(source.color, out int _color_code) ? _color_code : 0,
                            size_code = source.size ?? "",
                            dimension = source.size ?? "",
                            set_code = "",
                            hazardous_code = source.hazardous_code ?? "",
                            substitute_sku = 0,
                            status_code = "",
                            price_prompt = source.price_prompt ?? "",
                            no_tickets_item = 0,
                            department = dept, //product.department ?? "",
                            sub_department = sub_dept, //product.sub_department ?? "",
                            cy_class = cy_class, //product.cy_class ?? "",
                            sub_class = sub_class, // product.sub_class ?? "",
                            sku_type = source.sku_type ?? "",
                            buy_code_cs = "",
                            buy_um = source.buy_um ?? "",
                            primary_vendor = int.TryParse(source.PrimaryVendorAccountNumber, out int _primary_vendor) ? _primary_vendor : 0,
                            vendor_number = source.PrimaryVendorAccountNumber ?? "",
                            selling_um = source.SalesUnitSymbol ?? "",
                            case_pack = double.TryParse(source.case_pack, out double _case_pack) ? _case_pack : 0,
                            min_order_qty = int.TryParse(source.min_order_qty, out int _min_order_qty) ? _min_order_qty : 0,
                            order_at = 0,
                            maximum_anytime = int.TryParse(source.maximum_anytime, out int _maximum_anytime) ? _maximum_anytime : 0,
                            vat_code = source.SalesSalesTaxItemGroupCode ?? "",
                            tax_1_code = source.SalesSalesTaxItemGroupCode ?? "",
                            tax_2_code = "",
                            tax_3_code = "",
                            tax_4_code = "",
                            register_item_desc = (source.name?.Length > 18) ? source.name.Substring(0, 18) : source.name ?? "", //(source.name?.Length > 18) ? source.name.Substring(0, 18) : source.name ?? "",
                            allow_discount_yn = source.allow_discount_yn ?? "",
                            sku_hst_code = "",
                            controlled_stock = "",
                            pos_comment = source.pos_comment ?? "",
                            print_set_detail_yn = source.print_set_detail_yn ?? "",
                            ticket_type_reg = "",
                            ticket_type_ad = "",
                            season_code = source.season_code ?? "",
                            currency_code = source.currency_code ?? "",
                            core_sku = 0,
                            menu_category = source.advanced_notes_group
                        };

                        ////Validate before adding formatted customer (throw error if validation fails)
                        Validate(mapped);
                        ////Add the formatted Customers
                        acc.FormattedResults.Add(new ProductInitialPOLL53(mapped));
                    }
                    catch (Exception ex)
                    {
                        ////Add Customers with error
                        acc.ErrorResults.Add(new ProcessorErrorReturnModel
                        {
                            Id = source.sku_number,
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

        private static void Validate(Extensions.Cyware.Model.Products products)
        {

            if (string.IsNullOrEmpty(products.department))
            {
                throw new ArgumentException("Product Category Department is required.");
            }

            if (string.IsNullOrEmpty(products.sub_department))
            {
                throw new ArgumentException("Product Category Sub_Department is required.");
            }

            if (string.IsNullOrEmpty(products.cy_class))
            {
                throw new ArgumentException("Product Category Class is required.");
            }

            if (string.IsNullOrEmpty(products.sub_class))
            {
                throw new ArgumentException("Product Category Sub_Class is required.");
            }

            if (string.IsNullOrEmpty(products.item_description))
            {
                throw new ArgumentException("Product item_description is required.");
            }
        }
    }
}

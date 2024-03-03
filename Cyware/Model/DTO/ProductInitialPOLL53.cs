using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.CustomAttributes;
using KTI.Moo.Extensions.Cyware.Model;
using System.ComponentModel.DataAnnotations;

namespace KTI.Moo.Cyware.Model.DTO
{
    public class ProductInitialPOLL53
    {
        [SortOrder(1)]
        [MaxLength(20)]
        public string sku_number { get; set; }
        [SortOrder(2)]
        [MaxLength(1)]
        public string check_digit { get; set; }
        [SortOrder(3)]
        [MaxLength(30)]
        public string item_description { get; set; }
        [SortOrder(4)]
        [MaxLength(6)]
        public string style_vendor { get; set; }
        [SortOrder(5)]
        [MaxLength(15)]
        public string style_number { get; set; }
        [SortOrder(6)]
        [MaxLength(1)]
        public string color_prefix { get; set; }
        [SortOrder(7)]
        [MaxLength(4)]
        public string color_code { get; set; }
        [SortOrder(8)]
        [MaxLength(4)]
        public string size_code { get; set; }
        [SortOrder(9)]
        [MaxLength(4)]
        public string dimension { get; set; }
        [SortOrder(10)]
        [MaxLength(1)]
        public string set_code { get; set; }
        [SortOrder(11)]
        [MaxLength(2)]
        public string hazardous_code { get; set; }
        [SortOrder(12)]
        [MaxLength(9)]
        public string substitute_sku { get; set; }
        [SortOrder(13)]
        [MaxLength(1)]
        public string status_code { get; set; }
        [SortOrder(14)]
        [MaxLength(1)]
        public string price_prompt { get; set; }
        [SortOrder(15)]
        [MaxLength(2)]
        public string no_tickets_item { get; set; }
        [SortOrder(16)]
        [MaxLength(20)]
        public string department { get; set; }
        [SortOrder(17)]
        [MaxLength(20)]
        public string sub_department { get; set; }
        [SortOrder(18)]
        [MaxLength(20)]
        public string cy_class { get; set; }
        [SortOrder(19)]
        [MaxLength(20)]
        public string sub_class { get; set; }
        [SortOrder(20)]
        [MaxLength(10)]
        public string sku_type { get; set; }
        [SortOrder(21)]
        [MaxLength(1)]
        public string buy_code_cs { get; set; }
        [SortOrder(22)]
        [MaxLength(6)]
        public string primary_vendor { get; set; }
        [SortOrder(23)]
        [MaxLength(15)]
        public string vendor_number { get; set; }
        [SortOrder(24)]
        [MaxLength(3)]
        public string selling_um { get; set; }
        [SortOrder(25)]
        [MaxLength(3)]
        public string buy_um { get; set; }
        [SortOrder(26)]
        [MaxLength(9)]
        public string case_pack { get; set; }
        [SortOrder(27)]
        [MaxLength(7)]
        public string min_order_qty { get; set; }
        [SortOrder(28)]
        [MaxLength(7)]
        public string order_at { get; set; }
        [SortOrder(29)]
        [MaxLength(7)]
        public string maximum_anytime { get; set; }
        [SortOrder(30)]
        [MaxLength(10)]
        public string vat_code { get; set; }
        [SortOrder(31)]
        [MaxLength(10)]
        public string tax_1_code { get; set; }
        [SortOrder(32)]
        [MaxLength(1)]
        public string tax_2_code { get; set; }
        [SortOrder(33)]
        [MaxLength(1)]
        public string tax_3_code { get; set; }
        [SortOrder(34)]
        [MaxLength(1)]
        public string tax_4_code { get; set; }
        [SortOrder(35)]
        [MaxLength(18)]
        public string register_item_desc { get; set; }
        [SortOrder(36)]
        [MaxLength(1)]
        public string allow_discount_yn { get; set; }
        [SortOrder(37)]
        [MaxLength(1)]
        public string sku_hst_code { get; set; }
        [SortOrder(38)]
        [MaxLength(1)]
        public string controlled_stock { get; set; }
        [SortOrder(39)]
        [MaxLength(20)]
        public string pos_comment { get; set; }
        [SortOrder(40)]
        [MaxLength(1)]
        public string print_set_detail_yn { get; set; }
        [SortOrder(41)]
        [MaxLength(1)]
        public string ticket_type_reg { get; set; }
        [SortOrder(42)]
        [MaxLength(1)]
        public string ticket_type_ad { get; set; }
        [SortOrder(43)]
        [MaxLength(3)]
        public string season_code { get; set; }
        [SortOrder(44)]
        [MaxLength(9)]
        public string core_sku { get; set; }

        [SortOrder(45)]
        [MaxLength(60)]
        public string menu_category { get; set; }    


        public ProductInitialPOLL53(Products _product)
        {
            var helper = new PollMapping();
            this.sku_number = helper.FormatStringAddSpacePadding(_product.sku_number.ToString(), (typeof(ProductInitialPOLL53).GetProperty("sku_number").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.check_digit = helper.FormatIntAddZeroPrefix(_product.check_digit.ToString(), (typeof(ProductInitialPOLL53).GetProperty("check_digit").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.item_description = helper.FormatStringAddSpacePadding(_product.item_description, (typeof(ProductInitialPOLL53).GetProperty("item_description").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.style_vendor = helper.FormatIntAddZeroPrefix(_product.style_vendor.ToString(), (typeof(ProductInitialPOLL53).GetProperty("style_vendor").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.style_number = helper.FormatStringAddSpacePadding(_product.style_number, (typeof(ProductInitialPOLL53).GetProperty("style_number").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.color_prefix = helper.FormatIntAddZeroPrefix(_product.color_prefix.ToString(), (typeof(ProductInitialPOLL53).GetProperty("color_prefix").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.color_code = helper.FormatIntAddZeroPrefix(_product.color_code.ToString(), (typeof(ProductInitialPOLL53).GetProperty("color_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.size_code = helper.FormatStringAddSpacePadding(_product.size_code, (typeof(ProductInitialPOLL53).GetProperty("size_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.dimension = helper.FormatStringAddSpacePadding(_product.dimension, (typeof(ProductInitialPOLL53).GetProperty("dimension").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.set_code = helper.FormatStringAddSpacePadding(_product.set_code, (typeof(ProductInitialPOLL53).GetProperty("set_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.hazardous_code = helper.FormatStringAddSpacePadding(_product.hazardous_code, (typeof(ProductInitialPOLL53).GetProperty("hazardous_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.substitute_sku = helper.FormatIntAddZeroPrefix(_product.substitute_sku.ToString(), (typeof(ProductInitialPOLL53).GetProperty("substitute_sku").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.status_code = helper.FormatStringAddSpacePadding(_product.status_code, (typeof(ProductInitialPOLL53).GetProperty("status_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.price_prompt = helper.FormatStringAddSpacePadding(_product.price_prompt, (typeof(ProductInitialPOLL53).GetProperty("price_prompt").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.no_tickets_item = helper.FormatIntAddZeroPrefix(_product.no_tickets_item.ToString(), (typeof(ProductInitialPOLL53).GetProperty("no_tickets_item").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.department = helper.FormatStringAddSpacePadding(_product.department.ToString(), (typeof(ProductInitialPOLL53).GetProperty("department").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.sub_department = helper.FormatStringAddSpacePadding(_product.sub_department.ToString(), (typeof(ProductInitialPOLL53).GetProperty("sub_department").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.cy_class = helper.FormatStringAddSpacePadding(_product.cy_class.ToString(), (typeof(ProductInitialPOLL53).GetProperty("cy_class").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.sub_class = helper.FormatStringAddSpacePadding(_product.sub_class.ToString(), (typeof(ProductInitialPOLL53).GetProperty("sub_class").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.sku_type = helper.FormatStringAddSpacePadding(_product.sku_type, (typeof(ProductInitialPOLL53).GetProperty("sku_type").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.buy_code_cs = helper.FormatStringAddSpacePadding(_product.buy_code_cs, (typeof(ProductInitialPOLL53).GetProperty("buy_code_cs").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.primary_vendor = helper.FormatIntAddZeroPrefix(_product.primary_vendor.ToString(), (typeof(ProductInitialPOLL53).GetProperty("primary_vendor").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.vendor_number = helper.FormatStringAddSpacePadding(_product.vendor_number, (typeof(ProductInitialPOLL53).GetProperty("vendor_number").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.selling_um = helper.FormatStringAddSpacePadding(_product.selling_um, (typeof(ProductInitialPOLL53).GetProperty("selling_um").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.buy_um = helper.FormatStringAddSpacePadding(_product.buy_um, (typeof(ProductInitialPOLL53).GetProperty("buy_um").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.case_pack = helper.FormatDecimalAddZeroPrefixAndSuffix(_product.case_pack.ToString(), ((typeof(ProductInitialPOLL53).GetProperty("case_pack").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0), 2);
            this.min_order_qty = helper.FormatIntAddZeroPrefix(_product.min_order_qty.ToString(), (typeof(ProductInitialPOLL53).GetProperty("min_order_qty").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.order_at = helper.FormatIntAddZeroPrefix(_product.order_at.ToString(), (typeof(ProductInitialPOLL53).GetProperty("order_at").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.maximum_anytime = helper.FormatIntAddZeroPrefix(_product.maximum_anytime.ToString(), (typeof(ProductInitialPOLL53).GetProperty("maximum_anytime").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.vat_code = helper.FormatStringAddSpacePadding(_product.vat_code, (typeof(ProductInitialPOLL53).GetProperty("vat_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.tax_1_code = helper.FormatStringAddSpacePadding(_product.tax_1_code, (typeof(ProductInitialPOLL53).GetProperty("tax_1_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.tax_2_code = helper.FormatStringAddSpacePadding(_product.tax_2_code, (typeof(ProductInitialPOLL53).GetProperty("tax_2_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.tax_3_code = helper.FormatStringAddSpacePadding(_product.tax_3_code, (typeof(ProductInitialPOLL53).GetProperty("tax_3_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.tax_4_code = helper.FormatStringAddSpacePadding(_product.tax_4_code, (typeof(ProductInitialPOLL53).GetProperty("tax_4_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.register_item_desc = helper.FormatStringAddSpacePadding(_product.register_item_desc, (typeof(ProductInitialPOLL53).GetProperty("register_item_desc").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.allow_discount_yn = helper.FormatStringAddSpacePadding(_product.allow_discount_yn, (typeof(ProductInitialPOLL53).GetProperty("allow_discount_yn").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.sku_hst_code = helper.FormatStringAddSpacePadding(_product.sku_hst_code, (typeof(ProductInitialPOLL53).GetProperty("sku_hst_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.controlled_stock = helper.FormatStringAddSpacePadding(_product.controlled_stock, (typeof(ProductInitialPOLL53).GetProperty("controlled_stock").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.pos_comment = helper.FormatStringAddSpacePadding(_product.pos_comment, (typeof(ProductInitialPOLL53).GetProperty("pos_comment").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.print_set_detail_yn = helper.FormatStringAddSpacePadding(_product.print_set_detail_yn, (typeof(ProductInitialPOLL53).GetProperty("print_set_detail_yn").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.ticket_type_reg = helper.FormatStringAddSpacePadding(_product.ticket_type_reg, (typeof(ProductInitialPOLL53).GetProperty("ticket_type_reg").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.ticket_type_ad = helper.FormatStringAddSpacePadding(_product.ticket_type_ad, (typeof(ProductInitialPOLL53).GetProperty("ticket_type_ad").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.season_code = helper.FormatStringAddSpacePadding(_product.season_code, (typeof(ProductInitialPOLL53).GetProperty("season_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.core_sku = helper.FormatIntAddZeroPrefix(_product.core_sku.ToString(), (typeof(ProductInitialPOLL53).GetProperty("core_sku").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.menu_category = helper.FormatStringAddSpacePadding(_product.menu_category, (typeof(ProductInitialPOLL53).GetProperty("menu_category").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
        }

        public string Concat(ProductInitialPOLL53 obj)
        {
            var helper = new PollMapping();
            return helper.ConcatenateValues(obj);
        }
    }
}

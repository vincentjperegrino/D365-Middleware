using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class ProductsBase
    {
        //public virtual int sku_number { get; set; }
        public virtual string sku_number { get; set; }
        public virtual int check_digit { get; set; }
        public virtual string item_description { get; set; }
        public virtual int style_vendor { get; set; }
        public virtual string style_number { get; set; }
        public virtual int color_prefix { get; set; }
        public virtual int color_code { get; set; }
        public virtual string size_code { get; set; }
        public virtual string dimension { get; set; }
        public virtual string set_code { get; set; }
        public virtual string hazardous_code { get; set; }
        public virtual int substitute_sku { get; set; }
        public virtual string status_code { get; set; }
        public virtual string price_prompt { get; set; }
        public virtual int no_tickets_item { get; set; }
        public virtual string department { get; set; }  /// change from int to string
        public virtual string sub_department { get; set; } /// change from int to string
        public virtual string cy_class { get; set; } /// change from int to string
        public virtual string sub_class { get; set; } /// change from int to string
        public virtual string sku_type { get; set; }
        public virtual string buy_code_cs { get; set; }
        public virtual int primary_vendor { get; set; }
        public virtual string vendor_number { get; set; }
        public virtual string selling_um { get; set; }
        public virtual string buy_um { get; set; }
        public virtual double case_pack { get; set; }
        public virtual int min_order_qty { get; set; }
        public virtual int order_at { get; set; }
        public virtual int maximum_anytime { get; set; }
        public virtual string vat_code { get; set; }
        public virtual string tax_1_code { get; set; }
        public virtual string tax_2_code { get; set; }
        public virtual string tax_3_code { get; set; }
        public virtual string tax_4_code { get; set; }
        public virtual string register_item_desc { get; set; }
        public virtual string allow_discount_yn { get; set; }
        public virtual string sku_hst_code { get; set; }
        public virtual string controlled_stock { get; set; }
        public virtual string pos_comment { get; set; }
        public virtual string print_set_detail_yn { get; set; }
        public virtual string current_retail { get; set; }
        public virtual string current_cost { get; set; }
        public virtual string competitor_price { get; set; }
        public virtual string ticket_type_reg { get; set; }
        public virtual string season_code { get; set; }
        public virtual string ticket_type_ad { get; set; }
        public virtual string currency_code { get; set; }
        public virtual string price_book { get; set; }
        public virtual int core_sku { get; set; }
        public virtual string menu_category { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class ProductPriceBase
    {
        public virtual string sku_number { get; set; }
        public virtual int upc_code { get; set; }
        public virtual string upc_type { get; set; }
        public virtual string price_event_number { get; set; }
        public virtual string currency_code { get; set; }
        public virtual string price_book { get; set; }
        public virtual DateTime? start_date { get; set; }
        public virtual DateTime? end_date { get; set; }
        public virtual string promo_flag_yn { get; set; }
        public virtual int event_price_multiple { get; set; }
        public virtual decimal event_price { get; set; }
        public virtual int price_method_code { get; set; }
        public virtual int mix_match_code { get; set; }
        public virtual int deal_quantity { get; set; }
        public virtual decimal deal_price { get; set; }
        public virtual int buy_quantity { get; set; }
        public virtual decimal buy_value { get; set; }
        public virtual string buy_value_type { get; set; }
        public virtual int qty_end_value { get; set; }
        public virtual int quantity_break { get; set; }
        public virtual decimal quantity_group_price { get; set; }
        public virtual decimal quantity_unit_price { get; set; }
        public virtual string cust_promo_code { get; set; }
        public virtual string cust_number { get; set; }
        public virtual int precedence_level { get; set; }
        public virtual string default_currency { get; set; }
        public virtual string default_price_book { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model;

public class ProductVariantBase
{
    public string barcode { get; set; }
    public decimal compare_at_price { get; set; }
    public DateTime created_at { get; set; }
    public string fulfillment_service { get; set; }
    public long grams { get; set; }
    public decimal weight { get; set; }
    public string weight_unit { get; set; }
    public long id { get; set; }
    public long inventory_item_id { get; set; }
    public string inventory_management { get; set; }
    public string inventory_policy { get; set; }
    public long inventory_quantity { get; set; }
    public string option1 { get; set; }
    public int position { get; set; }
    public decimal price { get; set; }
    public long product_id { get; set; }
    public bool requires_shipping { get; set; }
    public string sku { get; set; }
    public bool taxable { get; set; }
    public string title { get; set; }
    public DateTime updated_at { get; set; }
}

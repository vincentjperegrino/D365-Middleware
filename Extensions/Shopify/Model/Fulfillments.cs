using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class Fulfillments

{
    
    public Fulfillments() { }

    public Fulfillments(ShopifySharp.Fulfillment fulfillment)

    {
        created_at = fulfillment.CreatedAt?.DateTime ?? default;
        id = fulfillment?.Id ?? default; 
        order_id = fulfillment.OrderId ?? default;
        status = fulfillment.Status;
        tracking_company = fulfillment.TrackingCompany;
        tracking_number = fulfillment.TrackingNumber;
        updated_at = fulfillment.UpdatedAt?.DateTime ?? default;

    }

    public DateTime created_at { get; set; }
    public long id { get; set; }
    public long order_id { get; set; }
    public string status { get; set; }
    public string tracking_company { get; set; }
    public string tracking_number { get; set; }
    public DateTime updated_at { get; set; }

}


using System.Collections.Generic;

namespace KTI.Moo.Extensions.Lazada.Model.DTO.Fulfillments;

public class PackRequirement
{
    public List<OrderHeaderID_OrderItemsID_Packed> pack_order_list { get; set; }
    public string delivery_type { get; set; } = "dropship";
    public string shipping_allocate_type { get; set; }
    public string shipment_provider_code { get; set; }

}

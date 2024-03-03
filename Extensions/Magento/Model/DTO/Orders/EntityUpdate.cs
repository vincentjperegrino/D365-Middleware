using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Magento.Model.DTO.Orders;

public class EntityUpdate
{
    [JsonProperty("entity_id")]
    public int order_id { get; set; }

    [JsonProperty("status")]
    public string status { get; set; }

    public List<OrderStatusHistory> status_histories { get; set; }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Magento.Model.DTO.Orders;

public class AddInvoice
{
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
    public bool appendComment { get; set; }

    public Comments comment { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
    public bool capture { get; set; }

    public List<InvoiceOrderItems> items { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
    public bool notify { get; set; }


}

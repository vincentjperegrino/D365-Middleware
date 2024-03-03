using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Magento.Model.DTO.Orders;

public class Comments
{

    public string comment { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
    public int is_visible_on_front { get; set; }
}

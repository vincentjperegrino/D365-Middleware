using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KTI.Moo.Extensions.OctoPOS.Model;

public class InventoryProducts : Core.Model.InventoryBase
{

    [JsonProperty("Quantity")]
    public override double qtyonhand { get; set; }
    //sku
    [JsonProperty("ProductCode")]
    public override string product { get; set; }

    public string BatchNumber { get; set; }
    public DateTime Expiredate { get; set; }
    public string SerialNumber { get; set; }
    public string[] SerialList { get; set; }
}

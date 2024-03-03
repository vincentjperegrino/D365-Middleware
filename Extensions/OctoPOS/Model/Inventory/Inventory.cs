using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Linq;

namespace KTI.Moo.Extensions.OctoPOS.Model;

public class Inventory : Core.Model.InventoryBase
{
    [JsonProperty("LocationCode")]
    public override string warehouse { get; set; }
    public string RaiseBy { get; set; } = "admin";
    public DateTime RaiseDate { get; set; } = Helper.DateTimeHelper.PHTnow();
    public string Remark { get; set; } 
    public string MovementType { get; set; } 
    public int Inventorypurpose { get; set; } = Helper.InventoryPurposeHelper.ForSales;
    public string CustomNumber { get; set; }
    public string StockMovementNumber { get; set; }

    public List<InventoryProducts> MovementItems { get; set; }

}

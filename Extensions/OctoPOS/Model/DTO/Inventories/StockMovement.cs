using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.OctoPOS.Model.DTO.Inventories;

public class StocksMovement
{
    public Model.Inventory StockMovement { get; set; }

    public List<Model.InventoryProducts> MovementItems { get;set;} 


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.OctoPOS.Model.DTO.Inventories;

public class GetQuantity
{


    public string ProductCode { get; set; }
    public string Description { get; set; }
    public double AvailableQuantity { get; set; }
    public double ReservedQuantity { get; set; }
    public double AvailableQuantityBeforeReserved { get; set; }
    public double Quantity { get; set; }
    public double PhysicalQuantity { get; set; }

}

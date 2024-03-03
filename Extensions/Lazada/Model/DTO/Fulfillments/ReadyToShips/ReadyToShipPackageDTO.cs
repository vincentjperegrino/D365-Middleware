using Lazop.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Lazada.Model.DTO.Fulfillments.ReadyToShips;

public class ReadyToShipPackageDTO
{
    public string msg { get; set; }
    public string item_err_code { get; set; }
    public string package_id { get; set; }
    public string retry { get; set; }
}

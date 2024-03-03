using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Lazada.Model;

public class OrderItempackage : Core.Model.PackageItemBase
{
    public string msg { get; set; }
    public string item_err_code { get; set; }
    public bool retry { get; set; }
}

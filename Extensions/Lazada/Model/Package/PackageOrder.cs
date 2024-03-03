using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Lazada.Model;

public class PackageOrder : Core.Model.PackageBase
{
    public List<OrderItempackage> order_item_list { get; set; }

}

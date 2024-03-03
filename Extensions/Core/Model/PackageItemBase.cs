using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model;

public class PackageItemBase
{

    public virtual long order_item_id { get; set; }
    public virtual string tracking_number { get; set; }
    public virtual string shipment_provider { get; set; }
    public virtual string package_id { get; set; }

}

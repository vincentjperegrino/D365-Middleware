using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Model.KTIdev.Receivers;

public class Shipment : CRM.Model.ShipmentBase
{
    public List<ShipmentItem> shipmentItems { get; set; }
}

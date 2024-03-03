using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Lazada.Model
{
    public class OrderStatus : Core.Model.OrderStatusBase
    {
        public int laz_cancelReason { get; set; }
    }
}

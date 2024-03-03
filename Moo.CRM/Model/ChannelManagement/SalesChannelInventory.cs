using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.Model.ChannelManagement
{
    public class Inventory : Base.Model.ChannelManagement.SalesChannelBase
    {
        public List<CustomField> CustomFieldList { get; set; }
        public List<InventoryBase> InventoryList { get; set; }
    }
}

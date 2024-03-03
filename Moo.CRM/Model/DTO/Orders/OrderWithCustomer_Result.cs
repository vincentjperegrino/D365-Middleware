using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.Model.DTO.Orders
{
    public class OrderWithCustomer_Result
    {
        [JsonProperty(PropertyName = "@odata.context")]
        public string context { get; set; }

        [JsonProperty(PropertyName = "kti_UpsertOrder_Response")]
        public bool value { get; set; }
    }
}

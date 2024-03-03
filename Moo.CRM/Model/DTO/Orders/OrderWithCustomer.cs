using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.Model.DTO.Orders
{
    public class OrderWithCustomer<T, R, D> where T : Model.DTO.Orders.Plugin.Customer where R : Model.DTO.Orders.Plugin.Order where D : Model.DTO.Orders.Plugin.OrderItem
    {
        public int companyid { get; set; }
        public string domainType { get; set; } = Moo.Base.Helpers.DomainType.order;

        [JsonProperty(PropertyName = "kti_UpsertOrder_CustomerParameters")]
        public T Customer { get; set; }
        [JsonProperty(PropertyName = "kti_UpsertOrder_OrderParameters")]
        public R Order { get; set; }
        [JsonProperty(PropertyName = "kti_UpsertOrder_OrderLineParameters")]
        public List<D> OrderLine { get; set; }


    }
}

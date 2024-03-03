using System.Collections.Generic;

namespace KTI.Moo.Extensions.Cyware.App.Receiver.Models
{
    public class D365FOConfig
    {
        public List<dynamic> RetailStores { get; set; }
        public List<dynamic> Warehouses { get; set; }
        public List<dynamic> Customers { get; set; }
        public List<dynamic> Terminals { get; set; }
    }
}

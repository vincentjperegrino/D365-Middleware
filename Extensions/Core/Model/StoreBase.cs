using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class StoreBase
    {
        //public virtual string store_name { get; set; }

        public string StoreNumber { get; set; }
        public string Name { get; set; }
        public string Address_Line1 { get; set; }
        public string Address_Line2 { get; set; }
        public string Address_Line3 { get; set; }
        public string PhoneNumber { get; set; }
        public string ManagerName { get; set; }
        public string StoreOffice { get; set; }
        public string Currency { get; set; }
        public string TaxCurrency { get; set; }
        public string Language { get; set; }

    }
}

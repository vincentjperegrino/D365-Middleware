using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class ProductBarcodeBase
    {
        public virtual string product_code { get; set; }
        public virtual string sku_number { get; set; }
        public virtual string upc_type { get; set; }
        public virtual string upc_unit_of_measure { get; set; }
    }
}

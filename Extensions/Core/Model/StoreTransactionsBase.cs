using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class StoreTransactionsBase
    {
        public virtual string Headers { get; set; }
        public virtual string Lines { get; set; }
        public virtual string Discounts { get; set; }
        public virtual string Payments { get; set; }
    }
}

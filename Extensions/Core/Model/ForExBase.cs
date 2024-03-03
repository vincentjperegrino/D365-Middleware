using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class ForExBase
    {
        public virtual string from_currency_code { get; set; }
        public virtual string to_currency_code { get; set; }
        public virtual decimal currency_exch_rate { get; set; }
        public virtual string code { get; set; }
        public virtual string conversion_rate_type { get; set; }
        public virtual string effective_date { get; set; }
        public virtual string rounding_multiple { get; set; }
        public virtual string rounding_multiple_to { get; set; }
        public virtual string currency_exch_rate_mt { get; set; }
    }
}

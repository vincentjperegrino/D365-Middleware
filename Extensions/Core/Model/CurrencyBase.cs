using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class CurrencyBase
    {
        public virtual string currency_code { get; set; }
        public virtual string description { get; set; }
        public virtual int is_default { get; set; }
        public virtual int default_action { get; set; }
        public virtual double default_rate { get; set; }
        public virtual float local_rate { get; set; }
    }
}

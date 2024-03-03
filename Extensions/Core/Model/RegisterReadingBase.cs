using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class RegisterReadingBase
    {
        public virtual string LocationCode { get; set; }
        public virtual int TransactionDate { get; set; }
        public virtual int RegisterNum { get; set; }
        public virtual int IsEod { get; set; }
        public virtual double TransactionCount { get; set; }
        public virtual double TransactionAmount { get; set; }
        public virtual double TransactionNumber { get; set; }
        public virtual int IsSync { get; set; }
        public virtual double TransNonVatAmount { get; set; }
        public virtual double TransZeroRatedAmount { get; set; }
        public virtual double TransVatAmount { get; set; }
        public virtual double TransVatableAmount { get; set; }
        public virtual decimal SeniorDiscount { get; set; }
    }
}

using KTI.Moo.Extensions.Core.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class DiscountTypeBase
    {
        public virtual string DiscountCode { get; set; }
        public virtual string DiscountTypeCd { get; set; }
        public virtual string Description { get; set; }
        public virtual string DiscType { get; set; }
        public virtual string Readonly { get; set; }
        public virtual string DiscValue { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public string StartTime { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public string EndTime { get; set; }
        public virtual double MinAmount { get; set; }
        public virtual double MaxAmount { get; set; }
        public virtual string DiscountRule { get; set; }
        public virtual string AccountTypeCode { get; set; }
        public virtual string RequireAccount { get; set; }
        public virtual string FreeItem { get; set; }
        public virtual int FreeItemLimit { get; set; }
        public virtual int FreeItemQty { get; set; }
        public virtual int EventNumber { get; set; }

        public virtual string ExportCurrentPrice { get; set; }

        public virtual int Posted { get; set; }

        public virtual DateTime PostedOn { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class TenderTypeBase
    {
        public virtual string TenderTypeCd { get; set; }
        public virtual string Description { get; set; }
        public virtual string AllowChange { get; set; }
        public virtual string RequireValidation { get; set; }
        public virtual string IsCreditCard { get; set; }
        public virtual string IsGc { get; set; }
        public virtual string Skey { get; set; }
        public virtual string IsDrawer { get; set; }
        public virtual string IsDebitCard { get; set; }
        public virtual string IsCheck { get; set; }
        public virtual string IsCharge { get; set; }
        public virtual string IsCash { get; set; }
        public virtual string IsGarbage { get; set; }
        public virtual string IsCashdec { get; set; }
        public virtual string IsRebate { get; set; }
        public virtual string IsEgc { get; set; }
        public virtual string IsTelcoPull { get; set; }
        public virtual string IsTelcoPush { get; set; }
        public virtual string IsGuarantor { get; set; }
        public virtual string IsCardConnect { get; set; }
        public virtual string IsGovRebate { get; set; }
        public virtual string ItemCode { get; set; }
        public virtual string IsIntegrated { get; set; }
        public virtual string IsLoyalty { get; set; }
        public virtual string IsHoreca { get; set; }
        public virtual string IsMobile { get; set; } 
    }
}

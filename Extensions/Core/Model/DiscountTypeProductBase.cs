using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class DiscountTypeProductBase
    {
        public virtual string ItemCode { get; set; }
        public virtual string DisccountCd { get; set; }
        public virtual decimal DiscValueInAmount { get; set; }     // change from string to decimal
        public virtual string DiscValueInPercentage { get; set; }
        public virtual string DiscType { get; set; }
        public virtual string ItemGroup { get; set; }
        public virtual string FreeItem { get; set; }
        public virtual string FreeItemQty { get; set; }
        public virtual string Tolerance { get; set; }
        public virtual string EventNum { get; set; }
        public virtual string ReqAmount { get; set; }
        public virtual string ReqQty { get; set; }

        ///Additional Fields
        ///

        public virtual string PartyCodeType { get; set; }
        public virtual string AccountSelection { get; set; }
        public virtual string Configuration { get; set; }
        public virtual string Site { get; set; }
        public virtual string Warehouse { get; set; }
        public virtual decimal From { get; set; }    //change from int to decimal
        public virtual decimal To { get; set; }     //change from int to decimal
        public virtual string Unit { get; set; }
        public virtual string Currency { get; set; }
        public virtual string AttributeBasedPricingID { get; set; }
        public virtual string DimensionValidation { get; set; }
        public virtual string TradeAgreementValidation { get; set; } //change from int to string
        public virtual string DimensionNumber { get; set; }
        public virtual string DiscountPercentage2 { get; set; }  //change from decimal to string
        public virtual string DisregardLeadTime { get; set; }  //Change  from int to string
        public virtual string FindNext { get; set; }          //Change from int to string
        public virtual DateTime FromDate { get; set; }
        public virtual string IncludeInUnitPrice { get; set; }  //Change from int to string
        public virtual string IncludeGenericCurrency { get; set; }  //Change from int to string
        public virtual string LeadTime { get; set; }  //Change from decimal to string
        public virtual string Log { get; set; }
        public virtual string Module { get; set; }  //Change from int to string
        public virtual string PriceAgreements { get; set; }
        public virtual string PriceCharges { get; set; }        //Change from decimal to string
        public virtual string PriceUnit { get; set; }      //Change from decimal to string
        public virtual DateTime ToDate { get; set; }
        public virtual string FromTime { get; set; }
        public virtual string ToTime { get; set; }
    }
}

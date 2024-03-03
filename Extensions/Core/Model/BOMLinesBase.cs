using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class BOMLinesBase
    {
        public string BOMID { get; set; }
        public int LINECREATIONSEQUENCENUMBER { get; set; }
        public decimal CATCHWEIGHTQUANTITY { get; set; }
        public string CONFIGURATIONGROUPID { get; set; }
        public decimal CONSTANTSCRAPQUANTITY { get; set; }
        public decimal CONSUMPTIONCALCULATIONCONSTANT { get; set; }
        public string CONSUMPTIONCALCULATIONMETHOD { get; set; }
        public string CONSUMPTIONSITEID { get; set; }
        public string CONSUMPTIONTYPE { get; set; }
        public string CONSUMPTIONWAREHOUSEID { get; set; }
        public string FLUSHINGPRINCIPLE { get; set; }
        public string ISCONSUMEDATOPERATIONCOMPLETE { get; set; }
        public string ISRESOURCECONSUMPTIONUSED { get; set; }
        public string ITEMNUMBER { get; set; }
        public decimal LINENUMBER { get; set; }
        public string LINETYPE { get; set; }
        public decimal PHYSICALPRODUCTDENSITY { get; set; }
        public decimal PHYSICALPRODUCTDEPTH { get; set; }
        public decimal PHYSICALPRODUCTHEIGHT { get; set; }
        public decimal PHYSICALPRODUCTWIDTH { get; set; }
        public string POSITIONNUMBER { get; set; }
        public string PRODUCTCOLORID { get; set; }
        public string PRODUCTCONFIGURATIONID { get; set; }
        public string PRODUCTSIZEID { get; set; }
        public string PRODUCTSTYLEID { get; set; }
        public string PRODUCTUNITSYMBOL { get; set; }
        public string PRODUCTVERSIONID { get; set; }
        public decimal QUANTITY { get; set; }
        public decimal QUANTITYDENOMINATOR { get; set; }
        public decimal QUANTITYROUNDINGUPMULTIPLES { get; set; }
        public string ROUNDINGUPMETHOD { get; set; }
        public int ROUTEOPERATIONNUMBER { get; set; }
        public string SUBBOMID { get; set; }
        public string SUBROUTEID { get; set; }
        public DateTime VALIDFROMDATE { get; set; }
        public DateTime VALIDTODATE { get; set; }
        public decimal VARIABLESCRAPPERCENTAGE { get; set; }
        public string VENDORACCOUNTNUMBER { get; set; }
        public string WAREHOUSEBOMRELEASERESERVATIONREQUIREMENTRULE { get; set; }
        public string WILLCOSTCALCULATIONINCLUDELINE { get; set; }
        public string WILLMANUFACTUREDITEMINHERITBATCHATTRIBUTES { get; set; }
        public string WILLMANUFACTUREDITEMINHERITSHELFLIFEDATES { get; set; }
    }
}

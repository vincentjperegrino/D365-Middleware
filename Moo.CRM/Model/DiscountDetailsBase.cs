using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.Model
{
    public class DiscountDetailsBase
    {
        public string OFFERID { get; set; }
        public double LINENUM { get; set; }
        public string BATCHNUMBER { get; set; }
        public string COLOR { get; set; }
        public string COMBINATIONLINESTRUCTURE { get; set; }
        public string CONFIGURATION { get; set; }
        public string DESCRIPTION { get; set; }
        public double DISCOUNTPERCENTORVALUE { get; set; }
        public int FREEITEMGROUP { get; set; }
        public string INVENTORYSTATUS { get; set; }
        public string ISDISCOUNTAPPLYINGLINE { get; set; }
        public string ISMANDATORY { get; set; }
        public double LEASTAMOUNT { get; set; }
        public double LEASTQUANTITY { get; set; }
        public string LICENSEPLATE { get; set; }
        public string LINE1 { get; set; }
        public string LINE10 { get; set; }
        public string LINE11 { get; set; }
        public string LINE12 { get; set; }
        public string LINE13 { get; set; }
        public string LINE14 { get; set; }
        public string LINE15 { get; set; }
        public string LINE2 { get; set; }
        public string LINE3 { get; set; }
        public string LINE4 { get; set; }
        public string LINE5 { get; set; }
        public string LINE6 { get; set; }
        public string LINE7 { get; set; }
        public string LINE8 { get; set; }
        public string LINE9 { get; set; }
        public string LINETYPE { get; set; }
        public string LOCATION { get; set; }
        public string MIXANDMATCHDISCOUNTTYPE { get; set; }
        public string MIXANDMATCHLINEGROUP { get; set; }
        public int MIXANDMATCHNUMBEROFITEMSNEEDED { get; set; }
        public string NAME { get; set; }
        public double OFFERDISCOUNTAMOUNT { get; set; }
        public string OFFERDISCOUNTMETHOD { get; set; }
        public string OFFERDISCOUNTMETHODN1 { get; set; }
        public double OFFERDISCOUNTPERCENTAGE { get; set; }
        public double OFFERPRICE { get; set; }
        public double OFFERPRICEINCLTAXN1 { get; set; }
        public double OFFERPRICEN1 { get; set; }
        public string OWNER { get; set; }
        public string PRICEATTRIBUTEGROUPNAME { get; set; }
        public string PRICEATTRIBUTEGROUPTYPE { get; set; }
        public string SERIALNUMBER { get; set; }
        public string SITE { get; set; }
        public string SIZE { get; set; }
        public string STYLE { get; set; }
        public string THRESHOLDAPPLYINGLINEDISCOUNTMETHOD { get; set; }
        public double THRESHOLDLINEQUANTITYLIMIT { get; set; }
        public double THRESHOLDTIERAMOUNT { get; set; }
        public string UNITAPPLIESTOALL { get; set; }
        public string UNITOFMEASURESYMBOL { get; set; }
        public string WAREHOUSE { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string moosourcesystem { get; set; }
        //Customized
        [DataType(DataType.Text)]
        public string mooexternalid { get; set; }
        //Customized
        //[CompanyIdAttribute]
        public int companyid { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moo.FO.Model
{
    public class StoreTransactionDiscounts
    {
        public string dataAreaId { get; set; }
        public string Terminal { get; set; }
        public string TransactionNumber { get; set; }
        public int SalesLineNumber { get; set; }
        public string OperatingUnitNumber { get; set; }
        public int LineNumber { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountCost { get; set; }
        public string DiscountOfferId { get; set; }
        public string DiscountOriginType { get; set; }
        public string DiscountCode { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string CustomerDiscountType { get; set; }
        public int BundleId { get; set; }
        public string ManualDiscountType { get; set; }
        public decimal EffectiveAmount { get; set; }
        public int DealPrice { get; set; }
        public int StoreId { get; set; }
        //public int Channel { get; set; }
        //public int RecVersion { get; set; }
        //public int Partition { get; set; }
        //public int RecID { get; set; }
        //public int RetailChannelTableOMOperatingUnitID { get; set; }
        //public int RecVersion2 { get; set; }
        //public int Partition2 { get; set; }
        //public int RecID2 { get; set; }
        //public int RecVersion3 { get; set; }
        //public int Partition3 { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.App.Receiver.Models
{
    public class FOSalesTransactionDiscount
    {
        public FOSalesTransactionDiscount()
        {
        }

        public FOSalesTransactionDiscount(string[] records)
        {
            this.dataAreaId = records[1];
            this.Terminal = records[2];
            //this.TransactionNumber = $"R-{records[3]}-1";
            this.TransactionNumber = records[3];
            this.SalesLineNumber = records[4];
            this.OperatingUnitNumber = records[5];
            this.LineNumber = records[6];
            this.DiscountAmount = records[7];
            this.DiscountCost = records[8];
            this.DiscountOfferId = records[9];
            this.DiscountOriginType = records[10];
            this.DiscountCode = records[11];
            this.DiscountPercentage = records[12];
            this.CustomerDiscountType = records[13];
            this.BundleId = records[14];
            this.ManualDiscountType = records[15];
            this.EffectiveAmount = records[16];
            this.DealPrice = records[17];
        }

        public string dataAreaId { get; set; }
        public string Terminal { get; set; }
        public string TransactionNumber { get; set; }
        public string SalesLineNumber { get; set; }
        public string OperatingUnitNumber { get; set; }
        public string LineNumber { get; set; }
        public string DiscountAmount { get; set; }
        public string DiscountCost { get; set; }
        public string DiscountOfferId { get; set; }
        public string DiscountOriginType { get; set; }
        public string DiscountCode { get; set; }
        public string DiscountPercentage { get; set; }
        public string CustomerDiscountType { get; set; }
        public string BundleId { get; set; }
        public string ManualDiscountType { get; set; }
        public string EffectiveAmount { get; set; }
        public string DealPrice { get; set; }
    }
}

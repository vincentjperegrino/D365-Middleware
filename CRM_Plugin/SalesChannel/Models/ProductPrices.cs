using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.SalesChannel.Models
{
    public class ProductPrices
    {
        public ProductPrices(Entity entity)
        {
            if (entity.Contains("amount"))
                amount = ((Money)entity["amount"]).Value;

            if (entity.Contains("discountamount"))
                discountamount = (decimal)entity["discountamount"];

            if (entity.Contains("discountpercentage"))
                discountpercentage = (decimal)entity["discountpercentage"];

            if (entity.Contains("pricelistid"))
                pricelistid = ((EntityReference)entity["pricelistid"]).Id.ToString();

            if (entity.Contains("pl.begindate"))
                startdatetime = ((DateTime)entity["begindate"]);

            if (entity.Contains("pl.enddate"))
                enddatetime = ((DateTime)entity["enddate"]);

            if (entity.Contains("sc.kti_saleschannelcode"))
                saleschannelcode = ((string)entity["sc.kti_saleschannelcode"]);
        }

        #region properties
        public decimal amount { get; set; }
        public decimal discountamount { get; set; }
        public decimal discountpercentage { get; set; }
        public string pricelistid { get; set; }
        public DateTime startdatetime { get; set; }
        public DateTime enddatetime { get; set; }
        public string saleschannelcode { get; set; }
        #endregion
    }
}

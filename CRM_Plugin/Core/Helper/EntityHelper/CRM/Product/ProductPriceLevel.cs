using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Core.Helper.EntityHelper.CRM
{
    internal class ProductPriceLevel : BaseEntity
    {
        new public static string entity_name = "productpricelevel";
        new public static string entity_id = "productpricelevelid";

        public static string amount = "amount";
        public static string exchangerate = "exchangerate";
        public static string discounttypeid = "discounttypeid";
        public static string percentage = "percentage";
        public static string pricelevelid = "pricelevelid";
        public static string pricingmethodcode = "pricingmethodcode";
        public static string productid = "productid";
        public static string productnumber = "productnumber";
        public static string quantitysellingcode = "quantitysellingcode";
        public static string roundingoptionamount = "roundingoptionamount";
        public static string roundingoptioncode = "roundingoptioncode";
        public static string roundingpolicycode = "roundingpolicycode";
        public static string transactioncurrencyid = "transactioncurrencyid";
        public static string uomid = "uomid";
        public static string uomscheduleid = "uomscheduleid";
    }
}

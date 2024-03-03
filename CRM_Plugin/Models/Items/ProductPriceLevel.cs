#region Namespaces
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
#endregion

namespace CRM_Plugin.Models.Items
{
    /// <summary>
    /// Product price level
    /// </summary>
    public class ProductPriceLevel
    {
        public ProductPriceLevel()
        {
        }

        public ProductPriceLevel(ProductPriceLevel _productPriceLevel)
        {
            #region properties
            this.amount = _productPriceLevel.amount;
            this.discounttypeid = _productPriceLevel.discounttypeid;
            this.createdon = _productPriceLevel.createdon;
            this.percentage = _productPriceLevel.percentage;
            this.pricelevelid = _productPriceLevel.pricelevelid;
            this.pricingmethodcode = _productPriceLevel.pricingmethodcode;
            this.productid = _productPriceLevel.productid;
            this.productpricelevelid = _productPriceLevel.productpricelevelid;
            this.quantitysellingcode = _productPriceLevel.quantitysellingcode;
            this.roundingoptionamount = _productPriceLevel.roundingoptionamount;
            this.roundingoptioncode = _productPriceLevel.roundingoptioncode;
            this.roundingpolicycode = _productPriceLevel.roundingpolicycode;
            this.transactioncurrencyid = _productPriceLevel.transactioncurrencyid;
            this.uomid = _productPriceLevel.uomid;
            this.uomscheduleid = _productPriceLevel.uomscheduleid;
            this.mooexternalid = _productPriceLevel.mooexternalid;
            this.moosourcesystem = _productPriceLevel.moosourcesystem;
            #endregion
        }

        #region Properties
        [Required]
        [Range(0, 100000000000000)]
        public decimal amount { get; set; }
        public string discounttypeid { get; set; }
        public DateTime createdon { get; set; }
        [Required]
        [Range(0, 100000000000000)]
        public string percentage { get; set; }
        [Required]
        public string pricelevelid { get; set; }
        [Required]
        [Range(1, 6)]
        public string pricingmethodcode { get; set; }
        [Required]
        public string productid { get; set; }
        public string productpricelevelid { get; set; }
        [Range(1, 3)]
        public int quantitysellingcode { get; set; }
        [Range(-100000000000000, 100000000000000)]
        [Required]
        public decimal roundingoptionamount { get; set; }
        [Range(1, 2)]
        public int roundingoptioncode { get; set; }
        [Range(1, 4)]
        public int roundingpolicycode { get; set; }
        public string transactioncurrencyid { get; set; }
        [Required]
        public string uomid { get; set; }
        public string uomscheduleid { get; set; }
        public string moosourcesystem { get; set; }
        [DataType(DataType.Text)]
        public string mooexternalid { get; set; }
        #endregion
        public static decimal GetActivePriceBySalesChannelProductID(Entity eSalesChannel, Guid productID, IOrganizationService service)
        {
            decimal priceAmount = 0;
            // move to model functions
            if (eSalesChannel.Contains("kti_salepricelist"))
            {
                var qeEntity = new QueryExpression();
                qeEntity.EntityName = "productpricelevel";
                qeEntity.ColumnSet = new ColumnSet(true);

                var entityFilter = new FilterExpression(LogicalOperator.And);
                entityFilter.AddCondition("pricelevelid", ConditionOperator.Equal, ((EntityReference)eSalesChannel["kti_salepricelist"]).Id);
                entityFilter.AddCondition("productid", ConditionOperator.Equal, productID);

                qeEntity.Criteria.AddFilter(entityFilter);

                var ecProductAmount = service.RetrieveMultiple(qeEntity);

                if(ecProductAmount.Entities.Count > 0)
                {
                    var eProductAmount = ecProductAmount.Entities.First();

                    if (eProductAmount.Contains("amount"))
                    {
                        priceAmount = ((Money)eProductAmount["amount"]).Value;
                    }
                }
            }

            // move to model functions
            if (eSalesChannel.Contains("kti_defaultpricelist") && priceAmount == 0)
            {
                var qeEntity = new QueryExpression();
                qeEntity.EntityName = "productpricelevel";
                qeEntity.ColumnSet = new ColumnSet(true);

                var entityFilter = new FilterExpression(LogicalOperator.And);
                entityFilter.AddCondition("pricelevelid", ConditionOperator.Equal, ((EntityReference)eSalesChannel["kti_defaultpricelist"]).Id);
                entityFilter.AddCondition("productid", ConditionOperator.Equal, productID);

                qeEntity.Criteria.AddFilter(entityFilter);

                var ecProductAmount = service.RetrieveMultiple(qeEntity);


                if (ecProductAmount.Entities.Count > 0)
                {
                    var eProductAmount = ecProductAmount.Entities.First();

                    if (eProductAmount.Contains("amount"))
                    {
                        priceAmount = ((Money)eProductAmount["amount"]).Value;
                    }
                }
            }

            return priceAmount;
        }
    }
}

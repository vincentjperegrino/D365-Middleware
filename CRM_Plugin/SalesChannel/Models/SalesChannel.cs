using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using System.ServiceModel;
using System.ComponentModel;

namespace CRM_Plugin.SalesChannel.Models
{
    public class SalesChannel
    {
        public string EntityName = "kti_saleschannel";
        public string PrimaryKey = "kti_saleschannelid";

        Entity eSalesChannel;
        IOrganizationService service;
        public SalesChannel()
        {
        }

        public SalesChannel(Entity entity, IOrganizationService organizationService)
        {
            eSalesChannel = entity;
            service = organizationService;
        }
        [DisplayName("kti_saleschannelid")]
        public string kti_saleschannelid { get; set; }
        [DisplayName("kti_account")]
        public string kti_account { get; set; }
        [DisplayName("kti_defaultpricelist")]
        public string kti_defaultpricelist { get; set; }
        [DisplayName("kti_salepricelist")]
        public string kti_salepricelist { get; set; }
        [DisplayName("kti_warehousecode")]
        public string kti_warehousecode { get; set; }

        public string GetChannelCategoryMappingByParentProduct(EntityReference parentProduct)
        {
            QueryExpression qeChannelCategory = new QueryExpression();
            qeChannelCategory.EntityName = "kti_channelcategorymapping";
            qeChannelCategory.ColumnSet = new ColumnSet("kti_channelcategory", "kti_sourcecategory", "kti_saleschannel");

            var channelCategoryFilter = new FilterExpression(LogicalOperator.And);

            channelCategoryFilter.AddCondition("kti_sourcecategory", ConditionOperator.Equal, parentProduct.Id);

            qeChannelCategory.Criteria.AddFilter(channelCategoryFilter);

            LinkEntity leSalesChannelProduct = new LinkEntity("kti_channelcategorymapping", "product", "kti_channelcategory", "productid", JoinOperator.Inner);
            leSalesChannelProduct.Columns = new ColumnSet("productnumber");
            leSalesChannelProduct.EntityAlias = "p";

            qeChannelCategory.LinkEntities.Add(leSalesChannelProduct);

            var categories = service.RetrieveMultiple(qeChannelCategory).Entities;

            var eChannelCategory = categories.First(entity => ((EntityReference)entity["kti_saleschannel"]).Id == eSalesChannel.Id);

            if (eChannelCategory.Id != Guid.Empty || eChannelCategory.Id != null)
            {
                if (eChannelCategory.Contains("p.productnumber"))
                {
                    var arrCategory = ((string)eChannelCategory.GetAttributeValue<AliasedValue>("p.productnumber").Value).Split('_');
                    //var arrCategory = ((string)((AliasedValue)eChannelCategory.Attributes["p.productnumber"]).Value).Split('_');

                    return arrCategory.Last();
                }
            }

            return "";
        }

        public string GetCompanyInstance()
        {
            if (eSalesChannel.Contains("kti_origincompanyid"))
            {
                return ((string)eSalesChannel["kti_origincompanyid"]);
            }

            return "";
        }

        public DateTime GetLastSyncDateTime()
        {
            if (eSalesChannel.Contains("kti_lastsyncdateandtime"))
            {
                return ((DateTime)eSalesChannel["kti_lastsyncdateandtime"]);
            }

            return DateTime.Parse("1/1/1753 12:00 AM");
        }

        public List<Entity> GetSalesChannelProductsByModifiedDate(DateTime dateTimeNow, DateTime lastSyncDateTime, int filterType)
        {
            List<Entity> listProduct = new List<Entity>();

            QueryExpression qeProduct = new QueryExpression();
            qeProduct.EntityName = "product";
            qeProduct.ColumnSet = new ColumnSet(true);

            var mainFilter = ModifiedOnExpression(dateTimeNow, lastSyncDateTime);

            if (filterType == 0)
                qeProduct.Criteria = mainFilter;

            LinkEntity leSalesChannelProduct = new LinkEntity("product", "kti_saleschannelproduct", "productid", "kti_product", JoinOperator.Inner);
            leSalesChannelProduct.Columns = new ColumnSet("kti_product", "kti_saleschannel", "createdon", "modifiedon");
            leSalesChannelProduct.EntityAlias = "scp";

            if (filterType == 1)
            {
                LinkEntity leProductPriceLevel = new LinkEntity("product", "productpricelevel", "productid", "productid", JoinOperator.Inner);
                leProductPriceLevel.Columns = new ColumnSet(true);
                leProductPriceLevel.EntityAlias = "ppl";

                var productPriceLevelFilter = new FilterExpression(LogicalOperator.Or);

                productPriceLevelFilter.AddCondition("pricelevelid", ConditionOperator.Equal, ((EntityReference)eSalesChannel["kti_defaultpricelist"]).Id);

                if (eSalesChannel.Contains("kti_salepricelist"))
                    productPriceLevelFilter.AddCondition("pricelevelid", ConditionOperator.Equal, ((EntityReference)eSalesChannel["kti_salepricelist"]).Id);

                FilterExpression productPriceLevelMainFilter = new FilterExpression(LogicalOperator.And);

                productPriceLevelMainFilter.AddFilter(mainFilter);

                productPriceLevelMainFilter.AddFilter(productPriceLevelFilter);

                leProductPriceLevel.LinkCriteria = productPriceLevelMainFilter;

                qeProduct.LinkEntities.Add(leProductPriceLevel);
            }

            qeProduct.LinkEntities.Add(leSalesChannelProduct);

            listProduct = service.RetrieveMultiple(qeProduct).Entities.ToList();

            if (filterType == 1)
            {
                listProduct = listProduct.GroupBy(p => p.Id)
                         .Select(p => p.First())
                         .ToList();
            }

            return listProduct.Where(entity => entity.Contains("scp.kti_saleschannel") &&  ((EntityReference)((AliasedValue)entity["scp.kti_saleschannel"]).Value).Id == eSalesChannel.Id).ToList();
        }

        public FilterExpression ModifiedOnExpression(DateTime dateTimeNow, DateTime lastSyncDateTime)
        {
            var productModifiedFilter = new FilterExpression(LogicalOperator.And);
            productModifiedFilter.AddCondition("modifiedon", ConditionOperator.GreaterEqual, lastSyncDateTime);
            productModifiedFilter.AddCondition("modifiedon", ConditionOperator.LessEqual, dateTimeNow);

            //var productCreatedFilter = new FilterExpression(LogicalOperator.And);
            //productCreatedFilter.AddCondition("createdon", ConditionOperator.GreaterEqual, lastSyncDateTime);
            //productCreatedFilter.AddCondition("createdon", ConditionOperator.LessEqual, dateTimeNow);

            FilterExpression mainFilter = new FilterExpression();
            mainFilter.AddFilter(productModifiedFilter);
            //   mainFilter.AddFilter(productCreatedFilter);

            return mainFilter;
        }
    }
}

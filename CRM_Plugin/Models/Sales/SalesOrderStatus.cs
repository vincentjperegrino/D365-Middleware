using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Models.Sales
{
    public class SalesOrderStatus
    {
        public SalesOrderStatus()
        {



        }

        public SalesOrderStatus(Entity order, IOrganizationService service, int companyid)
        {
            var SalesOrderDetail = new QueryExpression();
            SalesOrderDetail.EntityName = "salesorderdetail";
            SalesOrderDetail.ColumnSet = new ColumnSet("");
            var Filter = new FilterExpression(LogicalOperator.And);
            Filter.AddCondition("salesorderid", ConditionOperator.Equal, order.Id);
            SalesOrderDetail.Criteria.AddFilter(Filter);

            var SalesOrderDetailEntityCollection = service.RetrieveMultiple(SalesOrderDetail);
            var SourceItemIDs = SalesOrderDetailEntityCollection.Entities.Where(items => items.Contains("kti_sourceitemid")).Select(items => (string)items["kti_sourceitemid"]).ToList();

            this.companyid = companyid;
            this.domainType = "orderstatus";
            this.kti_sourcesalesorderid = order.Contains("kti_sourceid") ? (string)order["kti_sourceid"] : default;
            this.kti_sourcesalesorderitemids = SourceItemIDs;
            this.kti_orderstatus = order.Contains("kti_orderstatus") ? ((OptionSetValue)order["kti_orderstatus"]).Value : default; 
            this.kti_socialchannelorigin = order.Contains("kti_socialchannelorigin") ? ((OptionSetValue)order["kti_socialchannelorigin"]).Value : default; 

        }


        public int companyid { get; set; }
        public string domainType { get; set; }
        public string kti_sourcesalesorderid { get; set; }
        public List<string> kti_sourcesalesorderitemids { get; set; }
        public string orderstatus { get; set; }
        public int kti_orderstatus { get; set; }
        public string packageid { get; set; }
        public string tracking_number { get; set; }
        public string shipment_provider { get; set; }
        public string pdf_url { get; set; }
        public string cancelreason { get; set; }
        public string kti_shipmentid { get; set; }
        public string kti_salesorderid { get; set; }
        public string kti_shipmentitemid { get; set; }
        public string kti_salesorderitemid { get; set; }
        public int kti_socialchannelorigin { get; set; }
        public bool successful { get; set; }
    }
}

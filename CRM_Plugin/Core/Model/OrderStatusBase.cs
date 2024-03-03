using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Core.Model
{
    public class OrderStatusBase
    {
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
        public string kti_storecode { get; set; }
        public bool successful { get; set; }
    }
}

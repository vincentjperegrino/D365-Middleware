#region Namespaces
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace CRM_Plugin.Models.Sales
{
    public class OrderLineSerialNumber
    {
        public OrderLineSerialNumber(OrderLineSerialNumber _serialNumber)
        {
            #region properties
            this.kti_orderlineserialnumberid = _serialNumber.kti_orderlineserialnumberid;
            this.kti_orderline = _serialNumber.kti_orderline;
            this.kti_pickinglineitemnumber = _serialNumber.kti_pickinglineitemnumber;
            this.kti_product = _serialNumber.kti_product;
            this.kti_serialnumber = _serialNumber.kti_serialnumber;
            this.kti_sku = _serialNumber.kti_sku;
            this.kti_warrantystartdate = _serialNumber.kti_warrantystartdate;
            #endregion
        }

        public OrderLineSerialNumber()
        {
        }

        #region Properties
        public int kti_socialchannelorigin { get; set; }
        public Guid kti_orderlineserialnumberid { get; set; }
        public EntityReference kti_orderline { get; set; }
        public string kti_orderdetaillineitemnumber { get; set; }
        public string kti_pickinglineitemnumber { get; set; }
        public EntityReference kti_product { get; set; }
        public string kti_serialnumber { get; set; }
        public string kti_sku { get; set; }
        public DateTime kti_warrantystartdate { get; set; }
        public EntityReference kti_parentbundleproduct { get; set; }
        #endregion
    }
}

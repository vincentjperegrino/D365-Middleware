#region Namespaces
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace CRM_Plugin.Models.Items
{
    public class SerialNumber
    {

        public SerialNumber(SerialNumber _serialNumber)
        {
            #region properties
            this.kti_pickinglineitemnumber = _serialNumber.kti_pickinglineitemnumber;
            this.kti_orderline = _serialNumber.kti_orderline;
            this.kti_product = _serialNumber.kti_product;
            this.kti_serialnumber = _serialNumber.kti_serialnumber;
            this.kti_sku = _serialNumber.kti_sku;
            this.kti_warrantystartdate = _serialNumber.kti_warrantystartdate;
            #endregion
        }

        public SerialNumber()
        {
        }

        #region Properties
        public string kti_pickinglineitemnumber { get; set; }
        [Required]
        [JsonProperty(PropertyName = "kti_OrderLine@odata.bind")]
        public string kti_orderline { get; set; }
        [Required]
        [JsonProperty(PropertyName = "kti_Product@odata.bind")]
        public string kti_product { get; set; }
        [Required]
        public string kti_serialnumber { get; set; }
        public string kti_sku { get; set; }
        public string kti_warrantystartdate { get; set; }
        #endregion
    }
}

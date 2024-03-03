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
    /// Price level
    /// </summary>
    public class PriceLevel
    {
        public string EntityName = "pricelevel";
        public string PrimaryKey = "pricelevelid";

        public PriceLevel()
        {
        }

        public PriceLevel(Entity _entity)
        {
            #region Mapping
            string erroron = "";
            try
            {

                erroron = "pricelevelid";
                this.pricelevelid = !String.IsNullOrEmpty(_entity.Id.ToString()) ? _entity.Id.ToString() : throw new Exception("No GUID for the record");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + $" {erroron}");
            }
            #endregion
        }

        public PriceLevel(PriceLevel _priceLevel)
        {
            #region properties
            this.begindate = _priceLevel.begindate;
            this.description = _priceLevel.description;
            this.enddate = _priceLevel.enddate;
            this.freighttermscode = _priceLevel.freighttermscode;
            this.breakhoursbillable = _priceLevel.breakhoursbillable;
            this.copiedfrompriceLevel = _priceLevel.copiedfrompriceLevel;
            this.entity = _priceLevel.entity;
            this.module = _priceLevel.module;
            this.timeunit = _priceLevel.timeunit;
            this.pricelevelid = _priceLevel.pricelevelid;
            this.name = _priceLevel.name;
            this.createdon = _priceLevel.createdon;
            this.paymentmethodcode = _priceLevel.paymentmethodcode;
            this.shippingmethodcode = _priceLevel.shippingmethodcode;
            this.stateCode = _priceLevel.stateCode;
            this.statusCode = _priceLevel.statusCode;
            this.transactioncurrencyid = _priceLevel.transactioncurrencyid;
            this.mooexternalid = _priceLevel.mooexternalid;
            this.moosourcesystem = _priceLevel.moosourcesystem;
            #endregion
        }

        #region Properties
        [DataType(DataType.Date)]
        public DateTime begindate { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(2000)]
        public string description { get; set; }
        [DataType(DataType.Date)]
        public DateTime enddate { get; set; }
        [Range(1, 1)]
        public int freighttermscode { get; set; }
        public bool breakhoursbillable { get; set; }
        public bool copiedfrompriceLevel { get; set; }
        [Range(192350000, 192350003)]
        public int entity { get; set; }
        [Range(192350000, 192350002)]
        public int module { get; set; }
        public string timeunit { get; set; }
        public string pricelevelid { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string name { get; set; }
        public DateTime createdon { get; set; }
        [Range(1, 1)]
        public int paymentmethodcode { get; set; }
        [Range(1, 1)]
        public int shippingmethodcode { get; set; }
        [Range(0, 1)]
        public int stateCode { get; set; }
        [Range(100001, 100002)]
        public int statusCode { get; set; }
        public string transactioncurrencyid { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string moosourcesystem { get; set; }
        [DataType(DataType.Text)]
        public string mooexternalid { get; set; }
        public int kti_socialchannel { get; set; }
        #endregion
    }
}

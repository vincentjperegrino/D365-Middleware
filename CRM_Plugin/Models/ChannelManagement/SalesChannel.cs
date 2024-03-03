#region Namespaces
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
#endregion

namespace CRM_Plugin.Models.ChannelManagement
{
    /// <summary>
    /// Inventory
    /// </summary>
    public class SalesChannel
    {

        public SalesChannel() { }
        public SalesChannel(Entity _entity)
        {
            #region Mapping
            string erroron = "";

            try
            {
                erroron = "kti_saleschannelid";
                this.salesChannelID = !String.IsNullOrEmpty(_entity.Id.ToString()) ? _entity.Id.ToString() : throw new Exception("No GUID for the record");
                erroron = "kti_account";
                this.Account = !_entity.Contains("kti_account") ? ((EntityReference)_entity["kti_account"]).Id.ToString() : "";
                erroron = "kti_defaultpricelist";
                this.defaultPriceList = !_entity.Contains("kti_defaultpricelist") ? ((EntityReference)_entity["kti_defaultpricelist"]).Id.ToString() : throw new Exception("No default price list for the record");
                erroron = "kti_salepricelist";
                this.salePriceList = !_entity.Contains("kti_salepricelist") ? ((EntityReference)_entity["kti_salepricelist"]).Id.ToString() : "";
                erroron = "kti_warehousecode";
                this.warehouseCode = !_entity.Contains("kti_warehousecode") ? ((EntityReference)_entity["kti_warehousecode"]).Id.ToString() : "";
                erroron = "kti_branch";
                this.branch = !_entity.Contains("kti_branch") ? ((EntityReference)_entity["kti_branch"]).Id.ToString() : "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + $" {erroron}");
            }
            #endregion
        }
        public SalesChannel(SalesChannel salesChannel)
        {
            #region properties
            this.Account = salesChannel.Account;
            this.AppKey = salesChannel.AppKey;
            this.AppSecret = salesChannel.AppSecret;
            this.ChannelOrigin = salesChannel.ChannelOrigin;
            this.Country = salesChannel.Country;
            this.DefaultUrl = salesChannel.DefaultUrl;
            this.IsTest = salesChannel.IsTest;
            this.IsProduction = salesChannel.IsProduction;
            this.Name = salesChannel.Name;
            this.Password = salesChannel.Password;
            this.branch = salesChannel.branch;
            #endregion
        }

        #region Properties
        public EntityReference salesChannel { get; set; }
        [DataType(DataType.Text)]
        [StringLength(255)]
        [JsonProperty(PropertyName = "kti_Account@odata.bind")]
        public string Account { get; set; }
        [StringLength(100)]
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "kti_appkey")]
        public string AppKey { get; set; }
        [StringLength(100)]
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "kti_appsecret")]
        public string AppSecret { get; set; }
        [Range(100000000, 959080011)]
        [JsonProperty(PropertyName = "kti_channelorigin")]
        public int ChannelOrigin { get; set; }
        [DataType(DataType.Text)]
        [StringLength(255)]
        [JsonProperty(PropertyName = "kti_Country@odata.bind")]
        public string Country { get; set; }
        [StringLength(100)]
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "kti_defaulturl")]
        public string DefaultUrl { get; set; }
        [JsonProperty(PropertyName = "kti_istest")]
        public bool IsTest { get; set; }
        [JsonProperty(PropertyName = "kti_isproduction")]
        public bool IsProduction { get; set; }
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "kti_name")]
        public string Name { get; set; }
        [DataType(DataType.Text)]
        [JsonProperty(PropertyName = "kti_password")]
        public string Password { get; set; }
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string appKey { get; set; }
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string appSecret { get; set; }
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string password { get; set; }
        [StringLength(250)]
        public string appKeyFlag { get; set; }
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string appSecretFlag { get; set; }
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string passwordFlag { get; set; }
        [JsonIgnore]
        public string salesChannelID { get; set; }
        [DataType(DataType.Text)]
        [StringLength(255)]
        [JsonProperty(PropertyName = "kti_DefaultPriceList@odata.bind")]
        public string defaultPriceList { get; set; }
        [DataType(DataType.Text)]
        [StringLength(255)]
        [JsonProperty(PropertyName = "kti_SalePriceList@odata.bind")]
        public string salePriceList { get; set; }
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string warehouseCode { get; set; }
        public string branch { get; set; }
        #endregion
    }
}

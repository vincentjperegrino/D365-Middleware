#region Namespaces
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Xrm.Sdk.Query;
#endregion
using Microsoft.Crm.Sdk.Messages;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM_Plugin.Models.Items.CCPI
{
    /// <summary>
    /// Product
    /// </summary>
    public class Products : CRM_Plugin.Models.Items.Products
    {
        public Products()
        {
        }
        public Products(Entity _entity, Entity eSalesChannel, IOrganizationService service, CRM_Plugin.SalesChannel.Models.SalesChannel salesChannel, string domainType)
        {
            try
            {
                this._service = service;
                _salesChannel = salesChannel;
                originConfig = Moo.Config.GetOriginCredentials();

                #region properties
                this.productid = _entity.Id.ToString();
                this.currentcost = _entity.Contains("currentcost") ?
                    ((Money)_entity["currentcost"]).Value : 0;
                this.defaultuomid = _entity.Contains("defaultuomid") ?
                    (string)(service.Retrieve("uom", ((EntityReference)_entity["defaultuomid"]).Id, new ColumnSet("name")))["name"] : "";
                this.defaultuomscheduleid = _entity.Contains("defaultuomscheduleid") ?
                    (string)(service.Retrieve("uomschedule", ((EntityReference)_entity["defaultuomscheduleid"]).Id, new ColumnSet("name")))["name"] : "";
                this.description = _entity.Contains("description") ?
                    (string)_entity["description"] : "";
                this.iskit = _entity.Contains("iskit") ?
                    (bool)_entity["iskit"] : false;
                this.isreparented = _entity.Contains("isreparented") ?
                    (bool)_entity["isreparented"] : false;
                this.isstockitem = _entity.Contains("isstockitem") ?
                    (bool)_entity["isstockitem"] : false;

                this.name = _entity.Contains("name") ?
                    (string)_entity["name"] : "";


                this.price = _entity.Contains("price") ?
                    ((Money)_entity["price"]).Value : 0;
                this.pricelevelid = _entity.Contains("pricelevelid") ?
                    ((EntityReference)_entity["pricelevelid"]).Id.ToString() : "";
                this.productnumber = _entity.Contains("productnumber") ?
                    (string)_entity["productnumber"] : "";

                this.productstructure = _entity.Contains("productstructure") ? ((OptionSetValue)_entity["productstructure"]).Value : 0;

                this.quantityonhand = _entity.Contains("quantityonhand") ? (decimal)_entity["quantityonhand"] : 0;
                this.size = _entity.Contains("size") ? (string)_entity["size"] :
                    String.Format("{0},cm,{1},cm,{2},cm,{3},kg", (_entity.Contains("kti_height") ? ((decimal)_entity["kti_height"]).ToString("#.00") : "0")
                    , (_entity.Contains("kti_width") ? ((decimal)_entity["kti_width"]).ToString("#.00") : "0")
                    , (_entity.Contains("kti_length") ? ((decimal)_entity["kti_length"]).ToString("#.00") : "0")
                    , (_entity.Contains("kti_weight") ? ((decimal)_entity["kti_weight"]).ToString("#.00") : "0"));

                this.statecode = _entity.Contains("statecode") ? ((OptionSetValue)_entity["statecode"]).Value : 0;
                this.statuscode = _entity.Contains("statuscode") ? ((OptionSetValue)_entity["statuscode"]).Value : 0;

                this.suppliername = _entity.Contains("suppliername") ? (string)_entity["suppliername"] : "";
                this.transactioncurrencyid = _entity.Contains("transactioncurrencyid") ? (string)(service.Retrieve(((EntityReference)_entity["transactioncurrencyid"]).LogicalName, ((EntityReference)_entity["transactioncurrencyid"]).Id, new ColumnSet("isocurrencycode")))["isocurrencycode"] : "";
                this.validfromdate = _entity.Contains("validfromdate") ? (DateTime)_entity["validfromdate"] : new DateTime();
                this.validtodate = _entity.Contains("validtodate") ? (DateTime)_entity["validtodate"] : new DateTime();
                this.vendorid = _entity.Contains("vendorid") ? (string)_entity["vendorid"] : "";
                this.vendorname = _entity.Contains("vendorname") ? (string)_entity["vendorname"] : "";
                this.vendorpartnumber = _entity.Contains("vendorpartnumber") ? (string)_entity["vendorpartnumber"] : "";
                this.kti_sku = _entity.Contains("kti_sku") ? (string)_entity["kti_sku"] : "";

                this.price = CRM_Plugin.Models.Items.ProductPriceLevel.GetActivePriceBySalesChannelProductID(eSalesChannel, _entity.Id, service);

                var ecImages = this.GetImagesByProductID();

                this.producturl = ecImages.Count > 0 ? (string)(ecImages.Where(i => (bool)i["kti_primaryimage"] == true).First())["kti_producturl"] : "";

                this.parentproductid = _entity.Contains("parentproductid") ? _salesChannel.GetChannelCategoryMappingByParentProduct((EntityReference)_entity["parentproductid"]) : "";

                this.companyid = eSalesChannel.Contains("kti_origincompanyid") ? Convert.ToInt32(((string)eSalesChannel["kti_origincompanyid"])) : 0;
                this.kti_channelorigin = ((OptionSetValue)eSalesChannel["kti_channelorigin"]).Value;
                this.kti_storecode = (string)eSalesChannel["kti_saleschannelcode"];
                this.domainType = domainType;
                this.kti_sellercode = _entity.Contains("kti_sellerid") ? (string)eSalesChannel["kti_sellerid"] : ""; ; //store code
                #endregion properties

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}

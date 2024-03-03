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
using Microsoft.Crm.Sdk.Messages;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace CRM_Plugin.Models.Items
{
    /// <summary>
    /// Product
    /// </summary>
    public class Products
    {
        [JsonIgnore]
        public IOrganizationService _service;
        [JsonIgnore]
        public CRM_Plugin.SalesChannel.Models.SalesChannel _salesChannel;
        [JsonIgnore]
        public Dictionary<string, string> originConfig;

        public Products()
        {
        }
        public Products(Entity _entity, IOrganizationService service)
        {
            #region properties
            _service = service;
            originConfig = Moo.Config.GetOriginCredentials();

            this.currentcost = _entity.Contains("currentcost") ? ((Money)_entity["currentcost"]).Value : 0;
            this.defaultuomid = _entity.Contains("defaultuomid") ? (string)(service.Retrieve("uom", ((EntityReference)_entity["defaultuomid"]).Id, new ColumnSet("name")))["name"] : "";
            this.defaultuomscheduleid = _entity.Contains("defaultuomscheduleid") ? (string)(service.Retrieve("uomschedule", ((EntityReference)_entity["defaultuomscheduleid"]).Id, new ColumnSet("name")))["name"] : "";
            this.description = _entity.Contains("description") ? (string)_entity["description"] : "";
            this.iskit = _entity.Contains("iskit") ? (bool)_entity["iskit"] : false;
            this.isreparented = _entity.Contains("isreparented") ? (bool)_entity["isreparented"] : false;
            this.isstockitem = _entity.Contains("isstockitem") ? (bool)_entity["isstockitem"] : false;
            //this.converttocustomerasset = (bool)_entity["converttocustomerasset"];
            //this.defaultvendor = (string)_entity["defaultvendor"];
            //this.fieldserviceproducttype = (int)_entity["fieldserviceproducttype"];
            //this.purchasename = (string)_entity["purchasename"];
            //this.taxable = (bool)_entity["taxable"];
            //this.transactioncategory = (string)_entity["transactioncategory"];
            //this.upccode = (string)_entity["upccode"];
            this.name = _entity.Contains("name") ? (string)_entity["name"] : "";
            //this.parentproductid = (string)_entity["parentproductid"];


            this.price = _entity.Contains("price") ? ((Money)_entity["price"]).Value : 0;
            this.pricelevelid = _entity.Contains("pricelevelid") ? ((EntityReference)_entity["pricelevelid"]).Id.ToString() : "";
            this.productnumber = _entity.Contains("productnumber") ? (string)_entity["productnumber"] : "";

            var qeEntity = new QueryExpression();
            qeEntity.EntityName = "productpricelevel";
            qeEntity.ColumnSet = new ColumnSet(true);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition("pricelevelid", ConditionOperator.Equal, pricelevelid);
            // entityFilter.AddCondition("pricelevelid", ConditionOperator.Equal, "a8bae586-098b-ec11-93b0-000d3a852fac"); //SRP
            entityFilter.AddCondition("productnumber", ConditionOperator.Equal, productnumber);

            qeEntity.Criteria.AddFilter(entityFilter);

            var ecProductAmount = service.RetrieveMultiple(qeEntity);

            var eProductAmount = ecProductAmount.Entities.First();

            this.price = eProductAmount.Contains("amount") ? ((Money)eProductAmount["amount"]).Value : this.price;

            this.productstructure = _entity.Contains("productstructure") ? ((OptionSetValue)_entity["productstructure"]).Value : 0;
            //this.productyypecode = ((OptionSetValue)_entity["productyypecode"]).Value;
            //this.producturl = (string)_entity["producturl"];
            //this.quantitydecimal = (int)_entity["quantitydecimal"];
            this.quantityonhand = _entity.Contains("quantityonhand") ? (decimal)_entity["quantityonhand"] : 0;
            //this.size = _entity.Contains("size") ? (string)_entity["size"] :
            //    String.Format("{0},cm,{1},cm,{2},cm,{3},kg", (_entity.Contains("kti_height") ? ((decimal)_entity["kti_height"]).ToString("#.00") : "0")
            //    , (_entity.Contains("kti_width") ? ((decimal)_entity["kti_width"]).ToString("#.00") : "0")
            //    , (_entity.Contains("kti_length") ? ((decimal)_entity["kti_length"]).ToString("#.00") : "0")
            //    , (_entity.Contains("kti_weight") ? ((decimal)_entity["kti_weight"]).ToString("#.00") : "0"));

            this.size = _entity.Contains("size") ?
              string.Format("{0},cm,{1},cm,{2},cm,{3},kg", (_entity.Contains("kti_height") ? ((decimal)_entity["kti_height"]).ToString("#.00") : "0")
              , (_entity.Contains("kti_width") ? ((decimal)_entity["kti_width"]).ToString("#.00") : "0")
              , (_entity.Contains("kti_length") ? ((decimal)_entity["kti_length"]).ToString("#.00") : "0")
              , (_entity.Contains("kti_weight") ? ((decimal)_entity["kti_weight"]).ToString("#.00") : "0")) : "0,cm,0,cm,0,cm,0,kg";

            //this.standardcost = ((Money)_entity["standardcost"]).Value;
            this.statecode = _entity.Contains("statecode") ? ((OptionSetValue)_entity["statecode"]).Value : 0;
            this.statuscode = _entity.Contains("statuscode") ? ((OptionSetValue)_entity["statuscode"]).Value : 0;
            //this.stockvolume = (decimal)_entity["stockvolume"];
            //this.stockweight = (decimal)_entity["stockweight"];
            this.suppliername = _entity.Contains("suppliername") ? (string)_entity["suppliername"] : "";
            this.transactioncurrencyid = _entity.Contains("transactioncurrencyid") ? (string)(service.Retrieve(((EntityReference)_entity["transactioncurrencyid"]).LogicalName, ((EntityReference)_entity["transactioncurrencyid"]).Id, new ColumnSet("isocurrencycode")))["isocurrencycode"] : "";
            this.validfromdate = _entity.Contains("validfromdate") ? (DateTime)_entity["validfromdate"] : new DateTime();
            this.validtodate = _entity.Contains("validtodate") ? (DateTime)_entity["validtodate"] : new DateTime();
            this.vendorid = _entity.Contains("vendorid") ? (string)_entity["vendorid"] : "";
            this.vendorname = _entity.Contains("vendorname") ? (string)_entity["vendorname"] : "";
            this.vendorpartnumber = _entity.Contains("vendorpartnumber") ? (string)_entity["vendorpartnumber"] : "";
            this.kti_sku = _entity.Contains("kti_sku") ? (string)_entity["kti_sku"] : "";
            this.producturl = _entity.Contains("kti_productimage") ? (string)_entity["kti_productimage"] : "";


            this.ncci_productcategory = _entity.Contains("ncci_productcategory") ? ((OptionSetValue)_entity["ncci_productcategory"]).Value : 0;
            //this.prodtype = (string)_entity["prodtype"];
            //this.parentitem = (string)_entity["parentitem"];
            //this.possku = (string)_entity["possku"];
            //this.rp = (string)_entity["rp"];
            //this.positemname = (string)_entity["positemname"];
            //this.category = (string)_entity["category"];
            //this.intensity = (string)_entity["intensity"];
            //this.ingallergn = (string)_entity["ingallergn"];
            //this.recycling = (string)_entity["recycling"];
            //this.recommend1 = (string)_entity["recommend1"];
            //this.recommend2 = (string)_entity["recommend2"];
            //this.recommend3 = (string)_entity["recommend3"];
            //this.recommend4 = (string)_entity["recommend4"];
            //this.matchgroup = (string)_entity["matchgroup"];
            //this.shortdesc = (string)_entity["shortdesc"];
            //this.specification = (string)_entity["specification"];
            //this.acccategory = (string)_entity["acccategory"];
            //this.collection = (string)_entity["collection"];
            //this.color = (string)_entity["color"];
            //this.property = (string)_entity["property"];
            //this.mooexternalid = (string)_entity["mooexternalid"];
            //this.moosourcesystem = (string)_entity["moosourcesystem"];

            #endregion properties
        }
        public Products(Entity _entity, IOrganizationService service, CRM_Plugin.SalesChannel.Models.SalesChannel _salesChannel, Entity salesChannel, string _domainType, int _companyId = 0)
        {
            #region properties
            _service = service;
            originConfig = Moo.Config.GetOriginCredentials();

            this.productid = _entity.Id.ToString();
            this.currentcost = _entity.Contains("currentcost") ? ((Money)_entity["currentcost"]).Value : 0;
            this.defaultuomid = _entity.Contains("defaultuomid") ? (string)(service.Retrieve("uom", ((EntityReference)_entity["defaultuomid"]).Id, new ColumnSet("name")))["name"] : "";
            this.defaultuomscheduleid = _entity.Contains("defaultuomscheduleid") ? (string)(service.Retrieve("uomschedule", ((EntityReference)_entity["defaultuomscheduleid"]).Id, new ColumnSet("name")))["name"] : "";
            this.description = _entity.Contains("description") ? (string)_entity["description"] : "";
            this.iskit = _entity.Contains("iskit") ? (bool)_entity["iskit"] : false;
            this.isreparented = _entity.Contains("isreparented") ? (bool)_entity["isreparented"] : false;
            this.isstockitem = _entity.Contains("isstockitem") ? (bool)_entity["isstockitem"] : false;
            //this.converttocustomerasset = (bool)_entity["converttocustomerasset"];
            //this.defaultvendor = (string)_entity["defaultvendor"];
            //this.fieldserviceproducttype = (int)_entity["fieldserviceproducttype"];
            //this.purchasename = (string)_entity["purchasename"];
            //this.taxable = (bool)_entity["taxable"];
            //this.transactioncategory = (string)_entity["transactioncategory"];
            //this.upccode = (string)_entity["upccode"];
            this.name = _entity.Contains("name") ? (string)_entity["name"] : "";
            //this.parentproductid = (string)_entity["parentproductid"];


            this.price = _entity.Contains("price") ? ((Money)_entity["price"]).Value : 0;
            this.pricelevelid = _entity.Contains("pricelevelid") ? ((EntityReference)_entity["pricelevelid"]).Id.ToString() : "";
            this.productnumber = _entity.Contains("productnumber") ? (string)_entity["productnumber"] : "";

            var qeEntity = new QueryExpression();
            qeEntity.EntityName = "productpricelevel";
            qeEntity.ColumnSet = new ColumnSet(true);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition("pricelevelid", ConditionOperator.Equal, pricelevelid);
            // entityFilter.AddCondition("pricelevelid", ConditionOperator.Equal, "a8bae586-098b-ec11-93b0-000d3a852fac"); //SRP
            entityFilter.AddCondition("productnumber", ConditionOperator.Equal, productnumber);

            qeEntity.Criteria.AddFilter(entityFilter);

            var ecProductAmount = service.RetrieveMultiple(qeEntity);

            var eProductAmount = ecProductAmount.Entities.First();

            this.price = eProductAmount.Contains("amount") ? ((Money)eProductAmount["amount"]).Value : this.price;

            this.productstructure = _entity.Contains("productstructure") ? ((OptionSetValue)_entity["productstructure"]).Value : 0;
            //this.productyypecode = ((OptionSetValue)_entity["productyypecode"]).Value;
            //this.producturl = (string)_entity["producturl"];
            //this.quantitydecimal = (int)_entity["quantitydecimal"];
            this.quantityonhand = _entity.Contains("quantityonhand") ? (decimal)_entity["quantityonhand"] : 0;
            this.size = string.Format("{0},cm,{1},cm,{2},cm,{3},kg", (_entity.Contains("kti_height") ? ((decimal)_entity["kti_height"]).ToString("#.00") : "0")
                , (_entity.Contains("kti_width") ? ((decimal)_entity["kti_width"]).ToString("#.00") : "0")
                , (_entity.Contains("kti_length") ? ((decimal)_entity["kti_length"]).ToString("#.00") : "0")
                , (_entity.Contains("kti_weight") ? ((decimal)_entity["kti_weight"]).ToString("#.00") : "0"));
            //this.standardcost = ((Money)_entity["standardcost"]).Value;
            this.statecode = _entity.Contains("statecode") ? ((OptionSetValue)_entity["statecode"]).Value : 0;
            this.statuscode = _entity.Contains("statuscode") ? ((OptionSetValue)_entity["statuscode"]).Value : 0;
            //this.stockvolume = (decimal)_entity["stockvolume"];
            //this.stockweight = (decimal)_entity["stockweight"];
            this.suppliername = _entity.Contains("suppliername") ? (string)_entity["suppliername"] : "";
            this.transactioncurrencyid = _entity.Contains("transactioncurrencyid") ? (string)(service.Retrieve(((EntityReference)_entity["transactioncurrencyid"]).LogicalName, ((EntityReference)_entity["transactioncurrencyid"]).Id, new ColumnSet("isocurrencycode")))["isocurrencycode"] : "";
            this.validfromdate = _entity.Contains("validfromdate") ? (DateTime)_entity["validfromdate"] : new DateTime();
            this.validtodate = _entity.Contains("validtodate") ? (DateTime)_entity["validtodate"] : new DateTime();
            this.vendorid = _entity.Contains("vendorid") ? (string)_entity["vendorid"] : "";
            this.vendorname = _entity.Contains("vendorname") ? (string)_entity["vendorname"] : "";
            this.vendorpartnumber = _entity.Contains("vendorpartnumber") ? (string)_entity["vendorpartnumber"] : "";
            this.kti_sku = _entity.Contains("kti_sku") ? (string)_entity["kti_sku"] : "";
            //this.producturl = _entity.Contains("kti_productimage") ? (string)_entity["kti_productimage"] : "";

            this.ncci_productcategory = _entity.Contains("ncci_productcategory") ? ((OptionSetValue)_entity["ncci_productcategory"]).Value : 0;
            //this.prodtype = (string)_entity["prodtype"];
            //this.parentitem = (string)_entity["parentitem"];
            //this.possku = (string)_entity["possku"];
            //this.rp = (string)_entity["rp"];
            //this.positemname = (string)_entity["positemname"];
            //this.category = (string)_entity["category"];
            //this.intensity = (string)_entity["intensity"];
            //this.ingallergn = (string)_entity["ingallergn"];
            //this.recycling = (string)_entity["recycling"];
            //this.recommend1 = (string)_entity["recommend1"];
            //this.recommend2 = (string)_entity["recommend2"];
            //this.recommend3 = (string)_entity["recommend3"];
            //this.recommend4 = (string)_entity["recommend4"];
            //this.matchgroup = (string)_entity["matchgroup"];
            //this.shortdesc = (string)_entity["shortdesc"];
            //this.specification = (string)_entity["specification"];
            //this.acccategory = (string)_entity["acccategory"];
            //this.collection = (string)_entity["collection"];
            //this.color = (string)_entity["color"];
            //this.property = (string)_entity["property"];
            //this.mooexternalid = (string)_entity["mooexternalid"];
            //this.moosourcesystem = (string)_entity["moosourcesystem"];

            this.price = CRM_Plugin.Models.Items.ProductPriceLevel.GetActivePriceBySalesChannelProductID(salesChannel, _entity.Id, service);

            var ecImages = this.GetImagesByProductID();

            if (ecImages != null && ecImages.Any())
            {
                this.producturl = (string)(ecImages.Where(i => (bool)i["kti_primaryimage"] == true).First())["kti_producturl"];

                this.images = ecImages.OrderByDescending(image => (bool)image["kti_primaryimage"]).Select(image => new ProductImage()
                {
                    isprimary = (bool)image["kti_primaryimage"],
                    producturl = (string)image["kti_producturl"]
                }).ToList();

            }

            this.parentproductid = _entity.Contains("parentproductid") ? _salesChannel.GetChannelCategoryMappingByParentProduct((EntityReference)_entity["parentproductid"]) : "";

            this.companyid = salesChannel.Contains("kti_origincompanyid") ? Convert.ToInt32(((string)salesChannel["kti_origincompanyid"])) : 0;
            this.kti_channelorigin = ((OptionSetValue)salesChannel["kti_channelorigin"]).Value;
            this.kti_storecode = (string)salesChannel["kti_saleschannelcode"];
            this.domainType = _domainType;
            this.kti_sellercode = _entity.Contains("kti_sellerid") ? (string)salesChannel["kti_sellerid"] : ""; ; //store code

            #endregion properties
        }

        #region Properties
        [Range(0, 1000000000000)]
        public decimal currentcost { get; set; }
        [Required]
        [JsonProperty(PropertyName = "defaultuomid@odata.bind")]
        public String defaultuomid { get; set; }
        [Required]
        [JsonProperty(PropertyName = "defaultuomscheduleid@odata.bind")]
        public String defaultuomscheduleid { get; set; }
        [StringLength(2000)]
        public String description { get; set; }
        [Range(-2147483647, 2147483647)]
        public int dmtimportstate { get; set; }
        public String entityimage { get; set; }
        [Range(-2147483647, 2147483647)]
        public int importsequencenumber { get; set; }
        public bool iskit { get; set; }
        public bool isreparented { get; set; }
        public bool isstockitem { get; set; }
        public bool converttocustomerasset { get; set; }
        public String defaultvendor { get; set; }
        [Range(690970000, 690970002)]
        [JsonProperty(PropertyName = "msdyn_fieldserviceproducttype")]
        public int fieldserviceproducttype { get; set; }
        [StringLength(100)]
        public String purchasename { get; set; }
        public bool taxable { get; set; }
        public String transactioncategory { get; set; }
        [StringLength(50)]
        public String upccode { get; set; }
        [Required]
        [StringLength(100)]
        public String name { get; set; }
        public DateTime overriddencreatedon { get; set; }
        [JsonProperty(PropertyName = "parentproductid@odata.bind")]
        public String parentproductid { get; set; }
        [Range(0, 1000000000000)]
        public decimal price { get; set; }
        public String pricelevelid { get; set; }
        public String processid { get; set; }
        public String productid { get; set; }
        [Required]
        [StringLength(50)]
        public String productnumber { get; set; }
        [Range(1, 3)]
        public int productstructure { get; set; }
        [Range(1, 4)]
        public int productyypecode { get; set; }
        [StringLength(255)]
        public String producturl { get; set; }
        [Range(0, 5)]
        public int quantitydecimal { get; set; }
        [Range(0, 1000000000)]
        public decimal quantityonhand { get; set; }
        [StringLength(200)]
        public String size { get; set; }
        public String stageid { get; set; }
        [Range(0, 1000000000000)]
        public decimal standardcost { get; set; }
        [Range(0, 3)]
        public int statecode { get; set; }
        [Range(0, 3)]
        public int statuscode { get; set; }
        [Range(0, 1000000000)]
        public decimal stockvolume { get; set; }
        [Range(0, 1000000000)]
        public decimal stockweight { get; set; }
        public String subjectid { get; set; }
        [StringLength(100)]
        public String suppliername { get; set; }
        public String transactioncurrencyid { get; set; }
        public DateTime validfromdate { get; set; }
        public DateTime validtodate { get; set; }
        [StringLength(100)]
        public String vendorid { get; set; }
        [StringLength(100)]
        public String vendorname { get; set; }
        [StringLength(100)]
        public String vendorpartnumber { get; set; }
        public string prodtype { get; set; }
        public string parentitem { get; set; }
        public string possku { get; set; }
        public string rp { get; set; }
        public string positemname { get; set; }
        public string category { get; set; }
        public string intensity { get; set; }
        public string ingallergn { get; set; }
        public string recycling { get; set; }
        public string recommend1 { get; set; }
        public string recommend2 { get; set; }
        public string recommend3 { get; set; }
        public string recommend4 { get; set; }
        public string matchgroup { get; set; }
        public string shortdesc { get; set; }
        public string specification { get; set; }
        public string acccategory { get; set; }
        public string collection { get; set; }
        public string color { get; set; }
        public string property { get; set; }
        public string moosourcesystem { get; set; }
        public string mooexternalid { get; set; }
        public string kti_sku { get; set; }
        public int ncci_productcategory { get; set; }
        public string domainType { get; set; }
        public int kti_channelorigin { get; set; }
        public string kti_storecode { get; set; }
        public string kti_sellercode { get; set; }
        public int companyid { get; set; }
        public List<ProductImage> images { get; set; }
        #endregion

        public List<Entity> GetImagesByProductID()
        {
            QueryExpression qeProductImage = new QueryExpression();
            qeProductImage.EntityName = "kti_productimage";
            qeProductImage.ColumnSet = new ColumnSet("kti_product", "kti_producturl");
            qeProductImage.AddOrder("kti_primaryimage", OrderType.Ascending);

            var imageFilter = new FilterExpression(LogicalOperator.And);

            qeProductImage.Criteria.AddCondition(new ConditionExpression("kti_product", ConditionOperator.Equal, Guid.Parse(this.productid)));

            //kti_primaryimage, kti_producturl, kti_product, kti_productimage
            var ecImages = _service.RetrieveMultiple(qeProductImage);

            if (ecImages.Entities.Count > 0)
            {
                return ecImages.Entities.ToList();
            }

            return null;
        }

        public async Task<string> Replicate()
        {
            string accessToken = Authenticate.AccessToken.Generate(this.companyid).GetAwaiter().GetResult();

            using (HttpClient httpClient = new HttpClient())
            {
                var settings = new JsonSerializerSettings();

                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.DefaultValueHandling = DefaultValueHandling.Ignore;

                httpClient.BaseAddress = new Uri(Moo.Config.baseurl);
                httpClient.Timeout = new TimeSpan(0, 2, 0);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                httpClient.DefaultRequestHeaders.Add("OC-Api-App-Key", originConfig[$"{this.companyid}_occapikey"]);
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", originConfig[$"{this.companyid}_subkey"]);
                httpClient.DefaultRequestHeaders.Add("Instance-Type", "CRM");

                //Add this line for TLS complaience
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var jsonObject = JsonConvert.SerializeObject(this, Formatting.Indented, settings);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                var result = await httpClient.PostAsync(Moo.Config.replicateproduct_path, content);

                return await result.Content.ReadAsStringAsync();
            }
        }
    }
}

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
#endregion

namespace CRM_Plugin.SalesChannel.Models
{
    public class Products : CRM_Plugin.Models.Items.Products
    {
        IOrganizationService _service;
        CRM_Plugin.SalesChannel.Domain.SalesChannel _salesChannel;

        public Products(Entity _entity, IOrganizationService service, CRM_Plugin.SalesChannel.Domain.SalesChannel salesChannel)
        {
            try
            {
                _service = service;
                _salesChannel = salesChannel;

                if (!validate(_entity))
                    return;

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
                //this.converttocustomerasset = (bool)_entity["converttocustomerasset"];
                //this.defaultvendor = (string)_entity["defaultvendor"];
                //this.fieldserviceproducttype = (int)_entity["fieldserviceproducttype"];
                //this.purchasename = (string)_entity["purchasename"];
                //this.taxable = (bool)_entity["taxable"];
                //this.transactioncategory = (string)_entity["transactioncategory"];
                //this.upccode = (string)_entity["upccode"];
                this.name = _entity.Contains("name") ?
                    (string)_entity["name"] : "";
                //this.parentproductid = (string)_entity["parentproductid"];


                this.price = _entity.Contains("price") ?
                    ((Money)_entity["price"]).Value : 0;
                this.pricelevelid = _entity.Contains("pricelevelid") ?
                    ((EntityReference)_entity["pricelevelid"]).Id.ToString() : "";
                this.productnumber = _entity.Contains("productnumber") ?
                    (string)_entity["productnumber"] : "";

                this.price = 0;

                this.productstructure = _entity.Contains("productstructure") ? ((OptionSetValue)_entity["productstructure"]).Value : 0;

                //this.productyypecode = ((OptionSetValue)_entity["productyypecode"]).Value;
                //this.producturl = (string)_entity["producturl"];
                //this.quantitydecimal = (int)_entity["quantitydecimal"];
                this.quantityonhand = _entity.Contains("quantityonhand") ? (decimal)_entity["quantityonhand"] : 0;
                this.size = _entity.Contains("size") ? (string)_entity["size"] :
                    String.Format("{0},cm,{1},cm,{2},cm,{3},kg", (_entity.Contains("kti_height") ? ((decimal)_entity["kti_height"]).ToString("#.00") : "0")
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

                //this.ProductPriceList = get_productpricelevelbyproductid().Entities.Select(i => new SalesChannel.Models.ProductPrices(i)).ToList();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region properties
        //public List<SalesChannel.Models.ProductPrices> ProductPriceList{ get; set; }
        #endregion
        private List<Entity> get_pricelistwithperiod(List<Entity> listPriceList)
        {

            var listPriceListContainDate = listPriceList.Where(priceList => priceList.Contains("begindate")
            && priceList.Contains("enddate")).ToList();

            if (listPriceListContainDate.Count > 0)
            {
                return listPriceListContainDate;
            }

            return null;
        }

        private Entity get_pricelistbydatechannelname(List<Entity> listPriceList, string name, OptionSetValue optionSetValue)
        {
            var listPriceListCompared = listPriceList.Where(priceList =>
            (DateTime.UtcNow.Date >= ((DateTime)priceList["begindate"]).AddHours(8).Date
            && DateTime.UtcNow.Date <= ((DateTime)priceList["enddate"]).AddHours(8).Date)
            && ((string)priceList["name"]).ToLower().Contains(name)
            && ((OptionSetValue)priceList["kti_socialchannel"]).Value == optionSetValue.Value)
                .OrderByDescending(priceList => (DateTime)priceList["modifiedon"]).ToList();


            if (listPriceListCompared.Count > 0)
            {
                var ePriceList = listPriceListCompared.FirstOrDefault();

                return ePriceList;
            }

            return null;
        }

        private EntityCollection get_productpricelevelwithsaleschannelbyproductid()
        {
            List<Entity> listPriceList = new List<Entity>();

            QueryExpression qeProductPriceLevel = new QueryExpression();
            qeProductPriceLevel.EntityName = "prodductpricelevel";
            qeProductPriceLevel.ColumnSet = new ColumnSet("pricelevelid", "amount");

            var productPriceListFilter = new FilterExpression(LogicalOperator.And);
            productPriceListFilter.AddCondition("productid", ConditionOperator.Equal, this.productid);

            qeProductPriceLevel.Criteria.AddFilter(productPriceListFilter);

            LinkEntity lnkSalesChannel = new LinkEntity("kti_saleschannelproduct", "kti_saleschannel", "kti_saleschannel", "kti_saleschannelid", JoinOperator.LeftOuter);

            lnkSalesChannel.Columns = new ColumnSet(true);

            lnkSalesChannel.EntityAlias = "sc";

            qeProductPriceLevel.LinkEntities.Add(lnkSalesChannel);

            return _service.RetrieveMultiple(qeProductPriceLevel);
        }

        private EntityCollection get_productpricelevelbyproductid()
        {
            List<Entity> listPriceList = new List<Entity>();

            QueryExpression qeProductPriceLevel = new QueryExpression();
            qeProductPriceLevel.EntityName = "prodductpricelevel";
            qeProductPriceLevel.ColumnSet = new ColumnSet("pricelevelid", "amount");

            var productPriceListFilter = new FilterExpression(LogicalOperator.And);
            productPriceListFilter.AddCondition("productid", ConditionOperator.Equal, this.productid);

            qeProductPriceLevel.Criteria.AddFilter(productPriceListFilter);

            return _service.RetrieveMultiple(qeProductPriceLevel);
        }

        private List<Entity> get_pricelistlistbyproductpricelevellist_productid(EntityCollection ecProductPriceLevel)
        {
            var listPriceList = new List<Entity>();

            if (ecProductPriceLevel.Entities.Count > 0)
            {
                foreach (var eProductPriceLevel in ecProductPriceLevel.Entities)
                {
                    if (eProductPriceLevel.Contains("pricelevelid"))
                    {
                        listPriceList.Add(_service.Retrieve(((EntityReference)eProductPriceLevel["pricelevelid"]).LogicalName,
                            ((EntityReference)eProductPriceLevel["pricelevelid"]).Id,
                            new ColumnSet(true)));
                    }
                }
            }

            return listPriceList;
        }

        private Entity get_activepricelist(OptionSetValue optionSetValue)
        {
            EntityCollection ecSalesChannel = _salesChannel.GetSalesChannelByChannel(optionSetValue);

            EntityCollection ecProductPriceLevel = get_productpricelevelbyproductid();

            List<Entity> listPriceList = get_pricelistlistbyproductpricelevellist_productid(ecProductPriceLevel);

            //Sales price list

            //Regular price list

            if (listPriceList.Count > 0)
            {
                //Get if price list contains period dates
                var listPriceListContainsDate = get_pricelistwithperiod(listPriceList);

                if (listPriceListContainsDate.Count > 0)
                {
                    //Get if price list filtered period dates, name and channel sorted by modified on
                    //var ePriceList = get_pricelistbydatechannelname(listPriceListContainsDate, Moo.Config.sale_pricelist, optionSetValue);

                    //if (ePriceList != null)
                    //{
                    //    if (_fromDate)
                    //    {
                    //        return ((DateTime)ePriceList[Helper.GetPropertyDisplayName<WebsiteIntegration.Models.D365.PriceLevel>(i => i.begindate)]).AddHours(8);
                    //    }
                    //    else
                    //    {
                    //        return ((DateTime)ePriceList[Helper.GetPropertyDisplayName<WebsiteIntegration.Models.D365.PriceLevel>(i => i.enddate)]).AddHours(8);
                    //    }
                    //}
                }
            }

            return null;
        }

        private decimal get_price(Entity _entity)
        {
            try
            {
                var qeEntity = new QueryExpression();
                qeEntity.EntityName = "productpricelevel";
                qeEntity.ColumnSet = new ColumnSet(true);

                var entityFilter = new FilterExpression(LogicalOperator.And);
                //entityFilter.AddCondition("pricelevelid", ConditionOperator.Equal, pricelevelid);
                entityFilter.AddCondition("pricelevelid", ConditionOperator.Equal, "a8bae586-098b-ec11-93b0-000d3a852fac"); //SRP
                entityFilter.AddCondition("productnumber", ConditionOperator.Equal, productnumber);

                qeEntity.Criteria.AddFilter(entityFilter);

                var ecProductAmount = _service.RetrieveMultiple(qeEntity);

                var eProductAmount = ecProductAmount.Entities.First();

                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool validate(Entity _entity)
        {
            if (_entity.Contains("productstructure"))
            {
                if (((OptionSetValue)_entity["productstructure"]).Value == 1 || ((OptionSetValue)_entity["productstructure"]).Value == 3)
                {
                    return true;
                }
            }
            else
            {
                throw new Exception("Product structure not existing");
            }

            return false;
        }
    }
}

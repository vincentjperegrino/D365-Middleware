using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;

namespace CRM_Plugin.CustomAPI.Model.DTO
{
    public class Product : CRM_Plugin.Core.Model.ProductBase
    {
        public Product(ParameterCollection collection)
        {

            string ErrorEntity = "";

            try
            {

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.productnumber;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.productnumber))
                    this.productnumber = (string)collection[CustomAPI.Helper.EntityHelper.Product.productnumber];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.currentcost;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.currentcost))
                    this.currentcost = (decimal)collection[CustomAPI.Helper.EntityHelper.Product.currentcost];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.defaultuomid;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.defaultuomid))
                    this.defaultuomid = (string)collection[CustomAPI.Helper.EntityHelper.Product.defaultuomid];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.defaultuomscheduleid;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.defaultuomscheduleid))
                    this.defaultuomscheduleid = (string)collection[CustomAPI.Helper.EntityHelper.Product.defaultuomscheduleid];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.description;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.description))
                    this.description = (string)collection[CustomAPI.Helper.EntityHelper.Product.description];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.iskit;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.iskit))
                    this.iskit = (bool)collection[CustomAPI.Helper.EntityHelper.Product.iskit];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.isreparented;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.isreparented))
                    this.isreparented = (bool)collection[CustomAPI.Helper.EntityHelper.Product.isreparented];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.converttocustomerasset;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.converttocustomerasset))
                    this.converttocustomerasset = (bool)collection[CustomAPI.Helper.EntityHelper.Product.converttocustomerasset];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.defaultvendor;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.defaultvendor))
                    this.defaultvendor = (string)collection[CustomAPI.Helper.EntityHelper.Product.defaultvendor];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.fieldserviceproducttype;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.fieldserviceproducttype))
                    this.fieldserviceproducttype = (int)collection[CustomAPI.Helper.EntityHelper.Product.fieldserviceproducttype];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.purchasename;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.purchasename))
                    this.purchasename = (string)collection[CustomAPI.Helper.EntityHelper.Product.purchasename];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.taxable;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.taxable))
                    this.taxable = (bool)collection[CustomAPI.Helper.EntityHelper.Product.taxable];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.upccode;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.upccode))
                    this.upccode = (string)collection[CustomAPI.Helper.EntityHelper.Product.upccode];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.name;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.name))
                    this.name = (string)collection[CustomAPI.Helper.EntityHelper.Product.name];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.overriddencreatedon;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.overriddencreatedon))
                    this.overriddencreatedon = (DateTime)collection[CustomAPI.Helper.EntityHelper.Product.overriddencreatedon];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.parentproductid;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.parentproductid))
                    this.parentproductid = (string)collection[CustomAPI.Helper.EntityHelper.Product.parentproductid];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.price;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.price))
                    this.price = (decimal)collection[CustomAPI.Helper.EntityHelper.Product.price];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.pricelevelid;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.pricelevelid))
                    this.pricelevelid = (string)collection[CustomAPI.Helper.EntityHelper.Product.pricelevelid];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.processid;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.processid))
                    this.processid = (string)collection[CustomAPI.Helper.EntityHelper.Product.processid];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.productnumber;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.productnumber))
                    this.productnumber = (string)collection[CustomAPI.Helper.EntityHelper.Product.productnumber];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.productstructure;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.productstructure))
                    this.productstructure = (int)collection[CustomAPI.Helper.EntityHelper.Product.productstructure];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.productyypecode;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.productyypecode))
                    this.productyypecode = (int)collection[CustomAPI.Helper.EntityHelper.Product.productyypecode];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.producturl;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.producturl))
                    this.producturl = (string)collection[CustomAPI.Helper.EntityHelper.Product.producturl];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.quantitydecimal;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.quantitydecimal))
                    this.quantitydecimal = (int)collection[CustomAPI.Helper.EntityHelper.Product.quantitydecimal];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.quantityonhand;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.quantityonhand))
                    this.quantityonhand = (decimal)collection[CustomAPI.Helper.EntityHelper.Product.quantityonhand];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.size;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.size))
                    this.size = (string)collection[CustomAPI.Helper.EntityHelper.Product.size];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.stageid;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.stageid))
                    this.stageid = (string)collection[CustomAPI.Helper.EntityHelper.Product.stageid];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.standardcost;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.standardcost))
                    this.standardcost = (decimal)collection[CustomAPI.Helper.EntityHelper.Product.standardcost];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.statecode;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.statecode))
                    this.statecode = (int)collection[CustomAPI.Helper.EntityHelper.Product.statecode];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.statuscode;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.statuscode))
                    this.statuscode = (int)collection[CustomAPI.Helper.EntityHelper.Product.statuscode];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.stockvolume;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.stockvolume))
                    this.stockvolume = (decimal)collection[CustomAPI.Helper.EntityHelper.Product.stockvolume];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.stockweight;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.stockweight))
                    this.stockweight = (decimal)collection[CustomAPI.Helper.EntityHelper.Product.stockweight];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.subjectid;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.subjectid))
                    this.subjectid = (string)collection[CustomAPI.Helper.EntityHelper.Product.subjectid];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.suppliername;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.suppliername))
                    this.suppliername = (string)collection[CustomAPI.Helper.EntityHelper.Product.suppliername];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.transactioncurrencyid;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.transactioncurrencyid))
                    this.transactioncurrencyid = (string)collection[CustomAPI.Helper.EntityHelper.Product.transactioncurrencyid];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.validfromdate;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.validfromdate))
                    this.validfromdate = (DateTime)collection[CustomAPI.Helper.EntityHelper.Product.validfromdate];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.validtodate;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.validtodate))
                    this.validtodate = (DateTime)collection[CustomAPI.Helper.EntityHelper.Product.validtodate];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.vendorid;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.vendorid))
                    this.vendorid = (string)collection[CustomAPI.Helper.EntityHelper.Product.vendorid];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.vendorname;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.vendorname))
                    this.vendorname = (string)collection[CustomAPI.Helper.EntityHelper.Product.vendorname];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.vendorpartnumber;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.vendorpartnumber))
                    this.vendorpartnumber = (string)collection[CustomAPI.Helper.EntityHelper.Product.vendorpartnumber];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.kti_sku;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.kti_sku))
                    this.kti_sku = (string)collection[CustomAPI.Helper.EntityHelper.Product.kti_sku];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.ncci_productcategory;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.ncci_productcategory))
                    this.ncci_productcategory = (int)collection[CustomAPI.Helper.EntityHelper.Product.ncci_productcategory];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.moosourcesystem;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.moosourcesystem))
                    this.moosourcesystem = (string)collection[CustomAPI.Helper.EntityHelper.Product.moosourcesystem];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.mooexternalid;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.mooexternalid))
                    this.mooexternalid = (string)collection[CustomAPI.Helper.EntityHelper.Product.mooexternalid];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.companyid;
                if (collection.Contains(CustomAPI.Helper.EntityHelper.Product.companyid))
                    this.companyid = (int)collection[CustomAPI.Helper.EntityHelper.Product.companyid];

            }
            catch
            {
                throw new Exception("Product: Invalid " + ErrorEntity);
            }

            //EntityCollection priceListItem = (EntityCollection)collection[CustomAPI.Helper.EntityHelper.Product.pricelistitem];
            //this.pricelistitem = priceListItem.Entities.Select(entity => new CustomAPI.Product.Model.DTO.ProductPriceLevel(entity)).ToList();

        }

        public Product(Entity entity)
        {

            string ErrorEntity = "";

            try
            {

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.productnumber;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.productnumber))
                    this.productnumber = (string)entity[CustomAPI.Helper.EntityHelper.Product.productnumber];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.currentcost;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.currentcost))
                    this.currentcost = (decimal)entity[CustomAPI.Helper.EntityHelper.Product.currentcost];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.defaultuomid;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.defaultuomid))
                    this.defaultuomid = (string)entity[CustomAPI.Helper.EntityHelper.Product.defaultuomid];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.defaultuomscheduleid;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.defaultuomscheduleid))
                    this.defaultuomscheduleid = (string)entity[CustomAPI.Helper.EntityHelper.Product.defaultuomscheduleid];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.description;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.description))
                    this.description = (string)entity[CustomAPI.Helper.EntityHelper.Product.description];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.iskit;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.iskit))
                    this.iskit = (bool)entity[CustomAPI.Helper.EntityHelper.Product.iskit];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.isreparented;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.isreparented))
                    this.isreparented = (bool)entity[CustomAPI.Helper.EntityHelper.Product.isreparented];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.converttocustomerasset;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.converttocustomerasset))
                    this.converttocustomerasset = (bool)entity[CustomAPI.Helper.EntityHelper.Product.converttocustomerasset];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.defaultvendor;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.defaultvendor))
                    this.defaultvendor = (string)entity[CustomAPI.Helper.EntityHelper.Product.defaultvendor];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.fieldserviceproducttype;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.fieldserviceproducttype))
                    this.fieldserviceproducttype = (int)entity[CustomAPI.Helper.EntityHelper.Product.fieldserviceproducttype];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.purchasename;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.purchasename))
                    this.purchasename = (string)entity[CustomAPI.Helper.EntityHelper.Product.purchasename];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.taxable;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.taxable))
                    this.taxable = (bool)entity[CustomAPI.Helper.EntityHelper.Product.taxable];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.upccode;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.upccode))
                    this.upccode = (string)entity[CustomAPI.Helper.EntityHelper.Product.upccode];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.name;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.name))
                    this.name = (string)entity[CustomAPI.Helper.EntityHelper.Product.name];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.overriddencreatedon;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.overriddencreatedon))
                    this.overriddencreatedon = (DateTime)entity[CustomAPI.Helper.EntityHelper.Product.overriddencreatedon];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.parentproductid;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.parentproductid))
                    this.parentproductid = (string)entity[CustomAPI.Helper.EntityHelper.Product.parentproductid];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.price;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.price))
                    this.price = (decimal)entity[CustomAPI.Helper.EntityHelper.Product.price];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.pricelevelid;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.pricelevelid))
                    this.pricelevelid = (string)entity[CustomAPI.Helper.EntityHelper.Product.pricelevelid];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.processid;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.processid))
                    this.processid = (string)entity[CustomAPI.Helper.EntityHelper.Product.processid];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.productstructure;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.productstructure))
                    this.productstructure = (int)entity[CustomAPI.Helper.EntityHelper.Product.productstructure];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.productyypecode;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.productyypecode))
                    this.productyypecode = (int)entity[CustomAPI.Helper.EntityHelper.Product.productyypecode];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.producturl;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.producturl))
                    this.producturl = (string)entity[CustomAPI.Helper.EntityHelper.Product.producturl];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.quantitydecimal;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.quantitydecimal))
                    this.quantitydecimal = (int)entity[CustomAPI.Helper.EntityHelper.Product.quantitydecimal];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.quantityonhand;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.quantityonhand))
                    this.quantityonhand = (decimal)entity[CustomAPI.Helper.EntityHelper.Product.quantityonhand];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.size;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.size))
                    this.size = (string)entity[CustomAPI.Helper.EntityHelper.Product.size];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.stageid;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.stageid))
                    this.stageid = (string)entity[CustomAPI.Helper.EntityHelper.Product.stageid];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.standardcost;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.standardcost))
                    this.standardcost = (decimal)entity[CustomAPI.Helper.EntityHelper.Product.standardcost];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.statecode;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.statecode))
                    this.statecode = (int)entity[CustomAPI.Helper.EntityHelper.Product.statecode];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.statuscode;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.statuscode))
                    this.statuscode = (int)entity[CustomAPI.Helper.EntityHelper.Product.statuscode];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.stockvolume;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.stockvolume))
                    this.stockvolume = (decimal)entity[CustomAPI.Helper.EntityHelper.Product.stockvolume];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.stockweight;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.stockweight))
                    this.stockweight = (decimal)entity[CustomAPI.Helper.EntityHelper.Product.stockweight];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.subjectid;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.subjectid))
                    this.subjectid = (string)entity[CustomAPI.Helper.EntityHelper.Product.subjectid];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.suppliername;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.suppliername))
                    this.suppliername = (string)entity[CustomAPI.Helper.EntityHelper.Product.suppliername];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.transactioncurrencyid;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.transactioncurrencyid))
                    this.transactioncurrencyid = (string)entity[CustomAPI.Helper.EntityHelper.Product.transactioncurrencyid];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.validfromdate;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.validfromdate))
                    this.validfromdate = (DateTime)entity[CustomAPI.Helper.EntityHelper.Product.validfromdate];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.validtodate;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.validtodate))
                    this.validtodate = (DateTime)entity[CustomAPI.Helper.EntityHelper.Product.validtodate];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.vendorid;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.vendorid))
                    this.vendorid = (string)entity[CustomAPI.Helper.EntityHelper.Product.vendorid];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.vendorname;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.vendorname))
                    this.vendorname = (string)entity[CustomAPI.Helper.EntityHelper.Product.vendorname];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.vendorpartnumber;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.vendorpartnumber))
                    this.vendorpartnumber = (string)entity[CustomAPI.Helper.EntityHelper.Product.vendorpartnumber];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.kti_sku;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.kti_sku))
                    this.kti_sku = (string)entity[CustomAPI.Helper.EntityHelper.Product.kti_sku];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.ncci_productcategory;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.ncci_productcategory))
                    this.ncci_productcategory = (int)entity[CustomAPI.Helper.EntityHelper.Product.ncci_productcategory];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.moosourcesystem;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.moosourcesystem))
                    this.moosourcesystem = (string)entity[CustomAPI.Helper.EntityHelper.Product.moosourcesystem];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.mooexternalid;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.mooexternalid))
                    this.mooexternalid = (string)entity[CustomAPI.Helper.EntityHelper.Product.mooexternalid];

                ErrorEntity = CustomAPI.Helper.EntityHelper.Product.companyid;
                if (entity.Contains(CustomAPI.Helper.EntityHelper.Product.companyid))
                    this.companyid = (int)entity[CustomAPI.Helper.EntityHelper.Product.companyid];

            }
            catch
            {
                throw new Exception("Product: Invalid " + ErrorEntity);
            }

            //EntityCollection priceListItem = (EntityCollection)collection[CustomAPI.Helper.EntityHelper.Product.pricelistitem];
            //this.pricelistitem = priceListItem.Entities.Select(entity => new CustomAPI.Product.Model.DTO.ProductPriceLevel(entity)).ToList();

        }
        public Product(Product _product)
        {
            #region properties
            this.currentcost = _product.currentcost;
            this.defaultuomid = _product.defaultuomid;
            this.defaultuomscheduleid = _product.defaultuomscheduleid;
            this.description = _product.description;
            this.dmtimportstate = _product.dmtimportstate;
            this.entityimage = _product.entityimage;
            this.importsequencenumber = _product.importsequencenumber;
            this.iskit = _product.iskit;
            this.isreparented = _product.isreparented;
            this.isstockitem = _product.isstockitem;
            this.converttocustomerasset = _product.converttocustomerasset;
            this.defaultvendor = _product.defaultvendor;
            this.fieldserviceproducttype = _product.fieldserviceproducttype;
            this.purchasename = _product.purchasename;
            this.taxable = _product.taxable;
            this.transactioncategory = _product.transactioncategory;
            this.upccode = _product.upccode;
            this.name = _product.name;
            this.overriddencreatedon = _product.overriddencreatedon;
            this.parentproductid = _product.parentproductid;
            this.price = _product.price;
            this.pricelevelid = _product.pricelevelid;
            this.processid = _product.processid;
            this.productid = _product.productid;
            this.productnumber = _product.productnumber;
            this.productstructure = _product.productstructure;
            this.productyypecode = _product.productyypecode;
            this.producturl = _product.producturl;
            this.quantitydecimal = _product.quantitydecimal;
            this.quantityonhand = _product.quantityonhand;
            this.size = _product.size;
            this.stageid = _product.stageid;
            this.standardcost = _product.standardcost;
            this.statecode = _product.statecode;
            this.statuscode = _product.statuscode;
            this.stockvolume = _product.stockvolume;
            this.stockweight = _product.stockweight;
            this.subjectid = _product.subjectid;
            this.suppliername = _product.suppliername;
            this.transactioncurrencyid = _product.transactioncurrencyid;
            this.validfromdate = _product.validfromdate;
            this.validtodate = _product.validtodate;
            this.vendorid = _product.vendorid;
            this.vendorname = _product.vendorname;
            this.vendorpartnumber = _product.vendorpartnumber;
            this.kti_sku = _product.kti_sku;
            this.ncci_productcategory = _product.ncci_productcategory;
            this.moosourcesystem = _product.moosourcesystem;
            this.mooexternalid = _product.mooexternalid;
            #endregion properties
        }

        public Product()
        {

        }


        #region Properties
        //public decimal currentcost { get; set; }
        //public string defaultuomid { get; set; }
        //public string defaultuomscheduleid { get; set; }
        //public string description { get; set; }
        //public int dmtimportstate { get; set; }
        //public string entityimage { get; set; }
        //public int importsequencenumber { get; set; }
        //public bool iskit { get; set; }
        //public bool isreparented { get; set; }
        //public bool isstockitem { get; set; }
        //public bool converttocustomerasset { get; set; }
        //public string defaultvendor { get; set; }
        //public int fieldserviceproducttype { get; set; }
        //public string purchasename { get; set; }
        //public bool taxable { get; set; }
        //public string transactioncategory { get; set; }
        //public string upccode { get; set; }
        //public string name { get; set; }
        //public DateTime overriddencreatedon { get; set; }
        //public string parentproductid { get; set; }
        //public decimal price { get; set; }
        //public string pricelevelid { get; set; }
        //public string processid { get; set; }
        //public string productid { get; set; }
        //public string productnumber { get; set; }
        //public int productstructure { get; set; }
        //public int productyypecode { get; set; }
        //public string producturl { get; set; }
        //public int quantitydecimal { get; set; }
        //public decimal quantityonhand { get; set; }
        //public string size { get; set; }
        //public string stageid { get; set; }
        //public decimal standardcost { get; set; }
        //public int statecode { get; set; }
        //public int statuscode { get; set; }
        //public decimal stockvolume { get; set; }
        //public decimal stockweight { get; set; }
        //public string subjectid { get; set; }
        //public string suppliername { get; set; }
        //public string transactioncurrencyid { get; set; }

        //public string validfromdate { get; set; }
        //public string validtodate { get; set; }
        //public string vendorid { get; set; }
        //public string vendorname { get; set; }
        //public string vendorpartnumber { get; set; }
        //public string kti_sku { get; set; }

        //public string moosourcesystem { get; set; }
        //public string mooexternalid { get; set; }
        public int ncci_productcategory { get; set; }
        public int companyid { get; set; }

        //public List<CustomAPI.Product.Model.DTO.ProductPriceLevel> pricelistitem { get; set; }
        #endregion
    }
}

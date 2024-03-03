
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace CRM_Plugin.Core.Model
{
    public class ProductBase
    {
        //public BaseProduct(CRM_Plugin.CustomAPI.Product.Model.DTO.Product _product)
        //{

        //}

        #region columnSet
        public static ColumnSet columnSet = new ColumnSet(true);
        #endregion

        #region Properties
        public decimal currentcost { get; set; }
        public string defaultuomid { get; set; }
        public string defaultuomscheduleid { get; set; }
        public string description { get; set; }
        public int dmtimportstate { get; set; }
        public string entityimage { get; set; }
        public int importsequencenumber { get; set; }
        public bool iskit { get; set; }
        public bool isreparented { get; set; }
        public bool isstockitem { get; set; }
        public bool converttocustomerasset { get; set; }
        public string defaultvendor { get; set; }
        public int fieldserviceproducttype { get; set; }
        public string purchasename { get; set; }
        public bool taxable { get; set; }
        public string transactioncategory { get; set; }
        public string upccode { get; set; }
        public string name { get; set; }
        public DateTime overriddencreatedon { get; set; }
        public string parentproductid { get; set; }
        public decimal price { get; set; }
        public string pricelevelid { get; set; }
        public string processid { get; set; }
        public string productid { get; set; }
        public string productnumber { get; set; }
        public int productstructure { get; set; }
        public int productyypecode { get; set; }
        public string producturl { get; set; }
        public int quantitydecimal { get; set; }
        public decimal quantityonhand { get; set; }
        public string size { get; set; }
        public string stageid { get; set; }
        public decimal standardcost { get; set; }
        public int statecode { get; set; }
        public int statuscode { get; set; }
        public decimal stockvolume { get; set; }
        public decimal stockweight { get; set; }
        public string subjectid { get; set; }
        public string suppliername { get; set; }
        public string transactioncurrencyid { get; set; }
        public DateTime validfromdate { get; set; }
        public DateTime validtodate { get; set; }
        public string vendorid { get; set; }
        public string vendorname { get; set; }
        public string vendorpartnumber { get; set; }
        public string kti_sku { get; set; }
        public string moosourcesystem { get; set; }
        public string mooexternalid { get; set; }
        #endregion
    }
}

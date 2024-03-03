#region Namespaces
using System;
using System.ComponentModel.DataAnnotations;
#endregion

namespace CRM.Models.Items
{
    /// <summary>
    /// Product
    /// </summary>
    public class Product
    {
        public Product()
        {
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
            this.prodtype = _product.prodtype;
            this.parentitem = _product.parentitem;
            this.possku = _product.possku;
            this.rp = _product.rp;
            this.positemname = _product.positemname;
            this.category = _product.category;
            this.intensity = _product.intensity;
            this.ingallergn = _product.ingallergn;
            this.recycling = _product.recycling;
            this.recommend1 = _product.recommend1;
            this.recommend2 = _product.recommend2;
            this.recommend3 = _product.recommend3;
            this.recommend4 = _product.recommend4;
            this.matchgroup = _product.matchgroup;
            this.shortdesc = _product.shortdesc;
            this.specification = _product.specification;
            this.acccategory = _product.acccategory;
            this.collection = _product.collection;
            this.color = _product.color;
            this.property = _product.property;
            this.mooexternalid = _product.mooexternalid;
            this.moosourcesystem = _product.moosourcesystem;
            #endregion properties
        }

        #region Properties
        [Range(0, 1000000000000)]
        public decimal currentcost { get; set; }
        [Required]
        public String defaultuomid { get; set; }
        [Required]
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
        public CRM.CustomDataType.MooDateTime overriddencreatedon { get; set; }
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
        public CRM.CustomDataType.MooDateTime validfromdate { get; set; }
        public CRM.CustomDataType.MooDateTime validtodate { get; set; }
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
        #endregion
    }


    /// <summary>
    /// Product price level
    /// </summary>
    public class ProductPriceLevel
    {
        public ProductPriceLevel()
        {
        }

        public ProductPriceLevel(ProductPriceLevel _productPriceLevel)
        {
            #region properties
            this.amount = _productPriceLevel.amount;
            this.discounttypeid = _productPriceLevel.discounttypeid;
            this.createdon = _productPriceLevel.createdon;
            this.percentage = _productPriceLevel.percentage;
            this.pricelevelid = _productPriceLevel.pricelevelid;
            this.pricingmethodcode = _productPriceLevel.pricingmethodcode;
            this.productid = _productPriceLevel.productid;
            this.productpricelevelid = _productPriceLevel.productpricelevelid;
            this.quantitysellingcode = _productPriceLevel.quantitysellingcode;
            this.roundingoptionamount = _productPriceLevel.roundingoptionamount;
            this.roundingoptioncode = _productPriceLevel.roundingoptioncode;
            this.roundingpolicycode = _productPriceLevel.roundingpolicycode;
            this.transactioncurrencyid = _productPriceLevel.transactioncurrencyid;
            this.uomid = _productPriceLevel.uomid;
            this.uomscheduleid = _productPriceLevel.uomscheduleid;
            this.mooexternalid = _productPriceLevel.mooexternalid;
            this.moosourcesystem = _productPriceLevel.moosourcesystem;
            #endregion
        }

        #region Properties
        [Required]
        [Range(0, 100000000000000)]
        public decimal amount { get; set; }
        public string discounttypeid { get; set; }
        public CRM.CustomDataType.MooDateTime createdon { get; set; }
        [Required]
        [Range(0, 100000000000000)]
        public string percentage { get; set; }
        [Required]
        public string pricelevelid { get; set; }
        [Required]
        [Range(1, 6)]
        public string pricingmethodcode { get; set; }
        [Required]
        public string productid { get; set; }
        public string productpricelevelid { get; set; }
        [Range(1, 3)]
        public int quantitysellingcode { get; set; }
        [Range(-100000000000000, 100000000000000)]
        [Required]
        public decimal roundingoptionamount { get; set; }
        [Range(1, 2)]
        public int roundingoptioncode { get; set; }
        [Range(1, 4)]
        public int roundingpolicycode { get; set; }
        public string transactioncurrencyid { get; set; }
        [Required]
        public string uomid { get; set; }
        public string uomscheduleid { get; set; }
        public string moosourcesystem { get; set; }
        [DataType(DataType.Text)]
        public string mooexternalid { get; set; }
        #endregion
    }

    /// <summary>
    /// Inventory
    /// </summary>
    public class Inventory
    {
        public Inventory()
        {
        }

        public Inventory(Inventory _inventory)
        {
            #region properties
            this.bin = _inventory.bin;
            this.internalflags = _inventory.internalflags;
            this.name = _inventory.name;
            this.product = _inventory.product;
            this.productinventoryid = _inventory.productinventoryid;
            this.qtyallocated = _inventory.qtyallocated;
            this.qtyavailable = _inventory.qtyavailable;
            this.qtyonhand = _inventory.qtyonhand;
            this.qtyonorder = _inventory.qtyonorder;
            this.reorderpoint = _inventory.reorderpoint;
            this.row = _inventory.row;
            this.unit = _inventory.unit;
            this.warehouse = _inventory.warehouse;
            this.createdon = _inventory.createdon;
            this.statecode = _inventory.statecode;
            this.statuscode = _inventory.statuscode;
            this.mooexternalid = _inventory.mooexternalid;
            this.moosourcesystem = _inventory.moosourcesystem;
            #endregion
        }

        #region Properties
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string bin { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(1048576)]
        public string internalflags { get; set; }
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string name { get; set; }
        [Required]
        public string product { get; set; }
        public string sourcesystem { get; set; }
        public string productinventoryid { get; set; }
        [Range(0, 1000000000)]
        public double qtyallocated { get; set; }
        [Range(-1000000000, 1000000000)]
        public double qtyavailable { get; set; }
        [Range(-1000000000, 1000000000)]
        public double qtyonhand { get; set; }
        [Range(0, 1000000000)]
        public double qtyonorder { get; set; }
        [Range(0, 1000000000)]
        public double reorderpoint { get; set; }
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string row { get; set; }
        [DataType(DataType.Text)]
        [StringLength(100)]
        [Required]
        public string unit { get; set; }
        [DataType(DataType.Text)]
        [Required]
        public string warehouse { get; set; }
        public CRM.CustomDataType.MooDateTime createdon { get; set; }
        [Range(0, 1)]
        public int statecode { get; set; }
        [Range(0, 1)]
        public int statuscode { get; set; }
        [DataType(DataType.Text)]
        public string moosourcesystem { get; set; }
        [DataType(DataType.Text)]
        public string mooexternalid { get; set; }
        #endregion
    }

    /// <summary>
    /// Warehouse
    /// </summary>
    public class Warehouse
    {
        public Warehouse()
        {
        }
        public Warehouse(Warehouse _warehouse)
        {
            #region properties
            this.description = _warehouse.description;
            this.name = _warehouse.name;
            this.warehouseid = _warehouse.warehouseid;
            this.statecode = _warehouse.statecode;
            this.statuscode = _warehouse.statuscode;
            this.mooexternalid = _warehouse.mooexternalid;
            #endregion
        }

        #region Properties
        [StringLength(2000)]
        public string description { get; set; }
        [Required]
        [StringLength(100)]
        public string name { get; set; }
        public string warehouseid { get; set; }
        [Range(0, 1)]
        public int statecode { get; set; }
        [Range(1, 2)]
        public int statuscode { get; set; }
        //Customized
        [DataType(DataType.Text)]
        public string mooexternalid { get; set; }
        #endregion
    }

    /// <summary>
    /// Items
    /// </summary>
    public class Products
    {

    }
}

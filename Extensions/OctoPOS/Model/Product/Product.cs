using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Linq;
using KTI.Moo.Extensions.OctoPOS.Helper;

namespace KTI.Moo.Extensions.OctoPOS.Model
{
    public class Product : ProductBase
    {

        private DateTime _CreatedDate;
        private DateTime _LastEditedDate;
        private DateTime _ProductLaunchDate;

        [JsonProperty("CreatedDate")]
        public DateTime CreatedDate
        {
            get => _CreatedDate;
            set => _CreatedDate = Helper.DateTimeHelper.PHT_to_UTC(value);
        }

        [JsonProperty("LastEditedDate")]
        public DateTime LastEditedDate
        {
            get => _LastEditedDate;
            set => _LastEditedDate = Helper.DateTimeHelper.PHT_to_UTC(value);
        }

        [JsonProperty("ProductLaunchDate")]
        public DateTime ProductLaunchDate
        {
            get => _ProductLaunchDate;
            set => _ProductLaunchDate = Helper.DateTimeHelper.PHT_to_UTC(value);
        }


        [JsonIgnore]
        public override int companyid { get; set; }

        [JsonProperty("ProductSKU")]
        public override string sku { get; set; }

        [JsonProperty("ProductCode")]
        public override string productid { get; set; }

        [JsonIgnore]
        public override string category
        {
            get => ProductAttributes.Where(attribute => attribute.AttributeType == ProductAttributeTypeHelper.Category)
                .Select(attribute => attribute.AttributeCode).FirstOrDefault().ToString();

            set => ProductAttributes.Add(new()
            {
                AttributeType = ProductAttributeTypeHelper.Category,
                AttributeCode = value,
                AttributeName = value,
                AttributeStatus = 1
            });

        }

        [JsonIgnore]
        public override string size
        {
            get => ProductAttributes.Where(attribute => attribute.AttributeType == ProductAttributeTypeHelper.Size)
                .Select(attribute => attribute.AttributeCode).FirstOrDefault().ToString();

            set => ProductAttributes.Add(new()
            {
                AttributeType = ProductAttributeTypeHelper.Size,
                AttributeCode = value,
                AttributeName = value,
                AttributeStatus = 1
            });
        }

        [JsonIgnore]
        public override string suppliername
        {
            get => ProductAttributes.Where(attribute => attribute.AttributeType == ProductAttributeTypeHelper.Supplier)
                .Select(attribute => attribute.AttributeCode).FirstOrDefault().ToString();

            set => ProductAttributes.Add(new()
            {
                AttributeType = ProductAttributeTypeHelper.Supplier,
                AttributeCode = value,
                AttributeName = value,
                AttributeStatus = 1
            });
        }


        [StringLength(2000)]
        [JsonProperty("ProductDescription")]
        public override string description { get; set; }

        [Range(0, 1000000000000)]
        [JsonProperty("RetailSalesPrice")]
        public override decimal price { get; set; }

        [JsonProperty("Status", DefaultValueHandling = DefaultValueHandling.Include)]
        public int Status { get; set; }

        [JsonProperty("MinimumOrderQuantity", DefaultValueHandling = DefaultValueHandling.Include)]
        public int MinimumOrderQuantity { get; set; }


        [JsonProperty("Cost")]
        [Range(0, 1000000000000)]
        public override decimal currentcost { get; set; }

        [JsonIgnore]
        public override decimal standardcost
        {
            get
            {
                if (base.standardcost == 0)
                {
                    return currentcost;

                }
                return base.standardcost;
            }
            set => base.standardcost = value;
        }

        [JsonProperty("ReOrderQuantity")]
        public decimal ReOrderQuantity { get; set; }


        [JsonProperty("SupplierItemCode")]
        public string SupplierItemCode { get; set; }

        [JsonProperty("FloorPrice")]
        public decimal FloorPrice { get; set; }

        [JsonProperty("HasSerial", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool HasSerial { get; set; }

        [JsonProperty("HasBatch", DefaultValueHandling = DefaultValueHandling.Include)]
        public int HasBatch { get; set; }

        [JsonProperty("ProductBarcode")]
        public string ProductBarcode { get; set; }

        [JsonProperty("ProductPoint")]
        public decimal ProductPoint { get; set; }

        [JsonProperty("RedemptionPoint")]
        public decimal RedemptionPoint { get; set; }



        [JsonProperty("Sex")]
        public string Sex { get; set; }

        [JsonProperty("Width")]
        public string Width { get; set; }

        [JsonProperty("Height")]
        public string Height { get; set; }

        [JsonProperty("Depth")]
        public string Depth { get; set; }

        [JsonProperty("IsGiftCard", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool IsGiftCard { get; set; }

        [JsonProperty("Field1")]
        public string Field1 { get; set; }

        [JsonProperty("Field2")]
        public string Field2 { get; set; }

        [JsonProperty("Field3")]
        public string Field3 { get; set; }

        [JsonProperty("Field4")]
        public string Field4 { get; set; }

        [JsonProperty("Field5")]
        public string Field5 { get; set; }

        [JsonProperty("Field6")]
        public string Field6 { get; set; }

        [JsonProperty("Field7")]
        public string Field7 { get; set; }

        [JsonProperty("Field8")]
        public string Field8 { get; set; }

        [JsonProperty("Field9")]
        public string Field9 { get; set; }

        [JsonProperty("Field10")]
        public string Field10 { get; set; }

        [JsonProperty("IsNonInventoried", DefaultValueHandling = DefaultValueHandling.Include)]
        public int IsNonInventoried { get; set; }

        [JsonProperty("IsTaxable", DefaultValueHandling = DefaultValueHandling.Include)]
        public int IsTaxable { get; set; }

        [JsonProperty("TaxType", DefaultValueHandling = DefaultValueHandling.Include)]
        public int TaxType { get; set; }

        [JsonProperty("TaxGroupId", DefaultValueHandling = DefaultValueHandling.Include)]
        public int TaxGroupId { get; set; }

        [JsonProperty("ProductType", DefaultValueHandling = DefaultValueHandling.Include)]
        public int ProductType { get; set; }

        [JsonProperty("ProductType2", DefaultValueHandling = DefaultValueHandling.Include)]
        public int ProductType2 { get; set; }

        [JsonProperty("Expdays", DefaultValueHandling = DefaultValueHandling.Include)]
        public int Expdays { get; set; }

        [JsonProperty("ProductCommissionAmount")]
        public double ProductCommissionAmount { get; set; }

        [JsonProperty("ProductCommissionType", DefaultValueHandling = DefaultValueHandling.Include)]
        public int ProductCommissionType { get; set; }

        [JsonProperty("ProductAttributes")]
        public List<ProductAttribute> ProductAttributes { get; set; }

        //[JsonProperty("ProductAddons")]
        //public dynamic ProductAddons { get; set; }

        //[JsonProperty("InventoryMap")]
        //public dynamic InventoryMap { get; set; }

        public Product()
        {
            ProductAttributes = new();


        }

    }



}

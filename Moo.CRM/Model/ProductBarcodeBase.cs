using System.ComponentModel.DataAnnotations;

namespace KTI.Moo.CRM.Model
{
    public class ProductBarcodeBase
    {
        [JsonProperty("BarCode")]
        public string BarCode { get; set; }

        [JsonProperty("BarCodeSetupId")]
        public string BarCodeSetupId { get; set; }

        [JsonProperty("IsDefaultDisplayedBarcode")]
        public string IsDefaultDisplayedBarcode { get; set; }

        [JsonProperty("IsDefaultPrintedBarcode")]
        public string IsDefaultPrintedBarcode { get; set; }

        [JsonProperty("IsDefaultScannedBarcode")]
        public string IsDefaultScannedBarcode { get; set; }

        [JsonProperty("ItemNumber")]
        public string ItemNumber { get; set; }

        [JsonProperty("ProductColorId")]
        public string ProductColorId { get; set; }

        [JsonProperty("ProductConfigurationId")]
        public string ProductConfigurationId { get; set; }

        [JsonProperty("ProductDescription")]
        public string ProductDescription { get; set; }

        [JsonProperty("ProductQuantityUnitSymbol")]
        public string ProductQuantityUnitSymbol { get; set; }

        [JsonProperty("ProductSizeId")]
        public string ProductSizeId { get; set; }

        [JsonProperty("ProductStyleId")]
        public string ProductStyleId { get; set; }

        [JsonProperty("ProductVersionId")]
        public string ProductVersionId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string moosourcesystem { get; set; }
        //Customized
        [DataType(DataType.Text)]
        public string mooexternalid { get; set; }
        //Customized
        //[CompanyIdAttribute]
        public int companyid { get; set; }
    }
}

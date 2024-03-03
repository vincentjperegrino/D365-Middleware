using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.CustomAttributes;
using KTI.Moo.Extensions.Cyware.Model;
using System.ComponentModel.DataAnnotations;

namespace KTI.Moo.Cyware.Model.DTO
{
    public class ProductBarcodePoll54
    {
        [SortOrder(1)]
        [MaxLength(18)]
        public string product_code { get; set; }
        [SortOrder(2)]
        [MaxLength(20)]
        public string sku_number { get; set; }
        [SortOrder(3)]
        [MaxLength(5)]
        public string upc_type { get; set; }
        [SortOrder(4)]
        [MaxLength(3)]
        public string upc_unit_of_measure { get; set; }

        public ProductBarcodePoll54(ProductBarcode _productBarcode)
        {
            var helper = new PollMapping();
            this.product_code = helper.FormatStringAddSpacePadding(_productBarcode.product_code, (typeof(ProductBarcodePoll54).GetProperty("product_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.sku_number = helper.FormatStringAddSpacePadding(_productBarcode.sku_number, (typeof(ProductBarcodePoll54).GetProperty("sku_number").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.upc_type = helper.FormatStringAddSpacePadding(_productBarcode.upc_type, (typeof(ProductBarcodePoll54).GetProperty("upc_type").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.upc_unit_of_measure = helper.FormatStringAddSpacePadding(_productBarcode.upc_unit_of_measure, (typeof(ProductBarcodePoll54).GetProperty("upc_unit_of_measure").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
        }

        public string Concat(ProductBarcodePoll54 obj)
        {
            var helper = new PollMapping();
            return helper.ConcatenateValues(obj);
        }
    }
}

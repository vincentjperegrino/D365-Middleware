using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.CustomAttributes;
using KTI.Moo.Extensions.Cyware.Model;
using System.ComponentModel.DataAnnotations;

namespace KTI.Moo.Cyware.Model.DTO
{
    public class ProductCategoryPoll54
    {
        [SortOrder(1)]
        [MaxLength(20)]
        [Required]
        public virtual string department { get; set; }
        [SortOrder(2)]
        [MaxLength(20)]

        public virtual string sub_dept { get; set; }
        [SortOrder(3)]
        [MaxLength(20)]
        public virtual string cy_class { get; set; }
        [SortOrder(4)]
        [MaxLength(20)]
        public virtual string sub_class { get; set; }
        [SortOrder(5)]
        [MaxLength(60)]
        public virtual string name { get; set; }
        [SortOrder(6)]
        [MaxLength(5)]
        public virtual string planned_gm { get; set; }

        public ProductCategoryPoll54(ProductCategory _productCategory)
        {
            var helper = new PollMapping();
            this.department = helper.FormatStringAddSpacePadding(_productCategory.department, (typeof(ProductCategoryPoll54).GetProperty("department").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.sub_dept = helper.FormatStringAddSpacePadding(_productCategory.sub_dept, (typeof(ProductCategoryPoll54).GetProperty("sub_dept").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.cy_class = helper.FormatStringAddSpacePadding(_productCategory.cy_class, (typeof(ProductCategoryPoll54).GetProperty("cy_class").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.sub_class = helper.FormatStringAddSpacePadding(_productCategory.sub_class, (typeof(ProductCategoryPoll54).GetProperty("sub_class").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.name = helper.FormatStringAddSpacePadding(_productCategory.name ?? "", (typeof(ProductCategoryPoll54).GetProperty("name").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.planned_gm = helper.FormatDecimalAddZeroPrefixAndSuffix("000", ((typeof(ProductCategoryPoll54).GetProperty("planned_gm").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0), 2);
        }

        public string Concat(ProductCategoryPoll54 obj)
        {
            var helper = new PollMapping();
            return helper.ConcatenateValues(obj);
        }
    }
}

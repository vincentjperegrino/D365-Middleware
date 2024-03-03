using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Cyware.Model.DTO;
using KTI.Moo.Extensions.Core.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.Model.DTO
{
    public class DiscountLocationPOLL
    {
        [MaxLength(10)]
        [SortOrder(1)]
        public string LocationCode { get; set; }
        [MaxLength(20)]
        [SortOrder(2)]
        public string DiscountCode { get; set; }
        [MaxLength(12)]
        [SortOrder(3)]
        public string EventNum { get; set; }
       
        public DiscountLocationPOLL(DiscountLocation discountLocation)
        {
            PollMapping helper = new PollMapping();
            this.LocationCode = helper.FormatStringAddSpacePadding(discountLocation.LocationCode, (typeof(DiscountLocationPOLL).GetProperty("LocationCode").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.DiscountCode = helper.FormatStringAddSpacePadding(discountLocation.DiscountCode, (typeof(DiscountLocationPOLL).GetProperty("DiscountCode").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.EventNum = helper.FormatStringAddSpacePadding(discountLocation.EventNum, (typeof(DiscountLocationPOLL).GetProperty("EventNum").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
        }


        public string Concat(DiscountLocationPOLL obj)
        {
            PollMapping helper = new PollMapping();
            return helper.ConcatenateValues(obj);
        }
    }
}

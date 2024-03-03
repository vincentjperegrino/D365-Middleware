using System.ComponentModel.DataAnnotations;

namespace KTI.Moo.Extensions.Lazada.Model
{
    public class SellerRegion
    {
        [RegularExpression("ph|vn|sg|th|id|my")]
        public string Region { get; set; }
        public string SellerId { get; set; } = null;
    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Magento.Model;

public class PromoCoupon : Core.Model.PromoBase
{
    [JsonIgnore]
    public override int companyid { get; set; }

    [JsonProperty("rule_id")]
    public override string kti_channelid { get; set; }

    [JsonProperty("rule_name")]
    public override string kti_promocode { get; set; }

    [JsonProperty("discount_amount")]
    public override decimal kti_discountamount { get; set; }

    public string simple_action { get; set; } = Helper.Coupon.SimpleAction.Percent;

    // not required
    public string coupons_prefix { get; set; }
    public string coupons_suffix { get; set; }

}

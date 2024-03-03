using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Lazada.Model;

public class Promo : Core.Model.PromoBase
{
    [JsonProperty("offering_money_value_off")]
    public override decimal kti_discountamount { get; set; }

    [JsonProperty("voucher_name")]
    public override string kti_promocode { get; set; }

    public string id { get; set; }
    public string voucher_type { get; set; }

  
}

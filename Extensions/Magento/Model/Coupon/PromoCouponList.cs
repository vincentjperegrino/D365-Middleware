using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Magento.Model;

public class PromoCouponList : Core.Model.PromoCouponBase
{
    public string alias { get; set; }

    [JsonProperty("code")]
    public override string kti_uniquepromocode { get; set; }

    public int rule_id { get; set; }

    public int customer_id { get; set; }

    [JsonIgnore]
    public override string kti_customerid { get => customer_id.ToString(); }


    public string email { get; set; }

}

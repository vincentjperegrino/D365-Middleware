using Newtonsoft.Json;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class VertextCommodityCode
    {
        [JsonProperty("code")]
        public string code { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }
    }
}

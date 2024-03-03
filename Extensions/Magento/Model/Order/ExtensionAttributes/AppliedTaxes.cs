
using Newtonsoft.Json;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class AppliedTaxes
    {
        [JsonProperty("amount")]
        public decimal amount { get; set; }

        [JsonProperty("base_amount")]
        public decimal base_amount { get; set; }

        [JsonProperty("code")]
        public string code { get; set; }

        [JsonProperty("percent")]
        public decimal percent { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }
        
    }
}

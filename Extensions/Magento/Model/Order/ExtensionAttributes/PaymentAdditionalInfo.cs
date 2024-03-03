
using Newtonsoft.Json;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class PaymentAdditionalInfo
    {
        [JsonProperty("key")]
        public string key { get; set; }

        [JsonProperty("value")]
        public string value { get; set; }
    }
}

using Newtonsoft.Json;

namespace KTI.Moo.Extensions.OctoPOS.Model
{
    public class CustomerType
    {
        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("ValidityPeriod")]
        public int ValidityPeriod { get; set; }

        [JsonProperty("DollarToPointRatio")]
        public double DollarToPointRatio { get; set; }

        [JsonProperty("Status")]
        public int Status { get; set; }
    }
}

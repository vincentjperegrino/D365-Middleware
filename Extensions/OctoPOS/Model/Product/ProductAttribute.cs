using Newtonsoft.Json;

namespace KTI.Moo.Extensions.OctoPOS.Model
{
    public class ProductAttribute
    {
        [JsonProperty("AttributeType")]
        public string AttributeType { get; set; }

        [JsonProperty("AttributeCode")]
        public string AttributeCode { get; set; }

        [JsonProperty("AttributeName")]
        public string AttributeName { get; set; }

        [JsonProperty("AttributeStatus")]
        public int AttributeStatus { get; set; }
    }
}

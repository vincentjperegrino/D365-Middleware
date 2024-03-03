using KTI.Moo.Extensions.Core.Model;
using Newtonsoft.Json;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class Region : RegionBase
    {

        //Using this for adding Customer API code and properties

        [JsonProperty("region_id")]
        public override string region_id { get; set; }

        [JsonProperty("region")]
        public override string region_name { get; set; }

        [JsonProperty("region_code")]
        public override string region_code { get; set; }



        //Using this for Country API code and properties

        [JsonProperty("id")]
        public string id { set { region_id = value; } }
        [JsonProperty("name")]
        public string name { set { region_name = value; } }
        [JsonProperty("code")]
        public string code { set { region_code = value; } }

    }
}

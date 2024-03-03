using KTI.Moo.Extensions.Core.Model;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class Country : CountryBase
    {
        public string id { init { country_id = value; } }

        [JsonProperty("available_regions")]
        public List<Region> available_regions { get; set; }

    }
}

using Newtonsoft.Json;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.OctoPOS.Model.DTO.Products
{
    public class Search : Core.Model.SearchBase<Model.Product>
    {
        [JsonProperty("TotalItems")]
        public override int total_count { get; set; }

        [JsonProperty("ItemsInThisPage")]
        public int ItemsInThisPage { get; set; }

        [JsonProperty("Products")]
        public List<Model.Product> values { get; set; }

    }
}

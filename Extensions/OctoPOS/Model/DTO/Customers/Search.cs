using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.OctoPOS.Model.DTO.Customers
{
    public class Search : Core.Model.SearchBase<Model.Customer>
    {
        [JsonProperty("TotalPages")]
        public override int total_pages { get; set; }

        [JsonProperty("CurrentPage")]
        public override int current_page { get; set; }

        [JsonProperty("Customers")]
        public override List<Model.Customer> values { get; set; }

    }
}

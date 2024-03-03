

using Newtonsoft.Json;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.OctoPOS.Model.DTO.Invoices;

public class Search : Core.Model.SearchBase<Model.Invoice>
{

    [JsonProperty("TotalPages")]
    public override int total_pages { get; set; }

    [JsonProperty("PageSize")]
    public override int page_size { get; set; }

    [JsonProperty("Invoices")]
    public override List<Model.Invoice> values { get; set; }

}



using Newtonsoft.Json;
using System.Collections.Generic;


namespace KTI.Moo.Extensions.Lazada.Model.DTO.Fulfillments;

public class AWBDocumentReq
{
    public string doc_type { get; set; } = "PDF"; //HTML or PDF

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
    public bool print_item_list { get; set; } = true;

    public List<PackageIds> packages { get; set; }  
}

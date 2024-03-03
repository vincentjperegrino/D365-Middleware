namespace KTI.Moo.CRM.Model.DTO.Orders;

public class Search : Base.Model.SearchBase<Model.OrderBase>
{
    [JsonProperty(PropertyName = "@odata.context")]
    public string context { get; set; }

    [JsonProperty(PropertyName = "@odata.nextLink")]
    public string NextLink { get; set; }

    [JsonProperty(PropertyName = "value")]
    public override List<Model.OrderBase> values { get; set; }
}

namespace KTI.Moo.CRM.Model.DTO.Orders.Plugin;

public class Order : Model.OrderBase
{

    [JsonProperty(PropertyName = "@odata.type")]
    public string DataType { get; init; } = "#Microsoft.Dynamics.CRM.expando";

    [JsonProperty(PropertyName = "customerid")]
    public override string customerid { get; set; }

    [JsonProperty(PropertyName = "kti_branchassigned")]
    public override string branch_assigned { get; set; }

    public override string domainType { get; init; }

}

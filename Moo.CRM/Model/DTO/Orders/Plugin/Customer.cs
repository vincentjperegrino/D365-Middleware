namespace KTI.Moo.CRM.Model.DTO.Orders.Plugin;

public class Customer : Model.CustomerBase
{

    [JsonProperty(PropertyName = "@odata.type")]
    public string DataType { get; init; } = "#Microsoft.Dynamics.CRM.expando";
    public override string domainType { get; init; }
}

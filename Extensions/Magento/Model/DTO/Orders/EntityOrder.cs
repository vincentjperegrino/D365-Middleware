using Newtonsoft.Json;

namespace KTI.Moo.Extensions.Magento.Model.DTO.Orders;

public class EntityOrder : Order
{

    [JsonIgnore]
    public override Customer CustomerDetails { get; set; }
    [JsonIgnore] 
    public override string billto_city { get; set; }
    [JsonIgnore] 
    public override string billto_contactname { get; set; }
    [JsonIgnore] 
    public override string billto_country { get; set; }
    [JsonIgnore] 
    public override string billto_fax { get; set; }
    [JsonIgnore] 
    public override string billto_line1 { get; set; }
    [JsonIgnore] 
    public override string billto_line2 { get; set; }
    [JsonIgnore] 
    public override string billto_line3 { get; set; }
    [JsonIgnore] 
    public override string billto_name { get; set; }
    [JsonIgnore] 
    public override string billto_postalcode { get; set; }
    [JsonIgnore] 
    public override string billto_telephone { get; set; }
    [JsonIgnore] 
    public override string shipto_city { get; set; }
    [JsonIgnore] 
    public override string shipto_contactname { get; set; }
    [JsonIgnore] 
    public override string shipto_country { get; set; }
    [JsonIgnore] 
    public override string shipto_line1 { get; set; }
    [JsonIgnore] 
    public override string shipto_line2 { get; set; }
    [JsonIgnore] 
    public override string shipto_line3 { get; set; }
    [JsonIgnore] 
    public override string shipto_name { get; set; }
    [JsonIgnore] 
    public override string shipto_postalcode { get; set; }
    [JsonIgnore] 
    public override string shipto_telephone { get; set; }
    



}

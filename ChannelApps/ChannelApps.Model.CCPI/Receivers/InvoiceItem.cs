
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Model.CCPI.Receivers;

public class InvoiceItem : CRM.Model.InvoiceItemBase
{
    public InvoiceItem()
    {

    }



    #region NCCI properties
    public string ncci_remark { get; set; }

    [JsonProperty(PropertyName = "ncci_Salesman@odata.bind")]
    public string ncci_Salesman { get; set; }

    [JsonProperty(PropertyName = "ncci_Branch@odata.bind")]
    public string ncci_Branch { get; set; }

    [JsonProperty(PropertyName = "ncci_Customer_contact@odata.bind")]
    public string ncci_Customer { get; set; }

    public decimal ncci_discountpercentage { get; set; }

    public decimal ncci_taxpercentage { get; set; }
    public decimal ncci_coffee_qty { get; set; }
    public decimal ncci_machine_qty { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
    public override bool isproductoverridden { get; set; }

    #endregion



}

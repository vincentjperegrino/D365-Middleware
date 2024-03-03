using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.OctoPOS.Model.DTO.Invoices;

public class Add : Model.Invoice
{
    [JsonIgnore]
    public override Model.Customer CustomerDetails { get; set; }
}

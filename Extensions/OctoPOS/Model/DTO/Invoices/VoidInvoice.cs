
using Newtonsoft.Json;
using System;

namespace KTI.Moo.Extensions.OctoPOS.Model.DTO.Invoices
{
    public class Void : Model.Invoice
    {
        [JsonProperty("VoidBy")]
        public string VoidBy { get; set; }

        [JsonProperty("VoidDate")]
        public DateTime VoidDate { get; set; }

        [JsonProperty("VoidReason")]
        public string VoidReason { get; set; }
    }
}

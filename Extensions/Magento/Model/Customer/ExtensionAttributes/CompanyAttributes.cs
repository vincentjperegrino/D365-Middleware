using KTI.Moo.Extensions.Core.Model;
using Newtonsoft.Json;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class CompanyAttributes
    {
        [JsonProperty("company_id")]
        public int company_id { get; set; }

        public int customer_id { get; set; }

        public CustomerExtensionAttributes extension_attributes { get; set; }

        public string job_title { get; set; }

        public int status { get; set; }

        public string telephone { get; set; }
    }
}

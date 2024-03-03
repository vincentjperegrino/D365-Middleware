using KTI.Moo.Extensions.Core.Model;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class CustomerExtensionAttributes
    {
        public bool is_subscribed { get; set; }
        public string amazon_id { get; set; }

        public CompanyAttributes company_attributes { get; set; }

        public string vertex_customer_code { get; set; }

        public string vertex_customer_country { get; set; }

    }
}

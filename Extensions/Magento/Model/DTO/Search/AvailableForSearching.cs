using KTI.Moo.Extensions.Magento.Model;
using System;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Magento.Model.DTO.Search
{
    public class AvailableForSearching
    {

        public int customer_id { get; set; }
        public int group_id { get; set; }
        public string default_billing { get; set; }
        public string default_shipping { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string created_in { get; set; }
        public List<EmailAddress> EmailAddress { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int store_id { get; set; }
        public int website_id { get; set; }

        public AvailableForSearching(Customer customer)
        {
            customer_id = customer.customer_id;
            group_id = customer.group_id;
            default_billing = customer.default_billing;
            default_shipping = customer.default_shipping;
            created_at = customer.created_at;
            updated_at = customer.updated_at;
            created_in = customer.created_in;
            EmailAddress = customer.EmailAddress;
            firstname = customer.firstname;
            lastname = customer.lastname;
            store_id = customer.store_id;
            website_id = customer.website_id;

        }














    }


}

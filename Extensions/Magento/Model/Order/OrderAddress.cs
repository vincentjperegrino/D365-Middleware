
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class OrderAddress : Address
    {

        [JsonIgnore]
        public override Region region { get; set; }

        [JsonProperty("region")]
        public override string region_id { get => region.region_id; set => region.region_id = value; }

        [JsonProperty("region_code")]
        public string region_code { get => region.region_code; set => region.region_code = value; }


        [JsonProperty("customer_address_id")]
        public override int address_id { get; set; }

        [JsonProperty("entity_id")]
        public int entity_id { get; set; }

        [JsonProperty("address_type")]
        public string address_type { get; set; }

        
        [JsonProperty("parent_id")]
        public int parent_id { get; set; }

        [JsonProperty("email")]
        public string email
        {

            get
            {
                if (EmailAddress.Count > 0)
                {
                    return EmailAddress.Where(item => item.primary == true).Select(item => item.emailaddress).FirstOrDefault().ToString();
                }

                return null;
            }

            set => EmailAddress.Add(new Model.EmailAddress()
            {
                primary = true,
                emailaddress = value
            });
        }


        [JsonIgnore]
        public List<EmailAddress> EmailAddress { get; set; }


        public OrderAddress()
        {
            EmailAddress = new();
            region = new();
        }



    }

}

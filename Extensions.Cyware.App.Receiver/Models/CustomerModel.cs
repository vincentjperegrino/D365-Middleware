using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace KTI.Moo.Extensions.Cyware.App.Receiver.Models
{

    public class CustomerModel
    {
        #region oldfields
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("address")]
        public AddressModel Address { get; set; }
        #endregion

        public string dataAreaId { get; set; }
        public string LanguageId { get; set; }
        public string NameAlias { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        public string PersonLastNamePrefix { get; set; }
        public string PersonMiddleName { get; set; }
        public string PersonPhoneticFirstName { get; set; }
        public string PersonPhoneticLastName { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string SalesCurrencyCode { get; set; }
        public string OrganizationName { get; set; }



    }

    public class AddressModel
    {
        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("zipcode")]
        public string Zipcode { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }
    }

}

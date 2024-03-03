using KTI.Moo.Extensions.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class Address : AddressBase
    {


        [JsonProperty("region")]
        public virtual Region region { get; set; }

        [JsonProperty("id")]
        public override int address_id { get; set; }

        [JsonProperty("customer_id")]
        public int customer_id { get; set; }



        [JsonIgnore]
        public override string address_country
        {
            get
            {
                if (string.IsNullOrWhiteSpace(base.address_country))
                {

                    return country_id;
                }

                return base.address_country;
            }
            set => base.address_country = value;
        }

        [JsonProperty("region_id")]
        public virtual string region_id { get; set; }

        [JsonProperty("country_id")]
        public string country_id { get; set; }

        [JsonIgnore]
        public List<Telephone> Telephone { get; set; }

        [JsonProperty("firstname")]
        public override string first_name { get; set; }

        [JsonProperty("lastname")]
        public override string last_name { get; set; }



        [JsonIgnore]
        [StringLength(100)]
        public override string address_primarycontactname
        {
            get
            {
                if (string.IsNullOrWhiteSpace(base.address_primarycontactname))
                {

                    return first_name + " " + last_name;
                }

                return base.address_primarycontactname;
            }

            set => base.address_primarycontactname = value;
        }



        [JsonProperty("middlename")]
        public string middle_name { get; set; }

        public string[] street
        {
            get
            {
                return new string[] {
                address_line1,
                address_line2,
                address_line3
                };
            }

            set
            {
                address_line1 = value.Length > 0 ? value[0] : null;
                address_line2 = value.Length > 1 ? value[1] : null;
                address_line3 = value.Length > 2 ? value[2] : null;
            }
        }


        [JsonIgnore]
        [StringLength(250)]
        public override string address_line1 { get; set; }
        [JsonIgnore]
        [StringLength(250)]
        public override string address_line2 { get; set; }
        [JsonIgnore]
        [StringLength(250)]
        public override string address_line3 { get; set; }

        [JsonProperty("postcode")]
        public override string address_postalcode { get; set; }
        [JsonProperty("city")]
        public override string address_city { get; set; }


        public bool default_shipping { set => defaultShipping = value; }

        public bool default_billing { set => defaultBilling = value; }


        //for adding Customer
        public bool defaultShipping { get; set; }

        public bool defaultBilling { get; set; }

        [JsonProperty("telephone")]
        public string telephone
        {

            get
            {

                if (Telephone.Count > 0 && Telephone.Where(tel => !string.IsNullOrWhiteSpace(tel.telephone)).Any())
                {
                    return Telephone.Where(item => item.primary).Select(item => item.telephone).FirstOrDefault().ToString();
                }

                return default;
            }

            set => Telephone.Add(new Model.Telephone() { primary = true, telephone = value });
        }



        [JsonProperty("company")]
        public string company { get; set; }

        [JsonProperty("custom_attributes")]
        public List<Attribute> custom_attributes { get; set; }

        [JsonProperty("extension_attributes")]
        public CustomerExtensionAttributes extension_attributes { get; set; }

        [JsonProperty("fax")]
        public override string address_fax { get; set; }

        [JsonProperty("prefix")]
        public string prefix { get; set; }

        [JsonProperty("vat_id")]
        public string vat_id { get; set; }

        public Address()
        {
            Telephone = new List<Telephone>();

            int maximumAddressLine = 3;
            street = new string[maximumAddressLine];

        }
    }




}

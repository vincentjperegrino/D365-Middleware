using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KTI.Moo.Extensions.Magento.Helper;
using Newtonsoft.Json;
using System.Linq;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class Customer : CustomerBase
    {

        [JsonIgnore]
        public override int companyid { get; set; }

        [JsonProperty("extension_attributes")]
        public CustomerExtensionAttributes extension_attributes { get; set; }

        [JsonProperty("addresses")]
        public List<Address> address { get; set; }

        [JsonProperty("id")]
        public int customer_id { get; set; }

        [JsonIgnore]
        public override string kti_sourceid
        {
            get
            {
                if (string.IsNullOrEmpty(base.kti_sourceid))
                {
                    return customer_id.ToString();
                }


                return base.kti_sourceid;
            }

            set => base.kti_sourceid = value;
        }


        [JsonIgnore]
        public List<EmailAddress> EmailAddress { get; set; }

        [Required]
        public override string firstname { get; set; }

        [Required]
        public override string lastname { get; set; }

        public override string middlename { get; set; }

        [JsonIgnore]
        public string password { get; set; }

        public int group_id { get; set; }

        public int store_id { get; set; }

        public int website_id { get; set; }

        public string default_billing { get; set; }

        public string default_shipping { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public string created_in { get; set; }

        public int disable_auto_group_change { get; set; }

        [Required]
        public string email
        {

            get
            {
                if (EmailAddress.Count > 0 && EmailAddress.Any(emails => !string.IsNullOrWhiteSpace(emails.emailaddress)))
                {
                    return EmailAddress.Where(emails => !string.IsNullOrWhiteSpace(emails.emailaddress)).FirstOrDefault().emailaddress;
                }

                return null;
            }

            set => EmailAddress.Add(new Model.EmailAddress()
            {
                primary = true,
                emailaddress = value,
                created_date = created_at,
                updated_date = updated_at
            });
        }

        public string confirmation { get; set; }


        [JsonProperty("custom_attributes")]
        public List<Attribute> custom_attributes { get; set; }

        [JsonProperty("dob")]
        public override string birthdate { get; set; }

        public int gender { get; set; }

        public string prefix { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public string taxvat { get; set; }


        [JsonIgnore]
        public override DateTime customerjoineddate { get => this.created_at; }


        public Customer()
        {

            EmailAddress = new();

        }


        //For adding Manualy

        //public void ParseFromMagento(dynamic MagentoCustomerData)
        //{


        //    address = new List<Address>();

        //    foreach (var addressData in MagentoCustomerData.addresses)
        //    {

        //        string[] addressStreet = addressData.street.ToObject<string[]>();


        //        address.Add(new Address()
        //        {

        //            address_id = addressData.id,
        //            customer_id = addressData.customer_id,
        //            first_name = addressData.firstname,
        //            last_name = addressData.lastname,
        //            Telephone = new List<Telephone>() { new Telephone() { primary = true, telephone = addressData.telephone, created_date = created_at, updated_date = updated_at } },
        //            address_postalcode = addressData.postcode,
        //            region = new Region()
        //            {
        //                region_id = addressData.region.region_id,
        //                region_code = addressData.region.region_code,
        //                region_name = addressData.region.region
        //            },
        //            region_id = addressData.region_id,
        //            country_id = addressData.country_id,
        //            street = addressData.street.ToObject<string[]>(),
        //            address_line1 = addressStreet.Length > 0 ? addressStreet[0] : null,
        //            address_line2 = addressStreet.Length > 1 ? addressStreet[1] : null,
        //            address_line3 = addressStreet.Length > 2 ? addressStreet[2] : null,
        //            address_city = addressData.city,
        //            defaultShipping = addressData.default_shipping,
        //            defaultBilling = addressData.default_billing,


        //        });


        //    }





        //}


    }



}

using KTI.Moo.Extensions.Lazada.Model;
using KTI.Moo.Base.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Model.NCCI.Receivers;

public class Customer : CRM.Model.CustomerBase
{
    public Customer()
    {

    }

    public Customer(KTI.Moo.Extensions.Magento.Model.Customer _customer)
    {
        #region properties


        this.overriddencreatedon = _customer.created_at;
        this.companyid = _customer.companyid;
        this.kti_socialchannelorigin = CRM.Helper.ChannelOrigin.OptionSet_magento;

        if (!string.IsNullOrWhiteSpace(_customer.birthdate))
        {
            this.birthdate = _customer.birthdate;
        }

        if (!string.IsNullOrWhiteSpace(_customer.email))
        {
            this.emailaddress1 = _customer.email.ToLower();
        }

        if (!string.IsNullOrWhiteSpace(_customer.firstname))
        {
            this.firstname = _customer.firstname;
        }

        if (!string.IsNullOrWhiteSpace(_customer.lastname))
        {
            this.lastname = _customer.lastname;
        }

        if (!string.IsNullOrWhiteSpace(_customer.salutation))
        {
            this.salutation = _customer.salutation;
        }

        if (!string.IsNullOrWhiteSpace(_customer.kti_sourceid))
        {
            this.kti_sourceid = _customer.kti_sourceid;
            this.kti_magentoid = _customer.kti_sourceid;
        }

        if (_customer.custom_attributes is not null && _customer.custom_attributes.Any(custom => custom.attribute_code == "mobile_number"))
        {
            this.mobilephone = Convert.ToString(_customer.custom_attributes.Where(custom => custom.attribute_code == "mobile_number").FirstOrDefault().value);

            this.mobilephone = this.mobilephone.FormatPhoneNumber();
        }

        MapMagentoAddressToCRMAddress(_customer);

        //this.ncci_customerjoineddate = _customer.customerjoineddate;
        //this.ncci_clubmembershipid = _customer.MembershipCode;
        //this.ncci_customerjoinedbranch = _customer.Location;

        #endregion
    }

    public Customer(KTI.Moo.Extensions.OctoPOS.Model.Customer _customer)
    {
        #region properties

        this.overriddencreatedon = _customer.CreatedDate;
        this.companyid = _customer.companyid;
        this.kti_socialchannelorigin = CRM.Helper.ChannelOrigin.OptionSet_octopos;

        if (!string.IsNullOrWhiteSpace(_customer.kti_sourceid))
        {
            this.kti_sourceid = _customer.kti_sourceid;
        }

        if (!string.IsNullOrWhiteSpace(_customer.firstname))
        {
            this.firstname = _customer.firstname;
        }

        if (!string.IsNullOrWhiteSpace(_customer.lastname))
        {
            this.lastname = _customer.lastname;
        }
        //this.gendercode = _customer.gendercode != null ? _customer.gendercode : 0;

        if (!string.IsNullOrWhiteSpace(_customer.salutation))
        {
            this.salutation = _customer.salutation;
        }

        if (!string.IsNullOrWhiteSpace(_customer.birthdate))
        {
            this.birthdate = _customer.birthdate;
        }

        if (!string.IsNullOrWhiteSpace(_customer.Email))
        {
            this.emailaddress1 = _customer.Email.ToLower();
        }

        if (!string.IsNullOrWhiteSpace(_customer.Address1))
        {
            this.address1_line1 = _customer.Address1;
        }

        if (!string.IsNullOrWhiteSpace(_customer.Address2))
        {
            this.address1_line2 = _customer.Address2;
        }

        if (!string.IsNullOrWhiteSpace(_customer.Address3))
        {
            this.address1_city = _customer.Address3;
        }

        if (!string.IsNullOrWhiteSpace(_customer.PostalCode))
        {
            this.address1_postalcode = _customer.PostalCode;
        }

        if (!string.IsNullOrWhiteSpace(_customer.HandPhone))
        {
            this.address1_telephone1 = _customer.HandPhone;
            this.telephone1 = _customer.HandPhone;
            this.mobilephone = _customer.HandPhone.FormatPhoneNumber();
        }

        if (!string.IsNullOrWhiteSpace(_customer.HomePhone))
        {
            this.address1_telephone2 = _customer.HomePhone;
            this.telephone2 = _customer.HomePhone;
        }

        if (!string.IsNullOrWhiteSpace(_customer.OfficePhone))
        {
            this.address1_telephone3 = _customer.OfficePhone;
            this.telephone3 = _customer.OfficePhone;
        }

        if (!string.IsNullOrWhiteSpace(_customer.ShippingCity))
        {
            this.address2_city = _customer.ShippingCity;
        }

        if (!string.IsNullOrWhiteSpace(_customer.ShippingAddress))
        {
            this.address2_line1 = _customer.ShippingAddress;
        }

        if (!string.IsNullOrWhiteSpace(_customer.ShippingAddress2))
        {
            this.address2_line2 = _customer.ShippingAddress2;
        }

        if (!string.IsNullOrWhiteSpace(_customer.ShippingPostalCode))
        {
            this.address2_postalcode = _customer.ShippingPostalCode;
        }

        if (!string.IsNullOrWhiteSpace(_customer.ShippingContact))
        {
            this.address2_telephone1 = _customer.ShippingContact;
        }

        //this.country = _customer.country;
        //  this.ncci_customerjoineddate = _customer.customerjoineddate;
        //  this.ncci_clubmembershipid = _customer.MembershipCode;

        //this.MooExternalId = _customer.MooExternalId;
        //this.moosourcesystem = _customer.moosourcesystem;
        //  this.ncci_customerjoinedbranch = _customer.Location;

        #endregion
    }

    public Customer(KTI.Moo.Extensions.Lazada.Model.Customer _customer)
    {
        #region properties

        this.companyid = _customer.companyid;
        this.kti_socialchannelorigin = CRM.Helper.ChannelOrigin.OptionSet_lazada;

        if (!string.IsNullOrWhiteSpace(_customer.kti_sourceid))
        {
            this.kti_sourceid = _customer.kti_sourceid;
        }

        //Firstname in Lazada is Fullname

        if (!string.IsNullOrWhiteSpace(_customer.firstname))
        {
            var fullname = _customer.firstname.Split(' ').ToList();

            if (fullname.Count == 1)
            {
                this.lastname = fullname.FirstOrDefault();
            }

            if (fullname.Count > 1)
            {
                this.lastname = fullname.LastOrDefault();
                var lastindex = fullname.Count - 1;
                fullname.RemoveAt(lastindex);

                this.firstname = string.Join(" ", fullname.ToArray());
            }

        }

        MapLazadaAddressToCRMAddress(_customer);

        #endregion
    }

    #region NCCI_Properties

    public DateTime ncci_customerjoineddate { get; set; }
    public string ncci_clubmembershipid { get; set; }
    [JsonProperty(PropertyName = "ncci_customerjoinedbranch@odata.bind")]
    public string ncci_customerjoinedbranch { get; set; }
    public string kti_sapcardcode { get; set; }

    #endregion

    private void MapMagentoAddressToCRMAddress(KTI.Moo.Extensions.Magento.Model.Customer _customer)
    {
        //validate customer address if for mapping
        if (_customer.address is null || _customer.address.Count <= 0)
        {
            return;//skip mapping address
        }

        var address = _customer.address.FirstOrDefault();

        if (_customer.address.Where(adrs => adrs.address_id.ToString() == _customer.default_billing).Any())
        {
            address = _customer.address.Where(adrs => adrs.address_id.ToString() == _customer.default_billing).FirstOrDefault();
        }

        if (!string.IsNullOrWhiteSpace(address.address_line1))
        {
            this.address1_line1 = address.address_line1;
        }

        if (!string.IsNullOrWhiteSpace(address.address_line2))
        {
            this.address1_line2 = address.address_line2;
        }

        if (!string.IsNullOrWhiteSpace(address.address_line3))
        {
            this.address1_line3 = address.address_line3;
        }

        if (!string.IsNullOrWhiteSpace(address.address_city))
        {
            this.address1_city = address.address_city;
        }

        if (!string.IsNullOrWhiteSpace(address.address_postalcode))
        {
            this.address1_postalcode = address.address_postalcode;
        }

        if (!string.IsNullOrWhiteSpace(address.telephone))
        {
            this.address1_telephone1 = address.telephone;
            this.telephone1 = address.telephone;

            if (string.IsNullOrWhiteSpace(this.mobilephone))
            {
                this.mobilephone = address.telephone.FormatPhoneNumber();
            }
        }

        if (address.region is not null && !string.IsNullOrWhiteSpace(address.region.region_name))
        {
            this.address1_stateorprovince = address.region.region_name;
        }

        if (!string.IsNullOrWhiteSpace(address.address_country))
        {
            this.address1_country = address.address_country;
        }

        var addressShipping = address;

        if (_customer.address.Where(adrs => adrs.address_id.ToString() == _customer.default_shipping).Any())
        {
            addressShipping = _customer.address.Where(adrs => adrs.address_id.ToString() == _customer.default_shipping).FirstOrDefault();
        }

        if (!string.IsNullOrWhiteSpace(addressShipping.address_line1))
        {
            this.address2_line1 = addressShipping.address_line1;
        }

        if (!string.IsNullOrWhiteSpace(addressShipping.address_line2))
        {
            this.address2_line2 = addressShipping.address_line2;
        }

        if (!string.IsNullOrWhiteSpace(addressShipping.address_line3))
        {
            this.address2_line3 = addressShipping.address_line3;
        }

        if (!string.IsNullOrWhiteSpace(addressShipping.address_city))
        {
            this.address2_city = addressShipping.address_city;
        }

        if (!string.IsNullOrWhiteSpace(addressShipping.address_postalcode))
        {
            this.address2_postalcode = addressShipping.address_postalcode;
        }

        if (addressShipping.region is not null && !string.IsNullOrWhiteSpace(addressShipping.region.region_name))
        {
            this.address2_stateorprovince = addressShipping.region.region_name;
        }

        if (!string.IsNullOrWhiteSpace(addressShipping.address_country))
        {
            this.address2_country = addressShipping.address_country;
        }

        if (!string.IsNullOrWhiteSpace(addressShipping.telephone))
        {
            this.address2_telephone1 = addressShipping.telephone;
            this.telephone2 = addressShipping.telephone;
        }
    }

    private void MapLazadaAddressToCRMAddress(KTI.Moo.Extensions.Lazada.Model.Customer _customer)
    {
        //validate customer address if for mapping
        if (_customer.address is null || _customer.address.Count <= 0)
        {
            return;//skip mapping address
        }

        var address = _customer.address.FirstOrDefault();

        if (_customer.address.Where(adrs => adrs.default_billing).Any())
        {
            address = _customer.address.Where(adrs => adrs.default_billing).FirstOrDefault();
        }

        if (!string.IsNullOrWhiteSpace(address.address_line1))
        {
            this.address1_line1 = address.address_line1;
        }

        if (!string.IsNullOrWhiteSpace(address.address_line2))
        {
            this.address1_line2 = address.address_line2;
        }

        if (!string.IsNullOrWhiteSpace(address.address_line3))
        {
            this.address1_line3 = address.address_line3;
        }

        if (!string.IsNullOrWhiteSpace(address.address_city))
        {
            this.address1_city = address.address_city;
        }

        if (!string.IsNullOrWhiteSpace(address.address_postalcode))
        {
            this.address1_postalcode = address.address_postalcode;
        }

        if (!string.IsNullOrWhiteSpace(address.address_stateorprovince))
        {
            this.address1_stateorprovince = address.address_stateorprovince;
        }

        if (!string.IsNullOrWhiteSpace(address.address_country))
        {
            this.address1_country = address.address_country;
        }

        if (!string.IsNullOrWhiteSpace(_customer.mobilephone))
        {
            this.mobilephone = _customer.mobilephone.FormatPhoneNumber();
            this.telephone1 = mobilephone;
        }

        if (address.telephone is not null && address.telephone.Count > 0)
        {
            this.address1_telephone1 = address.telephone.FirstOrDefault().telephone;
        }

        var addressShipping = address;

        if (_customer.address.Where(adrs => adrs.default_shipping).Any())
        {
            addressShipping = _customer.address.Where(adrs => adrs.default_shipping).FirstOrDefault();
        }

        if (!string.IsNullOrWhiteSpace(addressShipping.address_line1))
        {
            this.address2_line1 = addressShipping.address_line1;
        }

        if (!string.IsNullOrWhiteSpace(addressShipping.address_line2))
        {
            this.address2_line2 = addressShipping.address_line2;
        }

        if (!string.IsNullOrWhiteSpace(addressShipping.address_line3))
        {
            this.address2_line3 = addressShipping.address_line3;
        }

        if (!string.IsNullOrWhiteSpace(addressShipping.address_city))
        {
            this.address2_city = addressShipping.address_city;
        }

        if (!string.IsNullOrWhiteSpace(addressShipping.address_postalcode))
        {
            this.address2_postalcode = addressShipping.address_postalcode;
        }

        if (!string.IsNullOrWhiteSpace(addressShipping.address_stateorprovince))
        {
            this.address1_stateorprovince = addressShipping.address_stateorprovince;
        }

        if (addressShipping.telephone is not null && addressShipping.telephone.Count > 0)
        {
            this.address2_telephone1 = addressShipping.telephone.FirstOrDefault().telephone;
            this.telephone2 = addressShipping.telephone.FirstOrDefault().telephone;
        }

    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Model.KTIdev.Receivers;

public class Customer : CRM.Model.CustomerBase
{

    public Customer()
    {

    }

    public Customer(KTI.Moo.Extensions.Magento.Model.Customer _customer)
    {
        #region properties

        this.companyid = _customer.companyid;
        this.kti_sapcardcode = _customer.kti_sapbpcode;
        // this.accountrolecode = _customer.accountrolecode;


        if (_customer.address is not null && _customer.address.Count > 0)
        {
            var address = _customer.address.FirstOrDefault();

            if (_customer.address.Where(adrs => adrs.address_id.ToString() == _customer.default_billing).Any())
            {

                address = _customer.address.Where(adrs => adrs.address_id.ToString() == _customer.default_billing).FirstOrDefault();

            }

            this.address1_line1 = address.address_line1;
            this.address1_line2 = address.address_line2;
            this.address1_line3 = address.address_line2;
            this.address1_city = address.address_city;
            this.address1_postalcode = address.address_postalcode;
            this.address1_telephone1 = address.telephone;


            this.address1_stateorprovince = address.region.region_name;
            //this.address1_telephone2 = address.HomePhone;
            //this.address1_telephone3 = address.OfficePhone;

            this.telephone1 = address.telephone;
            this.address1_country = address.address_country;

            try
            {

                if (_customer.custom_attributes is not null && _customer.custom_attributes.Any(custom => custom.attribute_code == "mobile_number"))
                {
                    this.mobilephone = _customer.custom_attributes.Where(custom => custom.attribute_code == "mobile_number").FirstOrDefault().value;
                }
            }
            catch
            {


                this.mobilephone = address.telephone;
            }


            var addressShipping = address;

            if (_customer.address.Where(adrs => adrs.address_id.ToString() == _customer.default_shipping).Any())
            {

                addressShipping = _customer.address.Where(adrs => adrs.address_id.ToString() == _customer.default_shipping).FirstOrDefault();

            }

            this.address2_line1 = addressShipping.address_line1;
            this.address2_line2 = addressShipping.address_line2;
            this.address2_line3 = addressShipping.address_line3;
            this.address2_city = addressShipping.address_city;
            this.address2_postalcode = addressShipping.address_postalcode;
            this.address2_telephone1 = addressShipping.telephone;
            this.telephone2 = addressShipping.telephone;

        }

        this.birthdate = _customer.birthdate;
        this.emailaddress1 = _customer.email;
        this.firstname = _customer.firstname;
        this.lastname = _customer.lastname;
        this.salutation = _customer.salutation;

        //this.ncci_customerjoineddate = _customer.customerjoineddate;
        //this.ncci_clubmembershipid = _customer.MembershipCode;
        //this.ncci_customerjoinedbranch = _customer.Location;

        var Magentochannelorigin = 959080010;
        this.kti_socialchannelorigin = Magentochannelorigin;
        this.kti_sourceid = _customer.kti_sourceid;

        #endregion
    }


    public Customer(KTI.Moo.Extensions.OctoPOS.Model.Customer _customer)
    {
        #region properties

        this.companyid = _customer.companyid;
        this.kti_sapcardcode = _customer.kti_sapbpcode;
        this.accountrolecode = _customer.accountrolecode;
        this.address1_line1 = _customer.Address1;
        this.address1_line2 = _customer.Address2;
        this.address1_city = _customer.Address3;
        this.address1_postalcode = _customer.PostalCode;
        this.address1_telephone1 = _customer.HandPhone;
        this.address1_telephone2 = _customer.HomePhone;
        this.address1_telephone3 = _customer.OfficePhone;
        this.address2_city = _customer.ShippingAddress;
        this.address2_line1 = _customer.ShippingAddress2;
        this.address2_line2 = _customer.ShippingCity;
        this.address2_postalcode = _customer.ShippingPostalCode;
        this.address2_telephone1 = _customer.ShippingContact;
        this.birthdate = _customer.birthdate;
        this.emailaddress1 = _customer.Email;
        this.firstname = _customer.firstname;
        //this.gendercode = _customer.gendercode != null ? _customer.gendercode : 0;
        this.lastname = _customer.lastname;
        this.mobilephone = _customer.HandPhone;
        this.salutation = _customer.salutation;
        this.telephone1 = _customer.HandPhone;
        this.telephone2 = _customer.HomePhone;
        this.telephone3 = _customer.OfficePhone;
        this.country = _customer.country;
        //  this.ncci_customerjoineddate = _customer.customerjoineddate;
        //  this.ncci_clubmembershipid = _customer.MembershipCode;


        this.MooExternalId = _customer.MooExternalId;
        this.moosourcesystem = _customer.moosourcesystem;
        //  this.ncci_customerjoinedbranch = _customer.Location;

        var OCTOPOSchannelorigin = 959080011;
        this.kti_socialchannelorigin = OCTOPOSchannelorigin;
        this.kti_sourceid = _customer.kti_sourceid;

        #endregion
    }


    public Customer(KTI.Moo.Extensions.Lazada.Model.Customer _customer)
    {
        #region properties

        this.companyid = _customer.companyid;


        //Firstname in Lazada is Fullname

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

        if (_customer.address is not null && _customer.address.Count > 0)
        {



            var address = _customer.address.FirstOrDefault();

            if (_customer.address.Where(adrs => adrs.default_billing).Any())
            {

                address = _customer.address.Where(adrs => adrs.default_billing).FirstOrDefault();

            }

            this.address1_line1 = address.address_line1;
            this.address1_line2 = address.address_line2;
            this.address1_line3 = address.address_line2;
            this.address1_city = address.address_city;
            this.address1_postalcode = address.address_postalcode;
            this.address1_stateorprovince = address.address_stateorprovince;
            this.address1_country = address.address_country;


            this.mobilephone = _customer.mobilephone;
            this.telephone1 = mobilephone;


            if (address.telephone is not null && address.telephone.Count > 0)
            {
                this.address1_telephone1 = address.telephone.FirstOrDefault().telephone;
            }


            var addressShipping = address;

            if (_customer.address.Where(adrs => adrs.default_shipping).Any())
            {
                addressShipping = _customer.address.Where(adrs => adrs.default_shipping).FirstOrDefault();
            }

            this.address2_line1 = addressShipping.address_line1;
            this.address2_line2 = addressShipping.address_line2;
            this.address2_line3 = addressShipping.address_line3;
            this.address2_city = addressShipping.address_city;
            this.address2_postalcode = addressShipping.address_postalcode;
            this.address1_stateorprovince = addressShipping.address_stateorprovince;
            this.emailaddress1 = _customer.laz_emailaddress;

            if (addressShipping.telephone is not null && addressShipping.telephone.Count > 0)
            {
                this.address2_telephone1 = addressShipping.telephone.FirstOrDefault().telephone;
                this.telephone2 = addressShipping.telephone.FirstOrDefault().telephone;
            }




        }

        this.kti_socialchannelorigin = CRM.Helper.ChannelOrigin.OptionSet_lazada;
        this.kti_sourceid = _customer.kti_sourceid;

        #endregion
    }




    #region NCCI_Properties

    public DateTime ncci_customerjoineddate { get; set; }
    public string ncci_clubmembershipid { get; set; }
    [JsonProperty(PropertyName = "ncci_customerjoinedbranch@odata.bind")]
    public string ncci_customerjoinedbranch { get; set; }
    public string kti_sapcardcode { get; set; }

    #endregion


}

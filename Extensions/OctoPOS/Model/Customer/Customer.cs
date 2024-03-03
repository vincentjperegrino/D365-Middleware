using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Linq;
using KTI.Moo.Extensions.OctoPOS.Helper;

namespace KTI.Moo.Extensions.OctoPOS.Model;

public class Customer : CustomerBase
{

    private DateTime _CreatedDate;
    private DateTime _ModifiedDate;

    [JsonIgnore]
    public override int companyid { get; set; }


    [JsonIgnore]
    public override string moosourcesystem { get; set; } = "OCTOPOS";

    [JsonProperty("CustomerCode")]
    public string CustomerCode { get; set; }

    [JsonIgnore]
    public override string kti_sourceid { get => CustomerCode; }

    [JsonIgnore]
    public override string employeeid
    {
        get
        {
            if (string.IsNullOrWhiteSpace(base.employeeid))
            {
                return CustomerCode;
            }

            return base.employeeid;
        }

        set => base.employeeid = value;
    }

    [JsonIgnore]
    public List<Address> Address { get; set; }

    [JsonIgnore]
    public List<EmailAddress> EmailAddress { get; set; }

    [JsonProperty("Address1")]
    public string Address1
    {
        get => Address.Where(details => details.address_name == AddressTypeHelper.billingAddressName).FirstOrDefault().address_line1;
        set => Address.Where(details => details.address_name == AddressTypeHelper.billingAddressName).FirstOrDefault().address_line1 = value;
    }

    [JsonProperty("Address2")]
    public string Address2
    {
        get => Address.Where(details => details.address_name == AddressTypeHelper.billingAddressName).FirstOrDefault().address_line2;
        set => Address.Where(details => details.address_name == AddressTypeHelper.billingAddressName).FirstOrDefault().address_line2 = value;
    }

    [JsonProperty("Address3")]
    public string Address3
    {
        get => Address.Where(details => details.address_name == AddressTypeHelper.billingAddressName).FirstOrDefault().address_line3;
        set
        {
            var addressModel = Address.Where(details => details.address_name == AddressTypeHelper.billingAddressName).FirstOrDefault();

            addressModel.address_line3 = value;

            if (string.IsNullOrWhiteSpace(addressModel.address_city))
            {
                addressModel.address_city = value;
            }

        }
    }

    [JsonProperty("Address4")]
    public string Address4
    {
        get => Address.Where(details => details.address_name == AddressTypeHelper.billingAddressName).FirstOrDefault().address_line4;
        set => Address.Where(details => details.address_name == AddressTypeHelper.billingAddressName).FirstOrDefault().address_line4 = value;
    }

    [JsonProperty("Country")]
    public string Country
    {
        get => Address.Where(details => details.address_name == AddressTypeHelper.billingAddressName).FirstOrDefault().address_country;
        set => Address.Where(details => details.address_name == AddressTypeHelper.billingAddressName).FirstOrDefault().address_country = value;
    }

    [JsonProperty("PostalCode")]
    public string PostalCode
    {
        get => Address.Where(details => details.address_name == AddressTypeHelper.billingAddressName).FirstOrDefault().address_postalcode;
        set => Address.Where(details => details.address_name == AddressTypeHelper.billingAddressName).FirstOrDefault().address_postalcode = value;
    }


    [JsonProperty("HandPhone")]
    public string HandPhone
    {
        get => GetPhone(AddressType: AddressTypeHelper.billingAddressName, TelephoneType: TelephoneTypeHelpher.HandPhoneType);

        set
        {
            AddPhone(AddressType: AddressTypeHelper.billingAddressName
                     , TelephoneType: TelephoneTypeHelpher.HandPhoneType
                     , isPrimary: true
                     , PhoneValue: value);

        }

    }

    [JsonProperty("HomePhone")]
    public string HomePhone
    {
        get => GetPhone(AddressType: AddressTypeHelper.billingAddressName, TelephoneTypeHelpher.HomePhoneType);
        set
        {
            AddPhone(AddressType: AddressTypeHelper.billingAddressName
                     , TelephoneType: TelephoneTypeHelpher.HomePhoneType
                     , isPrimary: false
                     , PhoneValue: value);

        }


    }

    [JsonProperty("OfficePhone")]
    public string OfficePhone
    {
        get => GetPhone(AddressType: AddressTypeHelper.billingAddressName, TelephoneTypeHelpher.OfficePhoneType);

        set
        {
            AddPhone(AddressType: AddressTypeHelper.billingAddressName
                     , TelephoneType: TelephoneTypeHelpher.OfficePhoneType
                     , isPrimary: false
                     , PhoneValue: value);

        }


    }


    [JsonProperty("CreatedDate")]
    public DateTime CreatedDate
    {
        get => _CreatedDate;
        set => _CreatedDate = value != default ? Helper.DateTimeHelper.PHT_to_UTC(value) : value;
    }

    [JsonProperty("Dob")]
    public DateTime Dob { get; set; } = Helper.DateTimeHelper.PHTnow();

    [JsonIgnore]
    public override string birthdate { get => Dob.ToString("yyyy-MM-dd"); }

    [JsonProperty("Email")]
    public string Email
    {

        get
        {
            if (EmailAddress.Count > 0)
            {
                return EmailAddress.Where(item => item.primary == true).Select(item => item.emailaddress).FirstOrDefault().ToString();
            }

            return null;
        }

        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                EmailAddress.Add(new Model.EmailAddress()
                {
                    primary = true,
                    emailaddress = value
                });

            }
        }
    }


    [JsonProperty("ExpiryDate")]
    public DateTime ExpiryDate { get; set; }

    [JsonProperty("JoinDate")]
    public DateTime JoinDate { get; set; }

    [JsonIgnore]
    public override DateTime customerjoineddate { get => Helper.DateTimeHelper.PHT_to_UTC(JoinDate); }

    [JsonProperty("Name")]
    public override string firstname
    {
        get => base.firstname;
        set
        {
            base.firstname = value;
            Address.Where(details => details.address_name == AddressTypeHelper.billingAddressName).FirstOrDefault().first_name = value;
            Address.Where(details => details.address_name == AddressTypeHelper.shippingAddressName).FirstOrDefault().first_name = value;
        }
    }

    [JsonProperty("Nationality")]
    public string Nationality { get; set; }

    [JsonProperty("IDNumber")]
    public string IDNumber { get; set; }

    [JsonProperty("NRIC")]
    public string NRIC { get; set; }


    [JsonProperty("Remarks")]
    public string Remarks { get; set; }

    [JsonProperty("Sex")]
    public string Sex { get; set; }


    [JsonIgnore]
    [Range(1, 2)]
    public override int gendercode
    {
        get
        {
            if (Sex == GenderHelper.MaleType)
            {

                return 1;
            }

            if (Sex == GenderHelper.FemaleType)
            {

                return 2;
            }

            throw new System.Exception("Invalid Gender");

        }
        set
        {

            if (value == 1)
            {
                Sex = GenderHelper.MaleType;
                return;
            }

            if (value == 2)
            {
                Sex = GenderHelper.FemaleType;
                return;
            }

            throw new System.Exception("Invalid Gender");
        }
    }

    [JsonProperty("ShippingAddress")]
    public string ShippingAddress
    {
        get => Address.Where(details => details.address_name == AddressTypeHelper.shippingAddressName).FirstOrDefault().address_line1;
        set => Address.Where(details => details.address_name == AddressTypeHelper.shippingAddressName).FirstOrDefault().address_line1 = value;
    }


    [JsonProperty("ShippingAddress2")]
    public string ShippingAddress2
    {
        get => Address.Where(details => details.address_name == AddressTypeHelper.shippingAddressName).FirstOrDefault().address_line2;
        set => Address.Where(details => details.address_name == AddressTypeHelper.shippingAddressName).FirstOrDefault().address_line2 = value;
    }


    [JsonProperty("ShippingCity")]
    public string ShippingCity
    {
        get => Address.Where(details => details.address_name == AddressTypeHelper.shippingAddressName).FirstOrDefault().address_city;

        set
        {
            var addressModel = Address.Where(details => details.address_name == AddressTypeHelper.shippingAddressName).FirstOrDefault();

            addressModel.address_city = value;

            if (string.IsNullOrWhiteSpace(addressModel.address_line3))
            {
                addressModel.address_line3 = value;
            }

        }
    }

    [JsonProperty("ShippingPostalCode")]
    public string ShippingPostalCode
    {
        get => Address.Where(details => details.address_name == AddressTypeHelper.shippingAddressName).FirstOrDefault().address_postalcode;
        set => Address.Where(details => details.address_name == AddressTypeHelper.shippingAddressName).FirstOrDefault().address_postalcode = value;
    }

    [JsonProperty("ShippingCountry")]
    public string ShippingCountry
    {
        get => Address.Where(details => details.address_name == AddressTypeHelper.shippingAddressName).FirstOrDefault().address_country;
        set => Address.Where(details => details.address_name == AddressTypeHelper.shippingAddressName).FirstOrDefault().address_country = value;
    }

    [JsonProperty("ShippingContact")]
    public string ShippingContact
    {

        get => GetPhone(AddressType: AddressTypeHelper.shippingAddressName, TelephoneTypeHelpher.HandPhoneType);

        set => AddPhone(AddressType: AddressTypeHelper.shippingAddressName
                      , TelephoneType: TelephoneTypeHelpher.HandPhoneType
                      , isPrimary: true
                      , PhoneValue: value);

    }



    [JsonProperty("Status")]
    public int Status { get; set; }

    [JsonProperty("Company")]
    public string Company { get; set; }


    [JsonProperty("Location")]
    public string Location { get; set; }

    [JsonProperty("Surname")]
    public override string lastname
    {
        get => base.lastname;
        set
        {
            base.lastname = value;
            Address.Where(details => details.address_name == AddressTypeHelper.billingAddressName).FirstOrDefault().last_name = value;
            Address.Where(details => details.address_name == AddressTypeHelper.shippingAddressName).FirstOrDefault().last_name = value;
        }
    }

    [StringLength(100)]
    [JsonProperty("Salutation")]
    public override string salutation { get; set; }

    [JsonProperty("CustomerPoints")]
    public double CustomerPoints { get; set; }

    [JsonProperty("ModifiedDate")]
    public DateTime ModifiedDate
    {
        get => _ModifiedDate;
        set => _ModifiedDate = value != default ? Helper.DateTimeHelper.PHT_to_UTC(value) : value;
    }

    [JsonProperty("Field1")]
    public string Field1 { get; set; }

    [JsonProperty("Field2")]
    public string Field2 { get; set; }

    [JsonProperty("MembershipCode")]
    public string MembershipCode { get; set; }

    [JsonProperty("SyncApp")]
    public string SyncApp { get; set; }

    [JsonProperty("SyncCustomerId")]
    public string SyncCustomerId { get; set; }

    [JsonProperty("InvoiceStatus")]
    public int InvoiceStatus { get; set; }

    [JsonProperty("Race")]
    public string Race { get; set; }

    [JsonProperty("MaritalStatus")]
    public int MaritalStatus { get; set; }

    [JsonProperty("Designation")]
    public string Designation { get; set; }

    [JsonProperty("Occupation")]
    public int Occupation { get; set; }

    [JsonProperty("CustomerUuid")]
    public string CustomerUuid { get; set; }

    [JsonProperty("CustomerType")]
    public List<CustomerType> CustomerType { get; set; }

    [JsonProperty("RewardPoint")]
    public double RewardPoint { get; set; }

    [JsonProperty("Username")]
    public string Username { get; set; }

    [JsonProperty("IncomeGroup")]
    public int IncomeGroup { get; set; }

    [JsonProperty("ContactedWhatsapp")]
    public bool ContactedWhatsapp { get; set; }

    [JsonProperty("ContactedTelegram")]
    public bool ContactedTelegram { get; set; }

    [JsonProperty("ContactedWechat")]
    public bool ContactedWechat { get; set; }

    [JsonProperty("ContactedLine")]
    public bool ContactedLine { get; set; }

    [JsonProperty("Userlogin")]
    public string Userlogin { get; set; }

    [JsonProperty("RegisterType")]
    public string RegisterType { get; set; }

    [JsonProperty("VipMediaValue")]
    public double? VipMediaValue { get; set; }

    [JsonProperty("CustomNumber")]
    public string CustomNumber { get; set; }


    [JsonProperty("Password")]
    public string Password { get; set; }


    public Customer()
    {
        Address = new();
        EmailAddress = new();

        Address.Add(new()
        {
            address_name = AddressTypeHelper.billingAddressName
        });

        Address.Add(new()
        {
            address_name = AddressTypeHelper.shippingAddressName
        });
    }



    private void AddPhone(string AddressType, string TelephoneType, bool isPrimary, string PhoneValue)
    {


        var AddressName = AddressType;
        var addressModel = Address.Where(details => details.address_name == AddressName).FirstOrDefault();
        var PhoneType = TelephoneType;
        var telephoneModel = addressModel.Telephone.Where(details => details.telephone_type == PhoneType).FirstOrDefault();
        telephoneModel.telephone = PhoneValue;

        if (isPrimary)
        {
            telephoneModel.primary = true;

        }

    }

    private string GetPhone(string AddressType, string TelephoneType)
    {

        var AddressName = AddressType;

        var addressModel = Address.Where(details => details.address_name == AddressName).FirstOrDefault();

        var PhoneType = TelephoneType;

        var telephoneModel = addressModel.Telephone.Where(details => details.telephone_type == PhoneType).FirstOrDefault();

        return telephoneModel.telephone;


    }



}


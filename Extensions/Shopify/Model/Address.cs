using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class Address : KTI.Moo.Extensions.Core.Model.AddressBase
{

    public Address()
    {

    }

    public Address(ShopifySharp.Address address)
    {



        Id = address.Id ?? default;
        FirstName = address.FirstName;
        LastName = address.LastName;
        Company = address.Company;
        Address1 = address.Address1;
        Address2 = address.Address2;
        City = address.City;
        Province = address.Province;
        Country = address.Country;
        Zip = address.Zip;
        Phone = address.Phone;
        ProvinceCode = address.ProvinceCode;
        CountryCode = address.CountryCode;
        CountryName = address.CountryName;
        Default = address.Default ?? default;

        //added for billingAddress

        name = address.Name;
        latitude = address.Latitude ?? default;
        longitude = address.Longitude ?? default;




    }
    public long Id { get; set; }
    
    public string FirstName { get; set; }
    public override string first_name { get => FirstName; set => FirstName = value; }
    public string LastName { get; set; }
    public override string last_name { get => LastName; set => LastName = value; }
    public string Company { get; set; }
    public string Address1 { get; set; }
    public override string address_line1 { get => Address1; set => Address1 = value; }
    public string Address2 { get; set; }
    public override string address_line2 { get => Address1; set => Address1 = value; }
    public string City { get; set; }
    public override string address_city { get => Address1; set => Address1 = value; }
    public string Province { get; set; }
    public override string address_stateorprovince { get => Province; set => Province = value; }
    public string Country { get; set; }
    public override string address_country { get => Country; set => Country = value; }
    public string Zip { get; set; }
    public string Phone { get; set; }
    public string ProvinceCode { get; set; }
    public string CountryCode { get; set; }
    public string CountryName { get; set; }
    public bool Default { get; set; }


    public string name { get; set; }
    public decimal latitude { get; set; }
    public decimal longitude { get; set; }
}

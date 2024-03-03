using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class Address : ShopifySharp.Address
    {
        public Address()
        {

        }

        public Address(Model.Address address)
        {
            //Id = address.Id;
            //CustomerId = adddress.CustomerId;
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
            Default = address.Default;
         
        }

        //public long CustomerId { get; set; }


    }

}

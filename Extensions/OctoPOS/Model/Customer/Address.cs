using KTI.Moo.Extensions.Core.Model;
using KTI.Moo.Extensions.OctoPOS.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.OctoPOS.Model
{
    public class Address : AddressBase
    {

        public string address_line4 { get; set; }

        [JsonIgnore]
        public List<Telephone> Telephone { get; set; }

        public Address() {

            Telephone = new();

            Telephone.Add( new()
            {
                telephone_type = TelephoneTypeHelpher.HandPhoneType

            });

            Telephone.Add(new()
            {
                telephone_type = TelephoneTypeHelpher.HomePhoneType

            });

            Telephone.Add(new()
            {
                telephone_type = TelephoneTypeHelpher.OfficePhoneType

            });

        }
    }



}

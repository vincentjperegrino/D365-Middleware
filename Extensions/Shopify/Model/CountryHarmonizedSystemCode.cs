using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model
{


    public class CountryHarmonizedSystemCode

    {

        public CountryHarmonizedSystemCode()

        { 
        
        }


        public CountryHarmonizedSystemCode(ShopifySharp.HSCode countryHarmonizedSystemCode)
        {

            harmonized_system_code = countryHarmonizedSystemCode.HarmonizedSystemCode;

            country_code = countryHarmonizedSystemCode.CountryCode;


        }


        public string harmonized_system_code { get; set; }
        public string country_code { get; set; }


    }
}

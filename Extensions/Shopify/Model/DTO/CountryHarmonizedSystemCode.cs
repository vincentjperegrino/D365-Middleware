using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class CountryHarmonizedsystemCode : ShopifySharp.HSCode
    {
        public CountryHarmonizedsystemCode()
        {

        }

        public CountryHarmonizedsystemCode(Model.CountryHarmonizedSystemCode countryHarmonizedSystemCode)
        {

            HarmonizedSystemCode = countryHarmonizedSystemCode.harmonized_system_code;


            CountryCode = countryHarmonizedSystemCode.country_code;



        }
    }
}

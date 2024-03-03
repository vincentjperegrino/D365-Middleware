using System;
using Xunit;
using KTI.Moo.Extensions.Magento.Domain;
using KTI.Moo.Extensions.Magento.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using KTI.Moo.Extensions.Magento.Service;
using TestMagento.Model;
using Country = KTI.Moo.Extensions.Magento.Domain.Country;

namespace TestMagento.Domain
{
    public class Countries : MagentoBase
    {

        private static MagentoService _MagentoService = new(defaultURL);

        Country _countriesDomain = new(_MagentoService);


        [Fact]
            
        public void IFWorkingApiCallofGetCountrylist()
        {
 
           var response =  _countriesDomain.GetCountries();
       
            Assert.IsAssignableFrom<List<KTI.Moo.Extensions.Magento.Model.Country>>(response);
            
        }

        [Fact]
        public void IFWorkingApiCallofGetRegion()
        {
            string SelectedRegion = "PH";

            var response =  _countriesDomain.GetCountry(SelectedRegion);

            Assert.IsAssignableFrom<Country>(response);

        }




    }
}

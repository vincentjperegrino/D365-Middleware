using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Magento.Model;
using KTI.Moo.Extensions.Magento.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Magento.Domain
{
    public class Country : ICountries<Model.Country>
    {
        private IMagentoService _service { get; init; }
        public const string APIDirectory = "/directory/countries/";

        public Country(Config config)
        {
            this._service = new MagentoService(config);
        }


        public Country(string defaultDomain)
        {
            this._service = new MagentoService(defaultDomain);
        }

        public Country(IMagentoService service)
        {
            this._service = service;
        }

        public List<Model.Country> GetCountries()
        {
            try
            {
                var response = _service.ApiCall(APIDirectory);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var _countryList = JsonConvert.DeserializeObject<List<Model.Country>>(response, settings);

                return _countryList;
            }
            catch
            {
                return new List<Model.Country>();
            }
        }


        public Model.Country GetCountry(string CountryID)
        {
            if (string.IsNullOrWhiteSpace(CountryID))
            {
                throw new ArgumentException("Invalid CountryID", nameof(CountryID));
            }

            try
            {
                var response = _service.ApiCall(APIDirectory + CountryID);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var _countryModel = JsonConvert.DeserializeObject<Model.Country>(response, settings);

                return _countryModel;
            }
            catch
            {
                return new Model.Country();
            }
        }


        public string GetRegionID(string region_name, string CountryCode = "PH")
        {

            if (string.IsNullOrWhiteSpace(region_name))
            {
                return default;
            }

            var _countryModel = GetCountry(CountryCode);

            if (AvailableRegionCountValid(_countryModel))
            {

                var RegionID = _countryModel.available_regions.Where(region => region.region_name.Contains(region_name)).FirstOrDefault().region_id;

                return RegionID;
            }

            return default;
        }

        private static bool AvailableRegionCountValid(Model.Country _countryModel)
        {
            return _countryModel.available_regions.Count > 0;
        }

        public string GetCountryID(string CountryName)
        {

            if (string.IsNullOrWhiteSpace(CountryName))
            {
                return "PH";
            }

            var _countryList = GetCountries();

            if (CountryListCountValid(_countryList))
            {

                if (_countryList.Where(country => country.full_name_english.ToUpper() == CountryName.ToUpper()).Any())
                {
                    var CountryID = _countryList.Where(country => country.full_name_english.ToUpper() == CountryName.ToUpper()).FirstOrDefault().country_id;
                    return CountryID;

                }


            }

            return "PH";

        }

        private static bool CountryListCountValid(List<Model.Country> _countryList)
        {
            return _countryList.Count > 0;
        }
    }
}

using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Magento.Model;
using KTI.Moo.Extensions.Magento.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using KTI.Moo.Extensions.Magento.Helper;
using Microsoft.Extensions.Caching.Distributed;

namespace KTI.Moo.Extensions.Magento.Domain
{
    internal sealed class ClientToken
    {
        private readonly IMagentoService _service;
        public const string APIDirectory = "/integration/admin/token";

        private readonly string _defaultDomain;
        private readonly string _username;
        private readonly string _password;
        private readonly string _redisConnectionString;
        private readonly IDistributedCache _cache;

        public ClientToken(Config config)
        {
            this._service = new MagentoService(config);
            _redisConnectionString = config.redisConnectionString;
            _defaultDomain = config.defaultURL;
            _username = config.username;
            _password = config.password;
        }

        public ClientToken(IDistributedCache cache, string defaultDomain, string redisConnectionString, string username, string password)
        {
            _cache = cache;
            this._service = new MagentoService(defaultDomain);
            _redisConnectionString = redisConnectionString;
            _defaultDomain = defaultDomain;
            _username = username;
            _password = password;
        }

        public ClientToken(string defaultDomain, string redisConnectionString, string username, string password)
        {
            this._service = new MagentoService(defaultDomain);
            _redisConnectionString = redisConnectionString;
            _defaultDomain = defaultDomain;
            _username = username;
            _password = password;
        }

        public ClientToken(IMagentoService service)
        {
            _service = service;
        }


        public Model.ClientTokens Get()
        {

            //return GetToken(_username, _password);

            CheckerValidUserNameAndPassword(_username, _password);

            string hashDomainUsernamePassword = Helper.Encryption.GetHashString(_defaultDomain + _username + _password);

            // fetch tokens from redis cache
            string tokensJson = _cache.GetString($"Magento_{hashDomainUsernamePassword}");

            if (string.IsNullOrWhiteSpace(tokensJson))
            {
                return GetTokenCached(_username, _password);
            }

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var returnToken = JsonConvert.DeserializeObject<ClientTokens>(tokensJson, settings);

            if (DateTime.UtcNow >= returnToken.AccessExpiration)
            {
                return GetTokenCached(_username, _password);
            }

            return returnToken;
        }


        public void Retry()
        {
            GetTokenCached(_username, _password);
        }

        private Model.ClientTokens GetTokenCached(string username, string password)
        {

            var returnToken = GetToken(username, password);

            string hashDomainUsernamePassword = Helper.Encryption.GetHashString(_defaultDomain + username + password);

            _cache.SetString($"Magento_{hashDomainUsernamePassword}", JsonConvert.SerializeObject(returnToken));

            return returnToken;
        }



        private Model.ClientTokens GetToken(string username, string password)
        {

            TokenParameters content = new();
            content.username = username;
            content.password = password;


            var stringContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

            bool isAuthenticated = false;

            string response = _service.ApiCall(APIDirectory, "POST", stringContent, isAuthenticated);

            Model.ClientTokens returnToken = new();

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            returnToken.AccessToken = JsonConvert.DeserializeObject(response, settings).ToString();

            int NumberOfHoursTokenExpiration = 4;
            returnToken.AccessExpiration = DateTime.UtcNow.AddHours(NumberOfHoursTokenExpiration);


            return returnToken;

        }

        private bool CheckerValidUserNameAndPassword(string username, string password)
        {

            string NameSpace = "KTI.Moo.Extensions.Magento.Domain";
            string className = "ClientToken";
            string methodName = "Get";


            if (string.IsNullOrWhiteSpace(username))
            {

                throw new ArgumentNullException(username, $"{NameSpace}.{className}.{methodName}:  Username is invalid");

            }

            if (string.IsNullOrWhiteSpace(password))
            {

                throw new ArgumentNullException(password, $"{NameSpace}.{className}.{methodName}:  Password is invalid");
            }

            return true;
        }














    }
}

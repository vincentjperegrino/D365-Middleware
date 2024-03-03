using KTI.Moo.Extensions.OctoPOS.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using KTI.Moo.Extensions.OctoPOS.Helper;
using KTI.Moo.Extensions.OctoPOS.Model;
using Microsoft.Extensions.Caching.Distributed;

namespace KTI.Moo.Extensions.OctoPOS.Domain
{
    internal sealed class ClientToken
    {

        private readonly IOctoPOSService _service;
        public const string APIDirectory = "/systemAuth";

        private readonly string _defaultDomain;
        private readonly string _username;
        private readonly string _password;
        private readonly string _redisConnectionString;
        private readonly string _ApiAuth;

        private readonly IDistributedCache _cache;


        public ClientToken(IDistributedCache cache, string defaultDomain, string username, string password, string ApiAuth)
        {
            _cache = cache;
            this._service = new OctoPOSService(cache, defaultDomain, username, password, ApiAuth);
            _defaultDomain = defaultDomain;
            _username = username;
            _password = password;
            _ApiAuth = ApiAuth;
        }

        public ClientToken(IOctoPOSService service)
        {
            _service = service;
        }


        public Model.ClientTokens Get()
        {
            CheckerValid_UserName_And_Password_AND_ApiAuth(_username, _password, _ApiAuth);


            string hashDomainUsernamePassword = Helper.Encryption.GetHashString(_defaultDomain + _username + _password + _ApiAuth);

            // fetch tokens from redis cache
            string tokensJson = _cache.GetString($"OctoPOS_{hashDomainUsernamePassword}");

            if (string.IsNullOrWhiteSpace(tokensJson))
            {
                return GetToken(_username, _password, _ApiAuth);
            }

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var returnToken = JsonConvert.DeserializeObject<ClientTokens>(tokensJson, settings);

            if (DateTime.UtcNow >= returnToken.AccessExpiration)
            {
                return GetToken(_username, _password, _ApiAuth);
            }

            return returnToken;


        }

        public void retry()
        {
            GetToken(_username, _password, _ApiAuth);
        }

        private Model.ClientTokens GetToken(string username, string password, string ApiAuth)
        {

            TokenParameters content = new();
            content.username = username;
            content.password = password;


            var stringContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

            bool isAuthenticated = false;

            string response = _service.ApiCall(APIDirectory, "POST", stringContent, isAuthenticated);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            Model.ClientTokens returnToken = JsonConvert.DeserializeObject<ClientTokens>(response, settings);

            int NumberOfHoursTokenExpiration = 24;
            returnToken.AccessExpiration = DateTime.UtcNow.AddHours(NumberOfHoursTokenExpiration);


            string hashDomainUsernamePassword = Helper.Encryption.GetHashString(_defaultDomain + username + password + ApiAuth);

            _cache.SetString($"OctoPOS_{hashDomainUsernamePassword}", JsonConvert.SerializeObject(returnToken));


            return returnToken;

        }




        private bool CheckerValid_UserName_And_Password_AND_ApiAuth(string username, string password, string ApiAuth)
        {

            string NameSpace = "KTI.Moo.Extensions.OctoPOS.Domain";
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



            if (string.IsNullOrWhiteSpace(ApiAuth))
            {

                throw new ArgumentNullException(ApiAuth, $"{NameSpace}.{className}.{methodName}:  ApiAuth is invalid");
            }





            return true;



        }

    }
}

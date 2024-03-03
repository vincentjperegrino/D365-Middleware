using KTI.Moo.Extensions.Magento.Domain;
using KTI.Moo.Extensions.Magento.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


using System.Runtime.CompilerServices;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("TestMagento")]

namespace KTI.Moo.Extensions.Magento.Service
{

    internal sealed class MagentoService : IMagentoService
    {


        //private readonly string _defaultDomain = "https://novateurdev.argomall.ph/rest/default/V1";

        private readonly string _defaultURL;
        private readonly string _username;
        private readonly string _password;
        private readonly string _redisConnectionString;

        private readonly IDistributedCache _cache;
        private readonly ILogger _log;

        public string DefaultURL => _defaultURL;

        public string RegionDomain { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public MagentoService(Config config)
        {
            _defaultURL = config.defaultURL;
            _redisConnectionString = config.redisConnectionString;
            _username = config.username;
            _password = config.password;
        }


        public MagentoService(Config config, IDistributedCache cache)
        {
            _defaultURL = config.defaultURL;
            _redisConnectionString = config.redisConnectionString;
            _username = config.username;
            _password = config.password;
            _cache = cache;
        }

        public MagentoService(Config config, IDistributedCache cache, ILogger log)
        {
            _defaultURL = config.defaultURL;
            _redisConnectionString = config.redisConnectionString;
            _username = config.username;
            _password = config.password;
            _cache = cache;
            _log = log;
        }

        public MagentoService(string defaultDomain)
        {
            _defaultURL = defaultDomain;
        }

        public MagentoService(string defaultDomain, string redisConnectionString, string username, string password)
        {
            _defaultURL = defaultDomain;
            _redisConnectionString = redisConnectionString;
            _username = username;
            _password = password;
        }


        public string ApiCall(string path)
        {
            string method = "GET";
            return ApiCall(path, method, null, null);
        }


        public string ApiCall(string path, string method, bool isAuthenticated)
        {
            if (isAuthenticated)
            {
                return AuthenticatedApiCall(path, method, null, null);
            }

            return ApiCall(path, method, null, null);
        }



        public string ApiCall(string path, string method, HttpContent content, bool isAuthenticated)
        {
            if (isAuthenticated)
            {
                return AuthenticatedApiCall(path, method, null, content);
            }

            return ApiCall(path, method, null, content);
        }



        public string ApiCall(string path, string method, Dictionary<string, string> parameters, bool isAuthenticated)
        {
            if (isAuthenticated)
            {
                return AuthenticatedApiCall(path, method, parameters, null);
            }
            return ApiCall(path, method, parameters, null);
        }


        public string ApiCall(string path, string method, Dictionary<string, string> parameters, HttpContent content, bool isAuthenticated)
        {
            if (isAuthenticated)
            {

                return AuthenticatedApiCall(path, method, parameters, content);
            }

            return ApiCall(path, method, parameters, content);
        }


        private string ApiCall(string path, string method, Dictionary<string, string> parameters, HttpContent content)
        {
            IsMethodForApiCall(method);

            string QueryParameters = "";

            if (parameters is not null && parameters.Count > 0)
            {
                QueryParameters = "?";

                foreach (var param in parameters)
                {

                    QueryParameters += param.Key.ToString() + "=" + param.Value + "&";
                }
            }


            HttpClient httpClient = new();

            httpClient.BaseAddress = new Uri(DefaultURL);
            httpClient.Timeout = new TimeSpan(0, 2, 0);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //Add this line for TLS complaience
            //   ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            if (_log is not null)
            {
                var response = httpClient.GetAsync(@"https://ifconfig.me").GetAwaiter().GetResult();

                var results = response.Content.ReadAsStringAsync().Result;

                _log.LogInformation("{path}, IP: {results} ", path, results);

            }



            if (method == "POST")
            {
                var retrieveResponsePost = httpClient.PostAsync(DefaultURL + path + QueryParameters, content).GetAwaiter().GetResult();

                if (!retrieveResponsePost.IsSuccessStatusCode)
                {
                    throw new System.Exception($"Error {retrieveResponsePost.StatusCode}: {retrieveResponsePost.Content.ReadAsStringAsync().Result}");
                }

                return retrieveResponsePost.Content.ReadAsStringAsync().Result;

            }


            var retrieveResponseGet = httpClient.GetAsync(DefaultURL + path + QueryParameters).GetAwaiter().GetResult();

            if (!retrieveResponseGet.IsSuccessStatusCode)
            {
                throw new System.Exception($"Error {retrieveResponseGet.StatusCode}: {retrieveResponseGet.Content.ReadAsStringAsync().Result}");
            }


            return retrieveResponseGet.Content.ReadAsStringAsync().Result;


        }


        private string AuthenticatedApiCall(string path, string method, Dictionary<string, string> parameters, HttpContent content, int retryCount = 0)
        {

            int MAXretryCount = 3;

            CanUseAuthenticatedApiCall(_username, _password);

            IsMethodForAuthenticatedApiCall(method);

            ClientToken _tokenDomain;
            _tokenDomain = new(_cache, _defaultURL, _redisConnectionString, _username, _password);

            var clientToken = _tokenDomain.Get();

            string QueryParameters = "";

            if (parameters is not null && parameters.Count > 0)
            {
                QueryParameters = "?";

                foreach (var param in parameters)
                {

                    QueryParameters += param.Key.ToString() + "=" + param.Value + "&";
                }
            }

            HttpClient httpClient = new();

            httpClient.BaseAddress = new Uri(DefaultURL);
            httpClient.Timeout = new TimeSpan(0, 2, 0);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", clientToken.AccessToken);


            if (_log is not null)
            {
                var response = httpClient.GetAsync(@"https://ifconfig.me").GetAwaiter().GetResult();

                var results = response.Content.ReadAsStringAsync().Result;

                _log.LogInformation("{path}, IP: {results} ", path, results);

            }

            if (method == "POST")
            {
                var retrieveResponsePost = httpClient.PostAsync(DefaultURL + path + QueryParameters, content).GetAwaiter().GetResult();

                if (retrieveResponsePost.StatusCode == HttpStatusCode.Unauthorized && retryCount <= MAXretryCount)
                {
                    _tokenDomain.Retry();
                    return AuthenticatedApiCall(path, method, parameters, content, ++retryCount);

                }

                if (!retrieveResponsePost.IsSuccessStatusCode)
                {
                    throw new System.Exception($"Error {retrieveResponsePost.StatusCode}: {retrieveResponsePost.Content.ReadAsStringAsync().Result}");
                }

                return retrieveResponsePost.Content.ReadAsStringAsync().Result;


            }

            if (method == "PUT")
            {
                var retrieveResponsePut = httpClient.PutAsync(DefaultURL + path + QueryParameters, content).GetAwaiter().GetResult();

                if (retrieveResponsePut.StatusCode == HttpStatusCode.Unauthorized && retryCount <= MAXretryCount)
                {
                    _tokenDomain.Retry();
                    return AuthenticatedApiCall(path, method, parameters, content, ++retryCount);

                }

                if (!retrieveResponsePut.IsSuccessStatusCode)
                {
                    throw new System.Exception($"Error {retrieveResponsePut.StatusCode}: {retrieveResponsePut.Content.ReadAsStringAsync().Result}");
                }

                return retrieveResponsePut.Content.ReadAsStringAsync().Result;


            }

            if (method == "DEL")
            {
                var retrieveResponseDel = httpClient.DeleteAsync(DefaultURL + path + QueryParameters).GetAwaiter().GetResult();

                if (retrieveResponseDel.StatusCode == HttpStatusCode.Unauthorized && retryCount <= MAXretryCount)
                {
                    _tokenDomain.Retry();
                    return AuthenticatedApiCall(path, method, parameters, content, ++retryCount);
                }

                if (!retrieveResponseDel.IsSuccessStatusCode)
                {
                    throw new System.Exception($"Error {retrieveResponseDel.StatusCode}: {retrieveResponseDel.Content.ReadAsStringAsync().Result}");
                }


                return retrieveResponseDel.Content.ReadAsStringAsync().Result;
            }

            var retrieveResponseGet = httpClient.GetAsync(DefaultURL + path + QueryParameters).GetAwaiter().GetResult();

            if (retrieveResponseGet.StatusCode == HttpStatusCode.Unauthorized && retryCount <= MAXretryCount)
            {
                _tokenDomain.Retry();
                return AuthenticatedApiCall(path, method, parameters, content, ++retryCount);

            }

            if (!retrieveResponseGet.IsSuccessStatusCode)
            {
                throw new System.Exception($"Error {retrieveResponseGet.StatusCode}: {retrieveResponseGet.Content.ReadAsStringAsync().Result}");
            }

            return retrieveResponseGet.Content.ReadAsStringAsync().Result;
        }




        private bool CanUseAuthenticatedApiCall(string username, string password)
        {
            string NameSpace = "KTI.Moo.Extensions.Magento.Service";
            string className = "MagentoService";
            string methodName = "ApiCall";

            if (string.IsNullOrWhiteSpace(username))
            {

                throw new ArgumentNullException(username, $"{NameSpace}.{className}.{methodName}: Username is invalid");

            }

            if (string.IsNullOrWhiteSpace(password))
            {

                throw new ArgumentNullException(password, $"{NameSpace}.{className}.{methodName}: Password is invalid");
            }


            return true;

        }

        private bool IsMethodForApiCall(string method)
        {
            string NameSpace = "KTI.Moo.Extensions.Magento.Service";
            string className = "MagentoService";
            string methodName = "ApiCall";


            if (method == "GET")
            {
                return true;
            }

            if (method == "POST")
            {
                return true;
            }


            throw new ArgumentOutOfRangeException(nameof(method), $"{NameSpace}.{className}.{methodName}: {method} is not invalid method for API call. Available method GET and POST");



        }

        private bool IsMethodForAuthenticatedApiCall(string method)
        {
            string NameSpace = "KTI.Moo.Extensions.Magento.Service";
            string className = "MagentoService";
            string methodName = "AuthenticatedApiCall";



            if (method == "GET")
            {
                return true;
            }

            if (method == "POST")
            {
                return true;
            }

            if (method == "PUT")
            {
                return true;
            }

            if (method == "DEL")
            {
                return true;
            }


            throw new ArgumentOutOfRangeException(nameof(method), $"{NameSpace}.{className}.{methodName}: {method} is not invalid method for authenticated API call. Available method GET, POST, PUT, and DEL");



        }


        public string AuthenticatedApiCall(string path, Dictionary<string, string> parameters, string method, bool useRegionDomain = true)
        {
            throw new NotImplementedException();
        }

        public string ApiCall(string path, Dictionary<string, string> parameters, string method, bool useRegionDomain = true)
        {
            throw new NotImplementedException();
        }


    }
}

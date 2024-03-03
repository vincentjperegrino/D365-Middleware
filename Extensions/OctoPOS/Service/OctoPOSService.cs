using KTI.Moo.Extensions.OctoPOS.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

using System.Runtime.CompilerServices;
using Microsoft.Extensions.Caching.Distributed;

[assembly: InternalsVisibleTo("TestOctoPOS")]

namespace KTI.Moo.Extensions.OctoPOS.Service;

internal sealed class OctoPOSService : IOctoPOSService
{
    //private readonly string _defaultDomain = "http://202.148.162.33:8080/NESPRESSOTEST/api.orm/1.0";

    private readonly string _defaultDomain;
    private readonly string _username;
    private readonly string _password;
    private readonly string _redisConnectionString;
    private readonly string _ApiAuth;

    private readonly IDistributedCache _cache;

    public string DefaultURL => _defaultDomain;

    public string RegionDomain { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public OctoPOSService(Config config)
    {
        _redisConnectionString = config.redisConnectionString;
        _defaultDomain = config.defaultURL;
        _username = config.username;
        _password = config.password;
        _ApiAuth = config.apiAuth;
    }

    public OctoPOSService(IDistributedCache cache, Config config)
    {
        _redisConnectionString = config.redisConnectionString;
        _defaultDomain = config.defaultURL;
        _username = config.username;
        _password = config.password;
        _ApiAuth = config.apiAuth;
        _cache = cache;
    }
    public OctoPOSService(IDistributedCache cache, string defaultDomain, string username, string password, string ApiAuth)
    {
        _defaultDomain = defaultDomain;
        _username = username;
        _password = password;
        _ApiAuth = ApiAuth;
        _cache = cache;
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
        httpClient.DefaultRequestHeaders.Add("ApiAuth", _ApiAuth);
        // httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //Add this line for TLS complaience
        //   ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;


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

        CanUseAuthenticatedApiCall(_username, _password, _ApiAuth);

        IsMethodForAuthenticatedApiCall(method);

        ClientToken _tokenDomain = new(_cache, _defaultDomain, _username, _password, _ApiAuth);

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
        httpClient.DefaultRequestHeaders.Add("ApiAuth", _ApiAuth);
        httpClient.DefaultRequestHeaders.Add("Token", clientToken.AccessToken);


        //  httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", clientToken.AccessToken);


        if (method == "POST")
        {
            var retrieveResponsePost = httpClient.PostAsync(DefaultURL + path + QueryParameters, content).GetAwaiter().GetResult();


            var responsePOST = retrieveResponsePost.Content.ReadAsStringAsync().Result;


            if (retrieveResponsePost.IsSuccessStatusCode == false && responsePOST.Contains("reason=Unauthorized") && retryCount <= MAXretryCount)
            {
                _tokenDomain.retry();
                return AuthenticatedApiCall(path, method, parameters, content, ++retryCount);

            }

            if (!retrieveResponsePost.IsSuccessStatusCode)
            {
                throw new System.Exception($"Error {retrieveResponsePost.StatusCode}: {retrieveResponsePost.Content.ReadAsStringAsync().Result}");
            }

            return retrieveResponsePost.Content.ReadAsStringAsync().Result;

        }

        //if (method == "PUT")
        //{


        //    var retrieveResponsePut = httpClient.PutAsync(DefaultDomain + path + QueryParameters, content).GetAwaiter().GetResult();

        //    if (retrieveResponsePut.IsSuccessStatusCode == false && retryCount <= MAXretryCount)
        //    {
        //        _tokenDomain.retry();
        //        return AuthenticatedApiCall(path, method, parameters, content, ++retryCount);

        //    }

        //    return retrieveResponsePut.Content.ReadAsStringAsync().Result;


        //}

        //if (method == "DEL")
        //{
        //    var retrieveResponseDel = httpClient.DeleteAsync(DefaultDomain + path + QueryParameters).GetAwaiter().GetResult();

        //    if (retrieveResponseDel.IsSuccessStatusCode == false && retryCount <= MAXretryCount)
        //    {
        //        _tokenDomain.retry();
        //        return AuthenticatedApiCall(path, method, parameters, content, ++retryCount);

        //    }

        //    return retrieveResponseDel.Content.ReadAsStringAsync().Result;


        //}

        //GET
        var retrieveResponseGet = httpClient.GetAsync(DefaultURL + path + QueryParameters).GetAwaiter().GetResult();

        var response = retrieveResponseGet.Content.ReadAsStringAsync().Result;


        if (retrieveResponseGet.IsSuccessStatusCode == false && response.Contains("reason=Unauthorized") && retryCount <= MAXretryCount)
        {
            _tokenDomain.retry();
            return AuthenticatedApiCall(path, method, parameters, content, ++retryCount);

        }

        if (!retrieveResponseGet.IsSuccessStatusCode)
        {
            throw new System.Exception($"Error {retrieveResponseGet.StatusCode}: {retrieveResponseGet.Content.ReadAsStringAsync().Result}");
        }

        return retrieveResponseGet.Content.ReadAsStringAsync().Result;
    }

    private bool IsMethodForApiCall(string method)
    {
        string NameSpace = "KTI.Moo.Extensions.OctoPOS.Service";
        string className = "OctoPOSService";
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
        string NameSpace = "KTI.Moo.Extensions.OctoPOS.Service";
        string className = "OctoPOSService";
        string methodName = "AuthenticatedApiCall";



        if (method == "GET")
        {
            return true;
        }

        if (method == "POST")
        {
            return true;
        }

        //if (method == "PUT")
        //{
        //    return true;
        //}

        //if (method == "DEL")
        //{
        //    return true;
        //}


        throw new ArgumentOutOfRangeException(nameof(method), $"{NameSpace}.{className}.{methodName}: {method} is not invalid method for authenticated API call. Available method GET and POST");



    }

    private bool CanUseAuthenticatedApiCall(string username, string password, string ApiAuth)
    {
        string NameSpace = "KTI.Moo.Extensions.OctoPOS.Service";
        string className = "OctoPOSService";
        string methodName = "ApiCall";

        if (string.IsNullOrWhiteSpace(username))
        {

            throw new ArgumentNullException(nameof(username), $"{NameSpace}.{className}.{methodName}: Username is invalid");

        }

        if (string.IsNullOrWhiteSpace(password))
        {

            throw new ArgumentNullException(nameof(password), $"{NameSpace}.{className}.{methodName}: Password is invalid");
        }


        if (string.IsNullOrWhiteSpace(ApiAuth))
        {

            throw new ArgumentNullException(nameof(ApiAuth), $"{NameSpace}.{className}.{methodName}: ApiAuth is invalid");
        }

        return true;

    }






    public string ApiCall(string path, Dictionary<string, string> parameters, string method, bool useRegionDomain = true)
    {
        throw new System.NotImplementedException();
    }

    public string AuthenticatedApiCall(string path, Dictionary<string, string> parameters, string method, bool useRegionDomain = true)
    {
        throw new System.NotImplementedException();
    }
}

using KTI.Moo.Extensions.SAP.Domain;
using KTI.Moo.Extensions.SAP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.SAP.Service;

internal sealed class SAPService : ISAPService
{
    private readonly string _defaultURL;
    private readonly string _username;
    private readonly string _password;
    private readonly string _companydb;
    private readonly string _redisConnectionString;

    //HttpClientHandler _clientHandler = new HttpClientHandler()
    //{
    //    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
    //};


    public string DefaultURL => _defaultURL;

    public string RegionDomain { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public SAPService(Config config)
    {
        _defaultURL = config.defaultURL;
        _redisConnectionString = config.redisConnectionString;
        _username = config.username;
        _password = config.password;
        _companydb = config.companyDB;
    }

    public SAPService(string defaultDomain)
    {

        _defaultURL = defaultDomain;

    }

    public SAPService(string defaultDomain, string redisConnectionString, string username, string password, string companydb)
    {
        _defaultURL = defaultDomain;
        _redisConnectionString = redisConnectionString;
        _username = username;
        _password = password;
        _companydb = companydb;
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


        //HttpClient httpClient = new(_clientHandler);
        HttpClient httpClient = new();

        httpClient.BaseAddress = new Uri(DefaultURL);
        httpClient.Timeout = new TimeSpan(0, 2, 0);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //Add this line for TLS complaience
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;







        if (method == "POST")
        {
            var retrieveResponsePost = httpClient.PostAsync(DefaultURL + path + QueryParameters, content).GetAwaiter().GetResult();


            return retrieveResponsePost.Content.ReadAsStringAsync().Result;


        }


        var retrieveResponseGet = httpClient.GetAsync(DefaultURL + path + QueryParameters).GetAwaiter().GetResult();



        return retrieveResponseGet.Content.ReadAsStringAsync().Result;


    }


    private string AuthenticatedApiCall(string path, string method, Dictionary<string, string> parameters, HttpContent content, int retryCount = 0)
    {

        int MAXretryCount = 3;

        CanUseAuthenticatedApiCall(_username, _password);

        IsMethodForAuthenticatedApiCall(method);

        ClientToken _tokenDomain;
        _tokenDomain = new(_defaultURL, _redisConnectionString, _username, _password, _companydb);


        // _tokenDomain.Retry();

        string QueryParameters = "";

        if (parameters is not null && parameters.Count > 0)
        {
            QueryParameters = "?";

            foreach (var param in parameters)
            {

                QueryParameters += param.Key.ToString() + "=" + param.Value + "&";
            }
        }

        //HttpClient httpClient = new(_clientHandler);
        HttpClient httpClient = new();

        httpClient.BaseAddress = new Uri(DefaultURL);
        httpClient.Timeout = new TimeSpan(0, 2, 0);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //   httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "a6af5c56-0a67-11ed-8000-00155d6d0102");

        TokenParameters tokenparam = new();
        tokenparam.UserName = _username;
        tokenparam.Password = _password;
        tokenparam.CompanyDB = _companydb;

        var stringContent = new StringContent(JsonConvert.SerializeObject(tokenparam), Encoding.UTF8, "application/json");
        var TokenretrieveResponsePost = httpClient.PostAsync(DefaultURL + "/Login", stringContent).GetAwaiter().GetResult();


        if (method == "PATCH")
        {
            var retrieveResponsePost = httpClient.PatchAsync(DefaultURL + path + QueryParameters, content).GetAwaiter().GetResult();

            if (retrieveResponsePost.StatusCode == HttpStatusCode.Unauthorized && retryCount <= MAXretryCount)
            {
                _tokenDomain.Retry();
                return AuthenticatedApiCall(path, method, parameters, content, ++retryCount);

            }

            return retrieveResponsePost.Content.ReadAsStringAsync().Result;


        }


        if (method == "POST")
        {
            var retrieveResponsePost = httpClient.PostAsync(DefaultURL + path + QueryParameters, content).GetAwaiter().GetResult();

            if (retrieveResponsePost.StatusCode == HttpStatusCode.Unauthorized && retryCount <= MAXretryCount)
            {
                _tokenDomain.Retry();
                return AuthenticatedApiCall(path, method, parameters, content, ++retryCount);

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

            return retrieveResponseDel.Content.ReadAsStringAsync().Result;


        }

        var retrieveResponseGet = httpClient.GetAsync(DefaultURL + path + QueryParameters).GetAwaiter().GetResult();

        if (retrieveResponseGet.StatusCode == HttpStatusCode.Unauthorized && retryCount <= MAXretryCount)
        {
            _tokenDomain.Retry();
            return AuthenticatedApiCall(path, method, parameters, content, ++retryCount);

        }

        return retrieveResponseGet.Content.ReadAsStringAsync().Result;
    }




    private bool CanUseAuthenticatedApiCall(string username, string password)
    {
        string NameSpace = "KTI.Moo.Extensions.SAP.Service";
        string className = "SAP";
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
        string NameSpace = "KTI.Moo.Extensions.SAP.Service";
        string className = "SAPService";
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
        string NameSpace = "KTI.Moo.Extensions.SAP.Service";
        string className = "SAPService";
        string methodName = "AuthenticatedApiCall";

        if (method == "PATCH")
        {
            return true;
        }

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

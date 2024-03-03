using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Service;

internal sealed class ShopifyAdminService : IShopifyService
{

    private readonly string _defaultURL;

    private readonly Config _config;


    public string DefaultURL => _defaultURL;

    public string RegionDomain { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public ShopifyAdminService(Config config)
    {
        _defaultURL = config.defaultURL;
        _config = config;
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


        HttpClient httpClient = new()
        {
            BaseAddress = new Uri(DefaultURL),
            Timeout = new TimeSpan(0, 2, 0)
        };
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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


    private string AuthenticatedApiCall(string path, string method, Dictionary<string, string> parameters, HttpContent content)
    {

        IsMethodForAuthenticatedApiCall(method);

        string QueryParameters = "";

        if (parameters is not null && parameters.Count > 0)
        {
            QueryParameters = "?";

            foreach (var param in parameters)
            {

                QueryParameters += param.Key.ToString() + "=" + param.Value + "&";
            }
        }

        HttpClient httpClient = new()
        {
            BaseAddress = new Uri(DefaultURL),
            Timeout = new TimeSpan(0, 2, 0)
        };

        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("X-Shopify-Access-Token", _config.admintoken);


        if (method == "POST")
        {
            var retrieveResponsePost = httpClient.PostAsync(DefaultURL + path + QueryParameters, content).GetAwaiter().GetResult();

            if (!retrieveResponsePost.IsSuccessStatusCode)
            {
                throw new System.Exception($"Error {retrieveResponsePost.StatusCode}: {retrieveResponsePost.Content.ReadAsStringAsync().Result}");
            }

            return retrieveResponsePost.Content.ReadAsStringAsync().Result;


        }

        if (method == "PUT")
        {
            var retrieveResponsePut = httpClient.PutAsync(DefaultURL + path + QueryParameters, content).GetAwaiter().GetResult();

            if (!retrieveResponsePut.IsSuccessStatusCode)
            {
                throw new System.Exception($"Error {retrieveResponsePut.StatusCode}: {retrieveResponsePut.Content.ReadAsStringAsync().Result}");
            }

            return retrieveResponsePut.Content.ReadAsStringAsync().Result;


        }

        if (method == "DEL")
        {
            var retrieveResponseDel = httpClient.DeleteAsync(DefaultURL + path + QueryParameters).GetAwaiter().GetResult();

            if (!retrieveResponseDel.IsSuccessStatusCode)
            {
                throw new System.Exception($"Error {retrieveResponseDel.StatusCode}: {retrieveResponseDel.Content.ReadAsStringAsync().Result}");
            }


            return retrieveResponseDel.Content.ReadAsStringAsync().Result;
        }

        var retrieveResponseGet = httpClient.GetAsync(DefaultURL + path + QueryParameters).GetAwaiter().GetResult();

        if (!retrieveResponseGet.IsSuccessStatusCode)
        {
            throw new System.Exception($"Error {retrieveResponseGet.StatusCode}: {retrieveResponseGet.Content.ReadAsStringAsync().Result}");
        }

        return retrieveResponseGet.Content.ReadAsStringAsync().Result;
    }



    private bool IsMethodForApiCall(string method)
    {
        string NameSpace = "KTI.Moo.Extensions.Shopify.Service";
        string className = "ShopifyAdminService";
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
        string NameSpace = "KTI.Moo.Extensions.Shopify.Service";
        string className = "ShopifyAdminService";
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

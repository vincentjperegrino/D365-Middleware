using KTI.Moo.Extensions.SAP.Model;
using KTI.Moo.Extensions.SAP.Service;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.SAP.Domain;

public class ClientToken
{
    private readonly ISAPService _service;
    public const string APIDirectory = "/Login";

    private readonly string _defaultDomain;
    private readonly string _username;
    private readonly string _password;
    private readonly string _companydb;
    private readonly string _redisConnectionString;


    public ClientToken(Config config)
    {
        this._service = new SAPService(config);
        _redisConnectionString = config.redisConnectionString;
        _defaultDomain = config.defaultURL;
        _username = config.username;
        _password = config.password;
        _companydb = config.companyDB;
    }


    public ClientToken(string defaultDomain, string redisConnectionString, string username, string password, string companydb)
    {
        this._service = new SAPService(defaultDomain);
        _redisConnectionString = redisConnectionString;
        _defaultDomain = defaultDomain;
        _username = username;
        _password = password;
        _companydb = companydb;
    }

    public ClientToken(ISAPService service)
    {
        _service = service;
    }

    public Model.ClientTokens Get()
    {
        CheckerValidUserNameAndPassword(_username, _password, _companydb);

        IDatabase _cache;

        _cache = ConnectionMultiplexer.Connect(_redisConnectionString).GetDatabase();

        string hashDomainUsernamePassword = Helper.Encryption.GetHashString( _username + _password + _companydb);

        // fetch tokens from redis cache
        string tokensJson;
        if (_cache.KeyExists($"SAP_{hashDomainUsernamePassword}"))
        {
            tokensJson = _cache.StringGet($"SAP_{hashDomainUsernamePassword}");

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var returnToken = JsonConvert.DeserializeObject<ClientTokens>(tokensJson, settings);

            if (DateTime.UtcNow >= returnToken.AccessExpiration)
            {
                return GetToken(_username, _password, _companydb, _redisConnectionString);

            }

            return returnToken;
        }

        return GetToken(_username, _password, _companydb, _redisConnectionString);
    }


    public void Retry()
    {
        GetToken(_username, _password, _companydb, _redisConnectionString);
    }



    private Model.ClientTokens GetToken(string username, string password, string companydb, string redisConnectionString)
    {

        IDatabase _cache;

        _cache = ConnectionMultiplexer.Connect(redisConnectionString).GetDatabase();


        TokenParameters content = new();
        content.UserName = username;
        content.Password = password;
        content.CompanyDB = companydb;


        var stringContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

        bool isAuthenticated = false;

        string response = _service.ApiCall(APIDirectory, "POST", stringContent, isAuthenticated);

        Model.ClientTokens returnToken = new();



        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };


        var TokenResponse = JsonConvert.DeserializeObject<dynamic>(response, settings);
        returnToken.AccessToken = TokenResponse.SessionId;
        returnToken.AccessExpiration = DateTime.UtcNow.AddMinutes(Convert.ToUInt32(TokenResponse.SessionTimeout));





        string hashDomainUsernamePassword = Helper.Encryption.GetHashString(username + password + companydb);

        _cache.StringSet($"SAP_{hashDomainUsernamePassword}", JsonConvert.SerializeObject(returnToken));


        return returnToken;
    }



    private bool CheckerValidUserNameAndPassword(string username, string password, string companydb)
    {

        string NameSpace = "KTI.Moo.Extensions.SAP.Domain";
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

        if (string.IsNullOrWhiteSpace(companydb))
        {

            throw new ArgumentNullException(companydb, $"{NameSpace}.{className}.{methodName}:  companydb is invalid");
        }



        return true;



    }




}

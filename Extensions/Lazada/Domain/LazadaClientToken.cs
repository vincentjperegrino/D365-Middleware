using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Lazada.Exception;
using KTI.Moo.Extensions.Lazada.Model;
using KTI.Moo.Extensions.Lazada.Service;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace KTI.Moo.Extensions.Lazada.Domain;
public class ClientToken : IClientToken_Oauth2<ClientTokens>
{
    private readonly Service.ILazopService _service;

    public ClientToken(ILazopService service)
    {
        this._service = service;
    }

    public ClientToken(string defaultURL, string AppKey, string AppSecret, string Region)
    {
        _service = new LazopService(defaultURL, AppKey, AppSecret, Region);
    }

    public ClientToken(Service.Queue.Config config)
    {
        _service = new LazopService(config);
    }


    public ClientTokens Create(string authorization)
    {
        // var uuid = Enumerable.Repeat("1234567890", 11).Select(c => c[new Random().Next(c.Length)]).ToString();
        var parameters = new Dictionary<string, string>
        {
            {"code", authorization}
            // {"uuid", uuid},
        };

        var response = this._service.ApiCall("/auth/token/create", parameters, "POST", false);
        var responseJson = JsonDocument.Parse(response).RootElement;

        var tokens = JsonSerializer.Deserialize<ClientTokens>(response, this._service.serializerOptions);
        tokens.AccessExpiration = DateTime.UtcNow.AddSeconds(responseJson.GetProperty("expires_in").GetDouble());
        tokens.RefreshExpiration = DateTime.UtcNow.AddSeconds(responseJson.GetProperty("refresh_expires_in").GetDouble());

        return tokens;
    }


    public ClientTokens Refresh(ClientTokens clientTokens)
    {
        if (clientTokens is null || clientTokens.RefreshToken is null)
        {
            throw new NullTokensException();
        }

        if (clientTokens.RefreshExpiration < DateTime.UtcNow)
        {
            throw new TokensExpiredException();
        }


        return RefreshToken(clientTokens.RefreshToken);


    }

    private ClientTokens RefreshToken(string RefreshToken)
    {
        var parameters = new Dictionary<string, string>
        {
            {"refresh_token", RefreshToken}
        };

        var response = this._service.ApiCall("/auth/token/refresh", parameters, "POST", false);
        var responseJson = JsonDocument.Parse(response).RootElement;

        var newTokens = JsonSerializer.Deserialize<ClientTokens>(response, this._service.serializerOptions);
        newTokens.AccessExpiration = DateTime.UtcNow.AddSeconds(responseJson.GetProperty("expires_in").GetDouble());
        newTokens.RefreshExpiration = DateTime.UtcNow.AddSeconds(responseJson.GetProperty("refresh_expires_in").GetDouble());

        return newTokens;
    }

}

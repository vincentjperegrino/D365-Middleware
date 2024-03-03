using System;
using System.Collections.Generic;
using System.Text.Json;
using KTI.Moo.Extensions.Lazada.Model;
using KTI.Moo.Extensions.Lazada.Service;
using KTI.Moo.Extensions.Lazada.Exception;
using Microsoft.Extensions.Caching.Distributed;

namespace KTI.Moo.Extensions.Lazada.Domain;

public class ClientToken_Cached
{
    private Service.ILazopService _service { get; init; }
    private readonly IDistributedCache _cache;

    public ClientToken_Cached(IDistributedCache cache, string defaultURL, string AppKey, string AppSecret, string Region)
    {
        _cache = cache;
        _service = new LazopService(cache, defaultURL, AppKey, AppSecret, Region);
    }

    public ClientToken_Cached(ILazopService service)
    {
        this._service = service;
    }

    /// <summary>
    /// Fetches the client tokens stored in the Redis cache.
    /// </summary>
    /// <param name="id">The ID of the token pair to get.</param>
    /// <param name="site">The platform and region identifier for the seller, i.e., "lazada_ph"</param>
    /// <returns>A pair of Access and Refresh tokens matching the given Id.</returns>
    public ClientTokens Get(string id, string site)
    {
        // fetch tokens from redis cache
        string tokensJson = _cache.GetString($"{site}_{id}");

        if (string.IsNullOrWhiteSpace(tokensJson))
        {
            throw new NullTokensException(id);
        }

        return JsonSerializer.Deserialize<ClientTokens>(tokensJson);
    }

    /// <summary>
    /// Insert or update client tokens to the cache.
    /// </summary>
    /// <param name="tokens">The tokens to be added.</param>
    public void Add(Lazada.Model.ClientTokens tokens)
    {
        foreach (CountryUserInfo c in tokens.CountryUserInfos)
        {     
            _cache.SetString($"lazada_{c.Country}_{c.SellerID}", JsonSerializer.Serialize(tokens));
        }
    }

    /// <summary>
    /// Calls the respective platform's API to generate new tokens based on an authorization code.
    /// </summary>
    /// <param name="authorization">An authorization code used to generate client tokens.</param>
    /// <returns>A pair of Access and Refresh tokens, along with other related information.</returns>
    public ClientTokens Create(string authorization)
    {
        // var uuid = Enumerable.Repeat("1234567890", 11).Select(c => c[new Random().Next(c.Length)]).ToString();
        var parameters = new Dictionary<string, string>
        {
            {"code", authorization}
            // {"uuid", uuid},
        };

        var response = this._service.ApiCall("/auth/token/create", parameters, "POST", false);
        JsonElement responseJson = JsonDocument.Parse(response).RootElement;

        ClientTokens tokens = JsonSerializer.Deserialize<ClientTokens>(response, this._service.serializerOptions);
        tokens.AccessExpiration = DateTime.UtcNow.AddSeconds(responseJson.GetProperty("expires_in").GetDouble());
        tokens.RefreshExpiration = DateTime.UtcNow.AddSeconds(responseJson.GetProperty("refresh_expires_in").GetDouble());

        // update value in cache
        // temporary: add the token for each country sellerid
        this.Add(tokens);

        return tokens;
    }

    /// <summary>
    /// Calls the respective platform's API to generate new tokens based on the refresh token.
    /// </summary>
    /// <param name="tokens">The tokens to be refreshed.</param>
    /// <returns>A pair of Access and Refresh tokens, along with other related information.</returns>
    public ClientTokens Refresh(ClientTokens tokens)
    {
        var parameters = new Dictionary<string, string>
        {
            {"refresh_token", tokens.RefreshToken}
        };

        var response = this._service.ApiCall("/auth/token/refresh", parameters, "POST", false);
        JsonElement responseJson = JsonDocument.Parse(response).RootElement;

        ClientTokens newTokens = JsonSerializer.Deserialize<ClientTokens>(response, this._service.serializerOptions);
        newTokens.AccessExpiration = DateTime.UtcNow.AddSeconds(responseJson.GetProperty("expires_in").GetDouble());
        newTokens.RefreshExpiration = DateTime.UtcNow.AddSeconds(responseJson.GetProperty("refresh_expires_in").GetDouble());

        // update value in cache
        this._cache.SetString($"lazada_{newTokens.Country}_{newTokens.Id}", JsonSerializer.Serialize(newTokens));

        return newTokens;
    }
}

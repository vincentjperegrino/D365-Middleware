using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Lazada.Domain;
using KTI.Moo.Extensions.Lazada.Exception;
using KTI.Moo.Extensions.Lazada.Model;
using KTI.Moo.Extensions.Lazada.Utils;
using Lazop.Api;
using Microsoft.Extensions.Caching.Distributed;

namespace KTI.Moo.Extensions.Lazada.Service
{
    internal sealed class LazopService : ILazopService
    {
        private string _defaultDomain = "https://api.lazada.com/rest";
        private string _appKey;
        private string _appSecret;
        private string _region;
        private string _sellerId;
        private ClientTokens _tokens;
        public JsonSerializerOptions serializerOptions { get; set; }
        public string DefaultURL => _defaultDomain;
        private readonly IDistributedCache _cache;
        public string AppKey
        {
            get => this._appKey;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    this._appKey = value;
                else
                    throw new ArgumentNullException(nameof(value), "AppKey must not be null.");
            }
        }
        public string AppSecret
        {
            get => this._appSecret;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    this._appSecret = value;
                else
                    throw new ArgumentNullException(nameof(value), "AppSecret must not be null.");
            }
        }
        [RegularExpression(@"id|my|ph|sg|th|vn", ErrorMessage = "Region must be one of 'id,' 'my,' 'ph,' 'sg,' 'th,' or 'vn.'")]
        public string RegionDomain
        {
            get => this._region switch
            {
                "id" => "https://api.lazada.co.id/rest",
                "my" => "https://api.lazada.com.my/rest",
                "ph" => "https://api.lazada.com.ph/rest",
                "sg" => "https://api.lazada.sg/rest",
                "th" => "https://api.lazada.co.th/rest",
                "vn" => "https://api.lazada.vn/rest",
                _ => throw new ArgumentOutOfRangeException(nameof(this._region), $"Unknown value '{this._region}'")
            };
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    this._region = value;
                else
                    throw new ArgumentOutOfRangeException(nameof(value), "Region must be one of 'id,' 'my,' 'ph,' 'sg,' 'th,' or 'vn.'");
            }
        }

        public string SellerId
        {
            get => this._sellerId;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    this._sellerId = value;
                else
                    throw new ArgumentNullException(nameof(value), "Seller ID cannot be null.");
            }
        }

        public Lazada.Model.ClientTokens ClientTokens
        {
            get => this._tokens;
            // throw exception if refresh token is expired
            set => this._tokens = value;
        }


        public LazopService()
        {
            serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            serializerOptions.Converters.Add(new BooleanJsonConverter());
            serializerOptions.Converters.Add(new DateTimeJsonConverter());
        }

        public LazopService(string key, string secret, Model.SellerRegion region)
        {
            serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            serializerOptions.Converters.Add(new BooleanJsonConverter());
            serializerOptions.Converters.Add(new DateTimeJsonConverter());

            AppKey = key;
            AppSecret = secret;
            RegionDomain = region.Region;
            SellerId = region.SellerId;
        }

        public LazopService(string key, string secret, string region, string accessToken = null, string refreshToken = null)
        {
            serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            serializerOptions.Converters.Add(new BooleanJsonConverter());
            serializerOptions.Converters.Add(new DateTimeJsonConverter());

            AppKey = key;
            AppSecret = secret;
            RegionDomain = region;

            this._tokens = new() { AccessToken = accessToken, RefreshToken = refreshToken };
        }


        public LazopService(Service.Queue.Config config, IDistributedCache cache)
        {
            serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            serializerOptions.Converters.Add(new BooleanJsonConverter());
            serializerOptions.Converters.Add(new DateTimeJsonConverter());

            _defaultDomain = config.defaultURL;
            AppKey = config.AppKey;
            AppSecret = config.AppSecret;
            RegionDomain = config.Region;
            SellerId = config.SellerId;
            _cache = cache;
        }

        public LazopService(IDistributedCache cache, string defaultURL, string key, string secret, string region)
        {
            serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            serializerOptions.Converters.Add(new BooleanJsonConverter());
            serializerOptions.Converters.Add(new DateTimeJsonConverter());

            _defaultDomain = defaultURL;
            AppKey = key;
            AppSecret = secret;
            RegionDomain = region;
            _cache = cache;
        }
        



        public LazopService(Service.Queue.Config config)
        {
            serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            serializerOptions.Converters.Add(new BooleanJsonConverter());
            serializerOptions.Converters.Add(new DateTimeJsonConverter());

            _defaultDomain = config.defaultURL;
            AppKey = config.AppKey;
            AppSecret = config.AppSecret;
            RegionDomain = config.Region;
            
        }


        public LazopService(Service.Queue.Config config, ClientTokens clientTokens)
        {
            serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            serializerOptions.Converters.Add(new BooleanJsonConverter());
            serializerOptions.Converters.Add(new DateTimeJsonConverter());

            _defaultDomain = config.defaultURL;
            AppKey = config.AppKey;
            AppSecret = config.AppSecret;
            RegionDomain = config.Region;
            SellerId = config.SellerId;
            _tokens = clientTokens;
        }     
        
        /// <summary>
        /// Check if tokens are expired, and refresh if necessary.
        /// </summary>
        /// <exception cref="TokensExpiredException">Thrown when tokens can't be refreshed.</exception>
        private string _updateTokens()
        {
            var clientdomain = new ClientToken_Cached(_cache, DefaultURL, AppKey, AppSecret, RegionDomain);

            ClientTokens = clientdomain.Get(_sellerId, $"lazada_{_region}");


            if (_tokens is null || _tokens.RefreshToken is null)
            {
                throw new NullTokensException();
            }

            if(_tokens.AccessExpiration < DateTime.UtcNow && _tokens.RefreshExpiration < DateTime.UtcNow)
            {
                throw new TokensExpiredException();
            }

            if(_tokens.AccessExpiration < DateTime.UtcNow)
            {
                ClientTokens = clientdomain.Refresh(_tokens);
                return ClientTokens.AccessToken;
            }

            return ClientTokens.AccessToken;

            //// check if access token is expired
            //if (_tokens.AccessExpiration < DateTime.UtcNow)
            //{
            //    if (!_tokens.Id.Equals("test"))
            //    {
            //        // get new tokens if refresh token is still valid
            //        // throw exception if they're not
            //        if (_tokens.RefreshExpiration > DateTime.UtcNow)
            //        {
            //            ClientTokens = clientdomain.Refresh(_tokens);

            //            return _tokens.AccessToken;
            //        }
            //        else
            //        {
            //            throw new TokensExpiredException();
            //        }
            //    }
            //    // throw exception if test access token is expired because we don't have a refresh token for them
            //    else
            //    {
            //        throw new TokensExpiredException();
            //    }
            //}

            //return _tokens.AccessToken;

        }

        public string ApiCall(string path, Dictionary<string, string> parameters, string method, bool useRegionDomain = true)
        {
            if (method is not "POST" && method is not "GET")
                throw new ArgumentOutOfRangeException(nameof(method), $"Value should be either 'POST' or 'GET' instead got {method}");

            if (parameters is null)
                parameters = new();

            var domain = useRegionDomain ? this.RegionDomain : this.DefaultURL;

            ILazopClient client = new LazopClient(domain, this._appKey, this._appSecret);

            LazopRequest request = new(path);
            request.SetHttpMethod(method);

            foreach (KeyValuePair<string, string> param in parameters)
            {
                request.AddApiParameter(param.Key, param.Value);
            }

            LazopResponse response = null;
            short retries = 0;
            while (response is null)
            {

                try
                {
                    response = client.Execute(request);
                }
                catch (System.Exception e)
                {
                    retries++;
                    if (retries > 3)
                    {
                        throw new TimeoutException("Max retries exceeded", e);
                    }
                }

            }

            // should write to log on response.IsError()
            if (response.IsError())
            {
                // 207: Cannot find a Sku by the Seller Sku.
                // 208: Cannot find an Item by the Item Id.
                if (response.Code.Equals("207") || response.Code.Equals("208"))
                {
                    string s;
                    string i;

                    if (parameters.TryGetValue("seller_sku", out s))
                        throw new NullProductException(s.ToString());
                    if (parameters.TryGetValue("item_id", out i))
                        throw new NullProductException(long.Parse(i));
                }

                throw new LazadaIntegrationServiceException(response.Body);
                // _logger.LogInformation(...);
                // return null;
            }

            return response.Body;
        }

        public string AuthenticatedApiCall(string path, Dictionary<string, string> parameters, string method, bool useRegionDomain = true)
        {

            if (parameters is null)
            {
                parameters = new();
            }

            var token = _tokens.AccessToken;

            parameters.Add("access_token", token);

            return ApiCall(path, parameters, method, useRegionDomain);
        }
    }
}

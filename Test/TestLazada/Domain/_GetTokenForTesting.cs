using Xunit;
using KTI.Moo.Extensions.Lazada.Domain;
using KTI.Moo.Extensions.Lazada.Model;
using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Policy;

namespace TestLazada.Domain
{
    public class GetTokenForTesting : Base.LazadaBase
    {
        private readonly ClientToken_Cached _tokenDomain;
        private readonly IDistributedCache _cache;

        public GetTokenForTesting()
        {

            var services = new ServiceCollection();
            services.AddDistributedRedisCache(o => { o.Configuration = redisconnection; });
            var provider = services.BuildServiceProvider();
            _cache = provider.GetService<IDistributedCache>();

            _tokenDomain = new(_cache, _DefaultUrl, _AppKey, _AppSecret, _Region);

        }

        [Fact]
        public void AddToken()
        {
            ClientTokens tokens = new()
            {
                AccessToken = Base.LazadaBase.AccessToken,
                AccessExpiration = DateTime.Now.AddSeconds(604800),
                RefreshToken = "testtoken",
                RefreshExpiration = DateTime.Now.AddSeconds(604800),
                Account = "LzdOp_PH_test@163.com",
                Country = Base.LazadaBase._Region,
                AccountPlatform = "seller_center",
                CountryUserInfos = new[] { new CountryUserInfo()
                       {
                           Country = Base.LazadaBase._Region,
                           ShortCode = "PH7S3PV6UR",
                           SellerID = Base.LazadaBase._SellerID
                       }
                   }
            };

            _tokenDomain.Add(tokens);

            Assert.True(true);
        }


        [Fact]
        public void RefreshToken()
        {

            var ChannelManagement = new KTI.Moo.CRM.Domain.ChannelManagement.SalesChannel();

            var ChannelManagementCached = new KTI.Moo.CRM.Domain.ChannelManagement.SalesChannelCached(ChannelManagement, _cache);

            var LazadaClientToken = new KTI.Moo.Extensions.Lazada.Domain.ClientToken(CongfigTNCCI);

            var clientTokenDomain = new KTI.Moo.Extensions.Lazada.Domain.Queue.ClientToken_SalesChannel<KTI.Moo.Extensions.Lazada.Model.ClientTokens, KTI.Moo.CRM.Model.ChannelManagement.SalesChannel>(LazadaClientToken, ChannelManagementCached);

            clientTokenDomain.CompanyID = 3389;

            var ChannelConfig = clientTokenDomain.GetbyLazadaSellerID(CongfigTNCCI.SellerId);
            var ChannelConfigToken = ChannelConfig.kti_access_token;

            var ClientTokenLazada = new KTI.Moo.Extensions.Lazada.Model.ClientTokens()
            {
                AccessToken = ChannelConfig.kti_access_token,
                RefreshToken = ChannelConfig.kti_refresh_token,
                AccessExpiration = ChannelConfig.kti_access_expiration,
                RefreshExpiration = ChannelConfig.kti_refresh_expiration
            };

            ClientTokenLazada = clientTokenDomain.Refresh(ClientTokenLazada, ChannelConfig);

            var ClientTokenLazadaToken = ClientTokenLazada.AccessToken;

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Lazada.Model.ClientTokens>(ClientTokenLazada);
            Assert.NotEqual(ClientTokenLazadaToken, ChannelConfigToken);
        }


        [Fact]
        public void DeleteTokenInCache()
        {

            _cache.Remove($"lazada_{_Region}_{500160021491}");

            Assert.True(true);
        }

    }
}


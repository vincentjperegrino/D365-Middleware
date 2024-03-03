using IntegrationTestLazada.Model;
using KTI.Moo.Extensions.Lazada.Domain;
using KTI.Moo.Extensions.Lazada.Model;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTestLazada
{
    public class LazadaTokens : TestSetup
    {
        private readonly ClientToken_Cached _tokenDomain;
        private readonly IDistributedCache _cache;

        public LazadaTokens()
        {

            var services = new ServiceCollection();
            services.AddDistributedRedisCache(o => { o.Configuration = redisconnection; });
            var provider = services.BuildServiceProvider();
            _cache = provider.GetService<IDistributedCache>();
            _tokenDomain = new(_cache, _DefaultUrl, _AppKey, _AppSecret, _Region);
        }

        [Fact]
        public void GetToken()
        {

            var Sellerid = "100124004";
            var site = $"lazada_{_Region}";
            var result = _tokenDomain.Get(Sellerid, site);

            Assert.IsAssignableFrom<ClientTokens>(result);
        }

        [Fact]
        public void RefreshToken()
        {
            var Sellerid = "100124004";

            var site = $"lazada_{_Region}";

            var Token = _tokenDomain.Get(Sellerid, site);

            var result = _tokenDomain.Refresh(Token);

            Assert.IsAssignableFrom<ClientTokens>(result);
        }
    }
}
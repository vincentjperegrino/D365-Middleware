using Xunit;
using KTI.Moo.Extensions.Lazada.Domain;
using KTI.Moo.Extensions.Lazada.Model;
using System;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;


namespace TestLazada.Domain
{
    public class Category : Base.LazadaBase
    {

        private readonly KTI.Moo.Extensions.Lazada.Domain.Category categoryDomain;
        private readonly IDistributedCache _cache;

        public Category()
        {

            var services = new ServiceCollection();
            services.AddDistributedRedisCache(o => { o.Configuration = redisconnection; });

            var provider = services.BuildServiceProvider();
            _cache = provider.GetService<IDistributedCache>();

            categoryDomain = new(CongfigTest, _cache);
        }

        [Fact]
        public void TestGetProducts_SKU_IFworking()
        {
            var response = categoryDomain.Get("en_US");

            Assert.True(false);
        }
    }
}

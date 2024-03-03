using Xunit;
using KTI.Moo.Extensions.Lazada.Domain;
using KTI.Moo.Extensions.Lazada.Model;
using System;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace TestLazada.Domain;

public class SellerVoucher : Base.LazadaBase
{


    private readonly KTI.Moo.Extensions.Lazada.Domain.SellerVoucher Domain;
    private readonly IDistributedCache _cache;

    public SellerVoucher()
    {

        var services = new ServiceCollection();
        services.AddDistributedRedisCache(o => { o.Configuration = redisconnection; });

        var provider = services.BuildServiceProvider();
        _cache = provider.GetService<IDistributedCache>();

        Domain = new(CongfigTest, _cache);
    }

    [Fact]
    public void TestGetVoucherFromId_IFworking()
    {

        string promoid = "900000010775051";

        var response = Domain.GetFromID(promoid);

        Assert.IsAssignableFrom<KTI.Moo.Extensions.Lazada.Model.Promo>(response);

    }


}

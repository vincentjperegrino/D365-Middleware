using KTI.Moo.CRM.Domain.ChannelManagement;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using Xunit;

namespace Moo.CRM.TestDomain;

public class ChannelManagementCachedIntegrationTest
{
    private readonly KTI.Moo.CRM.Domain.ChannelManagement.SalesChannel _Domain;
    private readonly SalesChannelCached _DomainCached;

    private readonly ILogger _logger;
    private readonly IDistributedCache _cache;

    public ChannelManagementCachedIntegrationTest()
    {
        var services = new ServiceCollection();
        services.AddDistributedRedisCache(o => { o.Configuration = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False"; });
        var provider = services.BuildServiceProvider();
        _cache = provider.GetService<IDistributedCache>();
        _Domain = new(3389);

        _DomainCached = new(_Domain, _cache);

    }

    [Fact]
    public void GetAllWorking()
    {
        var result = _DomainCached.GetChannelList();
        Assert.IsAssignableFrom<List<KTI.Moo.CRM.Model.ChannelManagement.SalesChannel>>(result);
    }

    [Fact]
    public void GetWorking()
    {
        var result = _DomainCached.Get("sapmain");
        //_cache.Remove("sapmain");
        Assert.IsAssignableFrom<KTI.Moo.CRM.Model.ChannelManagement.SalesChannel>(result);
    }

    [Fact]
    public void GetByLazadaSellerIDWorking()
    {
        var result = _DomainCached.GetbyLazadaSellerID("500160021491");
        //_cache.Remove("sapmain");
        Assert.IsAssignableFrom<KTI.Moo.CRM.Model.ChannelManagement.SalesChannel>(result);
    }
      
   
}


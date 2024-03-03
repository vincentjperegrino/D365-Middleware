using KTI.Moo.CRM.Model.ChannelManagement;
using Microsoft.Extensions.Caching.Distributed;

namespace KTI.Moo.CRM.Domain.ChannelManagement;

public class SalesChannelCached : Base.Domain.IChannelManagement<Model.ChannelManagement.SalesChannel>
{

    private readonly Base.Domain.IChannelManagement<Model.ChannelManagement.SalesChannel> _ChannelManagementDomain;
    private readonly IDistributedCache _cache;

    public SalesChannelCached(SalesChannel channelManagementDomain, IDistributedCache cache)
    {
        _ChannelManagementDomain = channelManagementDomain;
        _cache = cache;
    }

    public int CompanyID
    {
        get => _ChannelManagementDomain.CompanyID;
        set => _ChannelManagementDomain.CompanyID = value;
    }

    public Model.ChannelManagement.SalesChannel Get(string StoreCode)
    {
        var cachedkey = $"{CompanyID}_{Base.Helpers.RedisCache.GetChannelManagementStoreCode(StoreCode)}";

        var cachedresult = _cache.GetString(cachedkey);

        if (!string.IsNullOrWhiteSpace(cachedresult))
        {
            var cachedChannelList = JsonConvert.DeserializeObject<Model.ChannelManagement.SalesChannel>(cachedresult);
            return cachedChannelList;
        }

        var result = _ChannelManagementDomain.Get(StoreCode);
        var resultJson = JsonConvert.SerializeObject(result);

        var options = new DistributedCacheEntryOptions();
        options.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

        _cache.SetString(cachedkey, resultJson, options);

        return result;
    }

    public List<Model.ChannelManagement.SalesChannel> GetChannelList()
    {

        var cachedkey = $"{CompanyID}_{Base.Helpers.RedisCache.ChannelManagement}";

        var cachedresult = _cache.GetString(cachedkey);

        if (!string.IsNullOrWhiteSpace(cachedresult))
        {
            var cachedChannelList = JsonConvert.DeserializeObject<List<Model.ChannelManagement.SalesChannel>>(cachedresult);
            return cachedChannelList;
        }

        var result = _ChannelManagementDomain.GetChannelList();
        var resultJson = JsonConvert.SerializeObject(result);

        var options = new DistributedCacheEntryOptions();
        options.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
        _cache.SetString(cachedkey, resultJson, options);

        return result;
    }


    public Model.ChannelManagement.SalesChannel GetbyLazadaSellerID(string SellerID)
    {
        var cachedkey = $"{CompanyID}_{Base.Helpers.RedisCache.GetChannelManagementStoreCode(SellerID)}";

        var cachedresult = _cache.GetString(cachedkey);

        if (!string.IsNullOrWhiteSpace(cachedresult))
        {
            var cachedChannelList = JsonConvert.DeserializeObject<Model.ChannelManagement.SalesChannel>(cachedresult);
            return cachedChannelList;
        }

        var result = _ChannelManagementDomain.GetbyLazadaSellerID(SellerID);
        var resultJson = JsonConvert.SerializeObject(result);

        var options = new DistributedCacheEntryOptions();
        options.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

        _cache.SetString(cachedkey, resultJson, options);

        return result;

    }

    public bool UpdateToken(Model.ChannelManagement.SalesChannel ChannelConfig)
    {
        return _ChannelManagementDomain.UpdateToken(ChannelConfig);
    }


}

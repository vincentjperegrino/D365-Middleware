using KTI.Moo.Extensions.SAP.Helper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Moo.CRM.TestDomain;

public class ChannelManagement
{
    private readonly KTI.Moo.CRM.Domain.ChannelManagement.SalesChannel _Domain;
    private readonly ILogger _logger;

    public ChannelManagement()
    {
        _Domain = new(3388);
    }


    [Fact]
    public async Task GetAllWorking()
    {
        var result = await _Domain.GetChannelListAsync();
        Assert.IsAssignableFrom<List<KTI.Moo.CRM.Model.ChannelManagement.SalesChannel>>(result);
    }

    [Fact]
    public async Task GetWorking()
    {
        var result = await _Domain.GetAsync("lazadatestncci");
        Assert.IsAssignableFrom<KTI.Moo.CRM.Model.ChannelManagement.SalesChannel>(result);
    }

    [Fact]
    public async Task GetLazadaSellerIDWorking()
    {
        var result = await _Domain.GetbyLazadaSellerIDAsync("500203125266");
        Assert.IsAssignableFrom<KTI.Moo.CRM.Model.ChannelManagement.SalesChannel>(result);
    }

    [Fact]
    public async Task UpdateTokenWorking()
    {
        var saleschannel = await _Domain.GetbyLazadaSellerIDAsync("500203125266");

        saleschannel.kti_access_token = "123";
        saleschannel.kti_refresh_token = "1231332";
        saleschannel.kti_refresh_expiration = DateTime.UtcNow;
        saleschannel.kti_access_expiration = DateTime.UtcNow;

        var result = await _Domain.UpdateTokenAsync(saleschannel);
        Assert.True(result);
    }

}

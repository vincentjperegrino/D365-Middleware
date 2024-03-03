
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.IO;
using KTI.Moo.CRM.Model;

namespace Moo.CRM.TestDomain;

public class Order
{
    private KTI.Moo.CRM.Domain.Order _Domain;
    private readonly ILogger _logger;
    public Order()
    {
        var mock = new Mock<ILogger>();
        _logger = Mock.Of<ILogger>();

    }

    [Fact]
    public async Task GetOrdersIfWorking()
    {
        _Domain = new(companyId: 3388);
        var result = await _Domain.GetSyncToOrders();
        Assert.IsAssignableFrom<List<KTI.Moo.CRM.Model.OrderBase>>(result);
    }

    [Fact]
    public async Task SyncOrdersIfWorking()
    {
        _Domain = new(companyId: 3388);
        var result = await _Domain.SyncToOrders();
        Assert.IsAssignableFrom<string>(result);
    }

    [Fact]
    public async Task OrdersUpsertIfWorking()
    {

        _Domain = new(companyId: 3388);
        JObject samplejson = JObject.Parse(File.ReadAllText("..\\..\\..\\Order.json"));
        string content = JsonConvert.SerializeObject(samplejson);

        var result = await _Domain.upsertWithCustomer(content, _logger);
        Assert.True(result);
    }


    [Fact]
    public async Task ReplicateOrdersIfWorking()
    {
        _Domain = new(companyId: 3388);
        var orderid = "5f5c81d8-4ea7-ed11-aad1-002248593ae0";
        var result = await _Domain.ReplicateOrders(orderid);
        Assert.True(result);
    }


    [Fact]
    public void GetAllOrderList()
    {
        _Domain = new(companyId: 3389);
        var InitialList = new List<OrderBase>();

        var DateNow = DateTime.UtcNow.AddDays(-1);
        var DateEnd = DateTime.UtcNow;

        var result = _Domain.GetAll(InitialList, DateNow, DateEnd, 10, 1);

        Assert.IsType<List<OrderBase>>(result);
        Assert.True(result.Count > 0);
    }

    [Fact]
    public void GetAllOrderList_Default()
    {
        _Domain = new(companyId: 3389);
        var DateNow = DateTime.UtcNow.AddDays(-1);
        var DateEnd = DateTime.UtcNow;

        var result = _Domain.GetAll(DateNow, DateEnd);

        Assert.IsType<List<OrderBase>>(result);
        Assert.True(result.Count > 0);
    }  
    
    [Fact]
    public async Task GetAllOrderList_WithInnerJoin()
    {
        _Domain = new(companyId: 3389);
        DateTime startDate = new DateTime(2022, 5, 1, 0, 0, 0).AddHours(-8);
        DateTime endDate = new DateTime(2023, 5, 31, 23, 59, 59).AddHours(-8);

        var result = await _Domain.RetrieveDataFromJoinedTables(startDate, endDate);

        Assert.IsType<string>(result);

    }

}

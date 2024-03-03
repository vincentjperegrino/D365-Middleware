using Xunit;
using KTI.Moo.Extensions.Lazada.Domain;
using KTI.Moo.Extensions.Lazada.Model;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace TestLazada.Domain;

public class Fulfillment : Base.LazadaBase
{
    private readonly KTI.Moo.Extensions.Lazada.Domain.Fulfillment FulfillmentDomain;
    private readonly KTI.Moo.Extensions.Lazada.Domain.Order OrderDomain;
    private readonly IDistributedCache _cache;

    public Fulfillment()
    {

        var services = new ServiceCollection();
        services.AddDistributedRedisCache(o => { o.Configuration = redisconnection; });

        var provider = services.BuildServiceProvider();
        _cache = provider.GetService<IDistributedCache>();

        FulfillmentDomain = new(CongfigTest, _cache);
        OrderDomain = new(CongfigTest, _cache);
    }


    [Fact]
    public void TestGetShipmentProviderworking()
    {

        long id = 563449171755852;

        var orders = OrderDomain.Get(id);

        var response = FulfillmentDomain.GetShipmentProvider(orders.kti_sourceid, orders.order_items.Select(item => item.kti_sourceid).ToList());

        Assert.IsAssignableFrom<string>(response);

    }

    [Fact]
    public void TestGetPackworking()
    {

        long id = 561884244555852;

        var orders = OrderDomain.Get(id);

        var Fulfillment = FulfillmentDomain.GetShipmentProvider(orders.kti_sourceid, orders.order_items.Select(item => item.kti_sourceid).ToList());

        var response = FulfillmentDomain.Pack(orders.kti_sourceid, orders.order_items.Select(item => item.kti_sourceid).ToList(), Fulfillment);

        Assert.IsAssignableFrom<PackageOrder>(response);

    }


    [Fact]
    public void TestGetRePackworking()
    {

        long id = 561884244555852;

        var orders = OrderDomain.Get(id);

        var Fulfillment = FulfillmentDomain.GetShipmentProvider(orders.kti_sourceid, orders.order_items.Select(item => item.kti_sourceid).ToList());

        var PackageID = FulfillmentDomain.Pack(orders.kti_sourceid, orders.order_items.Select(item => item.kti_sourceid).ToList(), Fulfillment);

        var response = FulfillmentDomain.RecreatePackage(PackageID.order_item_list.FirstOrDefault().package_id);

        Assert.IsAssignableFrom<OrderItempackage>(response);

    }

    [Fact]
    public void TestGetAWBworking()
    {

        var PackageID = "FP039611738813178";

        var response = FulfillmentDomain.PrintAWB(PackageID);

        Assert.IsAssignableFrom<string>(response);

    }


    [Fact]
    public void TestGetReadyTOShipworking()
    {

        var PackageID = "FP039611738813178";

        var response = FulfillmentDomain.ReadyToShip(PackageID);

        Assert.IsAssignableFrom<string>(response);

    }




}

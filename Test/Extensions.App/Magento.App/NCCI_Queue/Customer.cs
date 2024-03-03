using Azure.Storage.Queues;
using Domain;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TestMagento.App.Model;
using Xunit;
using DomainTotest = KTI.Moo.Extensions.Magento.App.NCCI.Queue.Receivers;

namespace TestMagento.App.NCCI_Queue;

public class Customer : TestBase
{
    private readonly IDistributedCache _cache;
    private readonly ILogger _logger;

    public Customer()
    {
        var services = new ServiceCollection();
        services.AddDistributedRedisCache(o => { o.Configuration = redisConnectionString; });

        var provider = services.BuildServiceProvider();
        _cache = provider.GetService<IDistributedCache>();
        _logger = Mock.Of<ILogger>();
    }  



    [Fact]
    public async Task GetCustomer()
    {

        KTI.Moo.Extensions.Magento.Domain.Customer CustomerDomain = new(Prodconfig, _cache, _logger);

        var CustomerData = CustomerDomain.Get(401151);


        Assert.True(true);

    }


    [Fact]
    public async Task PushToCRM_Customer_DateRange()
    {

        KTI.Moo.Extensions.Magento.Domain.Customer CustomerDomain = new(Prodconfig, _cache);

        var startDate = DateTime.UtcNow.AddDays(-3);
        var endDate = DateTime.UtcNow.AddMinutes(1);

        var defaultpageSize = 100;
        var pageSize = defaultpageSize;

        var CustomerList = new List<KTI.Moo.Extensions.Magento.Model.Customer>();

        var NotFinish = true;
        var currentPage = 1;

        while (NotFinish)
        {
            var SearchResult = CustomerDomain.GetSearchCustomers(startDate, endDate, pagesize: pageSize, currentPage: currentPage);

            if (ValidSearch(SearchResult))
            {
                break;
            }

            CustomerList.AddRange(SearchResult.values);

            var currentItemCountCovered = pageSize * currentPage;

            if (IsFinish(SearchResult, currentItemCountCovered))
            {
                //Finish/Complete
                NotFinish = false;
            }

            currentPage++;
        }

        var returnList = GetChannelAppsDTO(CustomerList);


        QueueClient queueClient = new(_connectionstringProd, "3389-magento-customer", new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        // Create the queue if it doesn't already exist
        queueClient.CreateIfNotExistsAsync().Wait();

        foreach (var Customer in returnList)
        {

            var JsonSettings = new JsonSerializerSettings
            {

                ContractResolver = new KTI.Moo.Extensions.Core.Helper.JSONSerializer.DontIgnoreResolver()
            };

            var json = JsonConvert.SerializeObject(Customer, Formatting.None, JsonSettings);


            var Compress = json.ToBrotliAsync().GetAwaiter().GetResult().Result.Value;

            queueClient.SendMessage(Compress);
        }

        Assert.True(true);



    }




    private static bool IsFinish(KTI.Moo.Extensions.Magento.Model.DTO.Customers.Search SearchResult, int currentItemCountCovered)
    {
        return SearchResult.total_count <= currentItemCountCovered;
    }

    private static bool ValidSearch(KTI.Moo.Extensions.Magento.Model.DTO.Customers.Search SearchResult)
    {
        return SearchResult.values is null || SearchResult.values.Count <= 0;
    }

    private List<KTI.Moo.Extensions.Magento.Model.DTO.ChannelApps> GetChannelAppsDTO(List<KTI.Moo.Extensions.Magento.Model.Customer> CustomerAddedForTheLastMinute)
    {

        var DTOlist = CustomerAddedForTheLastMinute.Select(customer =>
        {
            return new KTI.Moo.Extensions.Magento.Model.DTO.ChannelApps()
            {
                customer = customer
            };

        }).ToList();

        return DTOlist;

    }



    [Fact]
    public async Task PushToCRM_Customer()
    {

        KTI.Moo.Extensions.Magento.Domain.Customer CustomerDomain = new(Stagingconfig, _cache);

        var customer = CustomerDomain.GetCustomersWithEmail("annajnavarro@gmail.com");

        var DTO = new KTI.Moo.Extensions.Magento.Model.DTO.ChannelApps()
        {
            customer = customer
        };

        var JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new JSONSerializerHelper.DontIgnoreResolver()
        };

        var json = JsonConvert.SerializeObject(DTO, Formatting.None, JsonSettings);

        var CompressionResults = await json.ToBrotliAsync();

        var CompressionResult = CompressionResults.Result.Value;

        var DecompressionResult = await CompressionResult.FromBrotliAsync();

        QueueClient queueClient = new(_connectionstring, "3388-magento-customer", new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        queueClient.CreateIfNotExistsAsync().Wait();

        queueClient.SendMessage(CompressionResult);

        Assert.Equal(json, DecompressionResult);

    }


    [Fact]
    public async Task PushToCRM_Customer_Prod()
    {

        KTI.Moo.Extensions.Magento.Domain.Customer CustomerDomain = new(Prodconfig, _cache);

        var customer = CustomerDomain.GetCustomersWithEmail("markie_third@yahoo.com");

        var DTO = new KTI.Moo.Extensions.Magento.Model.DTO.ChannelApps()
        {
            customer = customer
        };

        var JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new JSONSerializerHelper.DontIgnoreResolver()
        };

        var json = JsonConvert.SerializeObject(DTO, Formatting.None, JsonSettings);

        var CompressionResults = await json.ToBrotliAsync();

        var CompressionResult = CompressionResults.Result.Value;

        var DecompressionResult = await CompressionResult.FromBrotliAsync();

        QueueClient queueClient = new(_connectionstringProd, "3389-magento-customer", new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        queueClient.CreateIfNotExistsAsync().Wait();

        queueClient.SendMessage(CompressionResult);

        Assert.Equal(json, DecompressionResult);

    }



    [Fact]
    public async Task PushToCRM_CustomerProd_To_Staging()
    {

        KTI.Moo.Extensions.Magento.Domain.Customer CustomerDomain = new(ProdToStagingconfig, _cache);

        var customer = CustomerDomain.GetCustomersWithEmail("fname2@gmail.com");

        var DTO = new KTI.Moo.Extensions.Magento.Model.DTO.ChannelApps()
        {
            customer = customer
        };

        var JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new JSONSerializerHelper.DontIgnoreResolver()
        };

        var json = JsonConvert.SerializeObject(DTO, Formatting.None, JsonSettings);

        var CompressionResults = await json.ToBrotliAsync();

        var CompressionResult = CompressionResults.Result.Value;

        var DecompressionResult = await CompressionResult.FromBrotliAsync();

        QueueClient queueClient = new(_connectionstring, "3388-magento-customer", new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        queueClient.CreateIfNotExistsAsync().Wait();

        queueClient.SendMessage(CompressionResult);

        Assert.Equal(json, DecompressionResult);

    }


}

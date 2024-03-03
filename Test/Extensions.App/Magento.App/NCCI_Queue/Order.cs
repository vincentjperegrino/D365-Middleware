using Azure.Storage.Queues;
using Domain;
using FastJSON;
using KTI.Moo.Extensions.Core.Helper;
using KTI.Moo.Extensions.Magento.App.NCCI.Queue.Helper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestMagento.App.Model;
using Xunit;
using DomainTotest = KTI.Moo.Extensions.Magento.App.NCCI.Queue.Receivers;


namespace TestMagento.App.NCCI_Queue
{
    public class Order : TestBase
    {
        private readonly IDistributedCache _cache;

        public Order()
        {
            var services = new ServiceCollection();
            services.AddDistributedRedisCache(o => { o.Configuration = redisConnectionString; });

            var provider = services.BuildServiceProvider();
            _cache = provider.GetService<IDistributedCache>();

        }

        //[Fact]
        //public async Task GetDTOListSuccess()
        //{

        //    Type type = typeof(DomainTotest.Order);

        //    var OrderDomain = Activator.CreateInstance(type, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { Stagingconfig, _connectionstring }, null);

        //    //act
        //    MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
        //    .Where(x => x.Name == "GetDTOList" && x.IsPrivate)
        //    .First();


        //    KTI.Moo.Extensions.Magento.Domain.Order ExtensionOrderDomain = new(Stagingconfig);


        //    var order = ExtensionOrderDomain.Get(116056);

        //    List<KTI.Moo.Extensions.Magento.Model.Order> OrderList = new()
        //    {
        //        order
        //    };

        //    var Response = (List<KTI.Moo.Extensions.Magento.Model.DTO.ChannelApps>)method.Invoke(OrderDomain, new object[] { OrderList });

        //    var JsonSettings = new JsonSerializerSettings
        //    {
        //        ContractResolver = new JSONSerializerHelper.DontIgnoreResolver()
        //    };


        //    var json = JsonConvert.SerializeObject(Response.FirstOrDefault(), Formatting.None, JsonSettings);

        //    var CompressionResults = await json.ToBrotliAsync();

        //    var CompressionResult = CompressionResults.Result.Value;

        //    var DecompressionResult = await CompressionResult.FromBrotliAsync();

        //    QueueClient queueClient = new(_connectionstring, "3388-magento-order", new QueueClientOptions
        //    {
        //        MessageEncoding = QueueMessageEncoding.Base64
        //    });

        //    queueClient.CreateIfNotExistsAsync().Wait();

        //    queueClient.SendMessage(CompressionResult);

        //    Assert.Equal(json, DecompressionResult);

        //}

        [Fact]
        public async Task GetDTOListSuccess_NCCI_ORDERDOMAIN()
        {

            KTI.Moo.Extensions.Magento.Domain.Order ExtensionOrderDomain = new KTI.Moo.Extensions.Magento.Custom.NCCI.Domain.Order(Stagingconfig, _cache);
            KTI.Moo.Extensions.Magento.Domain.Customer CustomerDomain = new(Stagingconfig, _cache);

            var order = ExtensionOrderDomain.Get(115778);

            var CustomerModels = CustomerDomain.Get(order.customer_id);

            var Response = new KTI.Moo.Extensions.Magento.Model.DTO.ChannelApps()
            {
                order = order,
                customer = CustomerModels
            };

            var JsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new JSONSerializerHelper.DontIgnoreResolver()
            };

            var json = JsonConvert.SerializeObject(Response, Formatting.None, JsonSettings);

            var CompressionResults = await json.ToBrotliAsync();

            var CompressionResult = CompressionResults.Result.Value;

            var DecompressionResult = await CompressionResult.FromBrotliAsync();

            QueueClient queueClient = new(_connectionstring, "3388-magento-order", new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExistsAsync().Wait();

            queueClient.SendMessage(CompressionResult);

            Assert.Equal(json, DecompressionResult);

        }

        [Fact]
        public async Task GetDTOListSuccess_NCCI_ORDERDOMAIN_Prod()
        {

            KTI.Moo.Extensions.Magento.Domain.Order ExtensionOrderDomain = new KTI.Moo.Extensions.Magento.Custom.NCCI.Domain.Order(Prodconfig, _cache);
            KTI.Moo.Extensions.Magento.Domain.Customer CustomerDomain = new(Prodconfig, _cache);

            var order = ExtensionOrderDomain.Get(131296);

            var CustomerModels = CustomerDomain.Get(order.customer_id);

            var Response = new KTI.Moo.Extensions.Magento.Model.DTO.ChannelApps()
            {
                order = order,
                customer = CustomerModels
            };

            var JsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new JSONSerializerHelper.DontIgnoreResolver()
            };

            var json = JsonConvert.SerializeObject(Response, Formatting.None, JsonSettings);

            var CompressionResults = await json.ToBrotliAsync();

            var CompressionResult = CompressionResults.Result.Value;

            var DecompressionResult = await CompressionResult.FromBrotliAsync();

            QueueClient queueClient = new(_connectionstringProd, "3389-magento-order", new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExistsAsync().Wait();

            queueClient.SendMessage(CompressionResult);

            Assert.Equal(json, DecompressionResult);

        }

        [Fact]
        public async Task GetDTOListSuccess_NCCI_ORDERDOMAIN_TestProdtoStaging()
        {

            KTI.Moo.Extensions.Magento.Domain.Order ExtensionOrderDomain = new KTI.Moo.Extensions.Magento.Custom.NCCI.Domain.Order(ProdToStagingconfig, _cache);
            KTI.Moo.Extensions.Magento.Domain.Customer CustomerDomain = new(ProdToStagingconfig);

            //var order = ExtensionOrderDomain.Get(121923); 6329
            var order = ExtensionOrderDomain.Get(116084);

            var CustomerModels = CustomerDomain.Get(order.customer_id);

            var Response = new KTI.Moo.Extensions.Magento.Model.DTO.ChannelApps()
            {
                order = order,
                customer = CustomerModels
            };

            var JsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new JSONSerializerHelper.DontIgnoreResolver()
            };

            var json = JsonConvert.SerializeObject(Response, Formatting.None, JsonSettings);

            var CompressionResults = await json.ToBrotliAsync();

            var CompressionResult = CompressionResults.Result.Value;

            var DecompressionResult = await CompressionResult.FromBrotliAsync();

            QueueClient queueClient = new(_connectionstring, "3388-magento-order", new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExistsAsync().Wait();

            queueClient.SendMessage(CompressionResult);

            Assert.Equal(json, DecompressionResult);

        }


        [Fact]
        public void Compare_CustomDomain_and_ExtensionDomainNCCI()
        {

            KTI.Moo.Extensions.Magento.Domain.Order CustomOrderDomain = new KTI.Moo.Extensions.Magento.Custom.NCCI.Domain.Order(Prodconfig, _cache);


            var StandardOrderDomain = new KTI.Moo.Extensions.Magento.Domain.Order(Prodconfig, _cache);

            var ExtensionsDomain = new KTI.Moo.Extensions.Domain.NCCI.Magento.Order(StandardOrderDomain);

            var orderid = 127532;

            var CustomOrder = CustomOrderDomain.Get(orderid);
            var ExtensionOrder = ExtensionsDomain.Get(orderid);

            foreach (var items in CustomOrder.order_items)
            {
                Assert.Equal(items.tax, ExtensionOrder.order_items.Where(item => item.item_id == items.item_id).FirstOrDefault().tax);
            }
        }

        [Fact]
        public void Compare_CustomDomain_and_ExtensionDomainNCCI_SearchbyDate()
        {

            KTI.Moo.Extensions.Magento.Domain.Order CustomOrderDomain = new KTI.Moo.Extensions.Magento.Custom.NCCI.Domain.Order(Prodconfig, _cache);

            var StandardOrderDomain = new KTI.Moo.Extensions.Magento.Domain.Order(Prodconfig, _cache);

            var ExtensionsDomain = new KTI.Moo.Extensions.Domain.NCCI.Magento.OrderSearch(StandardOrderDomain);


            var DateStart = DateTime.UtcNow.AddMinutes(-60);
            var DateEnd = DateTime.UtcNow.AddMinutes(30); 

            var CustomOrder = CustomOrderDomain.GetSearchOrders(DateStart, DateEnd, 1000, 1);

            var ExtensionOrder = ExtensionsDomain.GetAll(DateStart, DateEnd, 1000);

            Assert.Equal(CustomOrder.values.Count(), ExtensionOrder.Count());

            foreach (var order in CustomOrder.values)
            {
                foreach (var items in order.order_items)
                {
                    Assert.Equal(items.tax, ExtensionOrder.Where(item => item.order_id == order.order_id).First().order_items.Where(item => item.item_id == items.item_id).FirstOrDefault().tax);
                }
            }
        }


        [Fact]
        public async Task SAmple()
        {


            //var json = "{\"companyid\":3389,\"domainType\":\"order\",\"kti_UpsertOrder_CustomerParameters\":{\"@odata.type\":\"#Microsoft.Dynamics.CRM.expando\",\"address1_city\":\"Lipa\",\"address1_country\":\"Philippines\",\"address1_line1\":\"1314 Balintawak Road\",\"address1_postalcode\":\"\",\"address1_stateorprovince\":\"Batangas\",\"address1_telephone1\":\"6309992244784\",\"address2_city\":\"Lipa\",\"address2_line1\":\"1314 Balintawak Road\",\"address2_postalcode\":\"\",\"address2_telephone1\":\"6309992244784\",\"telephone1\":\"+639992244784\",\"telephone2\":\"6309992244784\",\"kti_socialchannelorigin\":959080006,\"kti_sourceid\":\"500095083650\",\"firstname\":\"Selah\",\"lastname\":\"Baroja\",\"mobilephone\":\"+639992244784\"},\"kti_UpsertOrder_OrderParameters\":{\"@odata.type\":\"#Microsoft.Dynamics.CRM.expando\",\"kti_socialchannelorigin\":959080006,\"kti_channelurl\":\"https://sellercenter.lazada.com.ph/order/detail/634146434983650/lazada_ph_500160021491\",\"name\":\"634146434983650\",\"billto_city\":\"Lipa\",\"billto_contactname\":\"Selah Baroja\",\"billto_country\":\"Philippines\",\"billto_line1\":\"1314 Balintawak Road\",\"billto_postalcode\":\"\",\"billto_stateorprovince\":\"Batangas\",\"billto_telephone\":\"+639992244784\",\"description\":\"\",\"freightamount\":106.25,\"kti_sourceid\":\"634146434983650\",\"kti_couponcode\":\"\",\"kti_paymenttermscode\":959080019,\"kti_orderstatus\":959080001,\"kti_socialchannel\":\"lazadancci\",\"shipto_city\":\"Lipa\",\"shipto_contactname\":\"Selah Baroja\",\"shipto_country\":\"Philippines\",\"shipto_line1\":\"1314 Balintawak Road\",\"shipto_postalcode\":\"\",\"shipto_stateorprovince\":\"Batangas\",\"shipto_telephone\":\"6309992244784\",\"kti_gifttagmessage\":\"\",\"overriddencreatedon\":\"2023-04-21T01:40:42Z\"},\"kti_UpsertOrder_OrderLineParameters\":[{\"productid\":\"ITINIS003A\",\"@odata.type\":\"#Microsoft.Dynamics.CRM.expando\",\"kti_socialchannelorigin\":959080006,\"ispriceoverridden\":true,\"manualdiscountamount\":0.0,\"priceperunit\":8482.14285714,\"productdescription\":\"Nespresso® Inissia Coffee Maker Black with Free Coffee Capsules\",\"quantity\":1.0,\"shipto_city\":\"Lipa\",\"shipto_contactname\":\"Selah Baroja\",\"shipto_country\":\"Philippines\",\"shipto_line1\":\"1314 Balintawak Road\",\"shipto_postalcode\":\"\",\"shipto_stateorprovince\":\"Batangas\",\"shipto_telephone\":\"6309992244784\",\"tax\":1030.60714286,\"kti_lineitemnumber\":\"1\",\"kti_sourceid\":\"634146434983650\",\"kti_sourceitemid\":\"634146435083650\"}]}";
            //var CompressionResults = await json.ToBrotliAsync();
            //var result = CompressionResults.Result.Value;

            var CompressionResult = "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";
            var DecompressionResult = await CompressionResult.FromBrotliAsync();

            Assert.True(true);

        }

    }



    public static class Compression
    {

        public static async Task<CompressionResult> ToBrotliAsync(this string value, CompressionLevel level = CompressionLevel.Fastest)
        {
            var bytes = Encoding.Unicode.GetBytes(value);
            await using var input = new MemoryStream(bytes);
            await using var output = new MemoryStream();
            await using var stream = new BrotliStream(output, level);

            await input.CopyToAsync(stream);
            await stream.FlushAsync();

            var result = output.ToArray();

            return new CompressionResult(
                new CompressionValue(value, bytes.Length),
                new CompressionValue(Convert.ToBase64String(result), result.Length),
                level,
                "Brotli"
            );
        }

        public static async Task<string> FromBrotliAsync(this string value)
        {
            var bytes = Convert.FromBase64String(value);
            await using var input = new MemoryStream(bytes);
            await using var output = new MemoryStream();
            await using var stream = new BrotliStream(input, CompressionMode.Decompress);

            await stream.CopyToAsync(output);

            return Encoding.Unicode.GetString(output.ToArray());
        }


    }

    public record CompressionResult(
    CompressionValue Original,
    CompressionValue Result,
    CompressionLevel Level,
    string Kind)
    {
        public int Difference =>
            Original.Size - Result.Size;

        public decimal Percent =>
          Math.Abs(Difference / (decimal)Original.Size);
    }

    public record CompressionValue(
        string Value,
        int Size
    );
}

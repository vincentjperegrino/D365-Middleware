using Azure.Storage.Queues;
using Domain;
using FastJSON;
using KTI.Moo.Extensions.Core.Helper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TestOctoPOS.app
{
    public class Invoice : Model.TestBase
    {

        private KTI.Moo.Extensions.OctoPOS.Domain.Invoice OctoPOSInvoice;
        private KTI.Moo.Extensions.OctoPOS.Domain.Customer OctoPOSCustomer;
        private readonly IDistributedCache _cache;

        public Invoice()
        {

            var services = new ServiceCollection();
            services.AddDistributedRedisCache(o => { o.Configuration = redisConnectionString; });

            var provider = services.BuildServiceProvider();
            _cache = provider.GetService<IDistributedCache>();

            //OctoPOSInvoice = new KTI.Moo.Extensions.OctoPOS.Custom.NCCI.Domain.Invoice(ConfigProduction, _cache);
            //OctoPOSCustomer = new(ConfigProduction, _cache);

            //OctoPOSInvoice = new KTI.Moo.Extensions.OctoPOS.Custom.NCCI.Domain.Invoice(ConfigTest, _cache);
            //OctoPOSCustomer = new(ConfigTest, _cache);

        }



        [Fact]
        public void MIGRATION_PRODUCTION_Customer()
        {

            // OctoPOSInvoice = new KTI.Moo.Extensions.OctoPOS.Custom.NCCI.Domain.Invoice(ConfigProduction, _cache);
            OctoPOSCustomer = new(ConfigProduction, _cache);

            DateTime startdate = new DateTime(2023, 3, 7, 0, 0, 0);
            DateTime enddate = new DateTime(2023, 3, 8, 23, 59, 59);
            int pagenumber = 1;

            var TotalcustomerList = new List<KTI.Moo.Extensions.OctoPOS.Model.Customer>();

            for (var i = 1; i < 7; i++)
            {
                var CustomerList = OctoPOSCustomer.GetSearchListByDate(startdate, enddate, i);
                TotalcustomerList.AddRange(CustomerList.values);
            }


            Assert.True(true);
        }



        [Fact]
        public void MIGRATION_PRODUCTION_Pages()
        {

            OctoPOSInvoice = new KTI.Moo.Extensions.OctoPOS.Custom.NCCI.Domain.Invoice(ConfigProduction, _cache);
            //OctoPOSCustomer = new(ConfigProduction, _cache);

            DateTime startdate = new DateTime(2022, 4, 1, 0, 0, 0);
            DateTime enddate = new DateTime(2022, 4, 30, 23, 59, 59);

            var InvoiceList = OctoPOSInvoice.SearchInvoiceListWithdetails(startdate, enddate, 154);//april  15383
            //may 15793

            Assert.True(true);
        }



        [Fact]
        public async Task MIGRATION_PRODUCTION_Invoice()
        {

            OctoPOSInvoice = new KTI.Moo.Extensions.OctoPOS.Custom.NCCI.Domain.Invoice(ConfigProduction, _cache);
            //OctoPOSCustomer = new(ConfigProduction, _cache);


            for (int pagenumber = 1; pagenumber <= 154; pagenumber++)
            {
                DateTime startdate = new DateTime(2022, 4, 1, 0, 0, 0);
                DateTime enddate = new DateTime(2022, 4, 30, 23, 59, 59);

                var InvoiceList = OctoPOSInvoice.SearchInvoiceListWithdetails(startdate, enddate, pagenumber);

                // var listofLocation = InvoiceList.Invoices.Select(invoice => invoice.Location).Distinct();

                foreach (var invoice in InvoiceList.values)
                {
                    var lastname = string.Empty;
                    var firstname = string.Empty;

                    if (!string.IsNullOrWhiteSpace(invoice.CustomerName))
                    {
                        var fullname = invoice.CustomerName.Split(' ').ToList();

                        if (fullname.Count == 1)
                        {
                            lastname = fullname.FirstOrDefault();
                        }

                        if (fullname.Count > 1)
                        {
                            lastname = fullname.LastOrDefault();
                            var lastindex = fullname.Count - 1;
                            fullname.RemoveAt(lastindex);

                            firstname = string.Join(" ", fullname.ToArray());
                        }

                    }


                    //var invoice = OctoPOSInvoice.Get("RB005S030863");

                    // var CustomerModels = OctoPOSCustomer.Get(invoice.CustomerCode);

                    //  invoice.CustomerDetails = CustomerModels;
                    invoice.CustomerDetails.companyid = 3389;
                    invoice.CustomerDetails.firstname = firstname;
                    invoice.CustomerDetails.lastname = lastname;


                    var Response = new KTI.Moo.Extensions.OctoPOS.Model.DTO.ChannelApps()
                    {
                        invoice = invoice,
                        customer = invoice.CustomerDetails
                    };

                    var JsonSettings = new JsonSerializerSettings
                    {
                        ContractResolver = new JSONSerializerHelper.DontIgnoreResolver()
                    };

                    var json = JsonConvert.SerializeObject(Response, Formatting.None, JsonSettings);

                    var CompressionResults = await json.ToBrotliAsync();

                    var CompressionResult = CompressionResults.Result.Value;

                    QueueClient queueClient = new(_connectionstringProd, "3389-octopos-invoice-migration", new QueueClientOptions
                    {
                        MessageEncoding = QueueMessageEncoding.Base64
                    });

                    queueClient.CreateIfNotExistsAsync().Wait();

                    queueClient.SendMessage(CompressionResult);
                }
            }




            Assert.True(true);
        }


        [Fact]
        public async Task GetDTOListSuccess_PRODUCTION_Invoice()
        {
            OctoPOSInvoice = new KTI.Moo.Extensions.OctoPOS.Custom.NCCI.Domain.Invoice(ConfigProduction, _cache);
            OctoPOSCustomer = new(ConfigProduction, _cache);

            var invoice = OctoPOSInvoice.Get("RB011S024016");

            var CustomerModels = OctoPOSCustomer.Get(invoice.CustomerCode);

            invoice.CustomerDetails = CustomerModels;

            var Response = new KTI.Moo.Extensions.OctoPOS.Model.DTO.ChannelApps()
            {
                invoice = invoice,
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

            QueueClient queueClient = new(_connectionstringProd, "3389-octopos-invoice", new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExistsAsync().Wait();

            queueClient.SendMessage(CompressionResult);

            Assert.Equal(json, DecompressionResult);

        }


        [Fact]
        public async Task GetDTOListSuccess_Invoice_ProdToTesting()
        {

            OctoPOSInvoice = new KTI.Moo.Extensions.OctoPOS.Custom.NCCI.Domain.Invoice(NewConfigProductionToTesting, _cache);
            OctoPOSCustomer = new(NewConfigProductionToTesting, _cache);

            var invoice = OctoPOSInvoice.Get("BPPM03S049716");

            var CustomerModels = OctoPOSCustomer.Get(invoice.CustomerCode);


            var Response = new KTI.Moo.Extensions.OctoPOS.Model.DTO.ChannelApps()
            {
                invoice = invoice,
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

            QueueClient queueClient = new(_connectionstring, "3388-octopos-invoice", new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExistsAsync().Wait();

            queueClient.SendMessage(CompressionResult);

            Assert.Equal(json, DecompressionResult);

        }



        [Fact]
        public async Task GetDTOListSuccess_Customer()
        {

            var invoice = OctoPOSInvoice.Get("PPMS000017");

            var CustomerModels = OctoPOSCustomer.Get(invoice.CustomerCode);

            invoice.CustomerDetails = CustomerModels;

            var Response = new KTI.Moo.Extensions.OctoPOS.Model.DTO.ChannelApps()
            {
                // invoice = invoice,
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

            QueueClient queueClient = new(_connectionstring, "3388-octopos-customer", new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExistsAsync().Wait();

            queueClient.SendMessage(CompressionResult);

            Assert.Equal(json, DecompressionResult);

        }


        [Fact]
        public async Task GetDTOListSuccess_CustomerByEmail()
        {
            OctoPOSCustomer = new(ConfigTest, _cache);
            var CustomerModels = OctoPOSCustomer.GetByEmail("kobebryant0824@gmail.com");

            var Response = new KTI.Moo.Extensions.OctoPOS.Model.DTO.ChannelApps()
            {
                // invoice = invoice,
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

            QueueClient queueClient = new(_connectionstring, "3388-octopos-customer", new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExistsAsync().Wait();

            queueClient.SendMessage(CompressionResult);

            Assert.Equal(json, DecompressionResult);

        }



    }
}
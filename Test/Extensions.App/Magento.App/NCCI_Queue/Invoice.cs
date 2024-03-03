using Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestMagento.App.Model;
using Xunit;
using DomainTotest = KTI.Moo.Extensions.Magento.App.NCCI.Queue.Receivers;

namespace TestMagento.App.NCCI_Queue
{
    public class Invoice : TestBase
    {
        [Fact]
        public void ProcessSuccess()
        {

            Type type = typeof(DomainTotest.Invoice);

            var InvoiceDomain = Activator.CreateInstance(type, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { _config, _connectionstring },null);

            //act
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(x => x.Name == "process" && x.IsPrivate)
            .First();

            var Response = (bool)method.Invoke(InvoiceDomain, new object[] { });

            Assert.IsType<bool>(Response);

        }

        [Fact]
        public void TestProcessSuccess()
        {

            Type type = typeof(DomainTotest.Invoice);

            var InvoiceDomain = Activator.CreateInstance(type, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { _config, _connectionstring }, null);

            //act
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(x => x.Name == "Testprocess" && x.IsPrivate)
            .First();

            var Response = (bool)method.Invoke(InvoiceDomain, new object[] { });

            Assert.IsType<bool>(Response);

        }

        [Fact]
        public void GetDTOListSuccess()
        {

            Type type = typeof(DomainTotest.Invoice);

            var InvoiceDomain = Activator.CreateInstance(type, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { Stagingconfig, _connectionstring }, null);

            //act
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(x => x.Name == "GetDTOList" && x.IsPrivate)
            .First();


            KTI.Moo.Extensions.Magento.Domain.Invoice ExtensionInvoiceDomain = new(Stagingconfig);


            var invoice1 = ExtensionInvoiceDomain.Get(3);
            var invoice2 = ExtensionInvoiceDomain.Get(7);

            List<KTI.Moo.Extensions.Magento.Model.Invoice> InvoiceList = new()
            {
                invoice1
                //, invoice2
            };

            var Response = (List<KTI.Moo.Extensions.Magento.Model.DTO.ChannelApps>)method.Invoke(InvoiceDomain, new object[] { InvoiceList });

            var JsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new JSONSerializerHelper.DontIgnoreResolver()
            };

            var json = JsonConvert.SerializeObject(Response, Formatting.Indented, JsonSettings);


            Assert.IsType<List<KTI.Moo.Extensions.Magento.Model.DTO.ChannelApps>>(Response);

        }



    }
}
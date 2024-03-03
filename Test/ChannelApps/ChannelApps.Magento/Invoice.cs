using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using DomainToTest = KTI.Moo.ChannelApps.Magento.App.NCCI.Receiver;
using Xunit;
using System.Reflection;
using System.Linq;

namespace TestChannelApps.Magento
{
    public class Invoice : Model.BaseTest
    {
        
        [Fact]
        public void InsertToInvoiceCRMSuccess()
        {
            var decodedstring = GetJSONwithInvoice();

            Type type = typeof(DomainToTest.Invoice);

            var InvoiceDomain = Activator.CreateInstance(type, BindingFlags.NonPublic | BindingFlags.Instance, null
                                                        , new object[] { config.companyid, connectionstringKTI }, null);

            //act
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
            .Where(x => x.Name == "Process" && x.IsPrivate && x.IsStatic)
            .First();


            var Response = (bool)method.Invoke(InvoiceDomain, new object[] { decodedstring });

            Assert.IsType<bool>(Response);


        }
    }
}
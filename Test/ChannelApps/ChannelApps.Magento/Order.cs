using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using DomainToTest = KTI.Moo.ChannelApps.Magento.App.NCCI.Receiver;
using System.Reflection;
using System.Linq;
using Xunit;
using KTI.Moo.Base.Helpers;

namespace TestChannelApps.Magento
{
    public class Order : Model.BaseTest
    {
        [Fact]
        public void InsertToOrderCRMSuccess()
        {
            var decodedstring = GetJSONwithInvoice();

            Type type = typeof(DomainToTest.Order);

            var OrderDomain = Activator.CreateInstance(type, BindingFlags.NonPublic | BindingFlags.Instance, null
                                                        , new object[] { config.companyid, connectionstringKTI }, null);

            //act
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
            .Where(x => x.Name == "Process" && x.IsPrivate && x.IsStatic)
            .First();


            var Response = (bool)method.Invoke(OrderDomain, new object[] { decodedstring });

            Assert.IsType<bool>(Response);

        }


        [Fact]
        public void InsertToOrderCRM_WithOut_Invoice_Success()
        {
            var decodedstring = GetJSON_without_Invoice();

            Type type = typeof(DomainToTest.Order);

            var OrderDomain = Activator.CreateInstance(type, BindingFlags.NonPublic | BindingFlags.Instance, null
                                                        , new object[] { config.companyid, connectionstringKTI }, null);

            //act
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
            .Where(x => x.Name == "Process" && x.IsPrivate && x.IsStatic)
            .First();


            var Response = (bool)method.Invoke(OrderDomain, new object[] { decodedstring });

            Assert.IsType<bool>(Response);

        }


        [Fact]
        public void InsertToOrderCRM_WithOut_Invoice_Success1()
        {
            var decodedstring = GetJSON_without_Invoice();

            var companyid = "3389";
            string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=ktimoostoragencciuat;AccountKey=t7YYfz7GsBd2OK2zxidEuhGfJykCZEQzdheQ754gedR1QJiyQ51mp27PmQRnpGTcafr2Tlx/L+P6+AStNaiA8Q==;EndpointSuffix=core.windows.net";

            var Domain = new KTI.Moo.ChannelApps.Magento.Implementations.NCCI.Order(ConnectionString,companyid);

            var Response = Domain.WithCustomerProcess(decodedstring);

            Assert.IsType<bool>(Response);

        }
            
        
        [Fact]
        public void test()
        {

            dynamic myValue = 9989500989.0;
            string myString = Convert.ToString(myValue);

            Assert.IsType<bool>(true);

        }


        [Fact]
        public void testNumber()
        {

            string ForFormatValue = "09170398449".FormatPhoneNumber();

            var expectedValue = "+639170398449";

            Assert.Equal(expectedValue, ForFormatValue);

        }

        [Fact]
        public void testNumber2()
        {

            string ForFormatValue = "".FormatPhoneNumber();

            var expectedValue = "";

            Assert.Equal(expectedValue, ForFormatValue);

        }

        [Fact]
        public void testNumber3()
        {

            string ForFormatValue = "6309170398449".FormatPhoneNumber();

            var expectedValue = "+639170398449";

            Assert.Equal(expectedValue, ForFormatValue);

        }

        [Fact]
        public void testNumberOtherCountryCode()
        {

            string ForFormatValue = "61 444 561 549".FormatPhoneNumber();

            var expectedValue = "+61444561549";

            Assert.Equal(expectedValue, ForFormatValue);

        }


        [Fact]
        public void testNumber10Digits()
        {

            string ForFormatValue = "9170398449".FormatPhoneNumber();

            var expectedValue = "+639170398449";

            Assert.Equal(expectedValue, ForFormatValue);

        }



    }
}

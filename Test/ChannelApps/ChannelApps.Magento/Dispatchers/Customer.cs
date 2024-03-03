using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using DomainToTest = KTI.Moo.ChannelApps.Magento.App.NCCI.Dispatcher;
using Xunit;
using System.Reflection;
using System.Linq;

namespace TestChannelApps.Magento.Dispatchers;

public class Customer : Model.BaseTest
{
    //[Fact]
    //public void InsertToExtentionQueueSuccess()
    //{
    //    var decodedstring = GetJSON_CustomerDispatcher();

    //    var dispatcherDomain = new KTI.Moo.ChannelApps.Model.NCCI.Dispatchers.Magento.Customer(config);

    //    Type type = typeof(DomainToTest.Customer);

    //    var OrderDomain = Activator.CreateInstance(type, BindingFlags.NonPublic | BindingFlags.Instance, null
    //                                             , new object[] { dispatcherDomain }, null);

    //    //act
    //    MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
    //    .Where(x => x.Name == "Process" && x.IsPrivate)
    //    .First();

    //    var Response = (bool)method.Invoke(OrderDomain, new object[] { decodedstring, CustomerQueueName, connectionstringNCCIuat });

    //    Assert.IsType<bool>(Response);
    //}
}

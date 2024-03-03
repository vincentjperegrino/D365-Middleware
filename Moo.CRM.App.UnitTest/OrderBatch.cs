using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using DomainTotest = KTI.Moo.CRM.App;


namespace Moo.CRM.App.UnitTest
{
    public class OrderBatch
    {

        private readonly ILogger _logger;

        public OrderBatch()
        {

            _logger = Mock.Of<ILogger>();
        }

        [Fact]
        public void ReadFromAzureQueueIsWorking()
        {

            Type type = typeof(DomainTotest.OrderBatch);

            var Domain = new DomainTotest.OrderBatch();

            //act
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(x => x.Name == "ReadFromAzureQueue" && x.IsPrivate)
            .First();

            int CompanyID = 3387;

            string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=ktimoostoragedev;AccountKey=JYPEw/njeKthpGVUe+Pbc4oNRXElEvvjPZQfevdb3KMe+qIUOvCbEPIOkipA4tGxxgavBd49pvND+AStnbnrkQ==;EndpointSuffix=core.windows.net";

            QueueClient queueClient = new QueueClient(ConnectionString, $"{CompanyID}-crm-order-batch");


            var Response = (bool)method.Invoke(Domain, new object[] { queueClient, _logger, 32 });

            Assert.True(Response);

        }



    }
}

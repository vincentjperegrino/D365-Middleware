using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Moo.CRM.App.UnitTest
{
    public class OrderSchedule
    {

        private readonly ILogger _logger;

        public OrderSchedule()
        {

            _logger = Mock.Of<ILogger>();
        }

        [Fact]
        public async Task AddToOrderScheduleQueueWorking()
        {     
            string CompanyID = "3389";
            string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=ktimoostoragencciuat;AccountKey=t7YYfz7GsBd2OK2zxidEuhGfJykCZEQzdheQ754gedR1QJiyQ51mp27PmQRnpGTcafr2Tlx/L+P6+AStNaiA8Q==;EndpointSuffix=core.windows.net";
            var Response = await KTI.Moo.CRM.App.Schedule.Order.Process(CompanyID, ConnectionString, _logger);
            Assert.True(Response);
        }









    }
}

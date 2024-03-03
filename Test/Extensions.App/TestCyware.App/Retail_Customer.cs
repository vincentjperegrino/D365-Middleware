using KTI.Moo.Extensions.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCyware.App
{
    public class Retail_Customer : Model.TestBase
    {
        private readonly IBlobService blobService;
        private readonly KTI.Moo.Extensions.Cyware.App.Receiver.Receivers.PollLog pollLog;
        public Retail_Customer()
        {
            blobService = new KTI.Moo.Extensions.Cyware.Services.BlobService(config);
            pollLog = new KTI.Moo.Extensions.Cyware.App.Receiver.Receivers.PollLog(blobService);
        }

        [Fact]
        public void Regular_Transaction_No_Discount()
        {
            bool result = pollLog.ProcessToQueue(Model.Data.Regular_Transaction_No_Discount.SalesTransactionHeaders, Model.Data.Regular_Transaction_No_Discount.SalesTransactionLines, Model.Data.Regular_Transaction_No_Discount.SalesTransactionTenders, Model.Data.Regular_Transaction_No_Discount.SalesTransactionDiscounts, config.AzureConnectionString);
            Assert.True(result);
        }
    }
}

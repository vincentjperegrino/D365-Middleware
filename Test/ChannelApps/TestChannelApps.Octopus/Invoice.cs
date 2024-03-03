using TestChannelApps.Octopus.Model;

namespace TestChannelApps.Octopus
{
    public class Invoice : BaseTest
    {

        [Fact]
        public void InsertToOrderCRM_WithOut_Invoice_Success1()
        {
            var decodedstring = GetJSONwithInvoice();

            var companyid = "3388";
            string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=ktimoostoragencciuat;AccountKey=t7YYfz7GsBd2OK2zxidEuhGfJykCZEQzdheQ754gedR1QJiyQ51mp27PmQRnpGTcafr2Tlx/L+P6+AStNaiA8Q==;EndpointSuffix=core.windows.net";

            var Domain = new KTI.Moo.ChannelApps.OctoPOS.Implementation.NCCI.Order(ConnectionString, companyid);

            var Response = Domain.WithCustomerProcess(decodedstring);

            Assert.IsType<bool>(Response);

        }

    }
}
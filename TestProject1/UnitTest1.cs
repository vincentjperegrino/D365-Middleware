using NUnit.Framework;
using System.Threading.Tasks;

namespace TestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {

            KTI.Moo.CRM.Domain.Inventory _Domain = new(3388);

            string WarehouseCode = "WELF01";
            string productSKU = "MAC0008";
            string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=ktimoostoragencci;AccountKey=ToNHaXe9NF8YkwGA3u7Ec5we8Ykf2fI6bRdBOml3Xmnv2AW8KwMNhQRkh0v4Cl7ccSkygk3JU6wt+AStFfpjkw==;EndpointSuffix=core.windows.net";

            var result = await _Domain.MagentoProcess(WarehouseCode, productSKU, ConnectionString);

            Assert.Pass();
        }
    }
}
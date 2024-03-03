using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    [TestClass]
    public class Inventory
    {
        private readonly KTI.Moo.CRM.Domain.Inventory _Domain;

        public Inventory()
        {
            _Domain = new KTI.Moo.CRM.Domain.Inventory(3388);
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            string WarehouseCode = "WELF01";
            string productSKU = "MAC0008";
            string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=ktimoostoragencci;AccountKey=ToNHaXe9NF8YkwGA3u7Ec5we8Ykf2fI6bRdBOml3Xmnv2AW8KwMNhQRkh0v4Cl7ccSkygk3JU6wt+AStFfpjkw==;EndpointSuffix=core.windows.net";

            var result = await _Domain.MagentoProcess(WarehouseCode, productSKU, ConnectionString);

            Assert.IsTrue(result);
        }
    }
}

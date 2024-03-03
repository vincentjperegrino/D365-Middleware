using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginsUnitTest.Plugins.ChannelManagement
{
    [TestClass]
    public class SaleChannelInventory : TestBase
    {
        private readonly CRM_Plugin.CustomAPI.Domain.ChannelManagementInventory _Domain;

        public SaleChannelInventory()
        {
            _service = connectToCRM("https://nespresso-devt.crm5.dynamics.com");
            _Domain = new CRM_Plugin.CustomAPI.Domain.ChannelManagementInventory(_service);
        }

        [TestMethod]
        public void GetSalesChannelInventoryByCode()
        {
            var response = _Domain.Get("lazadatestncci");
            Assert.IsInstanceOfType(response, typeof(CRM_Plugin.CustomAPI.Model.DTO.ChannelManagement.Inventory));
        }

        [TestMethod]
        public void GetALLSalesChannelInventory()
        {
            var response = _Domain.GetChannelList();
            Assert.IsInstanceOfType(response, typeof(List<CRM_Plugin.CustomAPI.Model.DTO.ChannelManagement.Inventory>));
        }

        [TestMethod]
        public void GetProductsFromStoreList()
        {
            var Stores = _Domain.GetChannelList();
            var response = _Domain.GetProductsFromStoreList(Stores);
            Assert.IsInstanceOfType(response, typeof(List<CRM_Plugin.CustomAPI.Model.DTO.ChannelManagement.Inventory>));
        }

    }
}

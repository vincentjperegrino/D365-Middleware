using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Moo.CRM.TestDomain
{
    public class ChannelManagementInventory
    {
        private readonly KTI.Moo.CRM.Domain.ChannelManagement.Inventory _Domain;
        private readonly ILogger _logger;

        public ChannelManagementInventory()
        {
            _Domain = new(3388);
        }

        [Fact]
        public async Task GetAllWorking()
        {
            var result = await _Domain.GetChannelListAsync();
            Assert.IsAssignableFrom<List<KTI.Moo.CRM.Model.ChannelManagement.Inventory>>(result);
        }

        [Fact]
        public async Task GetWorking()
        {
            var result = await _Domain.GetAsync("lazadatestncci");
            Assert.IsAssignableFrom<KTI.Moo.CRM.Model.ChannelManagement.Inventory>(result);
        }

    }
}


using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Moo.CRM.TestDomain
{
    public class Inventory
    {

        private readonly KTI.Moo.CRM.Domain.Inventory _Domain;
        private readonly ILogger _logger;

        public Inventory()
        {
            var mock = new Mock<ILogger>();
            _logger = Mock.Of<ILogger>();
            _Domain = new(companyId: 3388);
        }

    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Moo.CRM.TestDomain
{
    public class Product
    {
        private readonly KTI.Moo.CRM.Domain.Product _Domain;

        public Product()
        {
            _Domain = new(3388);
        }

        [Fact]
        public async Task GetAllWorking()
        {
            var result = await _Domain.getAll();

            Assert.IsAssignableFrom<List<KTI.Moo.CRM.Model.ProductBase>>(result);
        }


    }
}

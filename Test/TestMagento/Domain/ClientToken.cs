using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KTI.Moo.Extensions.Magento.Domain;
using TestMagento.Model;
using Xunit;

namespace TestMagento.Domain
{
    public class ClientToken : MagentoBase
    {

        [Fact]
        public void IFworkToken()
        {
            KTI.Moo.Extensions.Magento.Domain.ClientToken MagentoToken = new(defaultURL,redisConnectionString, username, password);

            var response = MagentoToken.Get();
          
            Assert.NotNull(response);

        }




    }
}

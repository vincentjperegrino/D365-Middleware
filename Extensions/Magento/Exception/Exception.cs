using KTI.Moo.Extensions.Core.Exception;
namespace KTI.Moo.Extensions.Magento.Exception
{
    public class MagentoIntegrationException : ApiIntegrationServiceException
    {
        public MagentoIntegrationException(string Domain, string message) : base(Domain + " Magento response: " + message)
        {

        }

        public MagentoIntegrationException(string Domain, string className, string message) : base(Domain + $", Class: {className}, Magento response: " + message)
        {

        }


    }


}
    
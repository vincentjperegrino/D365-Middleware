

namespace KTI.Moo.Extensions.Magento.Exception
{
    public class NullException : MagentoIntegrationException
    {
        public NullException(string Domain, string className, string message) : base(Domain + " Magento Response: Class: ", className + ", ", message)
        {

        }
    }
}

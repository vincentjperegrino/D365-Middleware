using KTI.Moo.Extensions.Core.Exception;
namespace KTI.Moo.Extensions.OctoPOS.Exception
{
    public class OctoPOSIntegrationException : ApiIntegrationServiceException
    {
        public OctoPOSIntegrationException(string Domain, string message) : base(Domain + " OctoPOS response: " + message)
        {

        }

        public OctoPOSIntegrationException(string Domain, string className, string message) : base(Domain + $", Class: {className}, OctoPOS response: " + message)
        {

        }
    }
}

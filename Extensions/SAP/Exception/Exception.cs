using KTI.Moo.Extensions.Core.Exception;


namespace KTI.Moo.Extensions.SAP.Exception
{
    public class SAPIntegrationException : ApiIntegrationServiceException
    {
        public SAPIntegrationException(string Domain, string message) : base(Domain + " SAP response: " + message)
        {

        }

        public SAPIntegrationException(string Domain, string className, string message) : base(Domain + $", Class: {className}, SAP response: " + message)
        {

        }


    }


}

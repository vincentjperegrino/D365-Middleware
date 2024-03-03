using System;

namespace KTI.Moo.Extensions.Core.Exception
{
    public class ApiIntegrationServiceException : System.Exception
    {
        public ApiIntegrationServiceException(string message) : base(message)
        { }
    }
}
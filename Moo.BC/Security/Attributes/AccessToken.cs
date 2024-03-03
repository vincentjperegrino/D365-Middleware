using Microsoft.Azure.WebJobs.Description;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moo.BC.Security.Attributes
{
    /// <summary>
    /// Binder for the Authorize attribute decorator
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    [Binding]
    public class AccessToken: System.Attribute
    {
        public long CompanyId { get; set; }

        public bool IsValid { get; private set; }

        public Exception Exception { get; private set; }

        public static AccessToken Success(long companyId)
        {
            return new AccessToken
            {
                CompanyId = companyId,
                IsValid = true

            };
        }

        public static AccessToken Error(Exception exception)
        {
            return new AccessToken
            {
                Exception = exception,
                IsValid = false

            };
        }
    }
}

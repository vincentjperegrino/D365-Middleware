using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moo.BC.Security.Attributes
{
    /// <summary>
    /// Initialize all the properties for the AccessToken class
    /// </summary>
    public sealed class Token
    {
        public long CompanyId { get; set; }

        public bool IsValid { get; private set; }

        public Exception Exception { get; private set; }

        public static Token Success(long companyId)
        {
            return new Token
            {
                CompanyId = companyId,
                IsValid = true

            };
        }

        public static Token Error(Exception exception)
        {
            return new Token
            {
                Exception = exception,
                IsValid = false
            };
        }
    }
}

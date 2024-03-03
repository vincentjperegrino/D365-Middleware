using Microsoft.AspNetCore.Http;
using Moo.BC.Security.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moo.BC.Interface
{
    public interface IAuthentication
    {
        Task<AccessToken> ValidToken(HttpRequest request);
    }
}

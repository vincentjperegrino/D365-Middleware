using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Azure.WebJobs.Description;
//using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Moo.BC.App.Security.Attributes
{
    /// <summary>
    /// OVerride OnAuthorizationAsync to achieve functionality needed for the bearer type logic.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class Authorize : Moo.BC.Security.Attributes.AccessToken, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            Moo.BC.Interface.IAuthentication _authentication = new Authentication();

            var accessToken = await _authentication.ValidToken(context.HttpContext.Request);

            if (!accessToken.IsValid)
            {
                // If not valid we can show the exception message.
                context.Result = new UnauthorizedResult();
            }
        }
    }
}

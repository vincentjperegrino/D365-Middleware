using KTI.Moo.Extensions.Core.Service;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Magento.Service
{
    public interface IMagentoService : IntegrationServiceBase
    {

        /// <summary>
        /// Request with no query string and no body(content)
        /// </summary>
        /// <param name="path">API path</param>
        /// <returns>Serialized string result of API call</returns>
        string ApiCall(string path);

        /// <summary>
        /// Request with no query string and no body(content). Optionanl authentication <br/><br/>
        /// Recomended:<br/>
        /// bool isAuthenticated = true; <br/>
        /// ApiCall(path, isAuthenticated)
        /// </summary>
        /// <param name="path">API path</param>
        /// <param name="isAuthenticated">If true, will do an authenticated request</param>
        /// <returns>Serialized string result of API call</returns>
        string ApiCall(string path, string method, bool isAuthenticated);

        /// <summary>
        /// Request with body(content) and no query string. Optionanl authentication<br/><br/>.
        /// </summary>
        /// Recomended:<br/>
        /// bool isAuthenticated = true; <br/>
        /// ApiCall(path, isAuthenticated)
        /// <param name="path">API path</param>
        /// <param name="method">Available method/s: POST and PUT</param>
        /// <param name="content">Body(content)</param>
        /// <param name="isAuthenticated">If true, will do an authenticated request</param>
        /// <returns>Serialized string result of API call</returns>
        string ApiCall(string path, string method, HttpContent content, bool isAuthenticated);


        /// <summary>
        /// Request with query string and no body(content). Optionanl authentication
        /// </summary>
        /// <param name="path">API path</param>
        /// <param name="parameters">Query string</param>
        /// <param name="method">"Available methods: GET and POST"</param>
        /// <returns>Serialized string result of API call</returns>
        string ApiCall(string path, string method, Dictionary<string, string> parameters, bool isAuthenticated);

        /// <summary>
        /// Request with query string and body(content). Optionanl authentication
        /// </summary>
        /// <param name="path">API path</param>
        /// <param name="parameters">Query string</param>
        /// <param name="method">"Available methods: POST and PUT"</param>
        /// <param name="content">Body(content)</param>
        /// <returns>Serialized string result of API call</returns>
        string ApiCall(string path, string method, Dictionary<string, string> parameters, HttpContent content, bool isAuthenticated);

    }
}

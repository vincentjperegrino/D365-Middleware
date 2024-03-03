using System.Collections.Generic;

namespace KTI.Moo.Extensions.Core.Service
{
    /// <summary>
    /// Integration Service base class.
    /// </summary>
    public interface IntegrationServiceBase
    {
        // Default domain for non-region-specific api calls, e.g., "https://api.website.com/
        string DefaultURL { get; }
        // Domain for region-specific api calls, e.g., "https://api.website.com.ph/"
        string RegionDomain { get; set; }

        /// <summary>
        /// Make an HTTPS call to the domain+path with the specified method and parameters.
        /// </summary>
        /// <param name="path">The api path, e.g., /products/get</param>
        /// <param name="parameters">A dictionary of parameters according to api specification, e.g., {{"product_id", "1234"}, ...}</param>
        /// <param name="method">The HTTP method to use, e.g., GET, POST, etc.</param>
        /// <param name="useRegionDomain">Whether to use a region-specific domain, or the default domain.</param>
        /// <returns>The JSON response string.</returns>
        string ApiCall(string path, Dictionary<string, string> parameters, string method, bool useRegionDomain = true);

        /// <summary>
        /// ApiCall wrapper that adds any authentication.
        /// </summary>
        /// <param name="path">The api path, e.g., /products/get</param>
        /// <param name="parameters">A dictionary of parameters according to api specification, e.g., {{"product_id", "1234"}, ...}</param>
        /// <param name="method">The HTTP method to use, e.g., GET, POST, etc.</param>
        /// <param name="useRegionDomain">Whether to use a region-specific domain, or the default domain.</param>
        /// <returns>The JSON response string.</returns>
        string AuthenticatedApiCall(string path, Dictionary<string, string> parameters, string method, bool useRegionDomain = true);
    }
}

// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;

namespace Prolix.Api.Handlers
{
    /// <summary>
    /// Handle invalid routes and sens 503 Http status
    /// </summary>
    public class RouteHandler : DelegatingHandler
    {
        async protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Call the inner handler.
            var response = await base.SendAsync(request, cancellationToken);
                
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                var config = request.GetConfiguration();
                var selector = new DefaultHttpControllerSelector(config);
                var mappings = selector.GetControllerMapping();

                string controllerName = selector.GetControllerName(request);

                if (string.IsNullOrWhiteSpace(controllerName) || !mappings.ContainsKey(controllerName))
                {
                    // Return Http Status 503
                    response.StatusCode = HttpStatusCode.ServiceUnavailable;
                    response.Content = new StringContent("The requested endpoint does not exist");
                }
            }
            
            return response;
        }
    }
}

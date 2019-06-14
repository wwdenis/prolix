// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Prolix.AspNet.Filters
{
    public class OnlyLocalAttribute : ActionFilterAttribute, IDependencyFilter
    {
        public FilterScope Scope => FilterScope.Action;

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // Permite somente acesso local
            if (!actionContext.RequestContext.IsLocal)
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);

            base.OnActionExecuting(actionContext);
        }
    }
}

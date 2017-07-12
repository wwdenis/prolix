// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

using Wwa.Core.Extensions.Parsing;

namespace Wwa.Api.Filters
{
    public abstract class PermissionBaseAttribute : ActionFilterAttribute, IDependencyFilter
    {
        object sync = new object();

        public FilterScope Scope => FilterScope.Global;

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var identity = actionContext.RequestContext.Principal.Identity;

            if (identity.IsAuthenticated)
            {
                var request = actionContext.Request;
                var route = request.RequestUri.LocalPath.Trim('/');
                var method = request.Method.Method.ToCapital();

                bool allowAccess = false;

                // Thread-safe call
                lock (sync)
                {
                    allowAccess = Evaluate(identity, route, method);
                }

                // Deny access
                if (!allowAccess)
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
            }

            base.OnActionExecuting(actionContext);
        }

        protected abstract bool Evaluate(IIdentity identity, string route, string method);
    }
}

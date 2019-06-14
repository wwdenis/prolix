// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Filters;
using Prolix.Data;

namespace Prolix.AspNet.Filters
{
    public class UnitOfWorkAttribute : ActionFilterAttribute, IDependencyFilter
    {
        readonly Type _contextType;
        
        public UnitOfWorkAttribute(Type contextType)
        {
            _contextType = contextType ?? throw new ArgumentNullException(nameof(contextType));
        }

        public UnitOfWorkAttribute() : this(typeof(IDbContext))
        {
        }

        public FilterScope Scope => FilterScope.Action;

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var scope = actionContext.Request.GetDependencyScope();
            var context = scope.GetService(_contextType) as IDbContext;

            if (context == null)
                throw new InvalidOperationException("Invalid DataContext"); ;

            context.Start();

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            bool success = actionExecutedContext.Exception == null;

            var scope = actionExecutedContext.Request.GetDependencyScope();
            var context = scope.GetService(_contextType) as IDbContext;

            if (success)
                context.Commit();
            else
                context.Rollback();

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
